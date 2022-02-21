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

    [Required]
    public bool IsForMan { get; set; }


    public virtual ICollection<WorkerService> WorkerServices { get; set; }

}