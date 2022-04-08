using System.Net;
using HairdresserBookingApi.Models.Exceptions;

namespace HairdresserBookingApi.Middleware;

public class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (NotFoundException notFoundException)
        {
            context.Response.StatusCode = (int) HttpStatusCode.NotFound;
            _logger.LogError($"Not Found Exception error");
            await context.Response.WriteAsync(notFoundException.Message);
        }
        catch (EntityExistsException entityExistsException)
        {
            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            _logger.LogError($"Entity Exists Exception error");
            await context.Response.WriteAsync(entityExistsException.Message);
        }
        catch (NotAccessibleException notAccessibleException)
        {
            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            _logger.LogError($"NotAccessible  Exception error");
            await context.Response.WriteAsync(notAccessibleException.Message);
        }
        catch (ForbidException forbidException)
        {
            context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
            _logger.LogError($"Forbid Exception error");
            await context.Response.WriteAsync(forbidException.Message);
        }
        catch (AppException appException)
        {
            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            _logger.LogError($"App Exception error");
            await context.Response.WriteAsync(appException.Message);
        }
        catch (InvalidOperationException invalidOperationException)
        {
            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            _logger.LogError($"Invalid operation Exception error");
            await context.Response.WriteAsync(invalidOperationException.Message);
        }
        catch (Exception)
        {
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            _logger.LogCritical($"Internal server Exception error");
            await context.Response.WriteAsync("Error");
        }
    }
}