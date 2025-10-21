using FluentValidation;
using FundAdmin.Application.DTOs;

namespace FundAdmin.Application.Validators;

public class TransactionValidator : AbstractValidator<TransactionCreateDto>
{
    public TransactionValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0.");

        RuleFor(x => x.InvestorId).GreaterThan(0);

        RuleFor(x => x.TransactionDate).LessThanOrEqualTo(DateTime.UtcNow.AddMinutes(1));
    }
}
