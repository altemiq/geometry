// -----------------------------------------------------------------------
// <copyright file="PointM.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The <see cref="Altemiq.Geometry.PointM"/> <see cref="IGeometry"/>.
/// </summary>
/// <param name="x">The x-coordinate.</param>
/// <param name="y">The y-coordinate.</param>
/// <param name="m">The m-coordinate.</param>
[JsonConverter(typeof(PointConverters.PointMConverter))]
public sealed class PointM(double x, double y, double m) : IGeometry
{
    /// <summary>
    /// Initialises a new instance of the <see cref="PointM"/> class.
    /// </summary>
    /// <param name="point">The point to initialise from.</param>
    public PointM(Altemiq.Geometry.PointM point)
        : this(point.X, point.Y, point.Measurement)
    {
    }

    /// <inheritdoc cref="Altemiq.Geometry.PointM.X"/>
    public double X { get; } = x;

    /// <inheritdoc cref="Altemiq.Geometry.PointM.Y"/>
    public double Y { get; } = y;

    /// <inheritdoc cref="Altemiq.Geometry.PointM.Measurement"/>
    public double Measurement { get; } = m;

    /// <inheritdoc cref="Altemiq.Geometry.PointM.IsEmpty"/>
    public bool IsEmpty => this.X.Equals(default) && this.Y.Equals(default) && this.Measurement.Equals(default);

    /// <summary>
    /// Converts the <see cref="Altemiq.Geometry.PointM"/> to a <see cref="PointM"/>.
    /// </summary>
    /// <param name="point">The input point.</param>
    public static explicit operator PointM(Altemiq.Geometry.PointM point) => new(point);

    /// <summary>
    /// Converts the <see cref="PointM"/> to a <see cref="Altemiq.Geometry.PointM"/>.
    /// </summary>
    /// <param name="point">The input point.</param>
    public static implicit operator Altemiq.Geometry.PointM(PointM point) => new(point.X, point.Y, point.Measurement);

    /// <inheritdoc cref="Altemiq.Geometry.PointM.Clone"/>
    public object Clone() => new PointM(this);

    /// <inheritdoc cref="Altemiq.Geometry.PointM.Deconstruct"/>
    public void Deconstruct(out double x, out double y, out double m) => (x, y, m) = (this.X, this.Y, this.Measurement);

    /// <inheritdoc/>
    public override string ToString() => JsonSerializer.Serialize(this);
}