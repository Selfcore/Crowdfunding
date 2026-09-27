using Crowdfunding.Application.Services;
using Crowdfunding.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Crowdfunding.Api.Controllers;

public class ProjectsController : ApiControllerBase
{
    private readonly ProjectService _projectService;
    
    public ProjectsController(ProjectService projectService)
    {
        _projectService = projectService;
    }
    
    [SwaggerOperation(Summary = "Gets all projects", Description = "Retrieves a list of all crowdfunding projects.")]
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var list = await _projectService.GetAll(cancellationToken);

        return Ok(list);
    }
    
    [SwaggerOperation(Summary = "Gets a project by ID", Description = "Retrieves a crowdfunding project using its unique identifier.")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _projectService.GetById(id, cancellationToken);
        
        if(result == null)
            return NotFound();
        
        return Ok(result);
    }
    
    [SwaggerOperation(Summary = "Creates a new project", Description = "Creates a new crowdfunding project.")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Project project, CancellationToken cancellationToken)
    {
        try
        {
            await _projectService.CreateAsync(project, cancellationToken);

            return Ok(new { message = "Project created successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
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
        try 
        {
            await _projectService.Delete(id, cancellationToken);

            return NoContent();
        }
        catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}