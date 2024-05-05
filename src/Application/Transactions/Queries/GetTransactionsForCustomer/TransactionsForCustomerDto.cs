using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AML.Domain.Entities;
using AML.Domain.Enums;

namespace AML.Application.Transactions.Queries.GetTransactionsForCustomer;

public class TransactionsForCustomerDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public CurrencyType Currency { get; set; }
    public TransactionType TransactionType { get; set; }
    public string? Description { get; set; }
    public bool IsSuspicious { get; set; }
    private class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Transaction, TransactionsForCustomerDto>();

        }
    }
}

