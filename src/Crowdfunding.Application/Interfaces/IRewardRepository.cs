using Crowdfunding.Domain.Entities;

namespace Crowdfunding.Application.Interfaces;

public interface IRewardRepository : IRepository<Reward>
{
    Task<IReadOnlyList<Reward>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default);
}