using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AML.Domain.Entities;
public class CustomerBalance: BaseAuditableEntity
{
    public int Id { get; set; }
    public decimal Balance { get; set; }
    public CurrencyType Currency { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

}
