using System.ComponentModel.DataAnnotations;

namespace HairdresserBookingApi.Models.Dto.Availability;

public class AvailabilityDto
{
    public int Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}