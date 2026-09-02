using CarRenter.DB.DTOs.Cars;
using CarRenter.DB.Models;
using CarRenter.DB.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace CarRenter.DB.Repositories;

public class CarRepository(CarRenterDbContext context) : GenericRepository<Car>(context), ICarRepository

{
    public async Task<IEnumerable<Car>> GetAvailableCarsAsync()
    {
        return await _context.Cars
            .Where(c => !c.Reservations.Any(r => r.EndDate > DateTime.UtcNow))
            .ToListAsync();
    }

    public async Task<IEnumerable<CarResponseDto>> SearchCarsAsync(CarSearchFilterDto filter)
    {
       IQueryable<Car> carsQuery = _context.Cars;

        if (!string.IsNullOrWhiteSpace(filter.Brand))
            carsQuery = carsQuery.Where(c => c.Brand.Contains(filter.Brand));

        if (!string.IsNullOrWhiteSpace(filter.Model))
            carsQuery = carsQuery.Where(c => c.Model.Contains(filter.Model));

        if (!string.IsNullOrWhiteSpace(filter.Color))
            carsQuery = carsQuery.Where(c => c.Color.Contains(filter.Color));

        if (filter.HourlyPrice.HasValue && filter.HourlyPrice.Value > 0)
            carsQuery = carsQuery.Where(c => c.HourlyPrice <= filter.HourlyPrice.Value);
      

       
        if (!string.IsNullOrWhiteSpace(filter.Location))
        {
            carsQuery = carsQuery.Where(c =>
                !c.Reservations.Any() ||
                c.Reservations
                 .OrderByDescending(r => r.EndDate)
                 .FirstOrDefault()!.Location.Contains(filter.Location));
        }

        DateTime start;
        DateTime end;
        if (filter.StartDate.HasValue && filter.EndDate.HasValue)
        {
            start = filter.StartDate.Value;
            end = filter.EndDate.Value;
        }
        else if (filter.StartDate.HasValue)
        {
            start = filter.StartDate.Value;
            end = filter.StartDate.Value.AddHours(1); 
        }
        else if (filter.EndDate.HasValue)
        {
            end = filter.EndDate.Value;
            start = filter.EndDate.Value.AddHours(-1); 
        }
        else
        {
            end = DateTime.UtcNow.AddHours(1);
        }
        var responseQuery = Queryable.Select(carsQuery, c => new CarResponseDto
        {
            Id = c.Id,
            Brand = c.Brand,
            Model = c.Model,
            Color = c.Color,
            HourlyPrice = c.HourlyPrice,

            Location = c.Reservations
                .OrderByDescending(r => r.EndDate)
                .Select(r => r.Location)
                .FirstOrDefault() ?? "Main Branch",

            IsAvailable = !(filter.StartDate.HasValue && filter.EndDate.HasValue && c.Reservations.Any(r =>
                (filter.StartDate.Value >= r.StartDate && filter.StartDate.Value < r.EndDate) ||
                (filter.EndDate.Value > r.StartDate && filter.EndDate.Value <= r.EndDate) ||
                (filter.StartDate.Value <= r.StartDate && filter.EndDate.Value >= r.EndDate)
            ))
        });

        if (filter.IsAvailable.HasValue && filter.IsAvailable.Value)
        {
            responseQuery = responseQuery.Where(dto => dto.IsAvailable);
        }

        return await responseQuery.ToListAsync();
        
    }
}
