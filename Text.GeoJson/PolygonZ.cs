// -----------------------------------------------------------------------
// <copyright file="PolygonZ.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The <see cref="IGeometry"/> <see cref="Altemiq.Geometry.PolygonZ"/>.
/// </summary>
[JsonConverter(typeof(PolygonConverters.PolygonZConverter))]
public class PolygonZ : Polygon<PointZ>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="PolygonZ"/> class.
    /// </summary>
    /// <param name="points">The single ring that is wrapped by the new collection.</param>
    public PolygonZ(params PointZ[] points)
        : base(points)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="PolygonZ"/> class.
    /// </summary>
    /// <param name="rings">The list that is wrapped by the new collection.</param>
    public PolygonZ(params Altemiq.Geometry.LinearRing<PointZ>[] rings)
        : base(rings)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="PolygonZ"/> class.
    /// </summary>
    /// <param name="points">The single ring that is wrapped by the new collection.</param>
    public PolygonZ(IEnumerable<PointZ> points)
        : base(points)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="PolygonZ"/> class.
    /// </summary>
    /// <param name="polygon">The polygon.</param>
    public PolygonZ(PolygonZ polygon)
        : base(polygon)
    {
    }
}