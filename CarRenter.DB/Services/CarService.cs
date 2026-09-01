using CarRenter.DB.DTOs;
using CarRenter.DB.DTOs.Cars;
using CarRenter.DB.Models;
using CarRenter.DB.Repositories.Interfaces;
using CarRenter.DB.Services.Interfaces;
using Microsoft.Identity.Client.NativeInterop;

namespace CarRenter.DB.Services;

public class CarService(IUnitOfWork unitOfWork) : ICarService
{
    public async Task<IEnumerable<AvailableCarDto>> GetAvailableCarsAsync()
    {
        var cars = await unitOfWork.Cars.GetAvailableCarsAsync();
        
        var carDtos = cars.Select(car => new AvailableCarDto
        {
            CarId = car.Id,
            Brand = car.Brand,
            Model = car.Model,
            Color = car.Color,
            HourlyPrice = car.HourlyPrice
        });

        return carDtos;
    }

    public async Task<AvailableCarDto?> GetCarByIdAsync(string carId)
    {
        var car = await unitOfWork.Cars.GetByIdAsync(carId);
        
        if (car == null) return null;
        
        var carDto = new AvailableCarDto()
        {
            CarId = carId,
            Brand = car.Brand,
            Model = car.Model,
            Color = car.Color,
            HourlyPrice = car.HourlyPrice
        };
        return carDto;
    }

    public async Task<AvailableCarDto> CreateCarAsync(CreateCarDto createCarDto)
    {
        var carEntity = new Car()
        {
            Brand = createCarDto.Brand,
            Model = createCarDto.Model,
            Color = createCarDto.Color,
            HourlyPrice = createCarDto.HourlyPrice
        };
        
        await unitOfWork.Cars.AddAsync(carEntity);
        await unitOfWork.CompleteAsync();
        
        var carDto = new AvailableCarDto()
        {
            CarId = carEntity.Id,
            Brand = carEntity.Brand,
            Model = carEntity.Model,
            Color = carEntity.Color,
            HourlyPrice = carEntity.HourlyPrice
        };
        return carDto;
    }

    public async Task<bool> UpdateCarAsync(string carId, UpdateCarDto updateCarDto)
    {
        var carEntity = await unitOfWork.Cars.GetByIdAsync(carId);
        
        if (carEntity == null) return false;
        
        carEntity.Brand = updateCarDto.Brand;
        carEntity.Model = updateCarDto.Model;
        carEntity.Color = updateCarDto.Color;
        carEntity.HourlyPrice = updateCarDto.HourlyPrice;
        
        unitOfWork.Cars.Update(carEntity);
        await unitOfWork.CompleteAsync();
    
        return true;
    }

    public async Task<bool> DeleteCarAsync(string carId)
    {
        var carEntity = await unitOfWork.Cars.GetByIdAsync(carId);
    
        if (carEntity == null) return false;
        
        unitOfWork.Cars.Delete(carEntity);
        await unitOfWork.CompleteAsync();
    
        return true;
    }
}
