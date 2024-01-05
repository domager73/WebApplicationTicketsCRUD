namespace WebApplicationTicketsCRUD.Db.Models;

public class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; } = new List<Ticket>();
}
