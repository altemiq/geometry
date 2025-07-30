// -----------------------------------------------------------------------
// <copyright file="MultiPoint.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

#pragma warning disable SA1402
/// <summary>
/// The multiple <see cref="Altemiq.Geometry.Point"/>.
/// </summary>
/// <typeparam name="TPoint">The type of point.</typeparam>
/// <param name="points">The points.</param>
public abstract class MultiPoint<TPoint>(IEnumerable<TPoint> points) : IEnumerable<TPoint>, IGeometry
{
    /// <inheritdoc/>
    public IEnumerator<TPoint> GetEnumerator() => points.GetEnumerator();

    /// <inheritdoc/>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => ((System.Collections.IEnumerable)points).GetEnumerator();

    /// <inheritdoc/>
    public override string ToString() => JsonSerializer.Serialize(this);
}

#pragma warning disable MA0048
/// <summary>
/// The multiple <see cref="Point"/>.
/// </summary>
/// <param name="points">The points.</param>
[JsonConverter(typeof(MultiPointConverters.MultiPointConverter))]
public class MultiPoint(IEnumerable<Point> points) : MultiPoint<Point>(points);

/// <summary>
/// The multiple <see cref="PointZ"/>.
/// </summary>
/// <param name="points">The points.</param>
[JsonConverter(typeof(MultiPointConverters.MultiPointZConverter))]
public class MultiPointZ(IEnumerable<PointZ> points) : MultiPoint<PointZ>(points);

/// <summary>
/// The multiple <see cref="PointM"/>.
/// </summary>
/// <param name="points">The points.</param>
[JsonConverter(typeof(MultiPointConverters.MultiPointMConverter))]
public class MultiPointM(IEnumerable<PointM> points) : MultiPoint<PointM>(points);

/// <summary>
/// The multiple <see cref="PointZM"/>.
/// </summary>
/// <param name="points">The points.</param>
[JsonConverter(typeof(MultiPointConverters.MultiPointZMConverter))]
public class MultiPointZM(IEnumerable<PointZM> points) : MultiPoint<PointZM>(points);
#pragma warning restore MA0048, SA1402