using AML.Application.Common.Interfaces;
using AML.Application.Customers.Commands.CreateCustomer;
using AML.Domain.Entities;
using AML.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AML.Application.Customers.Commands.UpdateCustomerCommand;

public record UpdateCustomerCommand : IRequest
{
    public int CustomerId { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public DateTime DateOfBirth { get; init; }
    public string? Street { get; init; }
    public string? City { get; init; }
    public string? ZipCode { get; init; }
    public string? Country { get; init; }
    public ICollection<CustomerBalanceDto>? Balances { get; init; }
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UpdateCustomerCommand, Customer>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.Address, opt => opt.Ignore())
                .ForMember(dest => dest.Transactions, opt => opt.Ignore())
                .ForMember(dest => dest.Balances, opt => opt.Ignore());
            CreateMap<UpdateCustomerCommand, Address>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore());
            
        }
    }
}
public record CustomerBalanceDto
{
    public CurrencyType Currency { get; init; }
    public decimal Balance { get; init; }
}



public class UpdateCustomerCommandHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<UpdateCustomerCommand>
{
   
    public async Task Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
      
        var customer = await GetCustomerAsync(command.CustomerId, cancellationToken);

        Guard.Against.NotFound(command.CustomerId, customer);

        mapper.Map(command, customer);

        mapper.Map(command, customer.Address);

        UpdateCustomerBalancesAsync(command, customer);

        await context.SaveChangesAsync(cancellationToken);
    }
    private async Task<Customer?> GetCustomerAsync(int customerId, CancellationToken cancellationToken)
    {
        return await context.Customers
            .Include(c => c.Address)
            .Include(c => c.Balances)
            .FirstOrDefaultAsync(c => c.Id == customerId, cancellationToken);
      
    }

  

    private void UpdateCustomerBalancesAsync(UpdateCustomerCommand command, Customer customer)
    {
        if (command.Balances != null)
        {
            foreach (var balanceDto in command.Balances)
            {
                var customerBalance = customer.Balances.FirstOrDefault(b => b.Currency == balanceDto.Currency);
                if (customerBalance != null)
                {
                    customerBalance.Balance = balanceDto.Balance;
                }
                else
                {
                    customer.Balances.Add(new CustomerBalance
                    {
                        Currency = balanceDto.Currency,
                        Balance = balanceDto.Balance
                    });
                }
            }
        }
    }
}

