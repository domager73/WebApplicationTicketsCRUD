using System;
using System.Collections.Generic;

namespace WebApplicationTicketsCRUD.Db.Models;

public partial class TicketType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
