// -----------------------------------------------------------------------
// <copyright file="PolygonM.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The <see cref="IGeometry"/> <see cref="Altemiq.Geometry.PolygonM"/>.
/// </summary>
[JsonConverter(typeof(PolygonConverters.PolygonMConverter))]
public class PolygonM : Polygon<PointM>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="PolygonM"/> class.
    /// </summary>
    /// <param name="points">The single ring that is wrapped by the new collection.</param>
    public PolygonM(params PointM[] points)
        : base(points)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="PolygonM"/> class.
    /// </summary>
    /// <param name="rings">The list that is wrapped by the new collection.</param>
    public PolygonM(params Geometry.LinearRing<PointM>[] rings)
        : base(rings)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="PolygonM"/> class.
    /// </summary>
    /// <param name="points">The single ring that is wrapped by the new collection.</param>
    public PolygonM(IEnumerable<PointM> points)
        : base(points)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="PolygonM"/> class.
    /// </summary>
    /// <param name="polygon">The polygon.</param>
    public PolygonM(PolygonM polygon)
        : base(polygon)
    {
    }
}