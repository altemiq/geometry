// -----------------------------------------------------------------------
// <copyright file="PolygonZM.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

/// <summary>
/// Represents a 3-dimensional polygon with a measurement.
/// </summary>
[System.Runtime.InteropServices.ComVisible(false)]
public class PolygonZM : Polygon<PointZM>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="PolygonZM"/> class.
    /// </summary>
    public PolygonZM()
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="PolygonZM"/> class.
    /// </summary>
    /// <param name="rings">The list that is wrapped by the new collection.</param>
    public PolygonZM(params IEnumerable<LinearRing<PointZM>> rings)
        : base(rings)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="PolygonZM"/> class.
    /// </summary>
    /// <param name="rings">The list that is wrapped by the new collection.</param>
    public PolygonZM(params IEnumerable<IEnumerable<PointZM>> rings)
        : base(rings.Select(static ring => new LinearRing<PointZM>(ring)))
    {
    }

    /// <summary>
    /// Creates a new <see cref="PolygonZM"/> from the specified points.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <returns>The created <see cref="PolygonZM"/>.</returns>
    public static PolygonZM FromPoints(params IEnumerable<PointZM> points) => FromPoints(points, []);

    /// <summary>
    /// Creates a new <see cref="PolygonZM"/> from the specified list.
    /// </summary>
    /// <param name="list">The list of points.</param>
    /// <param name="holes">The holes.</param>
    /// <returns>The created <see cref="PolygonZM"/>.</returns>
    public static PolygonZM FromPoints(IEnumerable<PointZM> list, IEnumerable<IEnumerable<PointZM>> holes) => [[.. list], .. holes.Select(static hole => new LinearRing<PointZM>(hole))];
}