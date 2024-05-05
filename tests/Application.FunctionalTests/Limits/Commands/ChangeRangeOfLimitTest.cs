using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AML.Application.Limits.Commands.ChangeRnageOfLimit;
using AML.Application.Transactions.Commands.SetSuspiciousTransactionCommand;
using AML.Domain.Entities;
using AML.Domain.Enums;

namespace AML.Application.FunctionalTests.Limits.Commands;
using static Testing;

internal class ChangeRangeOfLimitTest : BaseTestFixture
{
    [Test]
    public async Task ShouldChangeRangeOfLimit()
    {
        var transactionLimit = new TransactionLimit()
        {
            Currency = CurrencyType.USD,
            LimitValue = 100.00m
        };
        await AddAsync(transactionLimit);


        var changeRangeOfLimitCommand = new ChangeRangeOfLimitCommand
        {
            CurrencyType = CurrencyType.USD,
            NewLimitValue = 200.00m
        };
        var id=await SendAsync(changeRangeOfLimitCommand);

        var transactionLimitAfterUpdate = await FindAsync<TransactionLimit>(id);

        transactionLimitAfterUpdate.Should().NotBeNull();
        transactionLimitAfterUpdate!.LimitValue.Should().Be(200.00m);
        transactionLimitAfterUpdate.Currency.Should().Be(CurrencyType.USD);
    }

    [Test]
    public async Task ShouldCreateHistoryOfChangeRangeLimit()
    {
        var transactionLimit = new TransactionLimit()
        {
            Currency = CurrencyType.USD,
            LimitValue = 100.00m
        };
        await AddAsync(transactionLimit);


        var changeRangeOfLimitCommand = new ChangeRangeOfLimitCommand
        {
            CurrencyType = CurrencyType.USD,
            NewLimitValue = 0
        };
        await SendAsync(changeRangeOfLimitCommand);


        var transactionLimitHistory= await FindAsync<LimitChangeHistory>(1);

        transactionLimitHistory.Should().NotBeNull();
        transactionLimitHistory!.OldLimitValue.Should().Be(transactionLimit.LimitValue);
        transactionLimitHistory!.NewLimitValue.Should().Be(changeRangeOfLimitCommand.NewLimitValue);
    

        transactionLimitHistory!.Currency.Should().Be(changeRangeOfLimitCommand.CurrencyType);

    }
    [Test]
    public async Task ShouldRequireValidCurrency()
    {
        var transactionLimit = new TransactionLimit()
        {
            Currency = CurrencyType.USD,
            LimitValue = 100.00m
        };
        await AddAsync(transactionLimit);


        var command = new ChangeRangeOfLimitCommand
        {
            CurrencyType = CurrencyType.ERN,
            NewLimitValue = 200.00m
        };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }
}
