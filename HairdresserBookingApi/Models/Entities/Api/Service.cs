using System.ComponentModel.DataAnnotations;

namespace HairdresserBookingApi.Models.Entities.Api;

public class Service
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [MaxLength(300)]
    public string Description { get; set; }

    public int RequiredMinutes { get; set; }

    public double Price { get; set; }

    public virtual ICollection<Worker> Workers { get; set; }

}