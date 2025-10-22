using FluentValidation;
using FundAdmin.Application.Interfaces.Dtos;

namespace FundAdmin.Application.Validators
{
    public class InvestorValidator<T> : AbstractValidator<T> where T : class, IInvestorBase
    {
        public InvestorValidator()
        {
            RuleFor(f => f.FullName).NotEmpty().MaximumLength(100);

            RuleFor(f => f.Email).NotEmpty().EmailAddress().WithMessage("Invalid email format.");

            RuleFor(f => f.FundId).NotEmpty();
        }
    }
}
