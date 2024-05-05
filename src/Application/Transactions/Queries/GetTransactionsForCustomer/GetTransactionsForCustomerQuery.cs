using AML.Application.Common.Interfaces;
using AML.Application.Common.Mappings;
using AML.Application.Common.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AML.Application.Transactions.Queries.GetTransactionsForCustomer;

public record GetTransactionsForCustomerQuery : IRequest<PaginatedList<TransactionsForCustomerDto>>
{
    public int CustomerId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}


public class GetTransactionsForCustomerQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetTransactionsForCustomerQuery, PaginatedList<TransactionsForCustomerDto>>
{
  

    public async Task<PaginatedList<TransactionsForCustomerDto>> Handle(GetTransactionsForCustomerQuery request, CancellationToken cancellationToken)
    {

        var entity = await context.Customers
        .FindAsync(new object[] { request.CustomerId }, cancellationToken);

        Guard.Against.NotFound(request.CustomerId, entity);

        return await context.Transactions
         .Where(x => x.CustomerId == request.CustomerId)
         .OrderBy(p=>p.Id)
         .ProjectTo<TransactionsForCustomerDto>(mapper.ConfigurationProvider)
         .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
