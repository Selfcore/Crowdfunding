using Crowdfunding.Application;
using Crowdfunding.Domain.Entities;

namespace Crowdfunding.Infrastructure.Repositories;

public class ProjectRepository : IRepository<Project>
{
    private readonly Dictionary<Guid, Project> _entities = [];
    
    public Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _entities.TryGetValue(id, out var entity);

        return Task.FromResult(entity);
    }

    public Task<IReadOnlyList<Project>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Project> result = _entities.Values.ToList();

        return Task.FromResult(result);
    }

    public Task AddAsync(Project project, CancellationToken cancellationToken = default)
    {
        _entities.Add(project.Id, project);

        return Task.CompletedTask;
    }

    public void Delete(Project project)
    {
        _entities.Remove(project.Id);
    }
}