using Crowdfunding.Domain.Entities;

namespace Crowdfunding.Application.Interfaces;

public interface IPledgeRepository : IRepository<Pledge>
{
    Task<IReadOnlyList<Pledge>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default);
}