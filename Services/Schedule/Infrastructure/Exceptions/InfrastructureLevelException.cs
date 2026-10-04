namespace Schedule.Infrastructure.Exceptions;

public class InfrastructureLevelException : Exception 
{
    public InfrastructureLevelException(string message) : base(message) { }  
    public InfrastructureLevelException(string message, Exception ex) : base(message, ex) { }
} 