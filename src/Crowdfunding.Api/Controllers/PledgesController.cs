using Crowdfunding.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Crowdfunding.Api.Controllers;

[ApiController]
[Route("api/v1/projects/{projectId:guid}/[controller]")]
public class PledgesController : ControllerBase
{
    private readonly ProjectService _projectService;
    
    public PledgesController(ProjectService projectService)
    {
        _projectService = projectService;
    }
    
    [SwaggerOperation(Summary = "Gets project pledges", Description = "Retrieves all pledges associated with the specified crowdfunding project.")]
    [HttpGet]
    public async Task<IActionResult> GetProjectPledges([FromRoute] Guid projectId, CancellationToken cancellationToken)
    {
        try
        {
            var pledges = await _projectService.GetAllPledges(projectId, cancellationToken);

            return Ok(pledges);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [SwaggerOperation(Summary = "Creates a project pledge", Description = "Creates a new pledge for the specified crowdfunding project.")]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromRoute] Guid projectId, 
        Guid userId, 
        decimal amount, 
        Guid rewardId,
        CancellationToken cancellationToken)
    {
        try
        {
            var pledge = await _projectService.CreatePledge(projectId, userId, amount, rewardId, cancellationToken);

            return CreatedAtAction(nameof(GetProjectPledges), new { projectId }, pledge);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}