using Microsoft.AspNetCore.Mvc;

namespace Crowdfunding.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public abstract class ApiControllerBase : ControllerBase { }