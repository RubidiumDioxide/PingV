namespace Schedule.Infrastructure.Auth; 

public class CurrentUserMock : ICurrentUser 
{
    public Guid Id { get; }  
    public string? Username { get; }
    public bool IsAuthenticated { get; }

    public CurrentUserMock(Guid? id = null, string? username = null, bool isAuthenticated = true)
    {
        Id = id ?? Guid.NewGuid();
        Username = username ?? "mock-user";
        IsAuthenticated = isAuthenticated;
    }
}
