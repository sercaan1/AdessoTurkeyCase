using FluentValidation;

namespace AdessoTurkey.Application.Validators
{
    public class GetDrawByIdValidator : AbstractValidator<int>
    {
        public GetDrawByIdValidator()
        {
            RuleFor(id => id)
                .GreaterThan(0)
                .WithMessage("Kura ID'si 0'dan büyük olmalıdır");
        }
    }
}
