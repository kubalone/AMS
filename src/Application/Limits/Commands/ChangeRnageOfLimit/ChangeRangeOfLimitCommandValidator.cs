using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AML.Domain.Enums;

namespace AML.Application.Limits.Commands.ChangeRnageOfLimit;
public class ChangeRangeOfLimitCommandValidator : AbstractValidator<ChangeRangeOfLimitCommand>
{
    public ChangeRangeOfLimitCommandValidator()
    {
        RuleFor(x => x.CurrencyType)
            .NotNull().WithMessage("Currency symbol is required.")
            .Must(BeValidCurrency).WithMessage("Invalid transaction type.");

        RuleFor(x => x.NewLimitValue)
            .GreaterThanOrEqualTo(0).WithMessage("New limit value must be greater or equal than 0.");
    }

    private bool BeValidCurrency(CurrencyType currency)
    {
        return Enum.IsDefined(typeof(CurrencyType), currency);
    }
}
