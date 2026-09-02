using CarRenter.DB.DTOs.Cars;
using CarRenter.DB.Models;

namespace CarRenter.DB.Repositories.Interfaces;
public interface ICarRepository:IGenericRepository<Car>
{
    Task<IEnumerable<Car>> GetAvailableCarsAsync();
    Task<IEnumerable<CarResponseDto>> SearchCarsAsync(CarSearchFilterDto filter);
}