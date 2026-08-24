using CarRenter.DB.Models;
using CarRenter.DB.Repositories.Interfaces;

namespace CarRenter.DB.Repositories;

public class CarRepository(CarRenterDbContext context) : GenericRepository<Car>(context), ICarRepository;