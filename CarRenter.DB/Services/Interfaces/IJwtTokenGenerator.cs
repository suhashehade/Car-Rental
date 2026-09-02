using CarRenter.DB.Models;

namespace CarRenter.DB.Services.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user, IList<string> roles);
    Task<bool> ValidateToken(string token);
}