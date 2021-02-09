using System;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class ConsoleCommandAttribute : Attribute
{
    public readonly string Name;
    public readonly string HelpString;

    public ConsoleCommandAttribute(string name, string helpString)
    {
        Name = name;
        HelpString = helpString;
    }
}