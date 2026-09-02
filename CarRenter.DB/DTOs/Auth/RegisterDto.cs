using CarRenter.DB.Models;

namespace CarRenter.DB.DTOs.Auth;

public class RegisterDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string DriverLicenseNumber { get; set; } = string.Empty;
    
    public AddressDto Address { get; set; } = new();
}