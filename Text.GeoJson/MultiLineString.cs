// -----------------------------------------------------------------------
// <copyright file="MultiLineString.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

#pragma warning disable SA1402
/// <summary>
/// Multiple <see cref="Altemiq.Geometry.Polyline{T}"/>.
/// </summary>
/// <typeparam name="TPoint">The type of point.</typeparam>
/// <typeparam name="TLine">The type of line.</typeparam>
/// <param name="lines">The lines.</param>
public abstract class MultiLineString<TPoint, TLine>(IEnumerable<TLine> lines) : IEnumerable<TLine>, IGeometry
    where TLine : Geometry.Polyline<TPoint>
{
    /// <inheritdoc/>
    public IEnumerator<TLine> GetEnumerator() => lines.GetEnumerator();

    /// <inheritdoc/>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => ((System.Collections.IEnumerable)lines).GetEnumerator();

    /// <inheritdoc/>
    public override string ToString() => JsonSerializer.Serialize(this);
}

#pragma warning disable MA0048
/// <summary>
/// Multiple <see cref="LineString"/>.
/// </summary>
[JsonConverter(typeof(MultiLineStringConverters.MultiLineStringConverter))]
public class MultiLineString(IEnumerable<LineString> lines) : MultiLineString<Point, LineString>(lines);

/// <summary>
/// Multiple <see cref="LineStringZ"/>.
/// </summary>
[JsonConverter(typeof(MultiLineStringConverters.MultiLineStringZConverter))]

public class MultiLineStringZ(IEnumerable<LineStringZ> lines) : MultiLineString<PointZ, LineStringZ>(lines);

/// <summary>
/// Multiple <see cref="LineStringM"/>.
/// </summary>
[JsonConverter(typeof(MultiLineStringConverters.MultiLineStringMConverter))]
public class MultiLineStringM(IEnumerable<LineStringM> lines) : MultiLineString<PointM, LineStringM>(lines);

/// <summary>
/// Multiple <see cref="LineStringZM"/>.
/// </summary>
[JsonConverter(typeof(MultiLineStringConverters.MultiLineStringZMConverter))]
public class MultiLineStringZM(IEnumerable<LineStringZM> lines) : MultiLineString<PointZM, LineStringZM>(lines);
#pragma warning restore MA0048, SA1402