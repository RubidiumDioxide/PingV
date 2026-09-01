using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PingV.Domain.Entities;
using PingV.Domain.Exceptions;
using PingV.Domain.Interfaces;
using PingV.Infrastructure.Data;
using PingV.Infrastructure.Mappers;
using PingV.Application.Resources;


namespace PingV.Infrastructure.Repositories;

public sealed class EfAvailabilitySlotRepository(
    PingVDbContext context, 
    ILogger<EfAvailabilitySlotRepository> logger 
) : IAvailabilitySlotRepository
{
    private readonly PingVDbContext _context = context;
    private readonly ILogger<EfAvailabilitySlotRepository> _logger = logger; 

    /// <summary>
    /// 
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="ServerSideException"></exception>
    public async Task AddAsync(AvailabilitySlot slot, CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            var newSlot = slot.DomainToEf();

            await _context.AvailabilitySlots.AddAsync(newSlot, ct);
            await _context.SaveChangesAsync(ct);
        }
        catch (ServerSideException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new ServerSideException(ServerSideErrorMessages.AddGenericError);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="ServerSideException"></exception>
    public async Task<IEnumerable<AvailabilitySlot>> GetSlotsByMonthAsync(Guid userId, int year, int month, CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            var monthStart = new DateTimeOffset(year, month, 1, 0, 0, 0, TimeSpan.Zero);
            var monthEnd = monthStart.AddMonths(1);

            var slots = await _context.AvailabilitySlots
                .Where(slot => slot.CreatorId == userId && slot.Start < monthEnd && slot.End > monthStart)
                .ToListAsync(ct);

            return slots.Select(s => s.AvailabilitySlotEfToDomain());
        }
        catch (ServerSideException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new ServerSideException(ServerSideErrorMessages.GetGenericError);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="ServerSideException"></exception>
    public async Task<AvailabilitySlot> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            var slot = await _context.AvailabilitySlots.FindAsync([id], ct);

            if (slot == null)
            {
                throw new ServerSideException(ServerSideErrorMessages.NotFound);
            }

            _context.AvailabilitySlots.Remove(slot);
            await _context.SaveChangesAsync(ct);

            return slot.AvailabilitySlotEfToDomain();
        }
        catch (ServerSideException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new ServerSideException(ServerSideErrorMessages.DeleteGenericError);
        }
    }
}
