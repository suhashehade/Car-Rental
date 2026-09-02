using CarRenter.DB.DTOs.Cars;
using FluentValidation;

namespace CarRenter.Validators;

public class CarSearchFilterDtoValidator: AbstractValidator<CarSearchFilterDto>
{
    public CarSearchFilterDtoValidator()
    {
        When(x => x.StartDate.HasValue, () =>
        {
            RuleFor(x => x.StartDate!.Value)
                .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
                .WithMessage("Start date must be today or in the future.");
        });
        
        When(x => x.EndDate.HasValue, () =>
        {
            RuleFor(x => x.EndDate!.Value)
                .GreaterThan(DateTime.UtcNow.Date)
                .WithMessage("End date must be in the future.");
        });
        
        When(x => x.StartDate.HasValue && x.EndDate.HasValue, () =>
        {
            RuleFor(x => x.EndDate!.Value)
                .GreaterThan(x => x.StartDate!.Value)
                .WithMessage("End date must be after the start date.");
        });
        
        When(x => x.HourlyPrice.HasValue, () =>
        {
            RuleFor(x => x.HourlyPrice!.Value)
                .GreaterThan(0)
                .WithMessage("Hourly price must be greater than zero.");
        });
    }
}