using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AML.Domain.Enums;

namespace AML.Application.Transactions.Commands.UpdateTransactionCommand;
public class UpdateTransactionCommandValidator : AbstractValidator<UpdateTransactionCommand>
{
    public UpdateTransactionCommandValidator()
    {
        RuleFor(x => x.Amount)
          .GreaterThanOrEqualTo(0).WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.Currency)
            .Must(BeValidCurrency).WithMessage("Invalid currency.");

        RuleFor(x => x.TransactionType)
            .Must(BeValidTransactionType).WithMessage("Invalid transaction type.");

        RuleFor(x => x.TransactionId)
            .NotEmpty().WithMessage("Transaction id is required.")
            .GreaterThan(0).WithMessage("Transaction id must be greater than zero.");

        RuleFor(x => x.Description)
            .MaximumLength(200).WithMessage("Description can have a maximum of 200 characters.");
    }

    private bool BeValidCurrency(CurrencyType currency)
    {
        return Enum.IsDefined(typeof(CurrencyType), currency);
    }

    private bool BeValidTransactionType(TransactionType transactionType)
    {
        return Enum.IsDefined(typeof(TransactionType), transactionType);
    }

    
}
