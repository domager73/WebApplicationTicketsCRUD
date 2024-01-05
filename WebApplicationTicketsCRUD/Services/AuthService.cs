using System.IdentityModel.Tokens.Jwt;
using WebApplicationTicketsCRUD.Db.DbConnector;
using WebApplicationTicketsCRUD.Db.Models;
using WebApplicationTicketsCRUD.Dto;
using WebApplicationTicketsCRUD.Exceptions;
using WebApplicationTicketsCRUD.Kafka;
using WebApplicationTicketsCRUD.Repositories;
using WebApplicationTicketsCRUD.Util;
using WebApplicationTicketsCRUD.Validators;

namespace WebApplicationTicketsCRUD.Services;

public class AuthService
{
    private readonly RedisUtil _cache;
    private readonly Random _random;
    private readonly IConfiguration _configuration;
    private readonly TestKafka _kafka;
    private readonly UserRepository _userRepository;
    private readonly UserValidator _userValidator;


    public AuthService(RedisUtil cache, IConfiguration configuration, TestKafka kafka, UserRepository userRepository,
        UserValidator userValidator)
    {
        _cache = cache;
        _configuration = configuration;
        _kafka = kafka;
        _userRepository = userRepository;
        _userValidator = userValidator;
        _random = new Random();
    }

    public void Login(RequestUserDto user)
    {
        var validator = _userValidator.Validate(user);

        if (!validator.IsValid)
        {
            throw new UserException("Login Exception", $"incorrect email", 400);
        }

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

        _userRepository.CreateNewUserOrNothing(new User()
        {
            Email = userWithCode.Email,
        });

        JwtSecurityToken token = Jwt.CreateNewJwtToken(userWithCode.Email, _configuration);

        _cache.Remove(userWithCode.Email);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}