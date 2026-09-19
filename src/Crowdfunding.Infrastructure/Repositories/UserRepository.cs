using System.Collections.Concurrent;
using Crowdfunding.Application;
using Crowdfunding.Domain.Entities;

namespace Crowdfunding.Infrastructure.Repositories;

public class UserRepository : IRepository<User>
{
    private readonly ConcurrentDictionary<Guid, User> _entities = [];
    
    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _entities.TryGetValue(id, out var entity);

        return Task.FromResult(entity);
    }

    public Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<User> result = _entities.Values.ToList();

        return Task.FromResult(result);
    }

    public Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        _entities.TryAdd(user.Id, user);

        return Task.CompletedTask;
    }

    public void Delete(User user)
    {
        _entities.TryRemove(user.Id, out _);
    }
}