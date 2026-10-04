namespace Schedule.Application.Exceptions;

public class ApplicationLevelException : Exception 
{
    public ApplicationLevelException(string message) : base(message) { }  
    public ApplicationLevelException(string message, Exception ex) : base(message, ex) { }
} 