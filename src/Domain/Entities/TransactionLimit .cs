using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AML.Domain.Entities;
public class TransactionLimit : BaseAuditableEntity
{
    public int Id { get; set; }
    public CurrencyType Currency { get; set; }
    public decimal LimitValue { get; set; }

}
