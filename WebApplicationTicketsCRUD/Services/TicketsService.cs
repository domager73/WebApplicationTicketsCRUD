using Microsoft.EntityFrameworkCore;
using WebApplicationTicketsCRUD.Db.DbConnector;
using WebApplicationTicketsCRUD.Db.Models;
using WebApplicationTicketsCRUD.Dto;

namespace WebApplicationTicketsCRUD.Services;

public class TicketsService
{
    private TicketsDbContext _dbContext;

    public TicketsService(TicketsDbContext dbContext)
    {
        _dbContext = dbContext;
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
        _dbContext.Tickets.Remove(_dbContext.Tickets.FirstOrDefault(ticket => ticket.Id == id)!);
        _dbContext.SaveChanges();
    }
}