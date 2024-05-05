using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AML.Application.Common.Exceptions;
using AML.Application.Customers.Commands.CreateCustomer;
using AML.Application.Customers.Commands.UpdateCustomerCommand;
using AML.Application.Transactions.Commands.CreateTransactionCommand;
using AML.Domain.Entities;
using AML.Domain.Enums;

namespace AML.Application.FunctionalTests.Customers.Commands;
using static Testing;

internal class UpdateCustomerTest : BaseTestFixture
{
    [Test]
    public async Task ShouldUpdateCustomer()
    {
        var command = new CreateCustomerCommand
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA",
            Balances = new List<Application.Customers.Commands.CreateCustomer.CustomerBalanceDto>
            {
                new Application.Customers.Commands.CreateCustomer.CustomerBalanceDto { Currency = CurrencyType.USD, Balance = 1000m }
            }
        };

        var itemId = await SendAsync(command);

        var updateCommand = new UpdateCustomerCommand
        {
            CustomerId = itemId,
            FirstName = "Jane",
            LastName = "Smith",
            DateOfBirth = new DateTime(1985, 5, 10),
            Street = "456 Elm St",
            City = "Othertown",
            ZipCode = "54321",
            Country = "Canada",
            Balances = new List<Application.Customers.Commands.UpdateCustomerCommand.CustomerBalanceDto>
            {
            new Application.Customers.Commands.UpdateCustomerCommand.CustomerBalanceDto { Currency = CurrencyType.EUR, Balance = 500.0m }
            }
        };
        await SendAsync(updateCommand);



        var customer = await FindAsync<Customer>(itemId, "Address", "Balances","Transactions");


        customer.Should().NotBeNull();
        customer!.Id.Should().Be(itemId);
        customer!.FirstName.Should().Be(updateCommand.FirstName);
        customer.LastName.Should().Be(updateCommand.LastName);
        customer.DateOfBirth.Should().Be(updateCommand.DateOfBirth);

        customer.Address.Should().NotBeNull();
        customer.Address!.CustomerId.Should().Be(itemId);
        customer.Address!.Street.Should().Be(updateCommand.Street);
        customer.Address!.City.Should().Be(updateCommand.City);
        customer.Address!.ZipCode.Should().Be(updateCommand.ZipCode);
        customer.Address!.Country.Should().Be(updateCommand.Country);

        customer.Transactions.Should().HaveCount(0);

        customer.Balances.Should().NotBeNull().And.HaveCount(2);
        var balanceOne = customer.Balances.FirstOrDefault();
        balanceOne.Should().NotBeNull();
        balanceOne!.Currency.Should().Be(CurrencyType.USD);
        balanceOne.Balance.Should().Be(1000m);

        var balancetwo= customer.Balances.Skip(1).FirstOrDefault();
        balancetwo.Should().NotBeNull();
        balancetwo!.Currency.Should().Be(CurrencyType.EUR);
        balancetwo.Balance.Should().Be(500.0m);

    }
   
    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidCurrency()
    {
        var command = new UpdateCustomerCommand
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA",
            Balances = new List<Application.Customers.Commands.UpdateCustomerCommand.CustomerBalanceDto>
            {
                new Application.Customers.Commands.UpdateCustomerCommand.CustomerBalanceDto { Currency = (CurrencyType)9999, Balance = 100.0m }
            }
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>()
            .Where(ex => ex.Errors.Any(error => error.Key == "Balances[0].Currency"));
    }
   
    [Test]
    public async Task ShouldThrowValidationExceptionForMissingId()
    {
        var command = new UpdateCustomerCommand
        {
            FirstName = "Apustaja",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(UpdateCustomerCommand.CustomerId)));
    }
    [Test]
    public async Task ShouldThrowValidationExceptionForMissingFirstName()
    {
        var command = new UpdateCustomerCommand
        {
            CustomerId = 5,
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(UpdateCustomerCommand.FirstName)));
    }

    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidFirstName()
    {
        var command = new UpdateCustomerCommand
        {
            CustomerId = 5,
            FirstName = "Jo",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(UpdateCustomerCommand.FirstName)));
    }

    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidFirstNameCharacters()
    {
        var command = new UpdateCustomerCommand
        {
            CustomerId = 5,
            FirstName = "Jo3",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(UpdateCustomerCommand.FirstName)));
    }
    [Test]
    public async Task ShouldThrowValidationExceptionForMissingLastName()
    {
        var command = new UpdateCustomerCommand
        {
            CustomerId = 5,
            FirstName = "John",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(UpdateCustomerCommand.LastName)));
    }

    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidLastName()
    {
        var command = new UpdateCustomerCommand
        {
            CustomerId = 5,
            FirstName = "John",
            LastName = "D",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(UpdateCustomerCommand.LastName)));
    }

    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidLastNameCharacters()
    {
        var command = new UpdateCustomerCommand
        {
            CustomerId = 5,
            FirstName = "John",
            LastName = "Doe3",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(UpdateCustomerCommand.LastName)));
    }
    [Test]
    public async Task ShouldThrowValidationExceptionForMissingDateOfBirth()
    {
        var command = new UpdateCustomerCommand
        {
            CustomerId = 5,
            FirstName = "John",
            LastName = "Doe",
            Street = "123 Main St",
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(UpdateCustomerCommand.DateOfBirth)));
    }

    [Test]
    public async Task ShouldThrowValidationExceptionForUnderageCustomer()
    {
        var command = new UpdateCustomerCommand
        {
            CustomerId = 5,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = DateTime.Now.AddYears(-17),
            Street = "123 Main St",
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(UpdateCustomerCommand.DateOfBirth)));
    }

    [Test]
    public async Task ShouldThrowValidationExceptionForMissingStreet()
    {
        var command = new UpdateCustomerCommand
        {
            CustomerId = 5,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(UpdateCustomerCommand.Street)));
    }
    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidStreetLength()
    {
        var command = new UpdateCustomerCommand
        {
            CustomerId = 5,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St. Apt. 123, Some Long Address Street Name That Exceeds 200 Characters " +
                     "123 Main St. Apt. 123, Some Long Address Street Name That Exceeds 200 Characters " +
                     "123 Main St. Apt. 123, Some Long Address Street Name That Exceeds 200 Characters " +
                     "123 Main St. Apt. 123, Some Long Address Street Name That Exceeds 200 Characters " +
                     "123 Main St. Apt. 123, Some Long Address Street Name That Exceeds 200 Characters " +
                     "123 Main St. Apt. 123, Some Long Address Street Name That Exceeds 200 Characters " +
                     "123 Main St. Apt. 123, Some Long Address Street Name That Exceeds 200 Characters " +
                     "123 Main St. Apt. 123, Some Long Address Street Name That Exceeds 200 Characters " +
                     "123 Main St. Apt. 123, Some Long Address Street Name That Exceeds 200 Characters " +
                     "123 Main St. Apt. 123, Some Long Address Street Name That Exceeds 200 Characters ", // 200 characters
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(UpdateCustomerCommand.Street)));
    }
    [Test]
    public async Task ShouldThrowValidationExceptionForMissingCity()
    {
        var command = new UpdateCustomerCommand
        {
            CustomerId = 5,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            ZipCode = "12345",
            Country = "USA"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(UpdateCustomerCommand.City)));
    }
    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidCityLength()
    {
        var command = new UpdateCustomerCommand
        {
            CustomerId = 5,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "AnytownIsAVeryLongCityNameOPX ABCDEFGHIJ", // More than 30 characters
            ZipCode = "12345",
            Country = "USA"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(UpdateCustomerCommand.City)));
    }
    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidCity()
    {
        var command = new UpdateCustomerCommand
        {
            CustomerId = 5,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "1Anytown", // Invalid city name
            ZipCode = "12345",
            Country = "USA"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(UpdateCustomerCommand.City)));
    }
    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidZipCodeLength()
    {
        var command = new UpdateCustomerCommand
        {
            CustomerId = 5,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Anytown",
            ZipCode = "12345678901", // More than 10 characters
            Country = "USA"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(UpdateCustomerCommand.ZipCode)));
    }
    [Test]
    public async Task ShouldThrowValidationExceptionForMissingZipCode()
    {
        var command = new UpdateCustomerCommand
        {
            CustomerId = 5,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Anytown",
            Country = "USA"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(UpdateCustomerCommand.ZipCode)));
    }
    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidZipCode()
    {
        var command = new UpdateCustomerCommand
        {
            CustomerId = 5,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Anytown",
            ZipCode = "12A45", // Invalid zip code
            Country = "USA"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(UpdateCustomerCommand.ZipCode)));
    }

    [Test]
    public async Task ShouldThrowValidationExceptionForMissingCountry()
    {
        var command = new UpdateCustomerCommand
        {
            CustomerId = 5,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Anytown",
            ZipCode = "12345"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(UpdateCustomerCommand.Country)));
    }

    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidCountry()
    {
        var command = new UpdateCustomerCommand
        {
            CustomerId = 5,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Anytown",
            ZipCode = "12345",
            Country = "US@" // Invalid country name
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(UpdateCustomerCommand.Country)));
    }
    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidCountryLength()
    {
        var command = new UpdateCustomerCommand
        {
            CustomerId=5,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Anytown",
            ZipCode = "12345",
            Country = "A country name longer than 20 characters"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(UpdateCustomerCommand.Country)));
    }

}
