namespace WebApplicationTicketsCRUD.Exceptions;

public class ServerException : CustomException
{
    public ServerException(string title, string message, int code) : base(title, message, code)
    {
    }
}