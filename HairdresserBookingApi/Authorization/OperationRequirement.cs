using Microsoft.AspNetCore.Authorization;

namespace HairdresserBookingApi.Authorization;

public enum Operation
{
    Create,
    Read,
    Update,
    Delete
}

public class OperationRequirement : IAuthorizationRequirement
{
    public Operation Operation { get; set; }

    public OperationRequirement(Operation operation)
    {
        Operation = operation;
    }
}