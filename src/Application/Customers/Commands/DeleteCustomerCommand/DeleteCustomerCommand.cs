using AML.Application.Common.Interfaces;

namespace AML.Application.Customers.Commands.DeleteCustomerCommand;

public record DeleteCustomerCommand(int Id) : IRequest;




public class DeleteCustomerCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteCustomerCommand>
{


    public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Customers
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.Customers.Remove(entity);


        await context.SaveChangesAsync(cancellationToken);
    }
}
