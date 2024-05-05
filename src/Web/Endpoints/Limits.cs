using AML.Application.Common.Models;
using AML.Application.Customers.Commands.CreateCustomer;
using AML.Application.Customers.Commands.UpdateCustomerCommand;
using AML.Application.Customers.Queries.GetCustomers;
using AML.Application.Limits.Commands.ChangeRnageOfLimit;
using AML.Application.Limits.Queries.GetLimitChangesHistory;

namespace AML.Web.Endpoints;

public class Limits : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetLimitChangesHistory)
            .MapPost(ChangeRangeOfLimit);

    }
    public async Task<List<LimitHistoryDTO>> GetLimitChangesHistory(ISender sender, [AsParameters] GetLimitChangesHistoryQuery query)
    {
         return await sender.Send(query);

    }
    public async Task<int> ChangeRangeOfLimit(ISender sender, ChangeRangeOfLimitCommand command)
    {
        return await sender.Send(command);

    }

}
