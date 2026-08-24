namespace CarRenter.DB.Models;

public class Preference
{
    public string? Id { get; set; } = Guid.NewGuid().ToString();
    public string PreferenceName { get; set; } = string.Empty;
    
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}