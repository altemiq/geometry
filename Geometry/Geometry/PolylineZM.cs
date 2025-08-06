// -----------------------------------------------------------------------
// <copyright file="PolylineZM.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

/// <summary>
/// Represents a 4-dimensional polyline.
/// </summary>
public class PolylineZM : Polyline<PointZM>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="PolylineZM"/> class.
    /// </summary>
    public PolylineZM()
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="PolylineZM"/> class.
    /// </summary>
    /// <param name="points">The points.</param>
    public PolylineZM(params IEnumerable<PointZM> points)
        : base(points)
    {
    }

    /// <summary>
    /// Creates a new <see cref="PolylineZM"/> from the specified points.
    /// </summary>
    /// <param name="points">The points.</param>
    /// <returns>The created <see cref="PolylineZM"/>.</returns>
    public static PolylineZM FromPoints(params IEnumerable<PointZM> points) => [.. points];
}