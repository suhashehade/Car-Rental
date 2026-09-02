using CarRenter.DB.DTOs.Auth;
using FluentValidation;

namespace CarRenter.Validators;

public class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(x => x.AddressLine1)
            .NotEmpty().WithMessage("Address Line 1 is required.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required.");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required.");
        
    }
}