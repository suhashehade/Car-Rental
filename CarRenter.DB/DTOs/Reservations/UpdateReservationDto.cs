namespace CarRenter.DB.DTOs.Reservations;

public class UpdateReservationDto
{
    public string? Id { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public string CarId  { get; set; } = string.Empty;
}