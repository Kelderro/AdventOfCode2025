namespace Aoc.Solutions.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class PuzzleInfoAttribute(string Title) : Attribute;