using CarRenter.DB.DTOs.Reservations;
using FluentValidation;

namespace CarRenter.Validators;

public class CreateReservationDtoValidator: AbstractValidator<CreateReservationDto>
{
    public CreateReservationDtoValidator()
    {
        RuleFor(x => x.CarId).NotEmpty().WithMessage("Car ID is required.");
        RuleFor(x => x.StartDate).NotEmpty().WithMessage("Start date is required.");
        RuleFor(x => x.EndDate).NotEmpty().WithMessage("End date is required.");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User is required.");
        RuleFor(x => x.Location).NotEmpty().WithMessage("Location is required.");
        RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.");
    }
}