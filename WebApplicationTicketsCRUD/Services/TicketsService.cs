using WebApplicationTicketsCRUD.Dto;
using WebApplicationTicketsCRUD.Exceptions;
using WebApplicationTicketsCRUD.Repositories;

namespace WebApplicationTicketsCRUD.Services;

public class TicketsService
{
    private readonly TicketRepository _ticketRepository;
    private readonly TicketTypeRepository _ticketTypeRepository;
    private readonly UserRepository _userRepository;

    public TicketsService(TicketRepository ticketRepository,
        TicketTypeRepository ticketTypeRepository, UserRepository userRepository)
    {
        _ticketTypeRepository = ticketTypeRepository;
        _userRepository = userRepository;
        _ticketRepository = ticketRepository;
    }

    public List<ResponseTicketDto> GetAllTickets()
    {
        return _ticketRepository.GetAll();
    }

    public List<ResponseTicketDto> GetTicketsByEmail(string email)
    {
        return _ticketRepository.GetTicketsByEmail(email);
    }

    public void CreateNewTicket(RequestTicketDto responseTicketDto)
    {
        var ticketType = _ticketTypeRepository.GetById(responseTicketDto.ticketTypeId);
        var user = _userRepository.GetByEmail(responseTicketDto.Email);

        if (ticketType == null)
        {
            throw new UserException("CreateNewTicket Exception",
                $"ticketTypes with {responseTicketDto.ticketTypeId} not found", 400);
        }

        if (user == null)
        {
            throw new UserException("CreateNewTicket Exception",
                $"user with {responseTicketDto.ticketTypeId} not found", 400);
        }

        _ticketRepository.Create(user.Id, responseTicketDto.ticketTypeId);
    }

    public void DeleteById(int id)
    {
        var ticket = _ticketRepository.GetById(id);

        if (ticket == null)
        {
            throw new UserException("DeleteById Exception", $"ticket with {id} not found", 400);
        }

        _ticketRepository.Remove(ticket);
    }
}