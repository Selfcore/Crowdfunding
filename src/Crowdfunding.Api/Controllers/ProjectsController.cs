using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Crowdfunding.Api.Controllers;

public class ProjectsController : ApiControllerBase
{
    [SwaggerOperation(Summary = "Gets all projects", Description = "Retrieves a list of all crowdfunding projects.")]
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        return Ok();
    }
    
    [SwaggerOperation(Summary = "Gets a project by ID", Description = "Retrieves a crowdfunding project using its unique identifier.")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return Ok();
    }
    
    [SwaggerOperation(Summary = "Creates a new project", Description = "Creates a new crowdfunding project.")]
    [HttpPost]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        return Created();
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return NoContent();
    }
    
    [SwaggerOperation(Summary = "Deletes a project", Description = "Deletes an existing crowdfunding project using its unique identifier.")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return NoContent();
    }
}