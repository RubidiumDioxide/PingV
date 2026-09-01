using Microsoft.AspNetCore.Mvc;
using PingV.Application.Dtos;
using PingV.Application.Services.Interafces;

namespace PingV.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AvailabilitySlotController(
    IAvailabilitySlotQueryService availabilitySlotQueryService,  
    IAvailabilitySlotCommandService availabilitySlotCommandService,  
    ILogger<AvailabilitySlotController> logger 
) : ControllerBase
{
    private readonly IAvailabilitySlotQueryService _availabilitySlotQueryService = availabilitySlotQueryService;
    private readonly IAvailabilitySlotCommandService _availabilitySlotCommandService = availabilitySlotCommandService;
    private readonly ILogger<AvailabilitySlotController> _logger = logger;


    [HttpGet("{userId:guid}/{year:int}/{month:int}")]
    public async Task<IActionResult> GetByMonth(Guid userId, int year, int month, CancellationToken ct)
    {
        try
        {
            var slots = await _availabilitySlotQueryService.GetMonthlyScheduleAsync(userId, year, month, ct);
            
            return Ok(slots);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message); 
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAvailabilitySlotRequest request, CancellationToken ct)
    {
        try
        {
            await _availabilitySlotCommandService.AddAvailabilitySlotAsync(request, ct);

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
            await _availabilitySlotCommandService.DeleteAvailabilitySlotAsync(id, ct);
            
            return Ok(); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500);
        }
    }
}
