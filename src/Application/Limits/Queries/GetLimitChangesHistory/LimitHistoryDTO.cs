using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AML.Domain.Entities;
using AML.Domain.Enums;

namespace AML.Application.Limits.Queries.GetLimitChangesHistory;
public class LimitHistoryDTO
{
    public int Id { get; set; }

    public CurrencyType Currency { get; set; }

    public decimal OldLimitValue { get; set; }

    public decimal NewLimitValue { get; set; }

    public DateTime ChangeDate { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<LimitChangeHistory, LimitHistoryDTO>();
        }
    }
}
