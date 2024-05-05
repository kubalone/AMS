using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AML.Application.Customers.Commands.UpdateCustomerCommand;
using AML.Domain.Enums;
using FluentValidation;

namespace AML.Application.Transactions.Commands.CreateTransactionCommand;
public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(0).WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.Currency)
            .NotNull().WithMessage("Currency is required.")
            .Must(BeValidCurrency).WithMessage("Invalid currency.");

        RuleFor(x => x.TransactionType)
            .NotNull().WithMessage("TransactionType is required.")
            .Must(BeValidTransactionType).WithMessage("Invalid transaction type.");

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required.")
            .GreaterThan(0).WithMessage("CustomerId must be greater than zero.");

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
