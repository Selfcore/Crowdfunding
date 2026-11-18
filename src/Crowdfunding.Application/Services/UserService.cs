using Crowdfunding.Application.Dtos.User;
using Crowdfunding.Application.Interfaces;
using Crowdfunding.Application.Interfaces.Caching;
using Crowdfunding.Domain.Entities;

namespace Crowdfunding.Application.Services;

public class UserService
{
    private readonly IRepository<User> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public UserService(IRepository<User> repository, ICacheService cacheService, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _cacheService = cacheService;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<User>> GetAll(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }

    public async Task<UserDto?> GetById(Guid targetId, CancellationToken cancellationToken = default)
    {
        var cached = await _cacheService.GetAsync<UserDto>($"user:{targetId}", cancellationToken);
        
        if (cached is not null)
            return cached;
        
        var targetUser = await _repository.GetByIdAsync(targetId, cancellationToken);
        
        if (targetUser is null)
            return null;

        UserDto user = new UserDto
        {
            Id = targetUser.Id,
            Name = targetUser.Name,
            Email = targetUser.Email.Address,
            Balance = targetUser.Balance,
        };
        
        await _cacheService.SetAsync(
            $"user:{targetUser.Id.ToString()}", 
            user, 
            TimeSpan.FromMinutes(10), 
            cancellationToken);
        
        return user;
    }
    
    public async Task CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        User newUser = new User(request.Name, request.Email);
        
        await _repository.AddAsync(newUser, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var targetUser = await _repository.GetByIdAsync(id, cancellationToken);

        if (targetUser is null)
            throw new Exception("User not found");

        await _repository.Delete(targetUser, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveAsync($"user:{id}", cancellationToken);
    }
}