// -----------------------------------------------------------------------
// <copyright file="PolygonM.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

/// <summary>
/// Represents a 2-dimensional polygon with a measurement.
/// </summary>
[System.Runtime.InteropServices.ComVisible(false)]
public class PolygonM : Polygon<PointM>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="PolygonM"/> class.
    /// </summary>
    public PolygonM()
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="PolygonM"/> class.
    /// </summary>
    /// <param name="rings">The list that is wrapped by the new collection.</param>
    public PolygonM(params IEnumerable<LinearRing<PointM>> rings)
        : base(rings)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="PolygonM"/> class.
    /// </summary>
    /// <param name="rings">The list that is wrapped by the new collection.</param>
    public PolygonM(params IEnumerable<IEnumerable<PointM>> rings)
        : base(rings.Select(static ring => new LinearRing<PointM>(ring)))
    {
    }

    /// <summary>
    /// Creates a new <see cref="PolygonM"/> from the specified points.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <returns>The created <see cref="PolygonM"/>.</returns>
    public static PolygonM FromPoints(params IEnumerable<PointM> points) => FromPoints(points, []);

    /// <summary>
    /// Creates a new <see cref="PolygonM"/> from the specified list.
    /// </summary>
    /// <param name="list">The list of points.</param>
    /// <param name="holes">The holes.</param>
    /// <returns>The created <see cref="PolygonM"/>.</returns>
    public static PolygonM FromPoints(IEnumerable<PointM> list, IEnumerable<IEnumerable<PointM>> holes) => [[.. list], .. holes.Select(static hole => new LinearRing<PointM>(hole))];
}