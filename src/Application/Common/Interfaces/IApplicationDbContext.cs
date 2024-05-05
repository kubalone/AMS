using AML.Domain.Entities;

namespace AML.Application.Common.Interfaces;

public interface IApplicationDbContext
{


    DbSet<Transaction> Transactions { get; }

    DbSet<Address> Addresses { get; }

    DbSet<Customer> Customers { get; }

    DbSet<TransactionLimit> TransactionLimits { get; }
    DbSet<LimitChangeHistory> LimitChangesHistory { get; }
    DbSet<CustomerBalance> CustomerBalances { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
