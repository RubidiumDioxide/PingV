namespace Schedule.Application.Auth.Interfaces; 

public interface ICurrentUser
{
    Guid Id { get; }
    string? Username { get; } 
    bool IsAuthenticated { get; }
}
