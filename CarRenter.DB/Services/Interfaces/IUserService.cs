using CarRenter.DB.Models;
using Microsoft.AspNetCore.Identity;

namespace CarRenter.DB.Services.Interfaces;

public interface IUserService
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<IdentityResult> RegisterUserAsync(User user, string password);
    Task<bool> CheckPasswordAsync(User user, string password);
    Task<IList<string>> GetUserRolesAsync(User user);
}