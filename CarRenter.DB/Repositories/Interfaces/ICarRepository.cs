using CarRenter.DB.Models;

namespace CarRenter.DB.Repositories.Interfaces;
public interface ICarRepository:IGenericRepository<Car>
{
    Task<IEnumerable<Car>> GetAvailableCarsAsync();
}