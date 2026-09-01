namespace CarRenter.DB.DTOs.Reservations;

public class CreateReservationDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public string CarId  { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}