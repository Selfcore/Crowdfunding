using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Crowdfunding.Api.Controllers;

public class UsersController : ApiControllerBase
{
    [SwaggerOperation(Summary = "Gets all users", Description = "Retrieves a list of all users.")]
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        return Ok();
    }
    
    [SwaggerOperation(Summary = "Gets a user by ID", Description = "Retrieves a user using their unique identifier.")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return Ok();
    }
    
    [SwaggerOperation(Summary = "Creates a new user", Description = "Creates a new user.")]
    [HttpPost]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        return Created();
    }
    
    [SwaggerOperation(Summary = "Deletes a user", Description = "Deletes an existing user using its unique identifier.")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return NoContent();
    }
}