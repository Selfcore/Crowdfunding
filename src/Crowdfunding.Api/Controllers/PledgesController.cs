using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Crowdfunding.Api.Controllers;

[ApiController]
[Route("api/v1/projects/{projectId:guid}/[controller]")]
public class PledgesController : ControllerBase
{
    [SwaggerOperation(Summary = "Gets project pledges", Description = "Retrieves all pledges associated with the specified crowdfunding project.")]
    [HttpGet]
    public async Task<IActionResult> GetProjectPledges([FromRoute] Guid projectId, CancellationToken cancellationToken)
    {
        return Ok();
    }
    
    [SwaggerOperation(Summary = "Creates a project pledge", Description = "Creates a new pledge for the specified crowdfunding project.")]
    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] Guid projectId, CancellationToken cancellationToken)
    {
        return Created();
    }
}