using CarRenter.DB.DTOs.Users;
using CarRenter.DB.Models;
using Microsoft.AspNetCore.Identity;

namespace CarRenter.DB.Services.Interfaces;

public interface IUserService
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<IdentityResult> RegisterUserAsync(User user, string password);
    Task<bool> IsDriverLicenseNumberExistsAsync(string driverLicenseNumber);
    Task<bool> CheckPasswordAsync(User user, string password);
    Task<IList<string>> GetUserRolesAsync(User user);
    public Task<bool> UpdateProfileAsync(string userId, UpdateProfileDto dto);
    public Task<UserProfileDto?> GetProfileAsync(string userId);


}