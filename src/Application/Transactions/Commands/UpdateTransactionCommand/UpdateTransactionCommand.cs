using AML.Application.Common.Interfaces;
using AML.Application.Customers.Commands.UpdateCustomerCommand;
using AML.Domain.Entities;
using AML.Domain.Enums;
using AutoMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AML.Application.Transactions.Commands.UpdateTransactionCommand;

public record UpdateTransactionCommand: IRequest
{
    public int TransactionId { get; init; }
    public decimal Amount { get; init; }
    public CurrencyType Currency { get; init; }
    public TransactionType TransactionType { get; init; }
    public string? Description { get; init; }
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UpdateTransactionCommand, Domain.Entities.Transaction>()
                 .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
                 .ForMember(dest => dest.Customer, opt => opt.Ignore())
                 .ForMember(dest => dest.IsSuspicious, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TransactionId));
          
        }
    }
}



public class UpdateTransactionCommandHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<UpdateTransactionCommand>
{


    public async Task Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await context.Transactions
            .Include(t => t.Customer)
            .ThenInclude(t=>t.Balances)
            .FirstOrDefaultAsync(c => c.Id == request.TransactionId, cancellationToken);

        Guard.Against.NotFound(request.TransactionId, transaction);

        var limit = await GetTransactionLimitForCurrency(request.Currency, cancellationToken);
        var isSuspicious = CheckTransactionSuspiciousness(request.Amount, limit?.LimitValue);
        transaction.IsSuspicious=isSuspicious;

        mapper.Map(request, transaction);
        UpdateCustomerBalance(request, transaction.Customer);
        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task<TransactionLimit?> GetTransactionLimitForCurrency(CurrencyType currency, CancellationToken cancellationToken)
    {
        return await context.TransactionLimits.FirstOrDefaultAsync(t => t.Currency == currency, cancellationToken);
    }

    private bool CheckTransactionSuspiciousness(decimal amount, decimal? limitValue)
    {
        return limitValue.HasValue && amount >= limitValue.Value;
    }
    private void UpdateCustomerBalance(UpdateTransactionCommand request, Customer customer)
    {
        var balance = customer.Balances.FirstOrDefault(b => b.Currency == request.Currency);

        if (request.TransactionType == TransactionType.Deposit)
        {
            if (balance != null)
            {
                balance.Balance += request.Amount;
            }
            else
            {
                var newBalance = new CustomerBalance
                {
                    Balance = request.Amount,
                    Currency = request.Currency,
                    CustomerId = customer.Id,
                    Customer = customer
                };
                customer.Balances.Add(newBalance);
            }
        }
        else if (request.TransactionType == TransactionType.Withdrawal)
        {
            if (balance != null)
            {
                if (balance.Balance >= request.Amount)
                {
                    balance.Balance -= request.Amount;
                }
                else
                {
                    throw new InvalidOperationException("Insufficient funds.");
                }
            }
            else
            {
                throw new InvalidOperationException("Balance not found for the specified currency.");
            }
        }
    }
}
