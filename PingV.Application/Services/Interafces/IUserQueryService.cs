using PingV.Application.Dtos;

namespace PingV.Application.Services.Interafces;

public interface IUserQueryService
{
    Task<IEnumerable<UserDto>> GetAsync(CancellationToken ct = default);

    Task<UserDto> GetByIdAsync(Guid id, CancellationToken ct = default);
}
