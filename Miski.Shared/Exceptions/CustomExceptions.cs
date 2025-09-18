namespace Miski.Shared.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
    public DomainException(string message, Exception innerException) : base(message, innerException) { }
}

public class NotFoundException : Exception
{
    public NotFoundException(string name, object key)
        : base($"Entidad \"{name}\" ({key}) no fue encontrada.") { }
}

public class ValidationException : Exception
{
    public object Errors { get; }

    public ValidationException()
        : base("Uno o m�s errores de validaci�n ocurrieron.")
    {
        Errors = "Uno o m�s errores de validaci�n ocurrieron.";
    }

    // Para mensaje simple
    public ValidationException(string message) : base(message)
    {
        Errors = message;
    }

    // Para m�ltiples errores (formularios grandes, etc.)
    public ValidationException(IDictionary<string, string[]> errors) : base("Uno o m�s errores de validaci�n ocurrieron.")
    {
        Errors = errors;
    }
}
