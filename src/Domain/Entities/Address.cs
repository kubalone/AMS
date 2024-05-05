using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AML.Domain.Entities;
public class Address:BaseAuditableEntity
{
    public int Id { get; set; }
    public  string? Street { get; set; }
    public  string? City { get; set; }
    public  string? ZipCode { get; set; }
    public  string? Country { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
}
