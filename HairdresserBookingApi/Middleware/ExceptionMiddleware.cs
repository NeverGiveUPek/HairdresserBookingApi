using HairdresserBookingApi.Models.Exceptions;

namespace HairdresserBookingApi.Middleware;

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (NotFoundException notFoundException)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync(notFoundException.Message);
        }
        catch (EntityExistsException entityExistsException)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(entityExistsException.Message);
        } 
        catch (Exception)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Error");
        }
    }
}