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
public class Item : Feature, IStacBase
{
    /// <inheritdoc />
    public new required string Id
    {
        get => base.Id!.Value.ToString();
        init => base.Id = new(value);
    }

    /// <inheritdoc />
    public required string Version { get; init; }

    /// <summary>
    /// Gets the extensions.
    /// </summary>
    public IReadOnlyList<string?>? Extensions { get; init; }

    /// <summary>
    /// Gets the collection.
    /// </summary>
    public string? Collection { get; init; }

    /// <inheritdoc />
    public required IReadOnlyList<Link> Links { get; init; }

    /// <summary>
    /// Gets the assets.
    /// </summary>
    public required IReadOnlyDictionary<string, Asset?> Assets { get; init; }
}