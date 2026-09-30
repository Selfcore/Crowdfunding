using Crowdfunding.Application.Dtos.Reward;
using Crowdfunding.Application.Dtos.User;

namespace Crowdfunding.Application.Dtos.Project;

public sealed class ProjectDetailsResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Threshold { get; set; }

    public decimal CollectedAmount { get; set; }
    public decimal Progress { get; set; }
    public int PledgesCount { get; set; }

    public ProjectAuthorResponse Author { get; set; }
    public IReadOnlyList<RewardResponse> Rewards { get; set; }
}