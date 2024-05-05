using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AML.Domain.Entities;
public class Transaction: BaseAuditableEntity
{
    public int Id { get; set; } 
    public decimal Amount { get; set; } 
    public CurrencyType Currency { get; set; } 
    public TransactionType TransactionType { get; set; } 
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsSuspicious { get; set; }
}
