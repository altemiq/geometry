// -----------------------------------------------------------------------
// <copyright file="Item.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson.Stac;

/// <summary>
/// The STAC item.
/// </summary>
[JsonConverter(typeof(ItemConverter))]
public class Item : Feature
{
    /// <summary>
    /// Gets the version.
    /// </summary>
    public string? Version { get; init; }

    /// <summary>
    /// Gets the extensions.
    /// </summary>
    public IReadOnlyList<string?>? Extensions { get; init; }

    /// <summary>
    /// Gets the collection.
    /// </summary>
    public string? Collection { get; init; }

    /// <summary>
    /// Gets the links.
    /// </summary>
    public IReadOnlyList<Link>? Links { get; init; }

    /// <summary>
    /// Gets the assets.
    /// </summary>
    public IReadOnlyDictionary<string, Asset?>? Assets { get; init; }
}