// -----------------------------------------------------------------------
// <copyright file="PolylineZM.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

/// <summary>
/// Represents a 4-dimensional polyline.
/// </summary>
public class PolylineZM : Polyline<PointZM>, IGeometryZM
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

    /// <inheritdoc />
    double IGeometryZ.MinZ() => this.Min(p => p.Z);

    /// <inheritdoc />
    double IGeometryZ.MaxZ() => this.Max(p => p.Z);

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