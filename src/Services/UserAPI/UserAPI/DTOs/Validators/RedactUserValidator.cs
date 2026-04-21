using FluentValidation;

namespace UserAPI.DTOs.Validators
{
    public class RedactUserValidator : AbstractValidator<RedactUser>
    {
        public RedactUserValidator()
        {
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Номер телефона пуст")
                .MaximumLength(12)
                .Matches(@"^(\+375|375)\d{10}$").WithMessage("Введите номер в формате 375#########");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Имя должно быть указано")
                .MaximumLength(20).WithMessage("Имя не должно состоять из более 20 символов");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Фамилия не должна быть пуста")
                .MaximumLength(20).WithMessage("Фамилия не может содержать более 20 символов");
        }
    }
}
