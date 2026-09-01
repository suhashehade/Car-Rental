using Microsoft.EntityFrameworkCore; 
using CarRenter.DB.Models;
using CarRenter.DB.Repositories.Interfaces;

namespace CarRenter.DB.Repositories;

public class ReservationRepository(CarRenterDbContext context)
    : GenericRepository<Reservation>(context), IReservationRepository
{

    public async Task<IEnumerable<Reservation>> GetReservationsWithDetailsByUserIdAsync(string userId)
    {
        return await _context.Reservations
            .Include(r => r.Car)
            .Include(r => r.Preferences)
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }

    
    public async Task<bool> HasUserOverlapAsync(string userId, DateTime startDate, DateTime endDate, string? currentReservationId = null)
    {
        return await _context.Reservations
            .AnyAsync(r => r.UserId == userId &&
                           r.Id != currentReservationId &&
                           startDate < r.EndDate && endDate > r.StartDate);
    }

    public async Task<bool> HasCarOverlapAsync(string carId, DateTime startDate, DateTime endDate, string? currentReservationId = null)
    {
        return await _context.Reservations
            .AnyAsync(r => r.CarId == carId &&
                           r.Id != currentReservationId &&
                           startDate < r.EndDate && endDate > r.StartDate);
    }
}