using AML.Application.Common.Models;
using AML.Application.Customers.Commands.CreateCustomer;
using AML.Application.Customers.Commands.DeleteCustomerCommand;
using AML.Application.Customers.Commands.UpdateCustomerCommand;
using AML.Application.Customers.Queries.GetCustomers;

namespace AML.Web.Endpoints;

public class Customers : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetCustomers)
            .MapPost(CreateCustomer)
            .MapPut(UpdateCustomer, "{id}")
            .MapDelete(DeleteCustomer, "{id}");

    }
    public async Task<PaginatedList<CustomerDTO>> GetCustomers(ISender sender, [AsParameters] GetCustomersQuery query)
    {
        return await sender.Send(query);
    }
    public async Task<int> CreateCustomer(ISender sender, CreateCustomerCommand command)
    {
        return await sender.Send(command);
    }
    public async Task<IResult> UpdateCustomer(ISender sender, int id, UpdateCustomerCommand command)
    {
        if (id != command.CustomerId) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }
    public async Task<IResult> DeleteCustomer(ISender sender, int id)
    {
        await sender.Send(new DeleteCustomerCommand(id));
        return Results.NoContent();
    }

}
