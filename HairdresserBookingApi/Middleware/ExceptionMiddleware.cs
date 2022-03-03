using System.Net;
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
            context.Response.StatusCode = (int) HttpStatusCode.NotFound;
            await context.Response.WriteAsync(notFoundException.Message);
        }
        catch (EntityExistsException entityExistsException)
        {
            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            await context.Response.WriteAsync(entityExistsException.Message);
        }
        catch (ForbidException forbidException)
        {
            context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
            await context.Response.WriteAsync(forbidException.Message);
        }
        catch (AppException appException)
        {
            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            await context.Response.WriteAsync(appException.Message);
        }
        catch (InvalidOperationException invalidOperationException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync(invalidOperationException.Message);
        }
        catch (Exception)
        {

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync("Error");
        }
    }
}