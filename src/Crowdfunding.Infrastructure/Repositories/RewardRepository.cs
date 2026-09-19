using Crowdfunding.Application;
using Crowdfunding.Domain.Entities;

namespace Crowdfunding.Infrastructure.Repositories;

public class RewardRepository : IRepository<Reward>
{
    private readonly Dictionary<Guid, Reward> _entities = [];
    
    public Task<Reward?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _entities.TryGetValue(id, out var entity);

        return Task.FromResult(entity);
    }

    public Task<IReadOnlyList<Reward>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Reward> result = _entities.Values.ToList();

        return Task.FromResult(result);
    }

    public Task AddAsync(Reward reward, CancellationToken cancellationToken = default)
    {
        _entities.Add(reward.Id, reward);

        return Task.CompletedTask;
    }

    public void Delete(Reward reward)
    {
        _entities.Remove(reward.Id);
    }
}