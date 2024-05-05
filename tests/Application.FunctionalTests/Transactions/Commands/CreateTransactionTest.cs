using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AML.Application.Common.Exceptions;
using AML.Application.Customers.Commands.CreateCustomer;
using AML.Application.Transactions.Commands.CreateTransactionCommand;
using AML.Domain.Entities;
using AML.Domain.Enums;
using MediatR;

namespace AML.Application.FunctionalTests.Transactions.Commands;
using static Testing;

internal class CreateTransactionTest : BaseTestFixture
{

    [Test]
    public async Task ShouldCreateTransaction()
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
            CustomerId= itemCustomerId
        };
        var itemTransactionId= await SendAsync(commandToCreateTransaction);
        var transaction = await FindAsync<Transaction>(itemTransactionId);

        transaction.Should().NotBeNull();
        transaction!.Amount.Should().Be(commandToCreateTransaction.Amount);
        transaction.Currency.Should().Be(commandToCreateTransaction.Currency);
        transaction.TransactionType.Should().Be(commandToCreateTransaction.TransactionType);
        transaction.Description.Should().Be(commandToCreateTransaction.Description);
        transaction.CustomerId.Should().Be(commandToCreateTransaction.CustomerId);
        transaction!.IsSuspicious.Should().Be(false);


    }
    [Test]
    [TestCase(1000, 1000,99,1099,1000,TransactionType.Deposit)]
    [TestCase(1000, 1000, 99, 901, 1000, TransactionType.Withdrawal)]
    public async Task ShouldUpdateNewBalanceForClient(decimal balanceCurrencyOne,decimal balanceCurrencyTwo, decimal ammount,decimal finalBalanceForOne, decimal finalBalanceForTwo, TransactionType transactionType)
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
                new CustomerBalanceDto { Currency = CurrencyType.USD, Balance = balanceCurrencyOne },
                new CustomerBalanceDto { Currency = CurrencyType.EUR, Balance = balanceCurrencyTwo }

            }
        };

        var itemCustomerId = await SendAsync(commandToCreateCustomer);

        var commandToCreateTransaction = new CreateTransactionCommand
        {
            Amount = ammount,
            Currency = CurrencyType.USD,
            TransactionType = transactionType,
            Description = "Transaction 1",
            CustomerId = itemCustomerId
        };

        var itemTransactionId = await SendAsync(commandToCreateTransaction);
        await FindAsync<Transaction>(itemTransactionId);

        var customer = await FindAsync<Customer>(itemCustomerId,  "Balances");

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
            Currency = CurrencyType.USD,
            TransactionType = TransactionType.Deposit,
            Description = "Transaction 1",
            CustomerId = itemCustomerId
        };

        await SendAsync(commandToCreateTransaction);

        var customer = await FindAsync<Customer>(itemCustomerId,  "Balances");

        var balanceOne = customer!.Balances.FirstOrDefault();
        var balanceTwo = customer!.Balances.Skip(1).FirstOrDefault();


      

        balanceOne!.Balance.Should().Be(1000.00m);
        balanceOne!.Currency.Should().Be(CurrencyType.EUR);

        balanceTwo!.Balance.Should().Be(99.00m);
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
            TransactionType = TransactionType.Withdrawal,
            Description = "Transaction 1",
            CustomerId = itemCustomerId
        };

        await FluentActions.Invoking(() =>
             SendAsync(commandToCreateTransaction)).Should().ThrowAsync<InvalidOperationException>();


    }
    [Test]
    public async Task ShouldSetTransactionAsSuspicious()
    {

        var transactionLimit = new TransactionLimit()
        {
            Currency = CurrencyType.USD,
            LimitValue =100.00m
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

        var customer = await FindAsync<Customer>(itemCustomerId, "Address");

        var commandToCreateTransaction = new CreateTransactionCommand
        {
            Amount = 100.00m,
            Currency = CurrencyType.USD,
            TransactionType = TransactionType.Deposit,
            Description = "Transaction 1",
            CustomerId = customer!.Id
        };
        var itemTransactionId = await SendAsync(commandToCreateTransaction);
        var transaction = await FindAsync<Transaction>(itemTransactionId);




        transaction.Should().NotBeNull();
        transaction!.IsSuspicious.Should().Be(true);

    }
    [Test]
    public async Task ShouldRequireValidCustomerId()
    {
        

        var commandToCreateTransaction = new CreateTransactionCommand
        {
            Amount = 100.00m,
            Currency = CurrencyType.USD,
            TransactionType = TransactionType.Deposit,
            Description = "Transaction 1",
            CustomerId = 5
        };
       
         await FluentActions.Invoking(() =>
            SendAsync(commandToCreateTransaction)).Should().ThrowAsync<NotFoundException>();

    }
    [Test]
    public async Task ShouldThrowValidationExceptionForWrongAmount()
    {
        var command = new CreateTransactionCommand
        {
            Currency = CurrencyType.USD,
            TransactionType = TransactionType.Deposit,
            CustomerId = 1,
            Amount=-20.00m,
            Description = "Test transaction"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(CreateTransactionCommand.Amount)));
    }

    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidCurrency()
    {
        var command = new CreateTransactionCommand
        {
            Amount = 100,
            Currency = (CurrencyType)1000, // Assuming 1000 is not a valid CurrencyType enum value
            TransactionType = TransactionType.Deposit,
            CustomerId = 1,
            Description = "Test transaction"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(CreateTransactionCommand.Currency)));
    }



    [Test]
    public async Task ShouldThrowValidationExceptionForMissingCustomerId()
    {
        var command = new CreateTransactionCommand
        {
            Amount = 100,
            Currency = CurrencyType.USD,
            TransactionType = TransactionType.Deposit,
            Description = "Test transaction"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(CreateTransactionCommand.CustomerId)));
    }

    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidCustomerId()
    {
        var command = new CreateTransactionCommand
        {
            Amount = 100,
            Currency = CurrencyType.USD,
            TransactionType = TransactionType.Deposit,
            CustomerId = -1, // Assuming -1 is an invalid customer ID
            Description = "Test transaction"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(CreateTransactionCommand.CustomerId)));
    }

    [Test]
    public async Task ShouldThrowValidationExceptionForExceedingDescriptionLength()
    {
        var command = new CreateTransactionCommand
        {
            Amount = 100,
            Currency = CurrencyType.USD,
            TransactionType = TransactionType.Deposit,
            CustomerId = 1,
            Description = new string('A', 201) // Assuming description length exceeds 200 characters
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(CreateTransactionCommand.Description)));
    }

}
