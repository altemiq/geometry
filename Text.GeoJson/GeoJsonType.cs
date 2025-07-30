// -----------------------------------------------------------------------
// <copyright file="GeoJsonType.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The geo-json type.
/// </summary>
public enum GeoJsonType
{
    /// <summary>
    /// Defines the <see cref="Feature"/> type.
    /// </summary>
    /// <see href="https://tools.ietf.org/html/rfc7946#section-3.2" />
    Feature,

    /// <summary>
    /// Defines the FeatureCollection type.
    /// </summary>
    /// <see href="https://tools.ietf.org/html/rfc7946#section-3.3" />
    FeatureCollection,
}