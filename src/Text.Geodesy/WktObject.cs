// -----------------------------------------------------------------------
// <copyright file="WktObject.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.Geodesy;

/// <summary>
/// The WKT object.
/// </summary>
/// <param name="id">The ID.</param>
/// <param name="values">The values.</param>
internal readonly struct WktObject(string id, IReadOnlyList<WktElement> values) : IReadOnlyList<WktElement>
{
    /// <summary>
    /// Gets the ID of the object.
    /// </summary>
    public string Id { get; } = id;

    /// <inheritdoc/>
    public int Count => values.Count;

    /// <inheritdoc/>
    public WktElement this[int index] => values[index];

    /// <inheritdoc/>
    public IEnumerator<WktElement> GetEnumerator() => values.GetEnumerator();

    /// <inheritdoc/>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => values.GetEnumerator();
}