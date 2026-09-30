using Crowdfunding.Application.Interfaces;
using Crowdfunding.Domain.Entities;

namespace Crowdfunding.Application.Services;

public class UserService
{
    private readonly IRepository<User> _repository;

    public UserService(IRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<User>> GetAll(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }

    public async Task<User?> GetById(Guid targetId, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(targetId, cancellationToken);
    }
    
    public async Task CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        await _repository.AddAsync(user, cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var targetUser = await GetById(id, cancellationToken);

        if (targetUser == null)
            throw new Exception("User not found");
        
        await _repository.Delete(targetUser, cancellationToken);
    }
}