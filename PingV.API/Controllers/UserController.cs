using Microsoft.AspNetCore.Mvc;
using PingV.Application.Services.Interafces;

namespace PingV.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UserController(
    IUserQueryService userQueryService,
    ILogger<UserController> logger
) : ControllerBase
{
    private readonly IUserQueryService _userQueryService = userQueryService;
    private readonly ILogger<UserController> _logger = logger;



    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        try
        {
            var users = await _userQueryService.GetAsync(ct);
            
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message); 
            return StatusCode(500);
        }
    }
}
