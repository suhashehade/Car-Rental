namespace CarRenter.DB.DTOs.Cars;

public class AvailableCarDto
{
    public string? CarId { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public decimal HourlyPrice { get; set; }
}