// -----------------------------------------------------------------------
// <copyright file="PolygonZM.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The <see cref="IGeometry"/> <see cref="Altemiq.Geometry.PolygonZM"/>.
/// </summary>
[JsonConverter(typeof(PolygonConverters.PolygonZMConverter))]
public class PolygonZM : Polygon<PointZM>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="PolygonZM"/> class.
    /// </summary>
    /// <param name="points">The single ring that is wrapped by the new collection.</param>
    public PolygonZM(params PointZM[] points)
        : base(points)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="PolygonZM"/> class.
    /// </summary>
    /// <param name="rings">The list that is wrapped by the new collection.</param>
    public PolygonZM(params Altemiq.Geometry.LinearRing<PointZM>[] rings)
        : base(rings)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="PolygonZM"/> class.
    /// </summary>
    /// <param name="points">The single ring that is wrapped by the new collection.</param>
    public PolygonZM(IEnumerable<PointZM> points)
        : base(points)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="PolygonZM"/> class.
    /// </summary>
    /// <param name="polygon">The polygon.</param>
    public PolygonZM(PolygonZM polygon)
        : base(polygon)
    {
    }
}