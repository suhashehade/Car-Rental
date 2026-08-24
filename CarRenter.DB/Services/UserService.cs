using CarRenter.DB.Models;
using Microsoft.AspNetCore.Identity;

namespace CarRenter.DB.Services;

public class UserService
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
}