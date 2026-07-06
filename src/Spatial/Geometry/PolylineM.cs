// -----------------------------------------------------------------------
// <copyright file="PolylineM.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

/// <summary>
/// Represents a 2-dimensional polyline with a measurement.
/// </summary>
public class PolylineM : Polyline<PointM>, IGeometryM
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

    /// <inheritdoc />
    double IGeometryM.MinM() => this.Min(p => p.Measurement);

    /// <inheritdoc />
    double IGeometryM.MaxM() => this.Max(p => p.Measurement);

    /// <inheritdoc />
    protected override double MinX() => this.Min(p => p.X);

    /// <inheritdoc />
    protected override double MaxX() => this.Max(p => p.X);

    /// <inheritdoc />
    protected override double MinY() => this.Min(p => p.Y);

    /// <inheritdoc />
    protected override double MaxY() => this.Max(p => p.Y);
}