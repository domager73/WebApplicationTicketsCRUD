using WebApplicationTicketsCRUD.Dto;
using WebApplicationTicketsCRUD.Repositories;

namespace WebApplicationTicketsCRUD.Services;

public class TicketTypeService
{
    private readonly TicketTypeRepository _ticketTypeRepository;
    
    public TicketTypeService(TicketTypeRepository ticketTypeRepository)
    {
        _ticketTypeRepository = ticketTypeRepository;
    }

    public List<ResponseTicketTypeDto> GetAllTicketTypes()
    {
        return _ticketTypeRepository.GetAll();
    }
}