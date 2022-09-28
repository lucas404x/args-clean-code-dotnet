using ArgsCleanCode.Main.Exceptions;
using System.Numerics;

namespace ArgsCleanCode.Marshalers;

public interface IArgumentMarshaler
{
    public void Set(string currentArgument);
}

public class BooleanArgumentMarshaler : IArgumentMarshaler
{
    public bool Value { get; set; }

    public static bool GetValue(IArgumentMarshaler am)
    {
        if (am is BooleanArgumentMarshaler marshaler)
        {
            return marshaler.Value;
        }

        return false;
    }

    public void Set(string currentArgument)
    {
        Value = true;
    }
}

public class IntegerArgumentMarshaler : IArgumentMarshaler
{
    public int Value { get; set; }

    public static int GetValue(IArgumentMarshaler am)
    {
        if (am is IntegerArgumentMarshaler marshaler)
            return marshaler.Value;

        return 0;
    }

    public void Set(string currentArgument)
    {
        try
        {
            Value = int.Parse(currentArgument);
        }
        catch (Exception ex)
        {
            throw new ArgsException(ex.Message);
        }
    }
}

public class DoubleArgumentMarshaler : IArgumentMarshaler
{
    public double Value { get; set; }

    public static double GetValue(IArgumentMarshaler am)
    {
        if (am is DoubleArgumentMarshaler marshaler)
            return marshaler.Value;

        return 0.0;
    }

    public void Set(string currentArgument)
    {
        try
        {
            Value = double.Parse(currentArgument);
        }
        catch (Exception ex)
        {
            throw new ArgsException(ex.Message);
        }
    }
}

public class ComplexArgumentMarshaler : IArgumentMarshaler
{
    public Complex Value { get; set; }

    public static Complex GetValue(IArgumentMarshaler am)
    {
        if (am is ComplexArgumentMarshaler marshaler)
            return marshaler.Value;

        return Complex.Zero;
    }

    public void Set(string currentArgument)
    {
        try
        {
            if (currentArgument.StartsWith('[') && currentArgument.EndsWith(']') && currentArgument.Length > 2)
            {
                var numbers = currentArgument
                    .Substring(1, currentArgument.Length - 2)
                    .Split(',')
                    .Select(x => x.Trim())
                    .ToArray();

                double real = double.Parse(numbers[0]);
                double imaginary = double.Parse(numbers[1]);

                Value = new Complex(real, imaginary);
            }
            else
            {
                throw new ArgsException(ArgsMessages.INVALID_ARGUMENT_FORMART);
            }
        }
        catch (ArgsException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            throw new ArgsException(ex.Message);
        }
    }
}

public class StringArgumentMarshaler : IArgumentMarshaler
{
    public string Value { get; set; } = string.Empty;

    public static string GetValue(IArgumentMarshaler am)
    {
        if (am is StringArgumentMarshaler marshaler)
            return marshaler.Value;

        return string.Empty;
    }

    public void Set(string currentArgument)
    {
        try
        {
            if (string.IsNullOrEmpty(currentArgument))
            {
                throw new ArgumentNullException();
            }


            Value = currentArgument;
        }
        catch (ArgumentNullException)
        {
            throw new ArgsException(ArgsMessages.MISSING_VALUE);
        }
        catch (Exception ex)
        {
            throw new ArgsException(ex.Message);
        }
    }
}

public class StringArrayArgumentMarshaler : IArgumentMarshaler
{
    public string[] Value { get; set; } = Array.Empty<string>();

    public static string[] GetValue(IArgumentMarshaler am)
    {
        if (am is StringArrayArgumentMarshaler marshaler)
            return marshaler.Value;

        return Array.Empty<string>();
    }

    public void Set(string currentArgument)
    {
        try
        {
            if (currentArgument.StartsWith('[') && currentArgument.EndsWith(']') && currentArgument.Length > 2)
            {
                Value = currentArgument
                    .Substring(1, currentArgument.Length - 2)
                    .Split(',')
                    .Select(x => x.Trim())
                    .ToArray();
            }
            else
            {
                throw new ArgsException(ArgsMessages.INVALID_ARGUMENT_FORMART);
            }
        }
        catch (ArgsException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            throw new ArgsException(ex.Message);
        }
    }
}