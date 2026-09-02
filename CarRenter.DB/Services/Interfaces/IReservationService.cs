using CarRenter.DB.DTOs.Reservations;

namespace CarRenter.DB.Services.Interfaces;

public interface IReservationService
{
     Task<ReservationResponseDto> CreateReservationAsync(string userId, CreateReservationDto createReservationDto);
     Task<ReservationResponseDto?> GetReservationByIdAsync(string id);
     Task<IEnumerable<ReservationResponseDto>> GetReservationsByUserIdAsync(string userId);
     Task<bool> CancelReservationAsync(string id);
}