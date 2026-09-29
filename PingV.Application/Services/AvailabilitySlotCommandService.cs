using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using PingV.Domain.Entities;
using PingV.Application.Resources;
using PingV.Domain.ValueObjects;
using PingV.Application.Dtos;
using PingV.Application.Services.Interafces;
using PingV.Domain.Exceptions;
using PingV.Domain.Interfaces;


namespace PingV.Application.Services;

public sealed class AvailabilitySlotCommandService(
    IAvailabilitySlotRepository slotRepository, 
    IUserRepository userRepository, 
    IDistributedCache cache, 
    string dbProviderPrefix, 
    ILogger<AvailabilitySlotCommandService> logger 
) : IAvailabilitySlotCommandService
{
    private readonly IAvailabilitySlotRepository _slotRepository = slotRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IDistributedCache _cache = cache; 
    private readonly string _dbProviderPrefix = dbProviderPrefix; 
    private readonly ILogger<AvailabilitySlotCommandService> _logger = logger; 
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="ServerSideException"></exception>
    public async Task AddAsync(CreateAvailabilitySlotRequest request, CancellationToken ct = default)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(request.CreatorId, ct);

            if (user == null)
            {
                throw new ServerSideException(ServerSideErrorMessages.NotFound);
            }

            var newSlot = new AvailabilitySlot(Guid.NewGuid(), request.CreatorId, new TimeRange(request.Start, request.End), request.Note); 
                
            await _slotRepository.AddAsync(newSlot, ct);
            
            var monthStart = new DateTimeOffset(request.Start.Year, request.Start.Month, 1, 0, 0, 0, TimeSpan.Zero);
            var monthEnd = new DateTimeOffset(request.End.Year, request.End.Month, 1, 0, 0, 0, TimeSpan.Zero);

            for (var month = monthStart; month <= monthEnd; month = month.AddMonths(1))
            {
                await _cache.RemoveAsync(GetCacheKey(month.Year, month.Month), ct);
            }

            return; 
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
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="ServerSideException"></exception>
    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            var deletedSlot = await _slotRepository.DeleteAsync(id, ct);

            var monthStart = new DateTimeOffset(deletedSlot.TimeRange.Start.Year, deletedSlot.TimeRange.Start.Month, 1, 0, 0, 0, TimeSpan.Zero);
            var monthEnd = new DateTimeOffset(deletedSlot.TimeRange.End.Year, deletedSlot.TimeRange.End.Month, 1, 0, 0, 0, TimeSpan.Zero);

            for (var month = monthStart; month <= monthEnd; month = month.AddMonths(1))
            {
                await _cache.RemoveAsync(GetCacheKey(month.Year, month.Month), ct);
            }
        }
        catch (ServerSideException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new ServerSideException(ServerSideErrorMessages.DeleteGenericError);
        }
    }


    // --- HELPERS --- 
    private string GetCacheKey(int year, int month) => $"{_dbProviderPrefix}:monthlySchedule:{year}:{month}"; 
}
