// -----------------------------------------------------------------------
// <copyright file="SpatialExtent.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson.Stac;

/// <summary>
/// The spatial extent.
/// </summary>
public class SpatialExtent
{
    /// <summary>
    /// Gets the bounding box.
    /// </summary>
    [JsonPropertyName("bbox")]
    public required IReadOnlyList<IReadOnlyList<double>> BoundingBox { get; init; }
}