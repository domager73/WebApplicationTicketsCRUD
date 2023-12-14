using Microsoft.AspNetCore.Mvc;
using WebApplicationTicketsCRUD.Dto;
using WebApplicationTicketsCRUD.Services;

namespace WebApplicationTicketsCRUD.Controllers;

[ApiController]
[Route("tickets/")]
public class TicketsController : ControllerBase
{
    private TicketsService _ticketsService;

    public TicketsController(TicketsService ticketsService)
    {
        _ticketsService = ticketsService;
    }

    [HttpGet("get-all")]
    public List<ResponseTicketDto> GetAllTickets()
    {
        return _ticketsService.GetAllTickets();
    }
}