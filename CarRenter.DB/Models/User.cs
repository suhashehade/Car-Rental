using Microsoft.AspNetCore.Identity;
namespace CarRenter.DB.Models;

public class User: IdentityUser
{
      
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string DriverLicenseNumber { get; set; } = string.Empty;

        public ICollection<Address>? Addresses { get; set; } = new List<Address>();
        public ICollection<Reservation>? Reservations { get; set; } = new List<Reservation>();
}