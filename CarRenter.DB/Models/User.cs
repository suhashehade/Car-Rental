namespace CarRenter.DB.Models;

public class User
{
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string DriverLicenseNumber { get; set; } = string.Empty;

        public ICollection<Address>? Addresses { get; set; } = new List<Address>();
        public ICollection<Reservation>? Reservations { get; set; } = new List<Reservation>();
}