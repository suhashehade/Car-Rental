using CarRenter.DB.Models;
using CarRenter.DB.Repositories.Interfaces;

namespace CarRenter.DB.Repositories;

public class PreferenceRepository(CarRenterDbContext context)
    : GenericRepository<Preference>(context), IPreferenceRepository;