// -----------------------------------------------------------------------
// <copyright file="Polyline.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

/// <summary>
/// Represents a 2-dimensional polyline.
/// </summary>
public class Polyline : Polyline<Point>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="Polyline"/> class.
    /// </summary>
    public Polyline()
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="Polyline"/> class.
    /// </summary>
    /// <param name="points">The points.</param>
    public Polyline(params IEnumerable<Point> points)
        : base(points)
    {
    }

    /// <summary>
    /// Creates a new <see cref="Polyline"/> from the specified points.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <returns>The created <see cref="Polyline"/>.</returns>
    public static Polyline FromPoints(params IEnumerable<Point> points) => [.. points];
}