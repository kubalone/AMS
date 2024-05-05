using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AML.Application.Common.Exceptions;
using AML.Application.Common.Models;
using AML.Application.Customers.Commands.CreateCustomer;
using AML.Domain.Entities;
using AML.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AML.Application.FunctionalTests.Customers.Commands;
using static Testing;

internal class CreateCustomerTest : BaseTestFixture
{


    [Test]
    public async Task ShouldCreateCustomer()
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
            Balances = new List<CustomerBalanceDto>
            {
                new CustomerBalanceDto { Currency = CurrencyType.USD, Balance = 1000 }
            }
        };

        var itemId = await SendAsync(command);

        var customer = await FindAsync<Customer>(itemId,"Address", "Balances");
        customer.Should().NotBeNull();
        customer!.FirstName.Should().Be(command.FirstName);
        customer.LastName.Should().Be(command.LastName);
        customer.DateOfBirth.Should().Be(command.DateOfBirth);
        customer.CustomerIdentifier.Should().NotBe(Guid.Empty);
        customer.Address.Should().NotBeNull();
        customer.Address!.CustomerId.Should().Be(itemId);
        customer.Address!.Street.Should().Be(command.Street);
        customer.Address!.City.Should().Be(command.City);
        customer.Address!.ZipCode.Should().Be(command.ZipCode);
        customer.Address!.Country.Should().Be(command.Country);

        customer.Balances.Should().NotBeNull();
        customer.Balances.Should().HaveCount(1);
        customer.Balances.First().Currency.Should().Be(CurrencyType.USD);
        customer.Balances.First().Balance.Should().Be(1000);
    }
    [Test]
    public async Task ShouldThrowValidationExceptionForMissingFirstName()
    {
        var command = new CreateCustomerCommand
        {
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(CreateCustomerCommand.FirstName)));
    }

    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidCurrency()
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
            Balances = new List<CustomerBalanceDto>
            {
                new CustomerBalanceDto { Currency = (CurrencyType)9999, Balance = 100.0m }
            }
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>()
            .Where(ex => ex.Errors.Any(error => error.Key == "Balances[0].Currency"));
    }

    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidFirstName()
    {
        var command = new CreateCustomerCommand
        {
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
                ex.Errors.Any(error => error.Key == nameof(CreateCustomerCommand.FirstName)));
    }

    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidFirstNameCharacters()
    {
        var command = new CreateCustomerCommand
        {
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
                ex.Errors.Any(error => error.Key == nameof(CreateCustomerCommand.FirstName)));
    }
    [Test]
    public async Task ShouldThrowValidationExceptionForMissingLastName()
    {
        var command = new CreateCustomerCommand
        {
            FirstName = "John",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(CreateCustomerCommand.LastName)));
    }

    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidLastName()
    {
        var command = new CreateCustomerCommand
        {
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
                ex.Errors.Any(error => error.Key == nameof(CreateCustomerCommand.LastName)));
    }

    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidLastNameCharacters()
    {
        var command = new CreateCustomerCommand
        {
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
                ex.Errors.Any(error => error.Key == nameof(CreateCustomerCommand.LastName)));
    }
    [Test]
    public async Task ShouldThrowValidationExceptionForMissingDateOfBirth()
    {
        var command = new CreateCustomerCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Street = "123 Main St",
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(CreateCustomerCommand.DateOfBirth)));
    }

    [Test]
    public async Task ShouldThrowValidationExceptionForUnderageCustomer()
    {
        var command = new CreateCustomerCommand
        {
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
                ex.Errors.Any(error => error.Key == nameof(CreateCustomerCommand.DateOfBirth)));
    }

    [Test]
    public async Task ShouldThrowValidationExceptionForMissingStreet()
    {
        var command = new CreateCustomerCommand
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(CreateCustomerCommand.Street)));
    }
    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidStreetLength()
    {
        var command = new CreateCustomerCommand
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = new string('A',201),
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(CreateCustomerCommand.Street)));
    }
    [Test]
    public async Task ShouldThrowValidationExceptionForMissingCity()
    {
        var command = new CreateCustomerCommand
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            ZipCode = "12345",
            Country = "USA"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(CreateCustomerCommand.City)));
    }
    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidCityLength()
    {
        var command = new CreateCustomerCommand
        {
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
                ex.Errors.Any(error => error.Key == nameof(CreateCustomerCommand.City)));
    }
    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidCity()
    {
        var command = new CreateCustomerCommand
        {
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
                ex.Errors.Any(error => error.Key == nameof(CreateCustomerCommand.City)));
    }
    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidZipCodeLength()
    {
        var command = new CreateCustomerCommand
        {
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
                ex.Errors.Any(error => error.Key == nameof(CreateCustomerCommand.ZipCode)));
    }
    [Test]
    public async Task ShouldThrowValidationExceptionForMissingZipCode()
    {
        var command = new CreateCustomerCommand
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Anytown",
            Country = "USA"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(CreateCustomerCommand.ZipCode)));
    }
    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidZipCode()
    {
        var command = new CreateCustomerCommand
        {
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
                ex.Errors.Any(error => error.Key == nameof(CreateCustomerCommand.ZipCode)));
    }

    [Test]
    public async Task ShouldThrowValidationExceptionForMissingCountry()
    {
        var command = new CreateCustomerCommand
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Anytown",
            ZipCode = "12345"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>().Where(ex =>
                ex.Errors.Any(error => error.Key == nameof(CreateCustomerCommand.Country)));
    }

    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidCountry()
    {
        var command = new CreateCustomerCommand
        {
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
                ex.Errors.Any(error => error.Key == nameof(CreateCustomerCommand.Country)));
    }
    [Test]
    public async Task ShouldThrowValidationExceptionForInvalidCountryLength()
    {
        var command = new CreateCustomerCommand
        {
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
                ex.Errors.Any(error => error.Key == nameof(CreateCustomerCommand.Country)));
    }

}

