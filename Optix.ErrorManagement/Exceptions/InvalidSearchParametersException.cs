namespace Optix.ErrorManagement.Exceptions;

public class InvalidSearchParametersException : Exception
{
    public InvalidSearchParametersException()
    {
    }

    public InvalidSearchParametersException(List<string> messages) : this(string.Join(",", messages))
    {
    }

    public InvalidSearchParametersException(string message)
        : base(message)
    {
    }

    public InvalidSearchParametersException(string message, Exception inner)
        : base(message, inner)
    {
    }
}