using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AML.Application.Customers.Commands.CreateCustomer;
using AML.Application.Customers.Commands.DeleteCustomerCommand;
using NUnit.Framework.Interfaces;
using NUnit.Framework;
using AML.Domain.Entities;

namespace AML.Application.FunctionalTests.Customers.Commands;
using static Testing;

internal class DeleteCustomerTest : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidCustomerId()
    {
        var command = new DeleteCustomerCommand(99);
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }
    [Test]
    public async Task ShouldDeleteCustomer()
    {
        var command = new CreateCustomerCommand
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA"
        };

        var itemId = await SendAsync(command);
        await SendAsync(new DeleteCustomerCommand(itemId));
        var customer = await FindAsync<Customer>(itemId);

        customer.Should().BeNull();
    }
}
