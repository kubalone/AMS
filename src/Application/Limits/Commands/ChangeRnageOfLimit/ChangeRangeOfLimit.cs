using System.Collections.Generic;
using AML.Application.Common.Interfaces;
using AML.Domain.Entities;
using AML.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AML.Application.Limits.Commands.ChangeRnageOfLimit;

public record ChangeRangeOfLimitCommand : IRequest<int>
{
    public CurrencyType CurrencyType { get; init; }
    public decimal NewLimitValue { get; init; }
}



public class ChangeRangeOfLimitCommandHandler(IApplicationDbContext context) : IRequestHandler<ChangeRangeOfLimitCommand, int>
{


    public async Task<int> Handle(ChangeRangeOfLimitCommand request, CancellationToken cancellationToken)
    {

        var entity = await context.TransactionLimits.Where(p => p.Currency == request.CurrencyType).FirstOrDefaultAsync();
        
        Guard.Against.NotFound(request.CurrencyType, entity);

        if (entity.LimitValue != request.NewLimitValue)
        {
            var oldLimitValue = entity.LimitValue;
            entity.LimitValue = request.NewLimitValue;

            var history = CreateLimitChangeHistory(request.CurrencyType, oldLimitValue, request.NewLimitValue, entity.LastModified);


            context.LimitChangesHistory.Add(history);

            await context.SaveChangesAsync(cancellationToken);
        }
        return entity.Id;
    }
    private LimitChangeHistory CreateLimitChangeHistory(CurrencyType currencyType, decimal oldLimitValue, decimal newLimitValue, DateTimeOffset LastModified)
    {
        
        return new LimitChangeHistory
        {
            Currency = currencyType,
            OldLimitValue = oldLimitValue,
            NewLimitValue = newLimitValue,
            TimeOfStorageLimit= DateTimeOffset.UtcNow - LastModified
        };
    }
}
