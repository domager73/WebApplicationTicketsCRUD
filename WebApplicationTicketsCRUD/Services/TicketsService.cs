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
            }).ToList();
    }
}