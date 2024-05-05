using AML.Application.Common.Interfaces;

namespace AML.Application.Transactions.Commands.SetSuspiciousTransactionCommand;

public record SetSuspiciousTransactionCommand: IRequest
{
    public int Id { get; init; }
    public bool IsSuspicious { get; init; }
}

public class SetSuspiciousTransactionCommandHandler(IApplicationDbContext context) : IRequestHandler<SetSuspiciousTransactionCommand>
{
 
    public async Task Handle(SetSuspiciousTransactionCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Transactions
          .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.IsSuspicious = request.IsSuspicious;
        await context.SaveChangesAsync(cancellationToken);
    }
}
