using CarRenter.DB.DTOs.Auth;
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
            return Results.BadRequest(new { Message = "Invalid email or password"});
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
}