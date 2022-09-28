namespace ArgsCleanCode.Main.Exceptions;

public class ArgsException : Exception
{
    public char ErrorArgumentId { get; private set; } = '\0';
    public string? ErrorParameter { get; set; }

    public ArgsException(string message) : base(message) { }

    public ArgsException(string message, char errorArgumentId) : base(message)
    {
        ErrorArgumentId = errorArgumentId;
    }

    public ArgsException(string message, char errorArgumentId, string errorParameter) : base(message)
    {
        ErrorArgumentId = errorArgumentId;
        ErrorParameter = errorParameter;
    }

    public void SetErrorArgumentId(char elementId)
    {
        ErrorArgumentId = elementId;
    }
}

public static class ArgsMessages
{
    public readonly static string UNEXPECTED_ARGUMENT = "UNEXPECTED_ARGUMENT";
    public readonly static string INVALID_ARGUMENT_NAME = "INVALID_ARGUMENT_NAME";
    public readonly static string NO_MORE_ARGS = "NO_MORE_ARGS";
    public readonly static string MISSING_VALUE = "MISSING_VALUE";
    public readonly static string INVALID_ARGUMENT_FORMART = "INVALID_ARGUMENT_FORMART";
}