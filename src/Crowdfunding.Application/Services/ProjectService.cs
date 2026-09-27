using Crowdfunding.Application.Interfaces;
using Crowdfunding.Domain.Entities;

namespace Crowdfunding.Application.Services;

public class ProjectService
{
    private readonly IRepository<Project> _projectRepository;
    private readonly IRewardRepository _rewardRepository;
    private readonly IPledgeRepository _pledgeRepository;
    
    public ProjectService(IRepository<Project> projectRepository, IRewardRepository rewardRepository,
        IPledgeRepository pledgeRepository)
    {
        _projectRepository = projectRepository;
        _rewardRepository = rewardRepository;
        _pledgeRepository = pledgeRepository;
    }
    
    public async Task<IReadOnlyList<Project>> GetAll(CancellationToken cancellationToken = default)
    {
        return await _projectRepository.GetAllAsync(cancellationToken);
    }

    public async Task<Project?> GetById(Guid targetId, CancellationToken cancellationToken = default)
    {
        return await _projectRepository.GetByIdAsync(targetId, cancellationToken);
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
        var target = await GetById(targetId, cancellationToken);
        
        if(target == null)
            throw new Exception("Project not found");

        var rewards = await _rewardRepository.GetByProjectIdAsync(targetId, cancellationToken);

        return rewards;
    }

    public async Task CreateReward(Guid projectId, Reward reward, CancellationToken cancellationToken = default)
    {
        var targetProject = await GetById(projectId, cancellationToken);

        if (targetProject == null)
            throw new Exception("Project not found");

        reward.ProjectId = projectId;
        
        await _rewardRepository.AddAsync(reward, cancellationToken);
    }
    
    public async Task<Pledge> CreatePledge(
        Guid projectId,
        Guid userId,
        decimal amount,
        Guid? rewardId = null,
        CancellationToken cancellationToken = default)
    {
        var project = await GetById(projectId, cancellationToken);

        if (project == null)
            throw new Exception("Project not found");

        if (rewardId.HasValue)
        {
            var reward = await _rewardRepository.GetByIdAsync(
                rewardId.Value,
                cancellationToken);

            if (reward == null)
                throw new Exception("Reward not found");

            if (reward.ProjectId != projectId)
                throw new Exception("Reward does not belong to this project");
        }

        var pledge = new Pledge(amount)
        {
            ProjectId = projectId,
            UserId = userId,
            RewardId = rewardId
        };

        await _pledgeRepository.AddAsync(pledge, cancellationToken);

        return pledge;
    }
    
    public async Task CreateAsync(Project project, CancellationToken cancellationToken = default)
    {
        await _projectRepository.AddAsync(project, cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var targetProject = await GetById(id, cancellationToken);

        if (targetProject == null)
            throw new Exception("Project not found");
        
        await _projectRepository.Delete(targetProject, cancellationToken);
    }
}