using Crowdfunding.Application;
using Crowdfunding.Application.Services;
using Crowdfunding.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Crowdfunding.Api.Controllers;

public class UsersController : ApiControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }
    
    [SwaggerOperation(Summary = "Gets all users", Description = "Retrieves a list of all users.")]
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var list = await _userService.GetAll(cancellationToken);
        
        return Ok(list);
    }
    
    [SwaggerOperation(Summary = "Gets a user by ID", Description = "Retrieves a user using their unique identifier.")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _userService.GetById(id, cancellationToken);
        
        if(result == null)
            return NotFound();
        
        return Ok(result);
    }
    
    [SwaggerOperation(Summary = "Creates a new user", Description = "Creates a new user.")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] User user, CancellationToken cancellationToken)
    {
        try
        {
            await _userService.CreateAsync(user, cancellationToken);

            return Ok(new { message = "User created successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [SwaggerOperation(Summary = "Deletes a user", Description = "Deletes an existing user using its unique identifier.")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try 
        {
            await _userService.Delete(id, cancellationToken);

            return NoContent();
        }
        catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}