// -----------------------------------------------------------------------
// <copyright file="PolylineM.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

/// <summary>
/// Represents a 2-dimensional polyline with a measurement.
/// </summary>
public class PolylineM : Polyline<PointM>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="PolylineM"/> class.
    /// </summary>
    public PolylineM()
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="PolylineM"/> class.
    /// </summary>
    /// <param name="points">The points.</param>
    public PolylineM(params IEnumerable<PointM> points)
        : base(points)
    {
    }

    /// <summary>
    /// Creates a new <see cref="PolylineM"/> from the specified points.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <returns>The created <see cref="PolylineM"/>.</returns>
    public static PolylineM FromPoints(params IEnumerable<PointM> points) => [.. points];
}