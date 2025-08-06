// -----------------------------------------------------------------------
// <copyright file="Polygon.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

/// <summary>
/// Represents a 2-dimensional polygon.
/// </summary>
[System.Runtime.InteropServices.ComVisible(false)]
public class Polygon : Polygon<Point>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="Polygon"/> class.
    /// </summary>
    public Polygon()
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="Polygon"/> class.
    /// </summary>
    /// <param name="rings">The list that is wrapped by the new collection.</param>
    public Polygon(params IEnumerable<LinearRing<Point>> rings)
        : base(rings)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="Polygon"/> class.
    /// </summary>
    /// <param name="rings">The list that is wrapped by the new collection.</param>
    public Polygon(params IEnumerable<IEnumerable<Point>> rings)
        : base(rings.Select(static ring => new LinearRing<Point>(ring)))
    {
    }

    /// <summary>
    /// Creates a new <see cref="Polygon"/> from the specified points.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <returns>The created <see cref="Polygon"/>.</returns>
    public static Polygon FromPoints(params IEnumerable<Point> points) => FromPoints(points, []);

    /// <summary>
    /// Creates a new <see cref="Polygon"/> from the specified list.
    /// </summary>
    /// <param name="list">The list of points.</param>
    /// <param name="holes">The holes.</param>
    /// <returns>The created <see cref="Polygon"/>.</returns>
    public static Polygon FromPoints(IEnumerable<Point> list, IEnumerable<IEnumerable<Point>> holes) => [[.. list], .. holes.Select(static hole => new LinearRing<Point>(hole))];
}