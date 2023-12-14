namespace WebApplicationTicketsCRUD.Dto;

public class ResponseTicketDto
{
    public int Id { get; set; }

    public string OwnerFirstName { get; set; }

    public string OwnerLastName { get; set; }

    public string Phone { get; set; }

    public string TicketType { get; set; }
}