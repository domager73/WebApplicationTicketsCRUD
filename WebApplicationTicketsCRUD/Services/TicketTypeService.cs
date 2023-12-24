using WebApplicationTicketsCRUD.Db.DbConnector;
using WebApplicationTicketsCRUD.Dto;

namespace WebApplicationTicketsCRUD.Services;

public class TicketTypeService
{
    private TicketsDbContext _dbContext;

    public TicketTypeService(TicketsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<ResponseTicketTypeDto> GetAllTicketTypes()
    {
        return _dbContext.TicketTypes
            .Select(item => new ResponseTicketTypeDto()
            {
                Id = item.Id,
                Name = item.Name
            }).ToList();
    }
}