namespace Schedule.Domain.Exceptions;

public class DomainLevelException : Exception 
{
    public DomainLevelException(string message) : base(message) { }  
    public DomainLevelException(string message, Exception ex) : base(message, ex) { }
} 