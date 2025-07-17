// -----------------------------------------------------------------------
// <copyright file="PolygonZ.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

/// <summary>
/// Represents a 3-dimensional polygon.
/// </summary>
[System.Runtime.InteropServices.ComVisible(false)]
public class PolygonZ : Polygon<PointZ>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="PolygonZ"/> class.
    /// </summary>
    public PolygonZ()
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="PolygonZ"/> class.
    /// </summary>
    /// <param name="rings">The list that is wrapped by the new collection.</param>
    public PolygonZ(params IEnumerable<LinearRing<PointZ>> rings)
        : base(rings)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="PolygonZ"/> class.
    /// </summary>
    /// <param name="rings">The list that is wrapped by the new collection.</param>
    public PolygonZ(params IEnumerable<IEnumerable<PointZ>> rings)
        : base(rings.Select(static ring => new LinearRing<PointZ>(ring)))
    {
    }

    /// <summary>
    /// Creates a new <see cref="PolygonZ"/> from the specified points.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <returns>The created <see cref="PolygonZ"/>.</returns>
    public static PolygonZ FromPoints(params PointZ[] points) => FromPoints(points, []);

    /// <summary>
    /// Creates a new <see cref="PolygonZ"/> from the specified list.
    /// </summary>
    /// <param name="list">The list of points.</param>
    /// <param name="holes">The holes.</param>
    /// <returns>The created <see cref="PolygonZ"/>.</returns>
    public static PolygonZ FromPoints(IEnumerable<PointZ> list, IEnumerable<IEnumerable<PointZ>> holes) => [[.. list], .. holes.Select(static hole => new LinearRing<PointZ>(hole))];
}