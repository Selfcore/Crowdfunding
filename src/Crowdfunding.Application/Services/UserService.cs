using Crowdfunding.Application.Dtos.User;
using Crowdfunding.Application.Interfaces;
using Crowdfunding.Domain.Entities;

namespace Crowdfunding.Application.Services;

public class UserService
{
    private readonly IRepository<User> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IRepository<User> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<User>> GetAll(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }

    public async Task<User?> GetById(Guid targetId, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(targetId, cancellationToken);
    }
    
    public async Task CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        User newUser = new User(request.Name, request.Email);
        
        await _repository.AddAsync(newUser, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var targetUser = await GetById(id, cancellationToken);

        if (targetUser == null)
            throw new Exception("User not found");
        
        await _repository.Delete(targetUser, cancellationToken);
    }
}