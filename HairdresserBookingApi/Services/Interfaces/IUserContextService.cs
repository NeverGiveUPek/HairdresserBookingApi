using System.Security.Claims;
using HairdresserBookingApi.Models.Entities.Users;

namespace HairdresserBookingApi.Services.Interfaces;

public interface IUserContextService
{
    ClaimsPrincipal? GetUser();
    int? GetUserId();
    string? GetUserRole();
}