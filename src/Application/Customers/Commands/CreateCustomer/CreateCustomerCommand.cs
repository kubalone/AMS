using System.Text.Json.Serialization;
using AML.Application.Common.Interfaces;
using AML.Domain.Entities;
using AML.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace AML.Application.Customers.Commands.CreateCustomer;

public record CreateCustomerCommand : IRequest<int>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
    public ICollection<CustomerBalanceDto>? Balances { get; init; }

}

public record CustomerBalanceDto
{
    public CurrencyType Currency { get; set; }
    public decimal Balance { get; set; }
}

public class CreateCustomerCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateCustomerCommand, int>
{


    public async Task<int> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await CreateCustomerAsync(command, cancellationToken);
        await AddCustomerBalancesAsync(command, customer.Id, cancellationToken);
        return customer.Id;

    }
    private async Task<Customer> CreateCustomerAsync(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            CustomerIdentifier=  Guid.NewGuid(),
            FirstName = command.FirstName,
            LastName = command.LastName,
            DateOfBirth = command.DateOfBirth,
            Address = new Address
            {
                Street = command.Street,
                City = command.City,
                ZipCode = command.ZipCode,
                Country = command.Country
            }
        };

        context.Customers.Add(customer);
        await context.SaveChangesAsync(cancellationToken);
        return customer;
    }

    private async Task AddCustomerBalancesAsync(CreateCustomerCommand command, int customerId, CancellationToken cancellationToken)
    {
        if (command.Balances != null)
        {
            foreach (var balanceDto in command.Balances)
            {
                var customerBalance = new CustomerBalance
                {
                    CustomerId = customerId,
                    Currency = balanceDto.Currency,
                    Balance = balanceDto.Balance
                };

                context.CustomerBalances.Add(customerBalance);
            }

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
