using CarRenter.DB.Models;

namespace CarRenter.DB.Validators;

public interface IReservationValidator
{
   public Task ValidateReservationAsync(string userId, string carId, DateTime startDate, DateTime endDate, string? reservationId = null);
}