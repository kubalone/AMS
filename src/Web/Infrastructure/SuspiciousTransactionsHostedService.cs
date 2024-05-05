using System.Threading;
using AML.Application.Common.Interfaces;
using AML.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AML.Web.Infrastructure;

public class SuspiciousTransactionsHostedService: BackgroundService
{

    private readonly IServiceProvider _serviceProvider;

    public SuspiciousTransactionsHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
       

        using PeriodicTimer timer = new(TimeSpan.FromSeconds(30));

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await DoWork(stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine($"Service stopped");
        }
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var suspiciousTransactionsQuery = from t in context.Transactions
                                              join l in context.TransactionLimits on t.Currency equals l.Currency
                                              where !t.IsSuspicious && t.Amount >= l.LimitValue
                                              select t;

            var suspiciousTransactions = suspiciousTransactionsQuery.ToList();

            suspiciousTransactions.ForEach(t => t.IsSuspicious = true);

            await context.SaveChangesAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while scanning transactions: {ex.Message}");
        }
    }
}

