using CarRenter.DB.DTOs.Cars;
using CarRenter.DB.Models;
using CarRenter.DB.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace CarRenter.Endpoints;

public static class CarEndpoints
{
    public static void MapCarEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/cars");
        
        group.MapGet("/",[Authorize] () => "Hi Cars!"); 
        group.MapGet("/available", GetAvailableCars);
        group.MapGet("/search", SearchCars);

        
        var adminGroup = group.MapGroup("/");

        adminGroup.MapPost("/", CreateCar);
        adminGroup.MapPut("/{id}", UpdateCar);
        adminGroup.MapDelete("/{id}", DeleteCar);

    }
    [Authorize]
    private static async Task<IResult> GetAvailableCars(ICarService carService)
    {
        var availableCars = await carService.GetAvailableCarsAsync();
        var result = availableCars.ToList();
        return Results.Ok(new {data = result, count = result.Count });
    }
    [Authorize(Roles = "Admin")]
    private static Task UpdateCar(HttpContext context)
    {
        throw new NotImplementedException();
    }
    [Authorize(Roles = "Admin")]
    private static Task CreateCar(HttpContext context)
    {
        throw new NotImplementedException();
    }
    [Authorize]
    private static async Task<IResult> SearchCars([AsParameters]CarSearchFilterDto filter, ICarService carService, IValidator<CarSearchFilterDto> validator)
    {
        var validationResult = await validator.ValidateAsync(filter);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Results.BadRequest(new { Errors = errors });
        }
        var response = await carService.SearchAvailableCarsAsync(filter);
        var result = response.ToList();
        return Results.Ok(new {data = result, count = result.Count});
    }
    [Authorize(Roles = "Admin")]
    private static Task  DeleteCar(HttpContext context)
    {
        throw new NotImplementedException();
    }
}