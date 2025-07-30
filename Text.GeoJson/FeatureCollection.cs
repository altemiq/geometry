// -----------------------------------------------------------------------
// <copyright file="FeatureCollection.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The geojson <see cref="Feature"/> collection.
/// </summary>
[JsonConverter(typeof(FeatureCollectionConverter))]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "This is a collection")]
public class FeatureCollection
{
    /// <summary>
    /// Gets the features.
    /// </summary>
    public IReadOnlyList<Feature> Features { get; init; } = [];

    /// <inheritdoc/>
    public override string ToString() => JsonSerializer.Serialize(this);
}