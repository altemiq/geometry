// -----------------------------------------------------------------------
// <copyright file="Point.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The <see cref="Altemiq.Geometry.Point"/> <see cref="IGeometry"/>.
/// </summary>
/// <param name="x">The x-coordinate.</param>
/// <param name="y">The y-coordinate.</param>
[JsonConverter(typeof(PointConverters.PointConverter))]
public sealed class Point(double x, double y) : IGeometry
{
    /// <summary>
    /// Initialises a new instance of the <see cref="Point"/> class.
    /// </summary>
    /// <param name="point">The point to initialise from.</param>
    public Point(Altemiq.Geometry.Point point)
        : this(point.X, point.Y)
    {
    }

    /// <inheritdoc cref="Altemiq.Geometry.Point.X"/>
    public double X { get; } = x;

    /// <inheritdoc cref="Altemiq.Geometry.Point.Y"/>
    public double Y { get; } = y;

    /// <inheritdoc cref="Altemiq.Geometry.Point.IsEmpty"/>
    public bool IsEmpty => this.X.Equals(default) && this.Y.Equals(default);

    /// <summary>
    /// Converts the <see cref="Altemiq.Geometry.Point"/> to a <see cref="Point"/>.
    /// </summary>
    /// <param name="point">The input point.</param>
    public static explicit operator Point(Altemiq.Geometry.Point point) => new(point);

    /// <summary>
    /// Converts the <see cref="Point"/> to a <see cref="Altemiq.Geometry.Point"/>.
    /// </summary>
    /// <param name="point">The input point.</param>
    public static implicit operator Altemiq.Geometry.Point(Point point) => new(point.X, point.Y);

    /// <inheritdoc cref="Altemiq.Geometry.Point.Clone"/>
    public object Clone() => new Point(this);

    /// <inheritdoc cref="Altemiq.Geometry.Point.Deconstruct"/>
    public void Deconstruct(out double x, out double y) => (x, y) = (this.X, this.Y);

    /// <inheritdoc/>
    public override string ToString() => JsonSerializer.Serialize(this);
}