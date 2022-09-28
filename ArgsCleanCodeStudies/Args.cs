using ArgsCleanCode.Main.Exceptions;
using ArgsCleanCode.Marshalers;
using System.Numerics;

namespace ArgsCleanCode.Main.ArgsClass;

public class Args
{
    private readonly Dictionary<char, IArgumentMarshaler> _marshelers;
    private readonly HashSet<char> _argsFound;

    private IEnumerator<string> _currentArgument;

    public Args(string schema, string[] args)
    {
        _marshelers = new Dictionary<char, IArgumentMarshaler>();
        _argsFound = new HashSet<char>();

        _currentArgument = new List<string>().GetEnumerator();

        ParseSchema(schema);
        ParseArgumentString(args);
    }

    private void ParseSchema(string schema)
    {
        foreach (string element in schema.Split('\n'))
        {
            if (element.Length > 0)
                ParseSchemaElement(element.Trim());
        }
    }

    private void ParseSchemaElement(string element)
    {
        char elementId = element.ElementAt(0);
        string elementTail = element.Substring(1);
        ValidateSchemaElementId(elementId);

        if (elementTail.Length == 0)
            _marshelers.Add(elementId, new BooleanArgumentMarshaler());
        else if (elementTail == "*")
            _marshelers.Add(elementId, new StringArgumentMarshaler());
        else if (elementTail == "#")
            _marshelers.Add(elementId, new IntegerArgumentMarshaler());
        else if (elementTail == "##")
            _marshelers.Add(elementId, new DoubleArgumentMarshaler());
        else if (elementTail == "[##,##]")
            _marshelers.Add(elementId, new ComplexArgumentMarshaler());
        else if (elementTail == "[*]")
            _marshelers.Add(elementId, new StringArrayArgumentMarshaler());
    }

    private void ValidateSchemaElementId(char elementId)
    {
        if (!char.IsLetter(elementId))
            throw new ArgsException(ArgsMessages.INVALID_ARGUMENT_NAME, elementId);
    }

    private void ParseArgumentString(string[] args)
    {
        _currentArgument = args.Cast<string>().GetEnumerator();
        while (_currentArgument.MoveNext())
        {
            var arg = _currentArgument.Current;

            if (arg.StartsWith('-'))
                ParseArgumentCharacters(arg.Substring(1).Trim());
            else
                break;
        }

        _currentArgument.Reset();
    }

    private void ParseArgumentCharacters(string arg)
    {
        ParseArgumentCharacter(arg.First(), arg.Substring(1).Trim());
    }

    private void ParseArgumentCharacter(char elementId, string value)
    {
        var masheler = _marshelers.GetValueOrDefault(elementId);
        if (masheler == null)
            throw new ArgsException(ArgsMessages.UNEXPECTED_ARGUMENT, elementId);

        _argsFound.Add(elementId);

        try
        {
            masheler.Set(value);
        }
        catch (ArgsException ex)
        {
            ex.SetErrorArgumentId(elementId);
            throw ex;
        }
    }

    public bool Has(char arg)
    {
        return _argsFound.Contains(arg);
    }

    public string NextArgument()
    {
        if (!_currentArgument.MoveNext())
        {
            throw new ArgsException(ArgsMessages.NO_MORE_ARGS);
        }

        return _currentArgument.Current;
    }

    public bool GetBool(char arg)
    {
        var am = _marshelers.GetValueOrDefault(arg);
        if (am == null) return false;

        return BooleanArgumentMarshaler.GetValue(am);
    }

    public int GetInt(char arg)
    {
        var am = _marshelers.GetValueOrDefault(arg);
        if (am == null) return 0;

        return IntegerArgumentMarshaler.GetValue(am);
    }

    public double GetDouble(char arg)
    {
        var am = _marshelers.GetValueOrDefault(arg);
        if (am == null) return 0.0;

        return DoubleArgumentMarshaler.GetValue(am);
    }

    public Complex GetComplex(char arg)
    {
        var am = _marshelers.GetValueOrDefault(arg);
        if (am == null) return Complex.Zero;

        return ComplexArgumentMarshaler.GetValue(am);
    }

    public string GetString(char arg)
    {
        var am = _marshelers.GetValueOrDefault(arg);
        if (am == null) return string.Empty;

        return StringArgumentMarshaler.GetValue(am);
    }

    public string[] GetStringArray(char arg)
    {
        var am = _marshelers.GetValueOrDefault(arg);
        if (am == null) return Array.Empty<string>();

        return StringArrayArgumentMarshaler.GetValue(am);
    }
}