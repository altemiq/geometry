// -----------------------------------------------------------------------
// <copyright file="Polygon.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The <see cref="IGeometry"/> <see cref="Altemiq.Geometry.Polygon"/>.
/// </summary>
[JsonConverter(typeof(PolygonConverters.PolygonConverter))]
public class Polygon : Polygon<Point>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="Polygon"/> class.
    /// </summary>
    /// <param name="points">The single ring that is wrapped by the new collection.</param>
    public Polygon(params Point[] points)
        : base(points)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="Polygon"/> class.
    /// </summary>
    /// <param name="rings">The list that is wrapped by the new collection.</param>
    public Polygon(params Altemiq.Geometry.LinearRing<Point>[] rings)
        : base(rings)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="Polygon"/> class.
    /// </summary>
    /// <param name="points">The single ring that is wrapped by the new collection.</param>
    public Polygon(IEnumerable<Point> points)
        : base(points)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="Polygon"/> class.
    /// </summary>
    /// <param name="polygon">The polygon.</param>
    public Polygon(Polygon polygon)
        : base(polygon)
    {
    }
}