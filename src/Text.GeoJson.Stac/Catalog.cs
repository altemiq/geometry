// -----------------------------------------------------------------------
// <copyright file="Catalog.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson.Stac;

/// <summary>
/// The STAC catalog.
/// </summary>
[JsonConverter(typeof(CatalogConverter))]
public class Catalog : IStacBase
{
    /// <inheritdoc />
    public required string Id { get; init; }

    /// <inheritdoc />
    public required string Version { get; init; }

    /// <inheritdoc />
    public IReadOnlyList<string?>? Extensions { get; init; }

    /// <inheritdoc />
    public required IReadOnlyList<Link> Links { get; init; }

    /// <summary>
    /// Gets a short descriptive one-line title for the <see cref="Catalog"/>.
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// Gets a detailed multi-line description to fully explain the <see cref="Catalog"/>.
    /// </summary>
    public required string Description { get; init; }
}