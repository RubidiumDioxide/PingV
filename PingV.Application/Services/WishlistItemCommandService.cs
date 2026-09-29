using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using PingV.Domain.Entities;
using PingV.Application.Resources;
using PingV.Application.Dtos;
using PingV.Application.Services.Interafces;
using PingV.Domain.Exceptions;
using PingV.Domain.Interfaces;


namespace PingV.Application.Services;

public sealed class WishlistItemCommandService(
    IWishlistItemRepository wishlistItemRepository, 
    IUserRepository userRepository, 
    IDistributedCache cache, 
    string dbProviderPrefix, 
    ILogger<WishlistItemCommandService> logger 
) : IWishlistItemCommandService
{
    private readonly IWishlistItemRepository _wishlistItemRepository = wishlistItemRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IDistributedCache _cache = cache; 
    private readonly string _dbProviderPrefix = dbProviderPrefix; 
    private readonly ILogger<WishlistItemCommandService> _logger = logger; 
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };


    public async Task AddAsync(CreateWishlistItemRequest request, CancellationToken ct = default)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(request.CreatorId, ct);

            if (user == null)
            {
                throw new ServerSideException(ServerSideErrorMessages.NotFound);
            }

            var newWishlistItem = new WishlistItem(Guid.NewGuid(), request.CreatorId, request.Title, request.Description); 
                
            await _wishlistItemRepository.AddAsync(newWishlistItem, ct);

            await _cache.RemoveAsync(GetCacheKey(newWishlistItem.CreatorId), ct);

            return; 
        }
        catch (ServerSideException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new ServerSideException(ServerSideErrorMessages.AddGenericError);
        }
    }


    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            var deletedWishlitstItem = await _wishlistItemRepository.DeleteAsync(id, ct);
            
            await _cache.RemoveAsync(GetCacheKey(deletedWishlitstItem.CreatorId), ct);
        }
        catch (ServerSideException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new ServerSideException(ServerSideErrorMessages.DeleteGenericError);
        }
    }


    // --- HELPERS --- 
    private string GetCacheKey(Guid creatorId) => $"{_dbProviderPrefix}:wishlistItemsByUserId:{creatorId}"; 
}
