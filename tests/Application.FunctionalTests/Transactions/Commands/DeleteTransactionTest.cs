using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AML.Application.Customers.Commands.CreateCustomer;
using AML.Application.Customers.Commands.DeleteCustomerCommand;
using AML.Application.Transactions.Commands.CreateTransactionCommand;
using AML.Application.Transactions.Commands.DeleteTransactionCommand;
using AML.Domain.Entities;
using AML.Domain.Enums;

namespace AML.Application.FunctionalTests.Transactions.Commands;
using static Testing;

internal class DeleteTransactionTest : BaseTestFixture
{
    [Test]
    public async Task ShouldDeleteTransaction()
    {
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
        await SendAsync(new DeleteTransactionCommand(itemTransactionId));
        var transaction = await FindAsync<Transaction>(itemTransactionId);

        transaction.Should().BeNull();
    }
    [Test]
    public async Task ShouldRequireValidCustomerId()
    {
        var command = new DeleteTransactionCommand(99);
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }
}
