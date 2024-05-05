using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AML.Domain.Entities;
public class LimitChangeHistory:BaseAuditableEntity

{
    public int Id { get; set; }

    public CurrencyType Currency { get; set; }

    public decimal OldLimitValue { get; set; }

    public decimal NewLimitValue { get; set; }

    public TimeSpan TimeOfStorageLimit { get; set; }
}
