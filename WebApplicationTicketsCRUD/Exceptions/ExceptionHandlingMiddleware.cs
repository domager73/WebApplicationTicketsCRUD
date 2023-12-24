using WebApplicationTicketsCRUD.Dto;

namespace WebApplicationTicketsCRUD.Exceptions;

public class ExceptionHandlingMiddleware
{
    private RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (UserException ex)
        {
            await HandleExceptionAsync(httpContext, 400, ex.Title, ex.Message, ex.Code);
        }
        catch (ServerException ex)
        {
            await HandleExceptionAsync(httpContext, 500, ex.Title, ex.Message, ex.Code);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, 501, "Unhandled message", ex.Message, 501);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, int httpCode, string title, string message,
        int code)
    {
        HttpResponse response = httpContext.Response;

        response.ContentType = "application/json";
        response.StatusCode = httpCode;

        ExceptionResponseDto exceptionDto = new ExceptionResponseDto()
        {
            Code = code,
            Title = title,
            Message = message
        };

        await response.WriteAsJsonAsync(exceptionDto);
    }
}