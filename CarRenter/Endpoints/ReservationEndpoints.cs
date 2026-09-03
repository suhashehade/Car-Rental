using System.Security.Claims;
using CarRenter.DB.DTOs.Reservations;
using CarRenter.DB.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRenter.Endpoints;

public static class ReservationEndpoints
{
    public static void MapReservationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/reservations").RequireAuthorization();
        
        group.MapGet("/", () => "Hi Registers!").RequireAuthorization(new AuthorizeAttribute{ Roles = "Admin" });
        group.MapPost("/", CreateReservation);
        group.MapGet("/user-reservations", GetReservationByUserId);
        group.MapDelete("/{id}", CancelReservation);
    }

    private static async Task<IResult> CreateReservation(
        ClaimsPrincipal user, 
        CreateReservationDto dto, 
        IReservationService reservationService, 
        IValidator<CreateReservationDto> validator)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                         ?? user.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }

            var validationResult = await validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                return Results.BadRequest(new { Errors = errors });
            }

            var result = await reservationService.CreateReservationAsync(userId, dto);
            return Results.Ok(result);
            
    }
    
    private static async Task<IResult> GetReservationByUserId(
        ClaimsPrincipal user, 
        IReservationService reservationService)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                     ?? user.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Results.Unauthorized();
        }

        var reservations = await reservationService.GetReservationsByUserIdAsync(userId);
        var result = reservations.ToList();
        return Results.Ok(new {
            data = result,
            count = result.Count
        });
            
    }
    
    
    private static async Task<IResult> CancelReservation(
        string id, 
        ClaimsPrincipal user,
        IReservationService reservationService)
    {
        
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                     ?? user.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Results.Unauthorized();
        }
        var result = await reservationService.CancelReservationAsync(id, userId);
        return Results.Ok(new {
            data = result
        });
    }
}