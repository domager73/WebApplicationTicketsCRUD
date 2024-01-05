using Microsoft.EntityFrameworkCore;
using WebApplicationTicketsCRUD.Db.DbConnector;
using WebApplicationTicketsCRUD.Db.Models;
using WebApplicationTicketsCRUD.Dto;

namespace WebApplicationTicketsCRUD.Repositories;

public class TicketRepository
{
    private readonly TicketsDbContext _dbContext;

    public TicketRepository(TicketsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<ResponseTicketDto> GetAll()
    {
        return _dbContext.Tickets
            .Include(item => item.TicketType)
            .Include(item => item.User)
            .Select(item => new ResponseTicketDto()
            {
                Id = item.Id,
                TicketType = item.TicketType.Name,
                Email = item.User.Email
            }).OrderBy(item => item.Id).ToList();
    }
    
    public List<ResponseTicketDto> GetTicketsByEmail(string email)
    {
        return _dbContext.Tickets
            .Include(item => item.TicketType)
            .Include(item => item.User)
            .Where(item => item.User.Email == email)
            .Select(item => new ResponseTicketDto()
            {
                Id = item.Id,
                TicketType = item.TicketType.Name,
                Email = item.User.Email
            }).OrderBy(item => item.Id).ToList();
    }

    public void Create(int id, int ticketTypeId)
    {
        var ticket = new Ticket()
        {
            UserId = id,
            TicketTypeId = ticketTypeId,
        };

        _dbContext.Tickets.Add(ticket);
        _dbContext.SaveChanges();
    }

    public Ticket? GetById(int id)
    {
        return _dbContext.Tickets.FirstOrDefault(ticket => ticket.Id == id);
    }

    public void Remove(Ticket ticket)
    {
        _dbContext.Tickets.Remove(ticket);
        _dbContext.SaveChanges();
    }

    public List<Ticket> GetTicketsByUser(string email)
    {
        return _dbContext.Tickets.Where(item => item.User.Email == email).ToList();
    }
}