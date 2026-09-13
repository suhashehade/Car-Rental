using CarRenter.DB.Repositories.Interfaces;

namespace CarRenter.DB.Validators;

public class ReservationValidator: IReservationValidator
{
    private readonly IUnitOfWork _unitOfWork;

    public ReservationValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task ValidateReservationAsync(string userId, string carId, DateTime startDate, DateTime endDate,
        string? reservationId = null)
    {
        if (endDate <= startDate)
        {
            throw new ArgumentException("End date must be greater than start date.");
        }
        
        if (reservationId == null && startDate < DateTime.UtcNow.AddMinutes(-5))
        {
            throw new ArgumentException("Start date cannot be in the past.");
        }
        
        var car = await _unitOfWork.Cars.GetByIdAsync(carId);
        if (car == null)
        {
            throw new KeyNotFoundException($"Car with ID '{carId}' was not found.");
        }
        
        bool isUserReserve = await _unitOfWork.Reservations.HasUserOverlapAsync(userId, startDate, endDate, reservationId);
        if (isUserReserve)
        {
            throw new InvalidOperationException("You already have a conflicting reservation during this period.");
        }

        bool isCarReserved = await _unitOfWork.Reservations.HasCarOverlapAsync(carId, startDate, endDate, reservationId);
        if (isCarReserved)
        {
            throw new InvalidOperationException("The car is already reserved during this period.");
        }
    }
}