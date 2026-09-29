using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PingV.Domain.Entities;
using PingV.Domain.Exceptions;
using PingV.Domain.Interfaces;
using PingV.Infrastructure.Data;
using PingV.Infrastructure.Mappers;
using PingV.Application.Resources;


namespace PingV.Infrastructure.Repositories;

public sealed class EfWishlistItemRepository(
    PingVDbContext context, 
    ILogger<EfWishlistItemRepository> logger 
) : IWishlistItemRepository
{
    private readonly PingVDbContext _context = context;
    private readonly ILogger<EfWishlistItemRepository> _logger = logger; 

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="ServerSideException"></exception>
    public async Task AddAsync(WishlistItem item, CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            var newItem = item.DomainToEf();

            await _context.WishlistItems.AddAsync(newItem, ct);
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
    /// <param name="creatorId"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="ServerSideException"></exception>
    public async Task<IEnumerable<WishlistItem>> GetByCreatorIdAsync(Guid creatorId, CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            var items = await _context.WishlistItems
                .Where(item => item.CreatorId == creatorId)
                .ToListAsync(ct);

            return items.Select(item => item.EfToDomain());
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
    public async Task<WishlistItem> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            var item = await _context.WishlistItems.FindAsync([id], ct);

            if (item == null)
            {
                throw new ServerSideException(ServerSideErrorMessages.NotFound);
            }

            _context.WishlistItems.Remove(item);
            await _context.SaveChangesAsync(ct);

            return item.EfToDomain();
        }
        catch (ServerSideException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new ServerSideException(ServerSideErrorMessages.DeleteGenericError);
        }
    }
}
