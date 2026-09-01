using CarRenter.DB.DTOs.Reservations;
using CarRenter.DB.Models;
using CarRenter.DB.Repositories.Interfaces;
using CarRenter.DB.Services.Interfaces;

namespace CarRenter.DB.Services;

public class ReservationService : IReservationService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReservationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    private async Task<bool> HasUserOverlapAsync(string userId, DateTime startDate, DateTime endDate, string? currentReservationId = null)
    {
        return await _unitOfWork.Reservations.HasUserOverlapAsync(userId, startDate, endDate, currentReservationId);
    }

    private async Task<bool> HasCarOverlapAsync(string carId, DateTime startDate, DateTime endDate, string? currentReservationId = null)
    {
        return await _unitOfWork.Reservations.HasCarOverlapAsync(carId, startDate, endDate, currentReservationId);
    }

    private decimal CalculateTotalPrice(DateTime startDate, DateTime endDate, Car car)
    {
        var totalHours = (decimal)(endDate - startDate).TotalHours;
        if (totalHours <= 0)
        {
            // ❌ ArgumentException لأن المشكلة بالمدخلات (التواريخ)
            throw new ArgumentException("The end date must be greater than start date.");
        }

        return totalHours * car.HourlyPrice; 
    }

    private async Task<Car?> GetCar(string carId)
    {
        return await _unitOfWork.Cars.GetByIdAsync(carId);
    }
    
    public async Task<ReservationResponseDto> CreateReservationAsync(CreateReservationDto createReservationDto)
    {
        var car = await GetCar(createReservationDto.CarId);
        if (car == null)      
        {
            // ❌ KeyNotFoundException للتعبير عن عدم وجود العنصر (404)
            throw new KeyNotFoundException($"Car with ID '{createReservationDto.CarId}' was not found.");
        }

        bool isUserReserve = await HasUserOverlapAsync(createReservationDto.UserId, createReservationDto.StartDate, createReservationDto.EndDate);
        bool isCarReserved = await HasCarOverlapAsync(createReservationDto.CarId, createReservationDto.StartDate, createReservationDto.EndDate);
        
        if (isUserReserve)
        {
            // ❌ InvalidOperationException لتعارض قواعد العمل (400/409)
            throw new InvalidOperationException("You already have a conflicting reservation during this period.");
        }

        if (isCarReserved)
        {
            throw new InvalidOperationException("The car is already reserved during this period.");
        }

        var totalPrice = CalculateTotalPrice(createReservationDto.StartDate, createReservationDto.EndDate, car);
        
        var reservationEntity = new Reservation()
        {
            CarId = createReservationDto.CarId,
            StartDate = createReservationDto.StartDate,
            EndDate = createReservationDto.EndDate,
            UserId = createReservationDto.UserId,
            Location = createReservationDto.Location,
            TotalPrice = totalPrice,
        };
        
        await _unitOfWork.Reservations.AddAsync(reservationEntity);
        await _unitOfWork.CompleteAsync();
        
        return new ReservationResponseDto()
        {
            ReservationId = reservationEntity.Id,
            StartDate = reservationEntity.StartDate,
            EndDate = reservationEntity.EndDate,
            CarName = $"{car.Brand} {car.Model}",
            TotalPrice = reservationEntity.TotalPrice,
        };
    }

    public async Task<ReservationResponseDto?> GetReservationByIdAsync(string id)
    {
        var reservationEntity = await _unitOfWork.Reservations.GetByIdAsync(id);
        if (reservationEntity == null) return null;

        var car = await GetCar(reservationEntity.CarId);

        return new ReservationResponseDto()
        {
            ReservationId = reservationEntity.Id,
            StartDate = reservationEntity.StartDate,
            EndDate = reservationEntity.EndDate,
            CarName = car != null ? $"{car.Brand} {car.Model}" : "N/A",
            TotalPrice = reservationEntity.TotalPrice,
        };
    }

    public async Task<IEnumerable<ReservationResponseDto>> GetReservationsByUserIdAsync(string userId)
    {
        var reservations = await _unitOfWork.Reservations.GetReservationsWithDetailsByUserIdAsync(userId);

        var enumerable = reservations as Reservation[] ?? reservations.ToArray();
        if (enumerable.Length == 0)
        {
            return [];
        }
    
        return enumerable.Select(reservation => new ReservationResponseDto()
        {
            ReservationId = reservation.Id,
            StartDate = reservation.StartDate,
            EndDate = reservation.EndDate,
            CarName = $"{reservation.Car.Brand} {reservation.Car.Model}",
            TotalPrice = reservation.TotalPrice,
        }).ToList();
    }

    public async Task<bool> CancelReservationAsync(string id)
    {
        var reservationEntity = await _unitOfWork.Reservations.GetByIdAsync(id);
        if (reservationEntity == null) return false;
        
        _unitOfWork.Reservations.Delete(reservationEntity);
        await _unitOfWork.CompleteAsync();
        return true;
    }

    public async Task<bool> UpdateReservationAsync(string reservationId, UpdateReservationDto updateReservationDto)
    {
        var reservationEntity = await _unitOfWork.Reservations.GetByIdAsync(reservationId);
        if (reservationEntity == null) return false;

        var car = await GetCar(updateReservationDto.CarId);
        if (car == null)      
        {
            throw new KeyNotFoundException($"Car with ID '{updateReservationDto.CarId}' was not found.");
        }
        
        bool isUserReserve = await HasUserOverlapAsync(reservationEntity.UserId, updateReservationDto.StartDate, updateReservationDto.EndDate, reservationId);
        bool isCarReserved = await HasCarOverlapAsync(updateReservationDto.CarId, updateReservationDto.StartDate, updateReservationDto.EndDate, reservationId);

        if (isUserReserve || isCarReserved)
        {
            throw new InvalidOperationException("Reservation dates conflict with an existing booking.");
        }

        var totalPrice = CalculateTotalPrice(updateReservationDto.StartDate, updateReservationDto.EndDate, car);
        
        reservationEntity.StartDate = updateReservationDto.StartDate;
        reservationEntity.EndDate = updateReservationDto.EndDate;
        reservationEntity.Location = updateReservationDto.Location;
        reservationEntity.CarId = updateReservationDto.CarId;
        reservationEntity.TotalPrice = totalPrice;
        
        _unitOfWork.Reservations.Update(reservationEntity);
        await _unitOfWork.CompleteAsync();
    
        return true;
    }
}