// -----------------------------------------------------------------------
// <copyright file="MultiPolygon.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

#pragma warning disable SA1402
/// <summary>
/// Multiple <see cref="Altemiq.Geometry.Polyline{T}"/>.
/// </summary>
/// <typeparam name="TPoint">The type of point.</typeparam>
/// <typeparam name="TPolygon">The type of polygons.</typeparam>
/// <param name="points">The points.</param>
public abstract class MultiPolygon<TPoint, TPolygon>(IEnumerable<TPolygon> points) : IEnumerable<TPolygon>, IGeometry
    where TPolygon : Altemiq.Geometry.Polygon<TPoint>
{
    /// <inheritdoc/>
    public IEnumerator<TPolygon> GetEnumerator() => points.GetEnumerator();

    /// <inheritdoc/>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => ((System.Collections.IEnumerable)points).GetEnumerator();

    /// <inheritdoc/>
    public override string ToString() => JsonSerializer.Serialize(this);
}

#pragma warning disable MA0048
/// <summary>
/// Multiple <see cref="Polygon"/>.
/// </summary>
[JsonConverter(typeof(MultiPolygonConverters.MultiPolygonConverter))]
public class MultiPolygon(IEnumerable<Polygon> lines) : MultiPolygon<Point, Polygon>(lines);

/// <summary>
/// Multiple <see cref="PolygonZ"/>.
/// </summary>
[JsonConverter(typeof(MultiPolygonConverters.MultiPolygonZConverter))]

public class MultiPolygonZ(IEnumerable<PolygonZ> lines) : MultiPolygon<PointZ, PolygonZ>(lines);

/// <summary>
/// Multiple <see cref="PolygonM"/>.
/// </summary>
[JsonConverter(typeof(MultiPolygonConverters.MultiPolygonMConverter))]
public class MultiPolygonM(IEnumerable<PolygonM> lines) : MultiPolygon<PointM, PolygonM>(lines);

/// <summary>
/// Multiple <see cref="PolygonZM"/>.
/// </summary>
[JsonConverter(typeof(MultiPolygonConverters.MultiPolygonZMConverter))]
public class MultiPolygonZM(IEnumerable<PolygonZM> lines) : MultiPolygon<PointZM, PolygonZM>(lines);
#pragma warning restore MA0048, SA1402