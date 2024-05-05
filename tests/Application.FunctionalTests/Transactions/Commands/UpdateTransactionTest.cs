using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AML.Application.Common.Exceptions;
using AML.Application.Customers.Commands.CreateCustomer;
using AML.Application.Transactions.Commands.CreateTransactionCommand;
using AML.Application.Transactions.Commands.UpdateTransactionCommand;
using AML.Domain.Entities;
using AML.Domain.Enums;

namespace AML.Application.FunctionalTests.Transactions.Commands;
using static Testing;

internal class UpdateTransactionTest : BaseTestFixture
{
    [Test]
    public async Task ShouldUpdateTransaction()
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

        var transactionToUpdate = new UpdateTransactionCommand
        {
            TransactionId = itemTransactionId,
            Amount = 100.00m,
            Currency = CurrencyType.USD,
            TransactionType = TransactionType.Deposit,
            Description = "Transaction 1 xyz",
        };

         await SendAsync(transactionToUpdate);

        var transaction = await FindAsync<Transaction>(itemTransactionId);
        transaction.Should().NotBeNull();
        transaction!.Amount.Should().Be(transactionToUpdate.Amount);
        transaction.Currency.Should().Be(transactionToUpdate.Currency);
        transaction.TransactionType.Should().Be(transactionToUpdate.TransactionType);
        transaction.Description.Should().Be(transactionToUpdate.Description);
        transaction!.IsSuspicious.Should().Be(true);

    }

    [Test]
    [TestCase(99, 1100, 1, TransactionType.Deposit)]
    [TestCase(99, 902, 1, TransactionType.Withdrawal)]
    public async Task ShouldUpdateNewBalanceForClient( decimal ammount, decimal finalBalanceForOne, decimal finalBalanceForTwo, TransactionType transactionType)
    {

        var commandToCreateCustomer = new CreateCustomerCommand
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA",
            Balances = new List<CustomerBalanceDto>
            {
                new CustomerBalanceDto { Currency = CurrencyType.USD, Balance = 1 },
                new CustomerBalanceDto { Currency = CurrencyType.EUR, Balance = 1}

            }
        };

        var itemCustomerId = await SendAsync(commandToCreateCustomer);

        var commandToCreateTransaction = new CreateTransactionCommand
        {
            Amount = 1000.00m,
            Currency = CurrencyType.USD,
            TransactionType = TransactionType.Deposit,
            Description = "Transaction 1",
            CustomerId = itemCustomerId
        };

        var itemTransactionId = await SendAsync(commandToCreateTransaction);

        var transactionToUpdate = new UpdateTransactionCommand
        {
            TransactionId = itemTransactionId,
            Amount = ammount,
            Currency = CurrencyType.USD,
            TransactionType = transactionType,
            Description = "Transaction 1 xyz",
        };

        await SendAsync(transactionToUpdate);

        var customer = await FindAsync<Customer>(itemCustomerId, "Balances");

        var balanceOne = customer!.Balances.FirstOrDefault();
        var balanceTwo = customer!.Balances.Skip(1).FirstOrDefault();



        balanceOne!.Balance.Should().Be(finalBalanceForOne);
        balanceOne!.Currency.Should().Be(CurrencyType.USD);

        balanceTwo!.Balance.Should().Be(finalBalanceForTwo);
        balanceTwo!.Currency.Should().Be(CurrencyType.EUR);

    }

    [Test]
    public async Task ShouldAddNewBalancerForClient()
    {

        var commandToCreateCustomer = new CreateCustomerCommand
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA",
            Balances = new List<CustomerBalanceDto>
            {
                new CustomerBalanceDto { Currency = CurrencyType.EUR, Balance = 1000m }

            }
        };

        var itemCustomerId = await SendAsync(commandToCreateCustomer);

        var commandToCreateTransaction = new CreateTransactionCommand
        {
            Amount = 99.00m,
            Currency = CurrencyType.EUR,
            TransactionType = TransactionType.Deposit,
            Description = "Transaction 1",
            CustomerId = itemCustomerId
        };

        var itemTransactionId = await SendAsync(commandToCreateTransaction);

        var transactionToUpdate = new UpdateTransactionCommand
        {
            TransactionId = itemTransactionId,
            Amount = 1.00m,
            Currency = CurrencyType.USD,
            TransactionType = TransactionType.Deposit,
            Description = "Transaction 1 xyz",
        };

        await SendAsync(transactionToUpdate);


        var customer = await FindAsync<Customer>(itemCustomerId, "Balances");

        var balanceOne = customer!.Balances.FirstOrDefault();
        var balanceTwo = customer!.Balances.Skip(1).FirstOrDefault();


      

        balanceOne!.Balance.Should().Be(1099.00m);
        balanceOne!.Currency.Should().Be(CurrencyType.EUR);

        balanceTwo!.Balance.Should().Be(1.00m);
        balanceTwo!.Currency.Should().Be(CurrencyType.USD);

    }
    [Test]
    public async Task ShouldIncorrectAmmount()
    {

        var commandToCreateCustomer = new CreateCustomerCommand
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA",
            Balances = new List<CustomerBalanceDto>
            {
                new CustomerBalanceDto { Currency = CurrencyType.USD, Balance = 1000m },
                new CustomerBalanceDto { Currency = CurrencyType.EUR, Balance = 1m }

            }
        };

        var itemCustomerId = await SendAsync(commandToCreateCustomer);

        var commandToCreateTransaction = new CreateTransactionCommand
        {
            Amount = 100m,
            Currency = CurrencyType.EUR,
            TransactionType = TransactionType.Deposit,
            Description = "Transaction 1",
            CustomerId = itemCustomerId
        };
        var itemTransactionId = await SendAsync(commandToCreateTransaction);

        var transactionToUpdate = new UpdateTransactionCommand
        {
            TransactionId = itemTransactionId,
            Amount = 1000.00m,
            Currency = CurrencyType.EUR,
            TransactionType = TransactionType.Withdrawal,
            Description = "Transaction 1 xyz",
        };

        

        await FluentActions.Invoking(() =>
             SendAsync(transactionToUpdate)).Should().ThrowAsync<InvalidOperationException>();


    }
   
   
    [Test]
    public async Task ShouldThrowValidationExceptionForMissingAmount()
    {
        var command = new UpdateTransactionCommand
        {
            Amount = -20.00m,
            Currency = CurrencyType.USD,
            TransactionType = TransactionType.Deposit,
            TransactionId = 1,
            Description = "Test transaction"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(UpdateTransactionCommand.Amount)));
    }

    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidCurrency()
    {
        var command = new UpdateTransactionCommand
        {
            Amount = 100,
            Currency = (CurrencyType)1000, // Assuming 1000 is not a valid CurrencyType enum value
            TransactionType = TransactionType.Deposit,
            TransactionId = 1,
            Description = "Test transaction"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(UpdateTransactionCommand.Currency)));
    }



    [Test]
    public async Task ShouldThrowValidationExceptionForExceedingDescriptionLength()
    {
        var command = new UpdateTransactionCommand
        {
            Amount = 100,
            Currency = CurrencyType.USD,
            TransactionType = TransactionType.Deposit,
            TransactionId = 1,
            Description = new string('A', 201) // Assuming description length exceeds 200 characters
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(UpdateTransactionCommand.Description)));
    }

}
