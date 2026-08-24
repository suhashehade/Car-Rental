namespace CarRenter.DB.Models;

public class Address
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string AddressLine1 { get; set; } = string.Empty;
    public string AddressLine2 { get; set; } = string.Empty;

    public User User { get; set; } = null!;
    public string UserId { get; set; }  = string.Empty;
}