using Crowdfunding.Application;
using Crowdfunding.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crowdfunding.Infrastructure.Repositories;

public class PledgeRepository : IRepository<Pledge>
{
    private readonly ApiDbContext _context;

    public PledgeRepository(ApiDbContext context)
    {
        _context = context;
    }
    
    public async Task<Pledge?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Pledges.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Pledge>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Pledges.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Pledge pledge, CancellationToken cancellationToken = default)
    {
        await _context.Pledges.AddAsync(pledge, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(Pledge pledge, CancellationToken cancellationToken = default)
    {
        _context.Pledges.Remove(pledge);
        await _context.SaveChangesAsync(cancellationToken);
    }
}