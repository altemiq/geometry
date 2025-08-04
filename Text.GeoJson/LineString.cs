// -----------------------------------------------------------------------
// <copyright file="LineString.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

#pragma warning disable SA1402
/// <summary>
/// The <see cref="IGeometry"/> <see cref="Altemiq.Geometry.Polyline{TPoint}"/>.
/// </summary>
/// <typeparam name="TPoint">The type of point.</typeparam>
/// <param name="points">The points.</param>
public abstract class LineString<TPoint>(IEnumerable<TPoint> points) : Geometry.Polyline<TPoint>(points), IGeometry
{
    /// <inheritdoc/>
    public override string ToString() => JsonSerializer.Serialize(this);
}

#pragma warning disable MA0048
/// <summary>
/// The <see cref="IGeometry"/> <see cref="Altemiq.Geometry.Polyline"/>.
/// </summary>
/// <param name="points">The points.</param>
[JsonConverter(typeof(LineStringConverters.LineStringConverter))]
public class LineString(IEnumerable<Point> points) : LineString<Point>(points)
{
    /// <summary>
    /// Converts the <see cref="LineString"/> to a <see cref="Altemiq.Geometry.Polyline"/>.
    /// </summary>
    /// <param name="line">The input point.</param>
    public static implicit operator Geometry.Polyline(LineString line) => [.. line.Select(p => (Altemiq.Geometry.Point)p)];
}

/// <summary>
/// The <see cref="IGeometry"/> <see cref="Altemiq.Geometry.PolylineZ"/>.
/// </summary>
/// <param name="points">The points.</param>
[JsonConverter(typeof(LineStringConverters.LineStringZConverter))]
public class LineStringZ(IEnumerable<PointZ> points) : LineString<PointZ>(points)
{
    /// <summary>
    /// Converts the <see cref="LineStringZ"/> to a <see cref="Altemiq.Geometry.PolylineZ"/>.
    /// </summary>
    /// <param name="line">The input point.</param>
    public static implicit operator Geometry.PolylineZ(LineStringZ line) => [.. line.Select(p => (Altemiq.Geometry.PointZ)p)];
}

/// <summary>
/// The <see cref="IGeometry"/> <see cref="Altemiq.Geometry.PolylineM"/>.
/// </summary>
/// <param name="points">The points.</param>
[JsonConverter(typeof(LineStringConverters.LineStringMConverter))]
public class LineStringM(IEnumerable<PointM> points) : LineString<PointM>(points)
{
    /// <summary>
    /// Converts the <see cref="LineStringM"/> to a <see cref="Altemiq.Geometry.PolylineM"/>.
    /// </summary>
    /// <param name="line">The input point.</param>
    public static implicit operator Geometry.PolylineM(LineStringM line) => [.. line.Select(p => (Altemiq.Geometry.PointM)p)];
}

/// <summary>
/// The <see cref="IGeometry"/> <see cref="Altemiq.Geometry.PolylineZM"/>.
/// </summary>
/// <param name="points">The points.</param>
[JsonConverter(typeof(LineStringConverters.LineStringZMConverter))]
public class LineStringZM(IEnumerable<PointZM> points) : LineString<PointZM>(points)
{
    /// <summary>
    /// Converts the <see cref="LineStringZM"/> to a <see cref="Altemiq.Geometry.PolylineZM"/>.
    /// </summary>
    /// <param name="line">The input point.</param>
    public static implicit operator Geometry.PolylineZM(LineStringZM line) => [.. line.Select(p => (Altemiq.Geometry.PointZM)p)];
}
#pragma warning restore MA0048, SA1402