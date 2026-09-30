using Crowdfunding.Application.Dtos.Reward;
using Crowdfunding.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Crowdfunding.Api.Controllers;

[ApiController]
[Route("api/v1/projects/{projectId:guid}/[controller]")]
public class RewardsController : ControllerBase
{
    private readonly ProjectService _projectService;
    
    public RewardsController(ProjectService projectService)
    {
        _projectService = projectService;
    }
    
    [SwaggerOperation(Summary = "Gets project rewards", Description = "Retrieves all rewards associated with the specified crowdfunding project.")]
    [HttpGet]
    public async Task<IActionResult> GetProjectRewards([FromRoute] Guid projectId, CancellationToken cancellationToken)
    {
        try
        {
            var rewards = await _projectService.GetAllRewards(projectId, cancellationToken);

            return Ok(rewards);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [SwaggerOperation(Summary = "Creates a project reward", Description = "Creates a new reward for the specified crowdfunding project.")]
    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] Guid projectId, [FromBody] CreateRewardRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            await _projectService.CreateReward(projectId, request, cancellationToken);

            return Ok(new { message = "Reward created successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [SwaggerOperation(Summary = "Update a project reward", Description = "Updates a reward belonging to the specified project.")]
    [HttpPut("{rewardId:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid projectId,
        [FromRoute] Guid rewardId,
        [FromBody] UpdateRewardRequest request,
        CancellationToken cancellationToken)
    {
        await _projectService.UpdateReward(projectId, rewardId, request, cancellationToken);

        return NoContent();
    }
}