using System.ComponentModel.DataAnnotations;
using HairdresserBookingApi.Models.Entities.Api;


namespace HairdresserBookingApi.Models.Dto.WorkerActivity;


public class CreateWorkerActivityDto
{
    [Required]
    public double? Price { get; set; }

    [Required]
    public int? RequiredMinutes { get; set; }

    [Required]
    public int WorkerId { get; set; }

    [Required]
    public int ActivityId { get; set; }

}