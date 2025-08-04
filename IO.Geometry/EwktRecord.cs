// -----------------------------------------------------------------------
// <copyright file="EwktRecord.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

/// <summary>
/// Represents an extended <see cref="WktRecord"/>.
/// </summary>
/// <param name="wkt">The well known text.</param>
public class EwktRecord(string wkt) : WktRecord(wkt), Data.ISridGeometryRecord
{
    /// <inheritdoc/>
    public int GetSrid() => GetSrid(this.Wkt.AsMemory());

    /// <inheritdoc/>
    public override object GetGeometry() => GetGeometry(this.GetWellKnownText());

    /// <inheritdoc/>
    public override Point GetPoint() => GetPoint(this.GetWellKnownText());

    /// <inheritdoc/>
    public override PointZ GetPointZ() => GetPointZ(this.GetWellKnownText());

    /// <inheritdoc/>
    public override PointM GetPointM() => GetPointM(this.GetWellKnownText());

    /// <inheritdoc/>
    public override PointZM GetPointZM() => GetPointZM(this.GetWellKnownText());

    /// <inheritdoc/>
    public override IReadOnlyCollection<Point> GetMultiPoint() => GetMultiPoint(this.GetWellKnownText());

    /// <inheritdoc/>
    public override IReadOnlyCollection<PointZ> GetMultiPointZ() => GetMultiPointZ(this.GetWellKnownText());

    /// <inheritdoc/>
    public override IReadOnlyCollection<PointM> GetMultiPointM() => GetMultiPointM(this.GetWellKnownText());

    /// <inheritdoc/>
    public override IReadOnlyCollection<PointZM> GetMultiPointZM() => GetMultiPointZM(this.GetWellKnownText());

    /// <inheritdoc/>
    public override Polyline GetLineString() => GetLineString(this.GetWellKnownText());

    /// <inheritdoc/>
    public override PolylineZ GetLineStringZ() => GetLineStringZ(this.GetWellKnownText());

    /// <inheritdoc/>
    public override PolylineM GetLineStringM() => GetLineStringM(this.GetWellKnownText());

    /// <inheritdoc/>
    public override PolylineZM GetLineStringZM() => GetLineStringZM(this.GetWellKnownText());

    /// <inheritdoc/>
    public override IReadOnlyCollection<Polyline> GetMultiLineString() => [.. GetMultiLineString(this.GetWellKnownText())];

    /// <inheritdoc/>
    public override IReadOnlyCollection<PolylineZ> GetMultiLineStringZ() => [.. GetMultiLineStringZ(this.GetWellKnownText())];

    /// <inheritdoc/>
    public override IReadOnlyCollection<PolylineM> GetMultiLineStringM() => [.. GetMultiLineStringM(this.GetWellKnownText())];

    /// <inheritdoc/>
    public override IReadOnlyCollection<PolylineZM> GetMultiLineStringZM() => [.. GetMultiLineStringZM(this.GetWellKnownText())];

    /// <inheritdoc/>
    public override Polygon GetPolygon() => GetPolygon(this.GetWellKnownText());

    /// <inheritdoc/>
    public override PolygonZ GetPolygonZ() => GetPolygonZ(this.GetWellKnownText());

    /// <inheritdoc/>
    public override PolygonM GetPolygonM() => GetPolygonM(this.GetWellKnownText());

    /// <inheritdoc/>
    public override PolygonZM GetPolygonZM() => GetPolygonZM(this.GetWellKnownText());

    /// <inheritdoc/>
    public override IReadOnlyCollection<Polygon> GetMultiPolygon() => [.. GetMultiPolygon(this.GetWellKnownText())];

    /// <inheritdoc/>
    public override IReadOnlyCollection<PolygonZ> GetMultiPolygonZ() => [.. GetMultiPolygonZ(this.GetWellKnownText())];

    /// <inheritdoc/>
    public override IReadOnlyCollection<PolygonM> GetMultiPolygonM() => [.. GetMultiPolygonM(this.GetWellKnownText())];

    /// <inheritdoc/>
    public override IReadOnlyCollection<PolygonZM> GetMultiPolygonZM() => [.. GetMultiPolygonZM(this.GetWellKnownText())];

    private static int GetSrid(ReadOnlyMemory<char> wkt)
    {
        var span = wkt.Span;
#pragma warning disable SA1008
        return (span.IndexOf('='), span.IndexOf(';')) switch
        {
            ( >= 0, >= 0) indexes => GetSridCore(span[(indexes.Item1 + 1)..indexes.Item2]),
            _ => 0,
        };
#pragma warning restore SA1008

        static int GetSridCore(ReadOnlySpan<char> span)
        {
#if NETSTANDARD2_1_OR_GREATER
            return int.Parse(span, provider: System.Globalization.CultureInfo.InvariantCulture);
#else
            return int.Parse(span.ToString(), System.Globalization.CultureInfo.InvariantCulture);
#endif
        }
    }

    private ReadOnlyMemory<char> GetWellKnownText()
    {
        var memory = this.Wkt.AsMemory();
        var span = memory.Span;
        var index = span.IndexOf(';');
        if (index is -1)
        {
            return memory;
        }

        // find the first and last indexes
        var start = index;
        while (++start < span.Length)
        {
            if (!char.IsWhiteSpace(span[start]))
            {
                break;
            }
        }

        var end = span.Length;
        while (--end > 0)
        {
            if (!char.IsWhiteSpace(span[end]))
            {
                break;
            }
        }

        return memory[start..(end + 1)];
    }
}