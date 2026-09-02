using CarRenter.DB.DTOs.Auth;
using FluentValidation;

namespace CarRenter.Validators;

public class RegisterUserDtoValidator: AbstractValidator<RegisterDto>
{
    public RegisterUserDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required.");
        
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("The email address is not valid.");
        
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"[A-Z]").WithMessage("Password must contains at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
            .Matches(@"[\^$*.\[\]{}()?\-' '!@#%&/\\,><':;|_~`]").WithMessage("Password must contain at least one special character.");
        
        
        RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Confirm password is required.");
        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required.");
        
        RuleFor(x => x.DateOfBirth)
            .Must(BeAtLeast18YearsOld)
            .When(x => x.DateOfBirth.HasValue) 
            .WithMessage("You must be at least 18 years old to register.");

        RuleFor(x => x.DriverLicenseNumber).NotEmpty().WithMessage("Driver license number is required.")
            .MinimumLength(5).WithMessage("Driver license number must be at least 5 characters.");
         
        
        RuleFor(x => x.Address).NotNull().WithMessage("Address details are required.").SetValidator(new AddressDtoValidator());
        
        RuleFor(x => x.Password).Matches(x => x.ConfirmPassword).WithMessage("The password and confirmation password do not match.");
    }
    
    private static bool BeAtLeast18YearsOld(DateTime? dateOfBirth)
    {
        if (!dateOfBirth.HasValue) return true;
        
        var today = DateTime.UtcNow.Date;
        var minimumAgeDate = today.AddYears(-18);
    
        return dateOfBirth.Value.Date <= minimumAgeDate;
    }
}