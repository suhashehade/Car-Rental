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
   }
}
