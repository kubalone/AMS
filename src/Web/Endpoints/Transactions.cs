using AML.Application.Common.Models;
using AML.Application.Transactions.Commands.CreateTransactionCommand;
using AML.Application.Transactions.Commands.DeleteTransactionCommand;
using AML.Application.Transactions.Commands.SetSuspiciousTransactionCommand;
using AML.Application.Transactions.Commands.UpdateTransactionCommand;
using AML.Application.Transactions.Queries.GetTransactions;
using AML.Application.Transactions.Queries.GetTransactionsForCustomer;

namespace AML.Web.Endpoints;

public class Transactions : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetTransactions, "GetTransactions")
            .MapGet(GetTransactionsForCustomer, "GetTransactionsForCustomer")
            .MapPost(CreateTransaction)
            .MapPut(SetSuspiciousTransaction, "SetSuspiciousTransaction/{id}")
            .MapPut(UpdateTransaction, "UpdateTransaction/{id}")
            .MapDelete(DeleteTransaction, "{id}");

    }
    public async Task<PaginatedList<TransactionDto>> GetTransactions(ISender sender, [AsParameters] GetTransactionsQuery query)
    {
        return await sender.Send(query);
    }
    public async Task<PaginatedList<TransactionsForCustomerDto>> GetTransactionsForCustomer(ISender sender, [AsParameters] GetTransactionsForCustomerQuery query)
    {
        return await sender.Send(query);
    }
    public async Task<int> CreateTransaction(ISender sender, CreateTransactionCommand command)
    {
        return await sender.Send(command);
    }
    public async Task<IResult> SetSuspiciousTransaction(ISender sender, int id, SetSuspiciousTransactionCommand command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }
    public async Task<IResult> UpdateTransaction(ISender sender, int id, UpdateTransactionCommand command)
    {
        if (id != command.TransactionId) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }
    public async Task<IResult> DeleteTransaction(ISender sender, int id)
    {
        await sender.Send(new DeleteTransactionCommand(id));
        return Results.NoContent();
    }

}
