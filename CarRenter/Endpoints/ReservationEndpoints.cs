namespace CarRenter.Endpoints;

public static class ReservationEndpoints
{
    public static void MapReservationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/reservations").RequireAuthorization();
        
        group.MapGet("/", () => "Hi Registers!");
    }
}