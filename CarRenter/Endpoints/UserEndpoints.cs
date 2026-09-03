using System.Security.Claims;
using CarRenter.DB.DTOs.Auth;
using CarRenter.DB.DTOs.Users;
using CarRenter.DB.Models;
using CarRenter.DB.Services.Interfaces;
using FluentValidation;

namespace CarRenter.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users");
        
        group.MapGet("/", () => "Hi Users!");
        group.MapPost("/register", RegisterUser);
        group.MapPost("/login", LoginUser);
        group.MapGet("/profile", GetProfileAsync).RequireAuthorization();
        group.MapPut("/profile", UpdateProfileAsync).RequireAuthorization();
    }
    
    private static async Task<IResult> RegisterUser(RegisterDto dto, IUserService userService, IValidator<RegisterDto> validator)
    {
        var validationResult = await validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Results.BadRequest(new { Errors = errors });
        }
        
        var existingUser = await userService.GetUserByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            return Results.Conflict(new { Message = "This user is already exist"});
        }

        if (!string.IsNullOrEmpty(dto.DriverLicenseNumber))
        {
            var isLicenseExists = await userService.IsDriverLicenseNumberExistsAsync(dto.DriverLicenseNumber);
            if (isLicenseExists)
            {
                return Results.Conflict(new { Message = "This user is already registered with the same driver licence number"});
            }
        }
        
        var newUser = new User()
        {
            UserName = dto.Email,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            DateOfBirth = dto.DateOfBirth ?? default,
            DriverLicenseNumber = dto.DriverLicenseNumber,
            PhoneNumber = dto.PhoneNumber,
            Address = new Address()
                {
                    City = dto.Address.City,
                    Country = dto.Address.Country,
                    AddressLine1 = dto.Address.AddressLine1,
                    AddressLine2 = dto.Address.AddressLine2 ?? string.Empty,
                },
        };
        
        var result = await userService.RegisterUserAsync(newUser, dto.Password);
        return !result.Succeeded ? Results.BadRequest(new { Errors = result.Errors.Select(e => e.Description) }) 
            : Results.Ok(new { Message = "Successfully registered" });
    }

    private static async Task<IResult> LoginUser(LoginDto dto, IUserService userService, IValidator<LoginDto> validator, IJwtTokenGenerator tokenGenerator)
    {
        var validationResult = await validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Results.BadRequest(new { Errors = errors });
        }
        
        var user = await userService.GetUserByEmailAsync(dto.Email);
        if (user == null)
        {
            var errors = new List<string> { "Invalid email or password" };
            return Results.BadRequest(errors.Select(e => new { Error = e }));
        }
        
        var isPasswordValid = await userService.CheckPasswordAsync(user, dto.Password);
        if (!isPasswordValid)
        {
            return Results.Unauthorized(); 
        }
        
        var userRoles = await userService.GetUserRolesAsync(user);
        var token = tokenGenerator.GenerateToken(
            user,
            userRoles
        );

        return Results.Ok(new
        {
            token
        });
    }

    private static async Task<IResult> GetProfileAsync(ClaimsPrincipal user, IUserService userService)
    {
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Results.Unauthorized();

            var profile = await userService.GetProfileAsync(userId);
            if (profile != null) return Results.Ok(profile);
            var errors = new List<string> { "User not found" };
            return Results.NotFound(errors.Select(e => new { Error = e }));
        }
    }
    
    private static async Task<IResult> UpdateProfileAsync (
        ClaimsPrincipal user,
        UpdateProfileDto dto,
        IValidator<UpdateProfileDto> validator,
        IUserService userService) 
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Results.Unauthorized();


        var validationResult = await validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var updated = await userService.UpdateProfileAsync(userId, dto);
        if (updated) return Results.Ok(new { Message = "Profile updated successfully." });
        var errors = new List<string> { "Failed to update profile." };
        return Results.NotFound(errors.Select(e => new { Error = e }));
    }
}