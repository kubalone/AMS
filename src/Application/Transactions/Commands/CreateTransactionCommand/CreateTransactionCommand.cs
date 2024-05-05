using AML.Application.Common.Interfaces;
using AML.Domain.Entities;
using AML.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AML.Application.Transactions.Commands.CreateTransactionCommand;

public record CreateTransactionCommand: IRequest<int>
{
    public decimal Amount { get; set; }
    public CurrencyType Currency { get; set; }
    public TransactionType TransactionType { get; set; }
    public int CustomerId { get; set; }
    public string? Description { get; set; }
}



public class CreateTransactionCommandHandler(IApplicationDbContext context ) : IRequestHandler<CreateTransactionCommand, int>
{


    public async Task<int> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var customer = await FindCustomerAsync(request.CustomerId);
        Guard.Against.NotFound(request.CustomerId, customer);

        var limit = await GetTransactionLimitForCurrency(request.Currency, cancellationToken);
        var isSuspicious = CheckTransactionSuspiciousness(request.Amount, limit?.LimitValue);
        var transaction = CreateTransactionFromCommand(request, isSuspicious);

        context.Transactions.Add(transaction);
        UpdateCustomerBalance(request, customer);

        await context.SaveChangesAsync(cancellationToken);
        return transaction.Id;
    }

    private async Task<Customer?> FindCustomerAsync(int customerId)
    {
        return await context.Customers.Include(p => p.Balances).FirstOrDefaultAsync(p => p.Id == customerId);
    }

    private async Task<TransactionLimit?> GetTransactionLimitForCurrency(CurrencyType currency, CancellationToken cancellationToken)
    {
        return await context.TransactionLimits.FirstOrDefaultAsync(t => t.Currency == currency, cancellationToken);
    }

    private bool CheckTransactionSuspiciousness(decimal amount, decimal? limitValue)
    {
        return limitValue.HasValue && amount >= limitValue.Value;
    }

    private Transaction CreateTransactionFromCommand(CreateTransactionCommand request, bool isSuspicious)
    {
        return new Transaction
        {
            Amount = request.Amount,
            Currency = request.Currency,
            TransactionType = request.TransactionType,
            CustomerId = request.CustomerId,
            Description = request.Description,
            IsSuspicious = isSuspicious,
        };
    }

    private void UpdateCustomerBalance(CreateTransactionCommand request, Customer customer)
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
