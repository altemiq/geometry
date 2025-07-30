// -----------------------------------------------------------------------
// <copyright file="Feature.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The <see cref="GeoJson"/> feature.
/// </summary>
[JsonConverter(typeof(FeatureConverter))]
public class Feature
{
    /// <summary>
    /// Gets the feature ID.
    /// </summary>
    public FeatureId? Id { get; init; }

    /// <summary>
    /// Gets the bounding box.
    /// </summary>
    public Altemiq.Geometry.Envelope? BoundingBox { get; init; }

    /// <summary>
    /// Gets the geometry of this feature.
    /// </summary>
    public required IGeometry? Geometry { get; init; }

    /// <summary>
    /// Gets the additional properties for the feature, the value
    /// is dynamic as it could be a string, bool, null, number,
    /// array, or object.
    /// </summary>
    public IReadOnlyDictionary<string, object?> Properties { get; init; } = new Dictionary<string, object?>(StringComparer.Ordinal);

    /// <inheritdoc/>
    public override string ToString() => JsonSerializer.Serialize(this);
}