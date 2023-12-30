using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplicationTicketsCRUD.Dto;
using WebApplicationTicketsCRUD.Services;

namespace WebApplicationTicketsCRUD.Controllers;

[Authorize]
[ApiController]
[Route("ticket-types/")]
public class TicketTypesController : ControllerBase
{
    private TicketTypeService _ticketTypeService;

    public TicketTypesController(TicketTypeService ticketTypeService)
    {
        _ticketTypeService = ticketTypeService;
    }

    [HttpGet("get-all")]
    public List<ResponseTicketTypeDto> GetAllTicketTypes()
    {
        return _ticketTypeService.GetAllTicketTypes();
    }
}