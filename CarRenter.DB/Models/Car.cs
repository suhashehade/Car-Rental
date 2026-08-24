namespace CarRenter.DB.Models;

public class Car
{
    public string? Id { get; set; } = Guid.NewGuid().ToString();
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public decimal HourlyPrice { get; set; }
    
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}