namespace Crowdfunding.Application.Dtos.Reward;

public sealed class UpdateRewardRequest
{
    public string Title { get; set; }
    public decimal Amount { get; set; }
}