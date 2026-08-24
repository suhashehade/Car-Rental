namespace CarRenter.DB.Models;

public class Reservation
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }

    public Car Car { get; set; } = null!;
    public string CarId  { get; set; } = string.Empty;
    
    public User User { get; set; } = null!;
    public string UserId { get; set; } = string.Empty;
    
    public ICollection<Preference> ? Preferences { get; set; } = new List<Preference>();
}