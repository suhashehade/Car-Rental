using CarRenter.DB.DTOs.Cars;

namespace CarRenter.DB.Services.Interfaces;

public interface ICarService
{
    Task<IEnumerable<CarDto>> GetAvailableCarsAsync();
    Task<IEnumerable<CarResponseDto>> SearchAvailableCarsAsync(CarSearchFilterDto filter);
    Task<CarDto?> GetCarByIdAsync(string carId);
    Task<CarDto> CreateCarAsync(CreateCarDto createCarDto);
    Task<bool> UpdateCarAsync(string carId, CarDto updateCarDto);
    Task<bool> DeleteCarAsync(string carId);
}