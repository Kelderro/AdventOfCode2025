namespace Aoc.Solutions.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class InputSpecificSolutionAttribute(string message) : Attribute
{
    private const string DefaultMessage =
        "This solution implementation is input specific, and may not work on all inputs";

    public InputSpecificSolutionAttribute() : this(DefaultMessage)
    {
    }

    public string Message { get; } = message;
}