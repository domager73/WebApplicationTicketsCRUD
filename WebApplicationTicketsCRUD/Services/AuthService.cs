using WebApplicationTicketsCRUD.Db.DbConnector;
using WebApplicationTicketsCRUD.Exceptions;
using WebApplicationTicketsCRUD.Util;

namespace WebApplicationTicketsCRUD.Services;

public class AuthService
{
    private RedisUtil _cache;
    private Random _random;

    public AuthService(RedisUtil cache)
    {
        _cache = cache;
        _random = new Random();
    }

    public void Login(string email)
    {
        int randomCode = _random.Next(1000, 9999 + 1);
        
        _cache.Save<int>(email, randomCode);
    }
    
    public void VerifyLogin(string email, int code)
    {
        if (_cache.ExistData(email))
        {
            throw new UserException("VerifyLogin Exception", $"data with {email} not found", 400);
        }

        int codeFromRedis = _cache.Get<int>(email);

        if (codeFromRedis != code)
        {
            throw new UserException("VerifyLogin Exception", $"Incorrect code", 400);
        }
        else
        {
            _cache.Remove(email);
        }
    }
}