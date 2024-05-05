using AML.Application.Common.Interfaces;

namespace AML.Application.Limits.Queries.GetLimitChangesHistory;

public record GetLimitChangesHistoryQuery : IRequest<List<LimitHistoryDTO>>;

public class GetLimitChangesHistoryQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetLimitChangesHistoryQuery, List<LimitHistoryDTO>>
{



    public async Task<List<LimitHistoryDTO>> Handle(GetLimitChangesHistoryQuery request, CancellationToken cancellationToken)
    {
        return await context.LimitChangesHistory
             .ProjectTo<LimitHistoryDTO>(mapper.ConfigurationProvider)
            .ToListAsync();

    }
}
