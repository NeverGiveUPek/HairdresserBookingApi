using HairdresserBookingApi.Models.Dto.Worker;
using HairdresserBookingApi.Models.Dto.WorkerActivity;

namespace HairdresserBookingApi.Models.Dto.Activity;

public class ActivityDetailsDto
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool IsForMan { get; set; }

    public List<WorkerActivityDto> WorkerActivity { get; set; } 
}