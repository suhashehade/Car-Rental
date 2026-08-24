namespace CarRenter.DB.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICarRepository Cars { get; }
    IReservationRepository Reservations { get; }
    IAddressRepository Addresses { get; }
    IPreferenceRepository Preferences { get; }
    
    Task<int> CompleteAsync();
}