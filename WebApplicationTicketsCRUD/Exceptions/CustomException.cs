namespace WebApplicationTicketsCRUD.Exceptions;

public class CustomException : Exception
{
    public string Title { get; private set; }
    public string Message { get; private set; }
    public int Code { get; private set; }

    public CustomException(string title, string message, int code)
    {
        Title = title;
        Message = message;
        Code = code;
    }
}