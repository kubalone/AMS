using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AML.Application.Customers.Queries.GetCustomers;
using AML.Domain.Entities;
using AML.Domain.Enums;

namespace AML.Application.Transactions.Queries.GetTransactions;
public class TransactionDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public CurrencyType Currency { get; set; }
    public TransactionType TransactionType { get; set; }
    public CustomerDTO Customer { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsSuspicious { get; set; }

}
public class CustomerDTO
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public AddressDTO? Address { get; set; }
}

public class AddressDTO
{
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
}
public class Mapping : Profile
{
    public Mapping()
    {
        CreateMap<Transaction, TransactionDto>();
        CreateMap<Customer, CustomerDTO>();
        CreateMap<Address, AddressDTO>();


    }
}
