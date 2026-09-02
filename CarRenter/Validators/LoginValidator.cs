using CarRenter.DB.DTOs.Auth;
using FluentValidation;

namespace CarRenter.Validators;

public class LoginValidator: AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("The email address is not valid.");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
    }
}