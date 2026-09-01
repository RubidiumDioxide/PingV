using PingV.Domain.Entities;

namespace PingV.Domain.Interfaces; 

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAsync(CancellationToken ct = default);

    Task<User> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<User> GetByLoginAsync(string login, CancellationToken ct = default);
}
