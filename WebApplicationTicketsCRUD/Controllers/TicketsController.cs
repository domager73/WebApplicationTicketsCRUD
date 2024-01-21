using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplicationTicketsCRUD.Dto;
using WebApplicationTicketsCRUD.Services;
using WebApplicationTicketsCRUD.Util;

namespace WebApplicationTicketsCRUD.Controllers;

[Authorize]
[ApiController]
[Route("tickets/")]
public class TicketsController : ControllerBase
{
    private readonly TicketsService _ticketsService;

    public TicketsController(TicketsService ticketsService)
    {
        _ticketsService = ticketsService;
    }

    [HttpGet("get-all")]
    public List<ResponseTicketDto> GetAllTickets()
    {
        return _ticketsService.GetAllTickets();
    }

    [HttpDelete("delete/{id}")]
    public void GetAllTickets(int id)
    {
        _ticketsService.DeleteById(id);
    }
    
    [HttpPost("add")]
    public void AddNewTicket(RequestTicketDto requestTicketDto)
    {
        _ticketsService.CreateNewTicket(requestTicketDto);
    }

    [HttpPost("get-by-email/{email}")]
    public List<ResponseTicketDto> GetTicketsByEmail(string email){
        return _ticketsService.GetTicketsByEmail(email);
    }
    
    [HttpPost("get-my")]
    public List<ResponseTicketDto> GetMyTickets()
    {
        var email = JwtUtil.GetEmailInJwt(Request);


        return _ticketsService.GetTicketsByEmail(email);
    }
}