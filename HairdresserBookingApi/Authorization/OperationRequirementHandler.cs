using System.Security.Claims;
using HairdresserBookingApi.Models.Entities.Api;
using Microsoft.AspNetCore.Authorization;

namespace HairdresserBookingApi.Authorization;

public class OperationRequirementHandler : AuthorizationHandler<OperationRequirement, Reservation>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationRequirement requirement,
        Reservation reservation)
    {
        if(requirement.Operation is Operation.Read or Operation.Create) context.Succeed(requirement);

        //if (reservation.Date < DateTime.Now.AddHours(2)) return Task.CompletedTask;
        
        var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (reservation.UserId == int.Parse(userId)) context.Succeed(requirement);

        return Task.CompletedTask;

    }
}