using Microsoft.AspNetCore.Mvc;
using WebApplicationTicketsCRUD.Services;

namespace WebApplicationTicketsCRUD.Controllers;

[ApiController]
[Route("auth/")]
public class AuthController : ControllerBase
{
    private AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public void Login(string email)
    {
        _authService.Login(email);
    }

    [HttpPost("verify-login")]
    public void VerifyLogin(string email, int code)
    {
        _authService.VerifyLogin(email, code);
    }
}