using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AML.Domain.Enums;
using FluentValidation;

namespace AML.Application.Customers.Commands.CreateCustomer;
public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.FirstName)
           .NotEmpty().WithMessage("First name is required.")
            .Matches(@"^[A-Za-z\s]*$").WithMessage("First should only contain letters.")
            .Length(3, 30).WithMessage("First name cannot exceed 30 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
             .Matches(@"^[A-Za-z\s]*$").WithMessage("Last Name should only contain letters.")
            .Length(3, 30).WithMessage("Last name cannot exceed 30 characters.");
     

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.")
            .Must(BeAtLeast18YearsOld).WithMessage("Customer must be at least 18 years old.");

        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Street is required.")
            .Length(2,200).WithMessage("Street cannot exceed 200 characters.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required.")
            .Matches(@"^[a-zA-Z\- ]+$").WithMessage("City can only contain letters, spaces, and hyphens.")
            .Length(2, 30).WithMessage("City cannot exceed 30 characters.");

        RuleFor(x => x.ZipCode)
            .NotEmpty().WithMessage("Zip code is required.")
            .Matches(@"^[0-9\-]+$").WithMessage("Zip code can only contain numbers and hyphens.")
            .Length(2,10).WithMessage("Zip code cannot exceed 10 characters.");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required.")
            .Matches(@"^[a-zA-Z ]+$").WithMessage("Country can only contain letters and spaces.")
            .Length(2,20).WithMessage("Country cannot exceed 20 characters.");

        When(x => x.Balances != null, () =>
        {
            RuleForEach(x => x.Balances)
                .ChildRules(balance =>
                {
                    balance.RuleFor(b => b.Currency)
                    .Must(i => Enum.IsDefined(typeof(CurrencyType), i)).WithMessage("Not found this specific currency");

                    balance.RuleFor(b => b.Balance)
                        .GreaterThan(0).WithMessage("Balance is Required");
                });
        });
    }

    private bool BeAtLeast18YearsOld(DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;

        if (dateOfBirth.Date > today.AddYears(-age))
            age--;

        return age >= 18;
    }
}

