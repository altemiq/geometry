// -----------------------------------------------------------------------
// <copyright file="Extent.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson.Stac;

/// <summary>
/// The extent of the <see cref="Collection"/>.
/// </summary>
public class Extent
{
    /// <summary>
    /// Gets the potential spatial extents covered by the <see cref="Collection"/>.
    /// </summary>
    [JsonPropertyName("spatial")]
    public required SpatialExtent Spatial { get; init; }

    /// <summary>
    /// Gets the potential temporal extents covered by the <see cref="Collection"/>.
    /// </summary>
    [JsonPropertyName("temporal")]
    public required TemporalExtent Temporal { get; init; }
}