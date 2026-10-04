namespace Schedule.API.Controllers;


[ApiController]
[Route("api/v1/[controller]")]
public sealed class ScheduleController(
    IScheduleQueryService scheduleQueryService,
    IScheduleCommandService scheduleCommandService,
    ILogger<ScheduleController> logger
) : ControllerBase
{
    private readonly IScheduleQueryService _scheduleQueryService = scheduleQueryService;
    private readonly IScheduleCommandService _scheduleCommandService = scheduleCommandService;
    private readonly ILogger<ScheduleController> _logger = logger;


    [HttpGet("{year:int}/{month:int}")]
    public async Task<IActionResult> GetByMonth(int year, int month, CancellationToken ct)
    {
        try
        {
            var slots = await _scheduleQueryService.GetByMonthAsync(year, month, ct);

            return Ok(slots);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get monthly schedule ");
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateScheduleSlotRequest request, CancellationToken ct)
    {
        try
        {
            await _scheduleCommandService.CreateAsync(request, ct);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create a schedule slot ");
            return StatusCode(500);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        try
        {
            await _scheduleCommandService.DeleteAsync(id, ct);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete a schedule slot ");
            return StatusCode(500);
        }
    }
}
