namespace Crowdfunding.Application.Dtos.Pledge;

public class CreatePledgeRequest
{
    public Guid BackerId { get; set; }
    public Guid? RewardId { get; set; }   
    public decimal? Amount { get; set; }
}