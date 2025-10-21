using FluentValidation;
using FundAdmin.Application.Interfaces.Dtos;

namespace FundAdmin.Application.Validators
{
    public class FundValidator<T> : AbstractValidator<T> where T : class, IFundBase
    {
        public FundValidator()
        {
            RuleFor(f => f.FundName).NotEmpty().MaximumLength(100);

            RuleFor(f => f.CurrencyCode).NotEmpty()
                .Length(3).WithMessage("Currency code must be a 3-letter ISO code.")
                .Matches("^[A-Z]{3}$").WithMessage("Currency code must be uppercase ISO format (e.g., USD, EUR).");

            RuleFor(f => f.LaunchDate).NotEmpty()
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Launch date cannot be in the future.");
        }
    }
}
