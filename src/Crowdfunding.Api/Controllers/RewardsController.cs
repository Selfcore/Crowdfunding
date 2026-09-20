using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Crowdfunding.Api.Controllers;

[ApiController]
[Route("api/v1/projects/{projectId:guid}/[controller]")]
public class RewardsController : ControllerBase
{
    [SwaggerOperation(Summary = "Gets project rewards", Description = "Retrieves all rewards associated with the specified crowdfunding project.")]
    [HttpGet]
    public async Task<IActionResult> GetProjectRewards([FromRoute] Guid projectId, CancellationToken cancellationToken)
    {
        return Ok();
    }
    
    [SwaggerOperation(Summary = "Creates a project reward", Description = "Creates a new reward for the specified crowdfunding project.")]
    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] Guid projectId, CancellationToken cancellationToken)
    {
        return Created();
    }
}