namespace Aoc.Utilities;

public static class CollectionExtensions
{
    /// <summary>
    ///     Creates chunks of non-empty strings from the source collection.
    ///     Empty or whitespace strings are used as delimiters between chunks.
    /// </summary>
    /// <param name="source">The source collection of strings to be chunked.</param>
    /// <returns>A list of arrays where each array contains a sequence of non-empty strings.</returns>
    /// <example>
    ///     <code>
    /// var input = new[] { "a", "", "b", " ", "c" };
    /// var result = input.ChunkByNonEmpty(); // Returns [["a"], ["b"], ["c"]]
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">Thrown when source is null.</exception>
    public static IList<string[]> ChunkByNonEmpty(this IEnumerable<string> source)
    {
        return source.ChunkBy(s => !string.IsNullOrWhiteSpace(s));
    }

    /// <summary>
    ///     Creates chunks from the source collection based on custom predicates for taking and skipping elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source collection.</typeparam>
    /// <param name="source">The source collection to be chunked.</param>
    /// <param name="takePredicate">
    ///     A predicate that determines which elements should be included in chunks.
    ///     Returns true for elements that should be included in the current chunk.
    /// </param>
    /// <param name="skipPredicate">
    ///     Optional. A predicate that determines which elements should be skipped between chunks.
    ///     If not provided, the inverse of takePredicate is used.
    ///     Returns true for elements that should be skipped.
    /// </param>
    /// <returns>
    ///     A list of arrays where each array contains elements that satisfied the takePredicate,
    ///     separated by elements that satisfied the skipPredicate.
    /// </returns>
    /// <example>
    ///     <code>
    /// var numbers = new[] { 1, 0, 2, 0, 3 };
    /// var result = numbers.ChunkBy(n => n != 0); // Returns [[1], [2], [3]]
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">Thrown when source or takePredicate is null.</exception>
    public static IList<T[]> ChunkBy<T>(this IEnumerable<T> source, Predicate<T> takePredicate,
        Predicate<T>? skipPredicate = null)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(takePredicate);

        var chunks = new List<T[]>();
        var currentChunk = new List<T>();
        skipPredicate ??= element => !takePredicate(element);

        using var enumerator = source.GetEnumerator();
        if (!enumerator.MoveNext()) return chunks;

        var isInTakeMode = true;
        do
        {
            var current = enumerator.Current;

            if (isInTakeMode)
            {
                if (takePredicate(current))
                {
                    currentChunk.Add(current);
                }
                else
                {
                    if (currentChunk.Count > 0)
                    {
                        chunks.Add(currentChunk.ToArray());
                        currentChunk.Clear();
                    }

                    isInTakeMode = false;
                }
            }
            else // Skip mode
            {
                if (!skipPredicate(current))
                {
                    isInTakeMode = true;
                    if (takePredicate(current)) currentChunk.Add(current);
                }
            }
        } while (enumerator.MoveNext());

        // Add any remaining items
        if (currentChunk.Count > 0) chunks.Add(currentChunk.ToArray());

        return chunks;
    }
}