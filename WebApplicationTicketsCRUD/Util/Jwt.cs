using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebApplicationTicketsCRUD.Util;

public static class Jwt
{
    private static JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

    public static JwtSecurityToken CreateNewJwtToken(string email, IConfiguration configuration)
    {
        Claim[] claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToFileTimeUtc().ToString()),
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        SigningCredentials signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            configuration["Jwt:Issuer"],
            configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddSeconds(10000),
            signingCredentials: signIn);

        return token;
    }

    public static string GetEmailInJwt(HttpRequest request)
    {
        var handler = new JwtSecurityTokenHandler();

        string header = request.Headers.Authorization!;
        header = header.Replace("Bearer ", "");

        var token = handler.ReadToken(header) as JwtSecurityToken;

        var email = token!.Claims.First(claim => claim.Type == "email").Value;

        return email;
    }
}