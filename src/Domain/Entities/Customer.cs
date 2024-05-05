using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AML.Domain.Entities;
public  class Customer: BaseAuditableEntity
{
    public int Id { get; set; }
    public  string? FirstName { get; set; }
    public  string? LastName { get; set; }
    public Guid CustomerIdentifier { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Address? Address { get; set; }
    public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();
    public ICollection<CustomerBalance> Balances { get; set; } = new List<CustomerBalance>();

}
