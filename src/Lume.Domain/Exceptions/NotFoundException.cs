namespace Lume.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public NotFoundException(string entityName, object key) 
        : this($"Entity \"{entityName}\" ({key}) was not found.")
    {
    }
}