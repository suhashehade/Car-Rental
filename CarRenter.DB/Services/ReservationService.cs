using CarRenter.DB.DTOs.Reservations;
using CarRenter.DB.Models;
using CarRenter.DB.Repositories.Interfaces;
using CarRenter.DB.Services.Interfaces;
using CarRenter.DB.Validators;

namespace CarRenter.DB.Services;

public class ReservationService(IUnitOfWork unitOfWork, IReservationValidator validator)
    : IReservationService
{
    private async Task<Car?> GetCarAsync(string carId)
    {
        return await unitOfWork.Cars.GetByIdAsync(carId);
    }
    
    public async Task<ReservationResponseDto> CreateReservationAsync(string userId, CreateReservationDto createReservationDto)
    {
        await validator.ValidateReservationAsync(userId, createReservationDto.CarId, createReservationDto.StartDate, createReservationDto.EndDate);
        
        var car = await GetCarAsync(createReservationDto.CarId);
        var totalPrice = Utils.Utils.CalculateTotalPrice(createReservationDto.StartDate, createReservationDto.EndDate, car!);
        
        var reservationEntity = new Reservation
        {
            CarId = createReservationDto.CarId,
            StartDate = createReservationDto.StartDate,
            EndDate = createReservationDto.EndDate,
            UserId = userId,
            Location = createReservationDto.Location,
            TotalPrice = totalPrice
        };
        
        await unitOfWork.Reservations.AddAsync(reservationEntity);
        await unitOfWork.CompleteAsync();
        
        return new ReservationResponseDto
        {
            ReservationId = reservationEntity.Id,
            StartDate = reservationEntity.StartDate,
            EndDate = reservationEntity.EndDate,
            CarName = $"{car?.Brand} {car?.Model}",
            TotalPrice = reservationEntity.TotalPrice,
            Location = reservationEntity.Location
        };
    }

    public async Task<ReservationResponseDto?> GetReservationByIdAsync(string id)
    {
        var reservationEntity = await unitOfWork.Reservations.GetByIdAsync(id);
        if (reservationEntity == null) return null;

        var car = await GetCarAsync(reservationEntity.CarId);

        return new ReservationResponseDto
        {
            ReservationId = reservationEntity.Id,
            StartDate = reservationEntity.StartDate,
            EndDate = reservationEntity.EndDate,
            CarName = car != null ? $"{car.Brand} {car.Model}" : "N/A",
            TotalPrice = reservationEntity.TotalPrice,
            Location = reservationEntity.Location
        };
    }

    public async Task<IEnumerable<ReservationResponseDto>> GetReservationsByUserIdAsync(string userId)
    {
        var reservations = await unitOfWork.Reservations.GetReservationsWithDetailsByUserIdAsync(userId);

        return reservations.Select(reservation => new ReservationResponseDto
        {
            ReservationId = reservation.Id,
            StartDate = reservation.StartDate,
            EndDate = reservation.EndDate,
            CarName = reservation?.Car != null ? $"{reservation?.Car.Brand} {reservation?.Car.Model}" : "N/A",
            TotalPrice = reservation?.TotalPrice ?? 0,
            Location = reservation?.Location ?? "N/A"
        }).ToList();
    }

    public async Task<bool> CancelReservationAsync(string id, string userId)
    {
        var reservation = await unitOfWork.Reservations.GetReservationByIdAndUserIdAsync(id, userId);

        if (reservation == null)
        {
            throw new KeyNotFoundException("Reservation not found or you do not have permission to modify it.");
        }

        unitOfWork.Reservations.Delete(reservation);
        await unitOfWork.CompleteAsync();
        return true;
    }

    public async Task<bool> UpdateReservationAsync(string reservationId, string userId, UpdateReservationDto updateReservationDto)
    {
        var reservation = await unitOfWork.Reservations.GetReservationByIdAndUserIdAsync(reservationId, userId);
        if (reservation == null)
        {
            throw new KeyNotFoundException("Reservation not found or you do not have permission to modify it.");
        }
        
        await validator.ValidateReservationAsync(userId, updateReservationDto.CarId, updateReservationDto.StartDate, updateReservationDto.EndDate, reservationId);
        
        var car = await GetCarAsync(updateReservationDto.CarId);
        var totalPrice = Utils.Utils.CalculateTotalPrice(updateReservationDto.StartDate, updateReservationDto.EndDate, car!);
        
        reservation.StartDate = updateReservationDto.StartDate;
        reservation.EndDate = updateReservationDto.EndDate;
        reservation.Location = updateReservationDto.Location;
        reservation.CarId = updateReservationDto.CarId;
        reservation.TotalPrice = totalPrice;
        
        unitOfWork.Reservations.Update(reservation);
        await unitOfWork.CompleteAsync();
    
        return true;
    }
}