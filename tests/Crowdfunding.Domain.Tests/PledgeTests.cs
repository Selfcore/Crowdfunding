using Crowdfunding.Domain.Entities;
using Crowdfunding.Domain.Enums;

namespace Crowdfunding.Domain.Tests;

public class PledgeTests
{
    [Fact]
    public void Constructor_ShouldCreatePledgeInProgress()
    {
        var pledge = new Pledge(100m);

        Assert.NotEqual(Guid.Empty, pledge.Id);
        Assert.Equal(100m, pledge.Amount);
        Assert.Equal(PledgeStatus.InProgress, pledge.Status);
        Assert.True(
            pledge.CreatedAt >= DateTime.UtcNow.AddSeconds(-1) && 
            pledge.CreatedAt <= DateTime.UtcNow.AddSeconds(1));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Constructor_ShouldThrow_WhenAmountIsNotPositive(decimal amount)
    {
        var exception = Assert.Throws<ArgumentException>(() => new Pledge(amount));

        Assert.Equal("Pledge amount must be greater than zero.", exception.Message);
    }

    [Fact]
    public void Complete_ShouldChangeStatusToCompleted()
    {
        var pledge = new Pledge(100m);

        pledge.Complete();

        Assert.Equal(PledgeStatus.Completed, pledge.Status);
    }
    
    [Fact]
    public void Complete_ShouldThrow_WhenPledgeIsAlreadyCompleted()
    {
        var pledge = new Pledge(100m);
        pledge.Complete();

        var exception = Assert.Throws<InvalidOperationException>(() => pledge.Complete());

        Assert.Equal("Only an active pledge can be completed.", exception.Message);
    }
    
    [Fact]
    public void Complete_ShouldThrow_WhenPledgeIsCancelled()
    {
        var pledge = new Pledge(100m);
        pledge.Cancel();

        var exception = Assert.Throws<InvalidOperationException>(() => pledge.Complete());

        Assert.Equal("Only an active pledge can be completed.", exception.Message);
    }

    [Fact]
    public void Cancel_ShouldChangeStatusToCancelled()
    {
        var pledge = new Pledge(100m);

        pledge.Cancel();

        Assert.Equal(PledgeStatus.Cancelled, pledge.Status);
    }
}