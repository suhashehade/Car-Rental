using CarRenter.DB.Models;
using CarRenter.DB.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRenter.DB.Repositories;

public class CarRepository(CarRenterDbContext context) : GenericRepository<Car>(context), ICarRepository

{
    public async Task<IEnumerable<Car>> GetAvailableCarsAsync()
    {
        return await _context.Cars
            .Where(c => !c.Reservations.Any(r => r.EndDate > DateTime.UtcNow))
            .ToListAsync();
    }
}
