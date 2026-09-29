using Microsoft.AspNetCore.Mvc;
using PingV.Application.Dtos;
using PingV.Application.Services.Interafces;


namespace PingV.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class WishlistItemController(
    IWishlistItemQueryService wishlistItemQueryService,  
    IWishlistItemCommandService wishlistItemCommandService,  
    ILogger<WishlistItemController> logger 
) : ControllerBase
{
    private readonly IWishlistItemQueryService _wishlistItemQueryService = wishlistItemQueryService;
    private readonly IWishlistItemCommandService _wishlistItemCommandService = wishlistItemCommandService;
    private readonly ILogger<WishlistItemController> _logger = logger;


    [HttpGet("{creatorId:guid}")]
    public async Task<IActionResult> GetByCreatorId(Guid creatorId, CancellationToken ct)
    {
        try
        {
            var items = await _wishlistItemQueryService.GetByCreatorId(creatorId, ct);  
            
            return Ok(items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message); 
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWishlistItemRequest request, CancellationToken ct)
    {
        try
        {
            await _wishlistItemCommandService.AddAsync(request, ct);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        try
        {
            await _wishlistItemCommandService.DeleteAsync(id, ct);
            
            return Ok(); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500);
        }
    }
}
