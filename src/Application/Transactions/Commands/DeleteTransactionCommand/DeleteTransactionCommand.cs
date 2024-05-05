using AML.Application.Common.Interfaces;

namespace AML.Application.Transactions.Commands.DeleteTransactionCommand;

public record DeleteTransactionCommand(int Id) : IRequest;



public class DeleteTransactionCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteTransactionCommand>
{
  
    public async Task Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Transactions
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.Transactions.Remove(entity);


        await context.SaveChangesAsync(cancellationToken);
    }
}
