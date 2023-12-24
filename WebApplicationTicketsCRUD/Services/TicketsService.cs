using Microsoft.EntityFrameworkCore;
using WebApplicationTicketsCRUD.Db.DbConnector;
using WebApplicationTicketsCRUD.Db.Models;
using WebApplicationTicketsCRUD.Dto;
using WebApplicationTicketsCRUD.Exceptions;
using WebApplicationTicketsCRUD.Validators;

namespace WebApplicationTicketsCRUD.Services;

public class TicketsService
{
    private TicketsDbContext _dbContext;
    private TicketValidator _ticketValidator;

    public TicketsService(TicketsDbContext dbContext, TicketValidator ticketValidator)
    {
        _dbContext = dbContext;
        _ticketValidator = ticketValidator;
    }

    public List<ResponseTicketDto> GetAllTickets()
    {
        return _dbContext.Tickets
            .Include(item => item.TicketType)
            .Select(item => new ResponseTicketDto()
            {
                Id = item.Id,
                OwnerFirstName = item.OwnerFirstName,
                OwnerLastName = item.OwnerLastName,
                Phone = item.Phone,
                TicketType = item.TicketType.Name
            }).OrderBy(item => item.Id).ToList();
    }
    public void CreateNewTicket(RequestTicketDto responseTicketDto)
    {
        var ticketType = _dbContext.TicketTypes.FirstOrDefault(ticket => ticket.Id == responseTicketDto.IdTicketType);

        if (ticketType == null)
        {
            throw new UserException("CreateNewTicket Exception", $"ticketTypes with {responseTicketDto.IdTicketType} not found", 400);
        }

        var validate = _ticketValidator.Validate(responseTicketDto);
        
        if (!validate.IsValid)
        {
            throw new UserException("CreateNewTicket Exception", $"request ticket dto is not validate", 400);
        }

        var ticket = new Ticket()
        {
            OwnerLastName = responseTicketDto.OwnerLastName,
            Phone = responseTicketDto.Phone,
            OwnerFirstName = responseTicketDto.OwnerFirstName,
            TicketTypeId = responseTicketDto.IdTicketType,
        };

        _dbContext.Tickets.Add(ticket);
        _dbContext.SaveChanges();
    }

    public void DeleteById(int id)
    {
        var ticket = _dbContext.Tickets.FirstOrDefault(ticket => ticket.Id == id);
        
        if (ticket == null)
        {
            throw new UserException("DeleteById Exception", $"ticket with {id} not found", 400);
        }

        _dbContext.Tickets.Remove(ticket);
        _dbContext.SaveChanges();
    }
}