using AML.Application.Common.Interfaces;
using AML.Application.Common.Mappings;
using AML.Application.Common.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AML.Application.Transactions.Queries.GetTransactions;

public record GetTransactionsQuery : IRequest<PaginatedList<TransactionDto>>
{
    public bool? IsSuspicious { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}



public class GetTransactionsQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetTransactionsQuery, PaginatedList<TransactionDto>>
{
  

    public async Task<PaginatedList<TransactionDto>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        

        var query = context.Transactions
                 .Include(t => t.Customer)
                     .ThenInclude(c => c.Address)
                 .AsQueryable();

        if (request.IsSuspicious.HasValue)
        {
            query = query.Where(t => t.IsSuspicious == request.IsSuspicious);
        }

        return await query
            .ProjectTo<TransactionDto>(mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
