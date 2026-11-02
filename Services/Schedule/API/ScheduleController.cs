namespace Schedule.API;


[ApiController]
[Route("api/v1/[controller]")]
public sealed class ScheduleController(
    IScheduleQueryService scheduleQueryService,
    IScheduleCommandService scheduleCommandService 
) : ControllerBase
{
    private readonly IScheduleQueryService _scheduleQueryService = scheduleQueryService;
    private readonly IScheduleCommandService _scheduleCommandService = scheduleCommandService; 

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var slot = await _scheduleQueryService.GetByIdAsync(id, ct);

        return Ok(slot);
    }

    [HttpGet("{year:int}/{month:int}")]
    public async Task<IActionResult> GetByMonth([FromRoute] int year, [FromRoute] int month, CancellationToken ct)
    {
        var slots = await _scheduleQueryService.GetByMonthAsync(year, month, ct);

        return Ok(slots);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateScheduleSlotRequest request, CancellationToken ct)
    {
        await _scheduleCommandService.CreateAsync(request, ct);

        return Ok();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateScheduleSlotRequest request, CancellationToken ct)
    {
        await _scheduleCommandService.UpdateAsync(id, request, ct);

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        await _scheduleCommandService.DeleteAsync(id, ct);

        return Ok();
    }
}
