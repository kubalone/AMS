using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AML.Application.Customers.Queries.GetCustomers;
using AML.Application.Transactions.Queries.GetTransactions;
using AML.Domain.Entities;
using AML.Domain.Enums;
using AML.Web.Endpoints;

namespace AML.Application.FunctionalTests.Transactions.Queries;
using static Testing;

internal class GetTransactionsTest : BaseTestFixture
{
    [Test]
    public async Task ShouldHaveProperPagination()
    {
        var customer = new Customer
        {
            CustomerIdentifier = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Transactions =
            {
                    new Transaction { Amount = 100.00m, Currency = CurrencyType.USD,  TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 1" },
                    new Transaction { Amount = 50.00m, Currency = CurrencyType.EUR,  TransactionType = TransactionType.Withdrawal, IsSuspicious = false, Description = "Transaction 2" },
                    new Transaction { Amount = 75.00m, Currency = CurrencyType.USD,  TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 3" }
             }
        };
        await AddAsync(customer);

        var query = new GetTransactionsQuery()
        {
            PageNumber = 1,
            PageSize = 2
        };
        var result = await SendAsync(query);

        result.Items.Should().NotBeNullOrEmpty();
        result.Items.Count.Should().BeLessOrEqualTo(query.PageSize);

        var expectedTotalPages = (int)Math.Ceiling((double)customer.Transactions.Count / query.PageSize);

        result.TotalPages.Should().Be(expectedTotalPages);

        result.TotalCount.Should().Be(customer.Transactions.Count);

        result.HasPreviousPage.Should().BeFalse();

        result.HasNextPage.Should().Be(expectedTotalPages > query.PageNumber);
    }
    [Test]
    public async Task ShouldReturnOnlySuspiciousTransactions()
    {
        var customer = new Customer
        {
            CustomerIdentifier = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Transactions =
            {
                    new Transaction { Amount = 100.00m, Currency = CurrencyType.USD,  TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 1" },
                    new Transaction { Amount = 50.00m, Currency = CurrencyType.EUR,  TransactionType = TransactionType.Withdrawal, IsSuspicious = true, Description = "Transaction 2" },
                    new Transaction { Amount = 75.00m, Currency = CurrencyType.USD,  TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 3" }
             }
        };
        await AddAsync(customer);

        var query = new GetTransactionsQuery()
        {
            IsSuspicious=true,
            PageNumber = 1,
            PageSize = 2
        };
        var result = await SendAsync(query);

        result.Items.Should().HaveCount(1);
    }
}
