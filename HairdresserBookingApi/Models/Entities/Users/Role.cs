using System.ComponentModel.DataAnnotations;

namespace HairdresserBookingApi.Models.Entities;

public class Role
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }

}