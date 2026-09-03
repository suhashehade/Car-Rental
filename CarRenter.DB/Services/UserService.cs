
using CarRenter.DB.DTOs.Auth;
using CarRenter.DB.DTOs.Users;
using CarRenter.DB.Models;
using CarRenter.DB.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarRenter.DB.Services;

public class UserService: IUserService
{
    private readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<IdentityResult> RegisterUserAsync(User user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<bool> IsDriverLicenseNumberExistsAsync(string driverLicenseNumber)
    {
        return await _userManager.Users.AnyAsync(u => u.DriverLicenseNumber == driverLicenseNumber);
    }

    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<IList<string>> GetUserRolesAsync(User user)
    {
        return await _userManager.GetRolesAsync(user);
    }
    
    public async Task<UserProfileDto?> GetProfileAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return null;

        return new UserProfileDto()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address != null ? new AddressDto()
            {
                City = user.Address.City,
                Country = user.Address.Country,
                AddressLine1 = user.Address.AddressLine1,
                AddressLine2 = user.Address.AddressLine2
            } : null
        };
    }

    public async Task<bool> UpdateProfileAsync(string userId, UpdateProfileDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.PhoneNumber = dto.PhoneNumber;
        if (dto.Address != null)
            user.Address = new Address()
            {
                City = dto.Address.City,
                Country = dto.Address.Country,
                AddressLine1 = dto.Address.AddressLine1,
                AddressLine2 = dto.Address.AddressLine2
            };

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }
}