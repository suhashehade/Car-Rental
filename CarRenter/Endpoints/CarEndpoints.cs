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
        var group = app.MapGroup("/api/cars").RequireAuthorization();
        
        group.MapGet("/", () => "Hi Cars!"); 
        group.MapGet("/available", GetAvailableCars);
        group.MapGet("/search", SearchCars);

        
        var adminGroup = group.MapGroup("/")
            .RequireAuthorization(r => r.RequireRole("Admin"));

        adminGroup.MapPost("/", CreateCar);
        adminGroup.MapPut("/{id}", UpdateCar);
        adminGroup.MapDelete("/{id}", DeleteCar);

    }

    private static async Task<IResult> GetAvailableCars(ICarService carService)
    {
        var availableCars = await carService.GetAvailableCarsAsync();
        var result = availableCars.ToList();
        return Results.Ok(new {data = result, count = result.Count });
    }

    private static Task UpdateCar(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static Task CreateCar(HttpContext context)
    {
        throw new NotImplementedException();
    }

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

    private static Task  DeleteCar(HttpContext context)
    {
        throw new NotImplementedException();
    }
}