using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AML.Application.Customers.Commands.CreateCustomer;
using AML.Application.Customers.Commands.DeleteCustomerCommand;
using AML.Application.Transactions.Queries.GetTransactionsForCustomer;
using AML.Domain.Entities;
using AML.Domain.Enums;

namespace AML.Application.FunctionalTests.Transactions.Queries;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Testing;

internal class GetTransactionsForCustomerTest : BaseTestFixture
{
    [Test]
    public async Task ShouldHaveProperPagination()
    {
        var commandToCreateCustomerOne = new CreateCustomerCommand
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA",
        };

        var customerOneId = await SendAsync(commandToCreateCustomerOne);
        var commandToCreateCustomerTwo = new CreateCustomerCommand
        {
            FirstName = "Apu",
            LastName = "Apustaja",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main RE",
            City = "Zielona Gora",
            ZipCode = "12345",
            Country = "Pol",
        };

        var customerTwoId = await SendAsync(commandToCreateCustomerTwo);

        var transactions = new List<Transaction>
                {
                    new Transaction { Amount = 100.00m, Currency = CurrencyType.USD,  TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 1",CustomerId=customerOneId },
                    new Transaction { Amount = 50.00m, Currency = CurrencyType.EUR,  TransactionType = TransactionType.Withdrawal, IsSuspicious = false, Description = "Transaction 2",CustomerId=customerOneId  },
                    new Transaction { Amount = 75.00m, Currency = CurrencyType.USD,  TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 3",CustomerId=customerOneId  },
                    new Transaction { Amount = 75.00m, Currency = CurrencyType.USD,  TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 4",CustomerId=customerTwoId  }
                };
        await AddRangeAsync(transactions);

        var getTransactionsForCustomerQuery = new GetTransactionsForCustomerQuery()
        {
            CustomerId = customerOneId,
            PageNumber = 1,
            PageSize = 2
        };
        var result = await SendAsync(getTransactionsForCustomerQuery);


        result.Items.Should().NotBeNullOrEmpty();
        result.Items.Count.Should().BeLessOrEqualTo(getTransactionsForCustomerQuery.PageSize);
        var transactionsCount= transactions.Where(p => p.CustomerId == customerOneId).Count();
        var expectedTotalPages = (int)Math.Ceiling((double)transactionsCount / getTransactionsForCustomerQuery.PageSize);

        result.TotalPages.Should().Be(expectedTotalPages);
        result.TotalCount.Should().Be(transactionsCount);
        result.HasPreviousPage.Should().BeFalse();

        result.HasNextPage.Should().Be(expectedTotalPages > getTransactionsForCustomerQuery.PageNumber);

    }
    [Test]
    public async Task ShouldRequireValidCustomerId()
    {
        var command = new GetTransactionsForCustomerQuery()
        {
            CustomerId = 999,
            PageNumber = 1,
            PageSize = 2
        };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }
}
