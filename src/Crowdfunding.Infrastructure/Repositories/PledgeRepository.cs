using Crowdfunding.Application;
using Crowdfunding.Domain.Entities;

namespace Crowdfunding.Infrastructure.Repositories;

public class PledgeRepository : IRepository<Pledge>
{
    private readonly Dictionary<Guid, Pledge> _entities = [];
    
    public Task<Pledge?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _entities.TryGetValue(id, out var entity);

        return Task.FromResult(entity);
    }

    public Task<IReadOnlyList<Pledge>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Pledge> result = _entities.Values.ToList();

        return Task.FromResult(result);
    }

    public Task AddAsync(Pledge pledge, CancellationToken cancellationToken = default)
    {
        _entities.Add(pledge.Id, pledge);

        return Task.CompletedTask;
    }

    public void Delete(Pledge pledge)
    {
        _entities.Remove(pledge.Id);
    }
}