namespace HairdresserBookingApi.Models.Exceptions;

public class NotAccessibleException : Exception
{
    public NotAccessibleException(string message) : base(message)
    {
    }
}