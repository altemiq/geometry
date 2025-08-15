// -----------------------------------------------------------------------
// <copyright file="GeometryType.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The <see cref="Geometry.IGeometry"/> type.
/// </summary>
public enum GeometryType
{
    /// <summary>
    /// Defines the Point type.
    /// </summary>
    /// <see href="https://tools.ietf.org/html/rfc7946#section-3.1.2"/>
    Point,

    /// <summary>
    /// Defines the MultiPoint type.
    /// </summary>
    /// <see href="https://tools.ietf.org/html/rfc7946#section-3.1.3"/>
    MultiPoint,

    /// <summary>
    /// Defines the LineString type.
    /// </summary>
    /// <see href="https://tools.ietf.org/html/rfc7946#section-3.1.4"/>
    LineString,

    /// <summary>
    /// Defines the MultiLineString type.
    /// </summary>
    /// <see href="https://tools.ietf.org/html/rfc7946#section-3.1.5"/>
    MultiLineString,

    /// <summary>
    /// Defines the Polygon type.
    /// </summary>
    /// <see href="https://tools.ietf.org/html/rfc7946#section-3.1.6"/>
    Polygon,

    /// <summary>
    /// Defines the MultiPolygon type.
    /// </summary>
    /// <see href="https://tools.ietf.org/html/rfc7946#section-3.1.7"/>
    MultiPolygon,

    /// <summary>
    /// Defines the GeometryCollection type.
    /// </summary>
    /// <see href="https://tools.ietf.org/html/rfc7946#section-3.1.8"/>
    GeometryCollection,
}