using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebApplicationTicketsCRUD.Dto;
using WebApplicationTicketsCRUD.Exceptions;
using WebApplicationTicketsCRUD.Kafka;
using WebApplicationTicketsCRUD.Util;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace WebApplicationTicketsCRUD.Services;

public class AuthService
{
    private RedisUtil _cache;
    private Random _random;
    private IConfiguration _configuration;
    private TestKafka _kafka;

    public AuthService(RedisUtil cache, IConfiguration configuration, TestKafka kafka)
    {
        _cache = cache;
        _configuration = configuration;
        _kafka = kafka;
        _random = new Random();
    }

    public void Login(RequestUserDto user)
    {
        _kafka.SendNotifyToLocalConsole($"Запрос на логинацию по {user.Email}");
        
        int randomCode = _random.Next(1000, 9999 + 1);

        _cache.Save<int>(user.Email, randomCode);
    }

    public string VerifyLogin(RequestUserWithCodeDto userWithCode)
    {
        if (_cache.ExistData(userWithCode.Email))
        {
            throw new UserException("VerifyLogin Exception", $"data with {userWithCode.Email} not found", 400);
        }

        int codeFromRedis = _cache.Get<int>(userWithCode.Email);

        if (codeFromRedis != userWithCode.Code)
        {
            throw new UserException("VerifyLogin Exception", $"Incorrect code", 400);
        }
        else
        {
            Claim[] claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userWithCode.Email),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToFileTimeUtc().ToString()),
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            SigningCredentials signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddSeconds(60),
                signingCredentials: signIn);
            
            _cache.Remove(userWithCode.Email);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}