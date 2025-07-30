// -----------------------------------------------------------------------
// <copyright file="PointZ.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The <see cref="Altemiq.Geometry.PointZ"/> <see cref="IGeometry"/>.
/// </summary>
/// <param name="x">The x-coordinate.</param>
/// <param name="y">The y-coordinate.</param>
/// <param name="z">The z-coordinate.</param>
[JsonConverter(typeof(PointConverters.PointZConverter))]
public sealed class PointZ(double x, double y, double z) : IGeometry
{
    /// <summary>
    /// Initialises a new instance of the <see cref="PointZ"/> class.
    /// </summary>
    /// <param name="point">The point to initialise from.</param>
    public PointZ(Altemiq.Geometry.PointZ point)
        : this(point.X, point.Y, point.Z)
    {
    }

    /// <inheritdoc cref="Altemiq.Geometry.PointZ.X"/>
    public double X { get; } = x;

    /// <inheritdoc cref="Altemiq.Geometry.PointZ.Y"/>
    public double Y { get; } = y;

    /// <inheritdoc cref="Altemiq.Geometry.PointZ.Z"/>
    public double Z { get; } = z;

    /// <inheritdoc cref="Altemiq.Geometry.PointZ.IsEmpty"/>
    public bool IsEmpty => this.X.Equals(default) && this.Y.Equals(default) && this.Z.Equals(default);

    /// <summary>
    /// Converts the <see cref="Altemiq.Geometry.PointZ"/> to a <see cref="PointZ"/>.
    /// </summary>
    /// <param name="point">The input point.</param>
    public static explicit operator PointZ(Altemiq.Geometry.PointZ point) => new(point);

    /// <summary>
    /// Converts the <see cref="PointZ"/> to a <see cref="Altemiq.Geometry.PointZ"/>.
    /// </summary>
    /// <param name="point">The input point.</param>
    public static implicit operator Altemiq.Geometry.PointZ(PointZ point) => new(point.X, point.Y, point.Z);

    /// <inheritdoc cref="Altemiq.Geometry.PointZ.Clone"/>
    public object Clone() => new PointZ(this);

    /// <inheritdoc cref="Altemiq.Geometry.PointZ.Deconstruct"/>
    public void Deconstruct(out double x, out double y, out double z) => (x, y, z) = (this.X, this.Y, this.Z);

    /// <inheritdoc/>
    public override string ToString() => JsonSerializer.Serialize(this);
}