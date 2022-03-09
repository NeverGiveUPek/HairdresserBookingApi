namespace HairdresserBookingApi.Models.Dto.Reservation;

public class ReservationInfoDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int RequiredMinutes { get; set; }
    public double Price { get; set; }
}