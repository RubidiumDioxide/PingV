namespace PingV.Domain.Entities; 

public class User
{
    public Guid Id { get; private set; }
    public string Login { get; private set; } = default!;

    private User() { } 

    public User(Guid id, string login)
    {
        Id = id; 
        Login = login; 
    }
}
