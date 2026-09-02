using CarRenter.DB.Models;
using CarRenter.DB.Seed;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarRenter.DB;

public class CarRenterDbContext: IdentityDbContext<User>
{
   public CarRenterDbContext() { }
   
   public CarRenterDbContext(DbContextOptions<CarRenterDbContext> options)
      : base(options) { }
   
   public DbSet<Address> Addresses { get; set; }
   public DbSet<Car> Cars { get; set; }
   public DbSet<Reservation> Reservations { get; set; }
   public DbSet<Preference> Preferences { get; set; }
   
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
       modelBuilder.Entity<User>().ToTable("Users");
       
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
           .HasOne<Address>(u => u.Address)
           .WithOne(a => a.User)
           .HasForeignKey<Address>(a => a.UserId)
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
       
       
       modelBuilder.Seed();
   }
}
