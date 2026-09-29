using PingV.Domain.ValueObjects;
using System.Linq.Expressions;
using PingV.Application.Dtos;
using PingV.Domain.Entities;
using PingV.Infrastructure.Data.Models;


namespace PingV.Infrastructure.Mappers;

public static class Mapper
{
    public static AvailabilitySlotEfModel DomainToEf(this AvailabilitySlot slot) => new()
    {
        Id = slot.Id, 
        CreatorId = slot.CreatorId, 
        Start = slot.TimeRange.Start,
        End = slot.TimeRange.End,
        Note = slot.Note
    };

    public static WishlistItemEfModel DomainToEf(this WishlistItem item) => new()
    {
        Id = item.Id,
        CreatorId = item.CreatorId, 
        Title = item.Title, 
        Description = item.Description 
    };

    public static AvailabilitySlot EfToDomain(this AvailabilitySlotEfModel slot)
        => new(slot.Id, slot.CreatorId, new TimeRange(slot.Start, slot.End), slot.Note); 

    public static WishlistItem EfToDomain(this WishlistItemEfModel item)
        => new(item.Id, item.CreatorId, item.Title, item.Description);

    public static User EfToDomain(this UserEfModel user)
        => new(user.Id, user.Login);


    public static Expression<Func<AvailabilitySlotEfModel, AvailabilitySlotDto>> AvailabilitySlotEfToDto
       => slot => new(slot.Id, slot.CreatorId, slot.Creator.Login, slot.Start, slot.End, slot.Note); 

    public static Expression<Func<WishlistItemEfModel, WishlistItemDto>> WishlistItemEfToDto
       => item => new(item.Id, item.CreatorId, item.Creator.Login, item.Title, item.Description);

    public static Expression<Func<UserEfModel, UserDto>> UserEfToDto 
        => user => new(user.Id, user.Login); 
}
