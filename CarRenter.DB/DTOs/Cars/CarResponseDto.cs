namespace CarRenter.DB.DTOs.Cars;

public class CarResponseDto
{
    public string? Id { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    
    public decimal HourlyPrice { get; set; }
    public bool IsAvailable { get; set; }
}