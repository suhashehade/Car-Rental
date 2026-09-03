using CarRenter.DB.Models;

namespace CarRenter.DB.Repositories.Interfaces;

public interface IReservationRepository: IGenericRepository<Reservation>
{
    Task<bool> HasCarOverlapAsync(string carId, DateTime startDate, DateTime endDate, string? currentReservationId = null);

    Task<bool> HasUserOverlapAsync(string userId, DateTime startDate, DateTime endDate,
        string? currentReservationId = null);

    Task<IEnumerable<Reservation>> GetReservationsWithDetailsByUserIdAsync(string userId);
    public Task<Reservation?> GetReservationByIdAndUserIdAsync(string id, string userId);
}