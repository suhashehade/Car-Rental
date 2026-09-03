using CarRenter.DB.DTOs.Auth;

namespace CarRenter.DB.DTOs.Users;

public class UserProfileDto
{
    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;
    
    public AddressDto? Address { get; set; } = new();
}