using System.ComponentModel.DataAnnotations;

namespace HairdresserBookingApi.Models.Dto.Activity;

public class UpdateActivityDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [MaxLength(300)]
    public string Description { get; set; }

    [Required]
    public bool IsForMan { get; set; }
}