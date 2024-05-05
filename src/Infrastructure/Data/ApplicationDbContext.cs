using System.Reflection;
using AML.Application.Common.Interfaces;
using AML.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AML.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Transaction> Transactions => Set<Transaction>();

    public DbSet<Address> Addresses => Set<Address>();

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<TransactionLimit> TransactionLimits => Set<TransactionLimit>();
    public DbSet<LimitChangeHistory> LimitChangesHistory => Set<LimitChangeHistory>();
    public DbSet<CustomerBalance> CustomerBalances => Set<CustomerBalance>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
