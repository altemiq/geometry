// -----------------------------------------------------------------------
// <copyright file="PointZM.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The <see cref="Altemiq.Geometry.PointZM"/> <see cref="IGeometry"/>.
/// </summary>
/// <param name="x">The x-coordinate.</param>
/// <param name="y">The y-coordinate.</param>
/// <param name="z">The z-coordinate.</param>
/// <param name="m">The m-coordinate.</param>
[JsonConverter(typeof(PointConverters.PointZMConverter))]
public sealed class PointZM(double x, double y, double z, double m) : IGeometry
{
    /// <summary>
    /// Initialises a new instance of the <see cref="PointZM"/> class.
    /// </summary>
    /// <param name="point">The point to initialise from.</param>
    public PointZM(Altemiq.Geometry.PointZM point)
        : this(point.X, point.Y, point.Z, point.Measurement)
    {
    }

    /// <inheritdoc cref="Altemiq.Geometry.PointZM.X"/>
    public double X { get; } = x;

    /// <inheritdoc cref="Altemiq.Geometry.PointZM.Y"/>
    public double Y { get; } = y;

    /// <inheritdoc cref="Altemiq.Geometry.PointZM.Z"/>
    public double Z { get; } = z;

    /// <inheritdoc cref="Altemiq.Geometry.PointZM.Measurement"/>
    public double Measurement { get; } = m;

    /// <inheritdoc cref="Altemiq.Geometry.PointZM.IsEmpty"/>
    public bool IsEmpty => this.X.Equals(default) && this.Y.Equals(default) && this.Z.Equals(default) && this.Measurement.Equals(default);

    /// <summary>
    /// Converts the <see cref="Altemiq.Geometry.PointZM"/> to a <see cref="PointZM"/>.
    /// </summary>
    /// <param name="point">The input point.</param>
    public static explicit operator PointZM(Altemiq.Geometry.PointZM point) => new(point);

    /// <summary>
    /// Converts the <see cref="PointZM"/> to a <see cref="Altemiq.Geometry.PointZM"/>.
    /// </summary>
    /// <param name="point">The input point.</param>
    public static implicit operator Altemiq.Geometry.PointZM(PointZM point) => new(point.X, point.Y, point.Z, point.Measurement);

    /// <inheritdoc cref="Altemiq.Geometry.PointZM.Clone"/>
    public object Clone() => new PointZM(this);

    /// <inheritdoc cref="Altemiq.Geometry.PointZM.Deconstruct"/>
    public void Deconstruct(out double x, out double y, out double z, out double m) => (x, y, z, m) = (this.X, this.Y, this.Z, this.Measurement);

    /// <inheritdoc/>
    public override string ToString() => JsonSerializer.Serialize(this);
}