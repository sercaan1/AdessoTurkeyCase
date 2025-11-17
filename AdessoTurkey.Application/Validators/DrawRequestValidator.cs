using AdessoTurkey.Application.DTOs;
using FluentValidation;

namespace AdessoTurkey.Application.Validators
{
    public class DrawRequestValidator : AbstractValidator<DrawRequestDto>
    {
        public DrawRequestValidator()
        {
            RuleFor(x => x.DrawerFirstName)
                .NotEmpty().WithMessage("Çeken kişinin adı zorunludur")
                .MaximumLength(100).WithMessage("Ad en fazla 100 karakter olabilir");

            RuleFor(x => x.DrawerLastName)
                .NotEmpty().WithMessage("Çeken kişinin soyadı zorunludur")
                .MaximumLength(100).WithMessage("Soyad en fazla 100 karakter olabilir");

            RuleFor(x => x.NumberOfGroups)
                .Must(x => x == 4 || x == 8)
                .WithMessage("Grup sayısı sadece 4 veya 8 olabilir");
        }
    }
}
