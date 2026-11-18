using Crowdfunding.Application.Dtos.Pledge;
using Crowdfunding.Application.Dtos.Project;
using Crowdfunding.Application.Dtos.Reward;
using Crowdfunding.Application.Dtos.User;
using Crowdfunding.Application.Interfaces;
using Crowdfunding.Application.Interfaces.Caching;
using Crowdfunding.Domain.Entities;
using Crowdfunding.Domain.Enums;

namespace Crowdfunding.Application.Services;

public class ProjectService
{
    private readonly IRepository<Project> _projectRepository;
    private readonly IRewardRepository _rewardRepository;
    private readonly IPledgeRepository _pledgeRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly ICacheInvalidator _cacheInvalidator;
    
    public ProjectService(IRepository<Project> projectRepository, IRewardRepository rewardRepository,
        IPledgeRepository pledgeRepository, IRepository<User> userRepository, IUnitOfWork unitOfWork,
        ICacheService cacheService, ICacheInvalidator cacheInvalidator)
    {
        _projectRepository = projectRepository;
        _rewardRepository = rewardRepository;
        _pledgeRepository = pledgeRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _cacheInvalidator = cacheInvalidator;
    }
    
    public async Task<IReadOnlyList<Project>> GetAll(CancellationToken cancellationToken = default)
    {
        return await _projectRepository.GetAllAsync(cancellationToken);
    }

    public async Task<Project?> GetById(Guid targetId, CancellationToken cancellationToken = default)
    {
        return await _projectRepository.GetByIdAsync(targetId, cancellationToken);
    }
    
    public async Task<ProjectDetailsResponse> GetDetails(Guid projectId, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"project:{projectId}:details";

        var cached = await _cacheService.GetAsync<ProjectDetailsResponse>(cacheKey, cancellationToken);

        if (cached is not null)
            return cached;
        
        var project = await GetById(projectId, cancellationToken);

        if (project == null)
            throw new Exception("Project not found");

        var author = await _userRepository.GetByIdAsync(project.AuthorId, cancellationToken);
        
        var pledges = await _pledgeRepository.GetByProjectIdAsync(projectId, cancellationToken);

        var rewards = await _rewardRepository.GetByProjectIdAsync(projectId, cancellationToken);

        var collectedAmount = pledges
            .Where(p => p.Status == PledgeStatus.InProgress)
            .Sum(p => p.Amount);

        var progress = project.Threshold > 0 ? collectedAmount / project.Threshold * 100 : 0;

        var response = new ProjectDetailsResponse
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            Threshold = project.Threshold,

            CollectedAmount = collectedAmount,
            Progress = progress,
            PledgesCount = pledges.Count,

            Author = new ProjectAuthorResponse
            {
                Id = author.Id,
                Name = author.Name
            },

            Rewards = rewards.Select(r => new RewardResponse
            {
                Id = r.Id,
                Title = r.Title,
                Amount = r.Amount
            }).ToList()
        };

        await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(10), cancellationToken);
        
        return response;
    }

    public async Task<IReadOnlyList<Pledge>> GetAllPledges(Guid targetId,
        CancellationToken cancellationToken = default)
    {
        var target = await GetById(targetId, cancellationToken);
        
        if(target == null)
            throw new Exception("Project not found");

        var pledges = await _pledgeRepository.GetByProjectIdAsync(targetId, cancellationToken);

        return pledges;
    }
    
    public async Task<IReadOnlyList<Reward>> GetAllRewards(Guid targetId,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = $"project:{targetId}:rewards";
        
        var cached = await _cacheService.GetAsync<IReadOnlyList<Reward>>(cacheKey, cancellationToken);
        
        if (cached is not null)
            return cached;
        
        var target = await GetById(targetId, cancellationToken);
        
        if(target == null)
            throw new Exception("Project not found");

        var rewards = await _rewardRepository.GetByProjectIdAsync(targetId, cancellationToken);

        await _cacheService.SetAsync(cacheKey, rewards, TimeSpan.FromMinutes(10), cancellationToken);
        
        return rewards;
    }

    public async Task CreateReward(Guid projectId, CreateRewardRequest request, CancellationToken cancellationToken = default)
    {
        var targetProject = await GetById(projectId, cancellationToken);

        if (targetProject == null)
            throw new Exception("Project not found");

        Reward newReward = new Reward(projectId, request.Title, request.Amount);
        
        await _rewardRepository.AddAsync(newReward, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _cacheInvalidator.RemoveByPrefixAsync($"project:{targetProject.Id}", cancellationToken);
    }
    
    public async Task UpdateReward(
        Guid projectId,
        Guid rewardId,
        UpdateRewardRequest request,
        CancellationToken cancellationToken = default)
    {
        var project = await GetById(projectId, cancellationToken);

        if (project == null)
            throw new Exception("Project not found");

        var reward = await _rewardRepository.GetByIdAsync(rewardId, cancellationToken);

        if (reward == null)
            throw new Exception("Reward not found");

        if (reward.ProjectId != projectId)
            throw new Exception("Reward does not belong to this project");
        
        reward.Update(request.Title, request.Amount);
        
        await _rewardRepository.UpdateAsync(reward, cancellationToken);
        await _cacheInvalidator.RemoveByPrefixAsync($"project:{project.Id}", cancellationToken);
    }
    
    public async Task<Pledge> CreatePledge(Guid projectId, CreatePledgeRequest request, CancellationToken cancellationToken = default)
    {
        var project = await GetById(projectId, cancellationToken);

        if (project == null)
            throw new Exception("Project not found");

        var user = await _userRepository.GetByIdAsync(request.BackerId, cancellationToken);

        if (user == null)
            throw new Exception("User not found");
        
        decimal amount;

        if (request.RewardId.HasValue)
        {
            var reward = await _rewardRepository.GetByIdAsync(request.RewardId.Value, cancellationToken);

            if (reward == null)
                throw new Exception("Reward not found");

            if (reward.ProjectId != projectId)
                throw new Exception("Reward does not belong to this project");

            amount = reward.Amount;
        }
        else
        {
            if (!request.Amount.HasValue || request.Amount <= 0)
                throw new ArgumentException("Amount must be greater than zero");

            amount = request.Amount.Value;
        }
        
        user.Withdraw(amount);

        var pledge = new Pledge(amount)
        {
            ProjectId = projectId,
            UserId = request.BackerId,
            RewardId = request.RewardId
        };
        
        await _pledgeRepository.AddAsync(pledge, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _cacheInvalidator.RemoveByPrefixAsync($"project:{project.Id}", cancellationToken);
        
        return pledge;
    }
    
    public async Task CreateAsync(CreateProjectRequest request, CancellationToken cancellationToken = default)
    {
        Project newProject = new Project(request.AuthorId, request.Title, request.Description, request.Threshold);
        
        await _projectRepository.AddAsync(newProject, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var targetProject = await GetById(id, cancellationToken);

        if (targetProject == null)
            throw new Exception("Project not found");
        
        await _projectRepository.Delete(targetProject, cancellationToken);
        await _cacheInvalidator.RemoveByPrefixAsync($"project:{targetProject.Id}", cancellationToken);
    }
}