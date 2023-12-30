using Microsoft.AspNetCore.Mvc;
using WebApplicationTicketsCRUD.Dto;
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
    public void Login(RequestUserDto userDto)
    {
        _authService.Login(userDto);
    }

    [HttpPost("verify-login")]
    public string VerifyLogin(RequestUserWithCodeDto userWithCode)
    {
        return _authService.VerifyLogin(userWithCode);
    }
}