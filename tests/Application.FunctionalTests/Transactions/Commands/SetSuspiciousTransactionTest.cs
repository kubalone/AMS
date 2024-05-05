using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AML.Application.Customers.Commands.CreateCustomer;
using AML.Application.Transactions.Commands.CreateTransactionCommand;
using AML.Application.Transactions.Commands.DeleteTransactionCommand;
using AML.Application.Transactions.Commands.SetSuspiciousTransactionCommand;
using AML.Domain.Entities;
using AML.Domain.Enums;

namespace AML.Application.FunctionalTests.Transactions.Commands;
using static Testing;

internal class SetSuspiciousTransactionTest : BaseTestFixture
{
    [Test]
    public async Task ShouldSetTransactionAsSuspicious()
    {
        var transactionLimit = new TransactionLimit()
        {
            Currency = CurrencyType.USD,
            LimitValue = 100.00m
        };
        await AddAsync(transactionLimit);
        var commandToCreateCustomer = new CreateCustomerCommand
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA"
        };

        var itemCustomerId = await SendAsync(commandToCreateCustomer);


        var commandToCreateTransaction = new CreateTransactionCommand
        {
            Amount = 99.00m,
            Currency = CurrencyType.USD,
            TransactionType = TransactionType.Deposit,
            Description = "Transaction 1",
            CustomerId = itemCustomerId
        };


        var itemTransactionId = await SendAsync(commandToCreateTransaction);
        var transactionBeforeSetSupicoius = await FindAsync<Transaction>(itemTransactionId);


        var setSuspiciousTransactionCommand = new SetSuspiciousTransactionCommand
        {
            Id = itemTransactionId,
            IsSuspicious = true
        };
        await SendAsync(setSuspiciousTransactionCommand);
        var transactionAfterSetSupicoius = await FindAsync<Transaction>(itemTransactionId);


        transactionBeforeSetSupicoius!.IsSuspicious.Should().Be(false);
        transactionAfterSetSupicoius!.IsSuspicious.Should().Be(true);

    }
    [Test]
    public async Task ShouldRequireValidId()
    {
        var command = new SetSuspiciousTransactionCommand
        {
            Id = 4,
            IsSuspicious = true
        };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }
}
