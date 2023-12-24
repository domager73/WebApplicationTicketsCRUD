namespace WebApplicationTicketsCRUD.Exceptions;

public class UserException : CustomException
{
    public UserException(string title, string message, int code) : base(title, message, code)
    {
    }
}