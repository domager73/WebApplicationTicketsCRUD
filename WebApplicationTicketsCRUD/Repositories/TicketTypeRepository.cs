using WebApplicationTicketsCRUD.Db.DbConnector;
using WebApplicationTicketsCRUD.Db.Models;
using WebApplicationTicketsCRUD.Dto;

namespace WebApplicationTicketsCRUD.Repositories;

public class TicketTypeRepository
{
    private readonly TicketsDbContext _dbContext;

    public TicketTypeRepository(TicketsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<ResponseTicketTypeDto> GetAll()
    {
        return _dbContext.TicketTypes
            .Select(item => new ResponseTicketTypeDto()
            {
                Id = item.Id,
                Name = item.Name
            }).ToList();
    }

    public TicketType? GetById(int id)
    {
        return _dbContext.TicketTypes.FirstOrDefault(ticket => ticket.Id == id);
    }
}