using FluentValidation;

namespace AuthorizationAPI.DTOs.Validators
{
    internal sealed class RegValidator : AbstractValidator<RegRequest>
    {
        public RegValidator()
        {
            RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .MaximumLength(50).WithMessage("Email must not exceed 50 characters")
            .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password)
    .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
                .MaximumLength(50).WithMessage("Password must not exceed 50 characters");

        }
    }
}
