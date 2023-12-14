using System;
using System.Collections.Generic;

namespace WebApplicationTicketsCRUD.Db.Models;

public partial class Ticket
{
    public int Id { get; set; }

    public string OwnerFirstName { get; set; } = null!;

    public string OwnerLastName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int TicketTypeId { get; set; }

    public virtual TicketType TicketType { get; set; } = null!;
}
