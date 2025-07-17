// -----------------------------------------------------------------------
// <copyright file="PolylineZ.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

/// <summary>
/// Represents a 3-dimensional polyline.
/// </summary>
public class PolylineZ : Polyline<PointZ>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="PolylineZ"/> class.
    /// </summary>
    public PolylineZ()
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="PolylineZ"/> class.
    /// </summary>
    /// <param name="points">The points.</param>
    public PolylineZ(params IEnumerable<PointZ> points)
        : base(points)
    {
    }

    /// <summary>
    /// Creates a new <see cref="PolylineZ"/> from the specified points.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <returns>The created <see cref="PolylineZ"/>.</returns>
    public static PolylineZ FromPoints(params IEnumerable<PointZ> points) => [.. points];
}