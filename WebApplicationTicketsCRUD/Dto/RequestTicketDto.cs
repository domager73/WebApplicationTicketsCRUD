namespace WebApplicationTicketsCRUD.Dto;

public class RequestTicketDto
{
    public string OwnerFirstName { get; set; }

    public string OwnerLastName { get; set; }

    public string Phone { get; set; }

    public int IdTicketType { get; set; }
}