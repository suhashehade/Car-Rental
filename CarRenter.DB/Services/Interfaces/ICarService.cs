using CarRenter.DB.DTOs.Cars;

namespace CarRenter.DB.Services.Interfaces;

public interface ICarService
{
    Task<IEnumerable<AvailableCarDto>> GetAvailableCarsAsync();
    Task<AvailableCarDto?> GetCarByIdAsync(string carId);
    Task<AvailableCarDto> CreateCarAsync(CreateCarDto createCarDto);
    Task<bool> UpdateCarAsync(string carId, UpdateCarDto updateCarDto);
    Task<bool> DeleteCarAsync(string carId);
}