using Crowdfunding.Application;
using Crowdfunding.Application.Interfaces;
using Crowdfunding.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crowdfunding.Infrastructure.Repositories;

public class RewardRepository : IRewardRepository
{
    private readonly ApiDbContext _context;

    public RewardRepository(ApiDbContext context)
    {
        _context = context;
    }
    
    public async Task<Reward?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Rewards.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Reward>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Rewards.ToListAsync(cancellationToken);
    }
    
    public async Task<IReadOnlyList<Reward>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        return await _context.Rewards.Where(p => p.ProjectId == projectId).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Reward reward, CancellationToken cancellationToken = default)
    {
        await _context.Rewards.AddAsync(reward, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(Reward reward, CancellationToken cancellationToken = default)
    {
        _context.Rewards.Remove(reward);
        await _context.SaveChangesAsync(cancellationToken);
    }
}