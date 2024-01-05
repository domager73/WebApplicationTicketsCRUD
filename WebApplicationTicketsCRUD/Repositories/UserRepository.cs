using WebApplicationTicketsCRUD.Db.DbConnector;
using WebApplicationTicketsCRUD.Db.Models;

namespace WebApplicationTicketsCRUD.Repositories;

public class UserRepository
{
    private readonly TicketsDbContext _dbContext;

    public UserRepository(TicketsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void CreateNewUserOrNothing(User newUser)
    {
        if (_dbContext.Users.FirstOrDefault(user => user.Email == newUser.Email) == null)
        {
            _dbContext.Users.Add(newUser);
        }
        
        _dbContext.SaveChanges();
    }

    public User? GetByEmail(string email)
    {
        return _dbContext.Users.FirstOrDefault(user => user.Email == email);
    }
}