using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AML.Application.Transactions.Queries.GetTransactions;
using AML.Domain.Common;
using AML.Domain.Entities;
using AML.Domain.Enums;

namespace AML.Application.Customers.Queries.GetCustomers;
public class CustomerDTO
{
        public int Id { get; set; }
        public Guid CustomerIdentifier { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public IList<TransactionDto> Transactions { get; set; } = new List<TransactionDto>();
        public IList<CustomerBalanceDto> Balances { get; set; } = new List<CustomerBalanceDto>();

    public AddressDto? Address { get; set; }
       
}

    public class TransactionDto
    {
         public int Id { get; set; }

        public decimal Amount { get; set; }
        public CurrencyType Currency { get; set; }
        public TransactionType TransactionType { get; set; }
        public string? Description { get; set; }
        public bool IsSuspicious { get; set; }
    }

    public class AddressDto
    {
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }
    }
    public class CustomerBalanceDto
    {
        public decimal Balance { get; set; }
        public CurrencyType Currency { get; set; }


    }
public class Mapping : Profile
{
    public Mapping()
    {
        CreateMap<Customer, CustomerDTO>();
        CreateMap<Transaction, TransactionDto>();
        CreateMap<Address, AddressDto>();
        CreateMap<CustomerBalance, CustomerBalanceDto>();

    }
}
