using HairdresserBookingApi.Models.Entities.Api;

namespace HairdresserBookingApi.Models.Dto;

public class AvailableServiceDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public bool IsForMan { get; set; }

    public int MinPrice { get; set; }

}