using CarRenter.DB.DTOs.Reservations;

namespace CarRenter.DB.Services.Interfaces;

public interface IReservationService
{
     Task<ReservationResponseDto> CreateReservationAsync(string userId, CreateReservationDto createReservationDto);
     Task<ReservationResponseDto?> GetReservationByIdAsync(string id);
     public Task<bool> UpdateReservationAsync(string reservationId, string userId, UpdateReservationDto updateReservationDto);
     Task<IEnumerable<ReservationResponseDto>> GetReservationsByUserIdAsync(string userId);
     Task<bool> CancelReservationAsync(string id, string userId);
}