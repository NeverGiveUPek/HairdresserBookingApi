using System.Security.Claims;
using HairdresserBookingApi.Models.Entities.Users;
using HairdresserBookingApi.Services.Interfaces;

namespace HairdresserBookingApi.Services.Implementations;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _accessor;

    public UserContextService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }


    public ClaimsPrincipal? GetUser()
    {
        return _accessor.HttpContext?.User;
    }

    public int? GetUserId()
    {
        if (GetUser() == null) return null;

        return int.Parse(GetUser()?.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }

    public string? GetUserRole()
    {
        if (GetUser() == null) return null;

        return GetUser()?.FindFirst(c => c.Type == ClaimTypes.Role).Value;
    }
}