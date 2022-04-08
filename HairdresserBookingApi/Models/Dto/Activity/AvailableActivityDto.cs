namespace HairdresserBookingApi.Models.Dto.Activity;

public class AvailableActivityDto
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool IsForMan { get; set; }

    public int MinPrice { get; set; }
}