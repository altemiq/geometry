// -----------------------------------------------------------------------
// <copyright file="Collection.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson.Stac;

/// <summary>
/// The STAC collection.
/// </summary>
[JsonConverter(typeof(CollectionConverter))]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Checked")]
public class Collection : Catalog
{
    /// <summary>
    /// Gets a list of keywords describing the <see cref="Collection"/>.
    /// </summary>
    public IReadOnlyList<string?>? Keywords { get; init; }

    /// <summary>
    /// Gets the license(s) of the data collection as SPDX License identifier, SPDX License expression, or <c>other</c>.
    /// </summary>
    public required string License { get; init; }

    /// <summary>
    /// Gets a list of providers, which may include all organizations capturing or processing the data or the hosting provider.
    /// </summary>
    public IReadOnlyList<Provider>? Providers { get; init; }

    /// <summary>
    /// Gets the spatial and temporal extents.
    /// </summary>
    public required Extent Extent { get; init; }
}