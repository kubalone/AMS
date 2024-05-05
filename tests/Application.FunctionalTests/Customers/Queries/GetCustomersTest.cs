using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AML.Application.Customers.Commands.CreateCustomer;
using AML.Application.Customers.Queries.GetCustomers;
using AML.Application.Transactions.Commands.CreateTransactionCommand;
using AML.Domain.Entities;
using AML.Domain.Enums;

namespace AML.Application.FunctionalTests.Customers.Queries;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Testing;

internal class GetCustomersTest : BaseTestFixture
{
    [Test]
    public async Task ShouldHaveProperPagination()
    {
        var customers = new List<Customer>
         {
            new Customer
            {
                CustomerIdentifier = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Address = new Address
                {
                    Street = "123 Main St",
                    City = "Anytown",
                    ZipCode = "12345",
                    Country = "Country 1"
                },
                Transactions =
                {
                    new Transaction { Amount = 100.00m, Currency = CurrencyType.USD, TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 1" },
                    new Transaction { Amount = 50.00m, Currency = CurrencyType.EUR,  TransactionType = TransactionType.Withdrawal, IsSuspicious = false, Description = "Transaction 2" },
                    new Transaction { Amount = 75.00m, Currency = CurrencyType.USD,  TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 3" }
                },
                Balances =
                {
                    new CustomerBalance {Balance= 100.00m,Currency=CurrencyType.USD},
                    new CustomerBalance {Balance= 100.00m,Currency=CurrencyType.EUR}
                }

            },
            new Customer
            {
                CustomerIdentifier = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = new DateTime(1985, 5, 15),
                Address = new Address
                {
                    Street = "456 Elm St",
                    City = "Othertown",
                    ZipCode = "54321",
                    Country = "Country 2"
                },
                Transactions =
                {
                    new Transaction { Amount = 200.00m, Currency = CurrencyType.EUR,  TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 1" },
                    new Transaction { Amount = 75.00m, Currency = CurrencyType.USD,  TransactionType = TransactionType.Withdrawal, IsSuspicious = false, Description = "Transaction 2" }
                }
                ,
                Balances =
                {
                    new CustomerBalance {Balance= 100.00m,Currency=CurrencyType.USD},
                    new CustomerBalance {Balance= 100.00m,Currency=CurrencyType.EUR}
                }
            },
            new Customer
            {
                CustomerIdentifier = Guid.NewGuid(),
                FirstName = "Mike",
                LastName = "Johnson",
                DateOfBirth = new DateTime(1983, 7, 20),
                Address = new Address
                {
                    Street = "789 Oak St",
                    City = "Anothertown",
                    ZipCode = "98765",
                    Country = "Country 3"
                },
                Transactions =
                {
                    new Transaction { Amount = 150.00m, Currency = CurrencyType.USD,  TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 1" },
                    new Transaction { Amount = 25.00m, Currency = CurrencyType.EUR,  TransactionType = TransactionType.Withdrawal, IsSuspicious = false, Description = "Transaction 2" },
                    new Transaction { Amount = 300.00m, Currency = CurrencyType.USD,  TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 3" }
                },
                Balances =
                {
                    new CustomerBalance {Balance= 100.00m,Currency=CurrencyType.USD},
                    new CustomerBalance {Balance= 100.00m,Currency=CurrencyType.EUR}
                }
            },
            new Customer
            {
                CustomerIdentifier = Guid.NewGuid(),
                FirstName = "Emily",
                LastName = "Brown",
                DateOfBirth = new DateTime(1992, 3, 10),
                Address = new Address
                {
                    Street = "101 Pine St",
                    City = "Yetanothertown",
                    ZipCode = "13579",
                    Country = "Country 4"
                },
                Transactions =
                {
                    new Transaction { Amount = 80.00m, Currency = CurrencyType.EUR,  TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 1" },
                    new Transaction { Amount = 150.00m, Currency = CurrencyType.USD,  TransactionType = TransactionType.Withdrawal, IsSuspicious = false, Description = "Transaction 2" },
                    new Transaction { Amount = 90.00m, Currency = CurrencyType.USD,  TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 3" }
                },
                Balances =
                {
                    new CustomerBalance {Balance= 100.00m,Currency=CurrencyType.USD},
                    new CustomerBalance {Balance= 100.00m,Currency=CurrencyType.EUR}
                }
            },
           
        };
        await AddRangeAsync(customers);
        var query = new GetCustomersQuery()
        {
            PageNumber = 1,
            PageSize = 2
        };
        var result = await SendAsync(query);

        result.Items.Should().NotBeNullOrEmpty();
        result.Items.Count.Should().BeLessOrEqualTo(query.PageSize);

        var expectedTotalPages = (int)Math.Ceiling((double)customers.Count / query.PageSize);

        result.TotalPages.Should().Be(expectedTotalPages);

        result.TotalCount.Should().Be(customers.Count);

        result.HasPreviousPage.Should().BeFalse(); 

        result.HasNextPage.Should().Be(expectedTotalPages > query.PageNumber);


    }
    [Test]
    public async Task ShouldReturnProperLinkedData()
    {
        var customers = new List<Customer>
         {
            new Customer
            {
                CustomerIdentifier= Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Address = new Address
                {
                    Street = "123 Main St",
                    City = "Anytown",
                    ZipCode = "12345",
                    Country = "Country 1"
                },
                Transactions =
                {
                    new Transaction { Amount = 100.00m, Currency = CurrencyType.USD,  TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 1" },
                    new Transaction { Amount = 50.00m, Currency = CurrencyType.EUR,  TransactionType = TransactionType.Withdrawal, IsSuspicious = false, Description = "Transaction 2" },
                    new Transaction { Amount = 75.00m, Currency = CurrencyType.USD,  TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 3" }
                },
                Balances =
                {
                    new CustomerBalance {Balance= 100.00m,Currency=CurrencyType.USD},
                    new CustomerBalance {Balance= 100.00m,Currency=CurrencyType.EUR}
                }

            }


        };
        await AddRangeAsync(customers);
        var query = new GetCustomersQuery()
        {
            PageNumber = 1,
            PageSize = 2
        };
        var result = await SendAsync(query);
        result.Items.Should().HaveCount(1);

        var customerDTO = result.Items.First();
        customerDTO.Address.Should().NotBeNull();
        customerDTO.Transactions.Should().HaveCount(3);
        customerDTO.Balances.Should().HaveCount(2);

    }
    [Test]
    public async Task ShouldReturnProperName()
    {
        var customers = new List<Customer>
         {
            new Customer
            {
                CustomerIdentifier= Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Address = new Address
                {
                    Street = "123 Main St",
                    City = "Anytown",
                    ZipCode = "12345",
                    Country = "Country 1"
                },
                Transactions =
                {
                    new Transaction { Amount = 100.00m, Currency = CurrencyType.USD,  TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 1" },
                    new Transaction { Amount = 50.00m, Currency = CurrencyType.EUR,  TransactionType = TransactionType.Withdrawal, IsSuspicious = false, Description = "Transaction 2" },
                    new Transaction { Amount = 75.00m, Currency = CurrencyType.USD,  TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 3" }
                },
                Balances =
                {
                    new CustomerBalance {Balance= 100.00m,Currency=CurrencyType.USD},
                    new CustomerBalance {Balance= 100.00m,Currency=CurrencyType.EUR}
                }

            },
            new Customer
            {
                CustomerIdentifier= Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = new DateTime(1985, 5, 15),
                Address = new Address
                {
                    Street = "456 Elm St",
                    City = "Othertown",
                    ZipCode = "54321",
                    Country = "Country 2"
                },
                Transactions =
                {
                    new Transaction { Amount = 200.00m, Currency = CurrencyType.EUR,  TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 1" },
                    new Transaction { Amount = 75.00m, Currency = CurrencyType.USD,  TransactionType = TransactionType.Withdrawal, IsSuspicious = false, Description = "Transaction 2" }
                }
                ,
                Balances =
                {
                    new CustomerBalance {Balance= 100.00m,Currency=CurrencyType.USD},
                    new CustomerBalance {Balance= 100.00m,Currency=CurrencyType.EUR}
                }
            },
            new Customer
            {
                CustomerIdentifier= Guid.NewGuid(),
                FirstName = "Mike",
                LastName = "Johnson",
                DateOfBirth = new DateTime(1983, 7, 20),
                Address = new Address
                {
                    Street = "789 Oak St",
                    City = "Anothertown",
                    ZipCode = "98765",
                    Country = "Country 3"
                },
                Transactions =
                {
                    new Transaction { Amount = 150.00m, Currency = CurrencyType.USD,  TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 1" },
                    new Transaction { Amount = 25.00m, Currency = CurrencyType.EUR,  TransactionType = TransactionType.Withdrawal, IsSuspicious = false, Description = "Transaction 2" },
                    new Transaction { Amount = 300.00m, Currency = CurrencyType.USD,  TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 3" }
                },
                Balances =
                {
                    new CustomerBalance {Balance= 100.00m,Currency=CurrencyType.USD},
                    new CustomerBalance {Balance= 100.00m,Currency=CurrencyType.EUR}
                }
            },
            new Customer
            {
                CustomerIdentifier= Guid.NewGuid(),
                FirstName = "Emily",
                LastName = "Brown",
                DateOfBirth = new DateTime(1992, 3, 10),
                Address = new Address
                {
                    Street = "101 Pine St",
                    City = "Yetanothertown",
                    ZipCode = "13579",
                    Country = "Country 4"
                },
                Transactions =
                {
                    new Transaction { Amount = 80.00m, Currency = CurrencyType.EUR,  TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 1" },
                    new Transaction { Amount = 150.00m, Currency = CurrencyType.USD,  TransactionType = TransactionType.Withdrawal, IsSuspicious = false, Description = "Transaction 2" },
                    new Transaction { Amount = 90.00m, Currency = CurrencyType.USD,  TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 3" }
                },
                Balances =
                {
                    new CustomerBalance {Balance= 100.00m,Currency=CurrencyType.USD},
                    new CustomerBalance {Balance= 100.00m,Currency=CurrencyType.EUR}
                }
            },

        };
        await AddRangeAsync(customers);
        var query = new GetCustomersQuery()
        {
            LastName= "Doe",
            PageNumber = 1,
            PageSize = 2
        };
        var result = await SendAsync(query);

        result.Items.Should().HaveCount(1);
        var customerDTO = result.Items.First();
        customerDTO.Should().NotBeNull();
        customerDTO.Transactions.Should().HaveCount(3);
        customerDTO.Balances.Should().HaveCount(2);
        customerDTO.FirstName.Should().Be("John");
        customerDTO.LastName.Should().Be("Doe");

    }
}
