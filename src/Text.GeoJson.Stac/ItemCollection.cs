// -----------------------------------------------------------------------
// <copyright file="ItemCollection.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson.Stac;

/// <summary>
/// The STAC item collection.
/// </summary>
[JsonConverter(typeof(ItemCollectionConverter))]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "This is a collection")]
public class ItemCollection : FeatureCollection
{
    /// <summary>
    /// Gets the number of items matched.
    /// </summary>
    public int NumberMatched { get; init; }

    /// <summary>
    /// Gets the number of items returned.
    /// </summary>
    public int NumberReturned { get; init; }

    /// <summary>
    /// Gets the list of links.
    /// </summary>
    public IReadOnlyList<Link> Links { get; init; } = [];
}