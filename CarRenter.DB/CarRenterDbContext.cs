using CarRenter.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRenter.DB;

public class CarRenterDbContext: DbContext
{
   public CarRenterDbContext() { }
   
   public CarRenterDbContext(DbContextOptions<CarRenterDbContext> options)
      : base(options) { }
   
   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
   {
       if (!optionsBuilder.IsConfigured)
       {
           optionsBuilder.UseSqlServer("Server=.;Database=RentCarDB;Trusted_Connection=True;TrustServerCertificate=True;");
       }
   }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
       base.OnModelCreating(modelBuilder);
       modelBuilder.Entity<User>()
           .HasIndex(u => u.Email)
           .IsUnique();
       
       modelBuilder.Entity<User>()
           .HasIndex(u => u.DriverLicenseNumber)
           .IsUnique();
       
       modelBuilder.Entity<User>()
           .HasMany<Reservation>(u => u.Reservations)
           .WithOne(r => r.User)
           .HasForeignKey(r => r.UserId)
           .OnDelete(DeleteBehavior.Cascade);
       
       modelBuilder.Entity<User>()
           .HasMany<Address>(u => u.Addresses)
           .WithOne(a => a.User)
           .HasForeignKey(a => a.UserId)
           .OnDelete(DeleteBehavior.Cascade);

       modelBuilder.Entity<Reservation>()
           .HasMany<Preference>(r => r.Preferences)
           .WithMany(p => p.Reservations)
           .UsingEntity(j => j.ToTable("ReservationPreferences"));
       
       modelBuilder.Entity<Reservation>()
           .HasOne<Car>(r => r.Car)
           .WithMany(c => c.Reservations)
           .HasForeignKey(r => r.CarId)
           .OnDelete(DeleteBehavior.Restrict);
       
       modelBuilder.Entity<Car>()
           .Property(c => c.HourlyPrice)
           .HasColumnType("decimal(18,2)");

       modelBuilder.Entity<Reservation>()
           .Property(r => r.TotalPrice)
           .HasColumnType("decimal(18,2)");
       
   }
}
