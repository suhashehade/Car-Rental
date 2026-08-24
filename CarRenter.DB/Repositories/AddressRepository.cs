using CarRenter.DB.Models;
using CarRenter.DB.Repositories.Interfaces;

namespace CarRenter.DB.Repositories;

public class AddressRepository(CarRenterDbContext context) : GenericRepository<Address>(context), IAddressRepository;