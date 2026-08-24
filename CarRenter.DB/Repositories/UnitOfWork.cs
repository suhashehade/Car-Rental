using CarRenter.DB.Repositories.Interfaces;

namespace CarRenter.DB.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly CarRenterDbContext _context;

    public ICarRepository Cars { get; }
    public IReservationRepository Reservations { get; }
    public IAddressRepository Addresses { get; }
    public IPreferenceRepository Preferences { get; }

    public UnitOfWork(CarRenterDbContext context)
    {
        _context = context;
        Cars = new CarRepository(_context);
        Reservations = new ReservationRepository(_context);
        Addresses = new AddressRepository(_context);
        Preferences = new PreferenceRepository(_context);
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}