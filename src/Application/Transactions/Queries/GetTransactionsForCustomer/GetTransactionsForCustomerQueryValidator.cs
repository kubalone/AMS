using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AML.Application.Transactions.Queries.GetTransactionsForCustomer;
public class GetTransactionsForCustomerQueryValidator : AbstractValidator<GetTransactionsForCustomerQuery>
{
    public GetTransactionsForCustomerQueryValidator()
    {
        RuleFor(x => x.CustomerId)
          .NotEmpty().WithMessage("CustomerId is required.");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
    }
}
