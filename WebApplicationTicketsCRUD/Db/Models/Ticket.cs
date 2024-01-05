namespace WebApplicationTicketsCRUD.Db.Models;

public class Ticket
{
    public int Id { get; set; }

    public int TicketTypeId { get; set; }

    public int UserId { get; set; }

    public virtual TicketType TicketType { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
