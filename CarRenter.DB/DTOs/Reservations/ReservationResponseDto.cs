namespace CarRenter.DB.DTOs.Reservations;

public class ReservationResponseDto
{
    public string ReservationId  { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalPrice { get; set; }
    public string CarName  { get; set; } = string.Empty;
}