// -----------------------------------------------------------------------
// <copyright file="WellKnownTextNodeExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geodesy.Serialization;

/// <summary>
/// <see cref="WellKnownTextNode"/> extensions.
/// </summary>
internal static class WellKnownTextNodeExtensions
{
    /// <summary>
    /// Flattens this instance.
    /// </summary>
    /// <param name="nodes">The nodes.</param>
    /// <returns>The result of flattening <paramref name="nodes"/>.</returns>
    /// <exception cref="InvalidOperationException"><paramref name="nodes"/> is empty.</exception>
    public static WellKnownTextNode Flatten(this IEnumerable<WellKnownTextNode> nodes)
    {
        var enumerator = nodes.GetEnumerator();
        if (enumerator.MoveNext())
        {
            var first = enumerator.Current;

            if (enumerator.MoveNext())
            {
                // wrap this up
                return new("FLATTEN", GetNodes(first, enumerator));
            }

            return first;
        }

        throw new InvalidOperationException();

        static IEnumerable<NodeValue> GetNodes(WellKnownTextNode initial, IEnumerator<WellKnownTextNode> nodes)
        {
            yield return initial;
            yield return nodes.Current;
            while (nodes.MoveNext())
            {
                yield return nodes.Current;
            }
        }
    }
}