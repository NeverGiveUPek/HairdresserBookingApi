using System.ComponentModel.DataAnnotations;

namespace HairdresserBookingApi.Models.Dto.Availability;


//Validate in AddAvailabilityDtoValidator
public class AddAvailabilityDto
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}