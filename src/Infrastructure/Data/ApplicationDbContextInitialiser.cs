using System.Runtime.InteropServices;
using AML.Domain.Entities;
using AML.Domain.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AML.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
 

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
           
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        if (!_context.TransactionLimits.Any())
        {
            var transactionsLimit = new List<TransactionLimit>
            {

                new TransactionLimit { Currency = CurrencyType.AFN, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.ALL, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.DZD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.USD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.EUR, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.AOA, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.XCD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.ARS, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.AMD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.AWG, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.BSD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.BHD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.BDT, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.BBD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.BYN, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.BZD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.XOF, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.BMD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.BTN, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.INR, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.BOB, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.BOV, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.BAM, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.BWP, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.NOK, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.BRL, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.BND, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.BGN, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.CLF, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.CLP, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.CNY, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.COP, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.COU, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.KMF, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.CDF, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.NZD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.CRC, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.CUC, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.CUP, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.ANG, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.CZK, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.DKK, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.DJF, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.DOP, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.ERN, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.ETB, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.FKP, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.FJD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.XPF, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.GMD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.GEL, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.GHS, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.GIP, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.GTQ, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.GBP, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.GNF, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.GYD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.HTG, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.HKD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.HUF, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.ISK, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.IDR, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.XDR, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.IRR, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.IQD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.ILS, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.JMD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.JPY, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.JOD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.KZT, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.KES, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.KPW, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.KRW, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.KWD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.KGS, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.LAK, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.LBP, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.LSL, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.LRD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.LYD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.CHF, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.MOP, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.MGA, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.MWK, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.MYR, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.MVR, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.MXN, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.MXV, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.MDL, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.MNT, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.MUR, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.XUA, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.MAD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.MZN, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.MMK, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.NAD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.NPR, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.NIO, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.NGN, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.OMR, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.PKR, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.PAB, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.PGK, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.PYG, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.PEN, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.PHP, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.PLN, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.QAR, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.MKD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.RON, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.RUB, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.RWF, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.SHP, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.SBD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.SCR, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.SLL, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.SGD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.XSU, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.SOS, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.ZAR, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.SSP, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.LKR, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.SDG, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.SRD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.SEK, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.SYP, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.TWD, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.TJS, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.TZS, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.THB, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.TND, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.TRY, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.TMT, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.UGX, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.UAH, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.AED, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.UYI, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.UYU, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.UZS, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.VUV, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.VEF, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.VND, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.YER, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.ZMW, LimitValue = 1000 },
                new TransactionLimit { Currency = CurrencyType.ZWL, LimitValue = 1000 },
            };
          _context.TransactionLimits.AddRange(transactionsLimit);
            await _context.SaveChangesAsync();

        }
            if (!_context.Customers.Any())
        {

         var customers = new List<Customer>
         {
            new Customer
            {
                CustomerIdentifier =  Guid.NewGuid(),
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
                CustomerIdentifier =  Guid.NewGuid(),
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
                    new Transaction { Amount = 200.00m, Currency = CurrencyType.EUR, TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 1" },
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
                CustomerIdentifier =  Guid.NewGuid(),
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
                    new Transaction { Amount = 150.00m, Currency = CurrencyType.USD, TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 1" },
                    new Transaction { Amount = 25.00m, Currency = CurrencyType.EUR, TransactionType = TransactionType.Withdrawal, IsSuspicious = false, Description = "Transaction 2" },
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
                CustomerIdentifier =  Guid.NewGuid(),
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
                    new Transaction { Amount = 80.00m, Currency = CurrencyType.EUR, TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 1" },
                    new Transaction { Amount = 150.00m, Currency = CurrencyType.USD,  TransactionType = TransactionType.Withdrawal, IsSuspicious = false, Description = "Transaction 2" },
                    new Transaction { Amount = 90.00m, Currency = CurrencyType.USD,  TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 3" }
                },
                Balances =
                {
                    new CustomerBalance {Balance= 100.00m,Currency=CurrencyType.USD},
                    new CustomerBalance {Balance= 100.00m,Currency=CurrencyType.EUR}
                }
            },
            new Customer
            {
                CustomerIdentifier =  Guid.NewGuid(),
                FirstName = "David",
                LastName = "Wilson",
                DateOfBirth = new DateTime(1988, 9, 5),
                Address = new Address
                {
                    Street = "202 Maple St",
                    City = "Lasttown",
                    ZipCode = "24680",
                    Country = "Country 5"
                },
                Transactions =
                {
                    new Transaction { Amount = 1200.00m, Currency = CurrencyType.USD, TransactionType = TransactionType.Deposit, IsSuspicious = false, Description = "Transaction 1" },
                    new Transaction { Amount = 7000.00m, Currency = CurrencyType.EUR,  TransactionType = TransactionType.Withdrawal, IsSuspicious = false, Description = "Transaction 2" }
                },
                Balances =
                {
                    new CustomerBalance {Balance= 100.00m,Currency=CurrencyType.USD},
                    new CustomerBalance {Balance= 100.00m,Currency=CurrencyType.EUR}
                }
            }
        };

             _context.Customers.AddRange(customers);
            await _context.SaveChangesAsync();
        }
    }
}
