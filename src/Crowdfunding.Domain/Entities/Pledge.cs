using Crowdfunding.Domain.Enums;

namespace Crowdfunding.Domain.Entities;

public class Pledge
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }
    public Guid RewardId { get; set; }
    public decimal Amount { get; set; }
    public PledgeStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    
    private Pledge() { }

    public Pledge(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Pledge amount must be greater than zero.");
        
        Id = Guid.NewGuid();
        Amount = amount;
        Status = PledgeStatus.InProgress;
        CreatedAt = DateTime.UtcNow;
    }
}