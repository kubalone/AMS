using AML.Application.Common.Interfaces;
using AML.Application.Common.Mappings;
using AML.Application.Common.Models;
using AML.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AML.Application.Customers.Queries.GetCustomers;

public record GetCustomersQuery : IRequest<PaginatedList<CustomerDTO>>
{
    public string? LastName { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}



public class GetCustomersQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetCustomersQuery, PaginatedList<CustomerDTO>>
{

    public async Task<PaginatedList<CustomerDTO>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Customer> query = context.Customers
            .Include(c => c.Address)
            .Include(c=>c.Balances)
            .Include(c => c.Transactions);

        if (!string.IsNullOrWhiteSpace(request.LastName))
        {
            query = query.Where(c => c.LastName != null && c.LastName.Contains(request.LastName));
        }


        return await query
            .ProjectTo<CustomerDTO>(mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
