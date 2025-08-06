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
    public int GetSrid() => GetSrid(this.Wkt.AsSpan());

    /// <inheritdoc/>
    public override Point GetPoint() => Altemiq.Buffers.Text.WktParser.TryParse(this.GetWellKnownText(), out Point point, out _) ? point : throw new InvalidGeometryTypeException();

    /// <inheritdoc/>
    public override PointZ GetPointZ() => Altemiq.Buffers.Text.WktParser.TryParse(this.GetWellKnownText(), out PointZ point, out _) ? point : throw new InvalidGeometryTypeException();

    /// <inheritdoc/>
    public override PointM GetPointM() => Altemiq.Buffers.Text.WktParser.TryParse(this.GetWellKnownText(), out PointM point, out _) ? point : throw new InvalidGeometryTypeException();

    /// <inheritdoc/>
    public override PointZM GetPointZM() => Altemiq.Buffers.Text.WktParser.TryParse(this.GetWellKnownText(), out PointZM point, out _) ? point : throw new InvalidGeometryTypeException();

    /// <inheritdoc/>
    public override IReadOnlyCollection<Point> GetMultiPoint() => Altemiq.Buffers.Text.WktParser.TryParse(this.GetWellKnownText(), out IReadOnlyCollection<Point>? points, out _) ? points : throw new InvalidGeometryTypeException();

    /// <inheritdoc/>
    public override IReadOnlyCollection<PointZ> GetMultiPointZ() => Altemiq.Buffers.Text.WktParser.TryParse(this.GetWellKnownText(), out IReadOnlyCollection<PointZ>? points, out _) ? points : throw new InvalidGeometryTypeException();

    /// <inheritdoc/>
    public override IReadOnlyCollection<PointM> GetMultiPointM() => Altemiq.Buffers.Text.WktParser.TryParse(this.GetWellKnownText(), out IReadOnlyCollection<PointM>? points, out _) ? points : throw new InvalidGeometryTypeException();

    /// <inheritdoc/>
    public override IReadOnlyCollection<PointZM> GetMultiPointZM() => Altemiq.Buffers.Text.WktParser.TryParse(this.GetWellKnownText(), out IReadOnlyCollection<PointZM>? points, out _) ? points : throw new InvalidGeometryTypeException();

    /// <inheritdoc/>
    public override Polyline GetLineString() => Altemiq.Buffers.Text.WktParser.TryParse(this.GetWellKnownText(), out Polyline? polyline, out _) ? polyline : throw new InvalidGeometryTypeException();

    /// <inheritdoc/>
    public override PolylineZ GetLineStringZ() => Altemiq.Buffers.Text.WktParser.TryParse(this.GetWellKnownText(), out PolylineZ? polyline, out _) ? polyline : throw new InvalidGeometryTypeException();

    /// <inheritdoc/>
    public override PolylineM GetLineStringM() => Altemiq.Buffers.Text.WktParser.TryParse(this.GetWellKnownText(), out PolylineM? polyline, out _) ? polyline : throw new InvalidGeometryTypeException();

    /// <inheritdoc/>
    public override PolylineZM GetLineStringZM() => Altemiq.Buffers.Text.WktParser.TryParse(this.GetWellKnownText(), out PolylineZM? polyline, out _) ? polyline : throw new InvalidGeometryTypeException();

    /// <inheritdoc/>
    public override IReadOnlyCollection<Polyline> GetMultiLineString() => Altemiq.Buffers.Text.WktParser.TryParse(this.GetWellKnownText(), out IReadOnlyCollection<Polyline>? polylines, out _) ? polylines : throw new InvalidGeometryTypeException();

    /// <inheritdoc/>
    public override IReadOnlyCollection<PolylineZ> GetMultiLineStringZ() => Altemiq.Buffers.Text.WktParser.TryParse(this.GetWellKnownText(), out IReadOnlyCollection<PolylineZ>? polylines, out _) ? polylines : throw new InvalidGeometryTypeException();

    /// <inheritdoc/>
    public override IReadOnlyCollection<PolylineM> GetMultiLineStringM() => Altemiq.Buffers.Text.WktParser.TryParse(this.GetWellKnownText(), out IReadOnlyCollection<PolylineM>? polylines, out _) ? polylines : throw new InvalidGeometryTypeException();

    /// <inheritdoc/>
    public override IReadOnlyCollection<PolylineZM> GetMultiLineStringZM() => Altemiq.Buffers.Text.WktParser.TryParse(this.GetWellKnownText(), out IReadOnlyCollection<PolylineZM>? polylines, out _) ? polylines : throw new InvalidGeometryTypeException();

    /// <inheritdoc/>
    public override Polygon GetPolygon() => Altemiq.Buffers.Text.WktParser.TryParse(this.GetWellKnownText(), out Polygon? polygon, out _) ? polygon : throw new InvalidGeometryTypeException();

    /// <inheritdoc/>
    public override PolygonZ GetPolygonZ() => Altemiq.Buffers.Text.WktParser.TryParse(this.GetWellKnownText(), out PolygonZ? polygon, out _) ? polygon : throw new InvalidGeometryTypeException();

    /// <inheritdoc/>
    public override PolygonM GetPolygonM() => Altemiq.Buffers.Text.WktParser.TryParse(this.GetWellKnownText(), out PolygonM? polygon, out _) ? polygon : throw new InvalidGeometryTypeException();

    /// <inheritdoc/>
    public override PolygonZM GetPolygonZM() => Altemiq.Buffers.Text.WktParser.TryParse(this.GetWellKnownText(), out PolygonZM? polygon, out _) ? polygon : throw new InvalidGeometryTypeException();

    /// <inheritdoc/>
    public override IReadOnlyCollection<Polygon> GetMultiPolygon() => Altemiq.Buffers.Text.WktParser.TryParse(this.GetWellKnownText(), out IReadOnlyCollection<Polygon>? polygons, out _) ? polygons : throw new InvalidGeometryTypeException();

    /// <inheritdoc/>
    public override IReadOnlyCollection<PolygonZ> GetMultiPolygonZ() => Altemiq.Buffers.Text.WktParser.TryParse(this.GetWellKnownText(), out IReadOnlyCollection<PolygonZ>? polygons, out _) ? polygons : throw new InvalidGeometryTypeException();

    /// <inheritdoc/>
    public override IReadOnlyCollection<PolygonM> GetMultiPolygonM() => Altemiq.Buffers.Text.WktParser.TryParse(this.GetWellKnownText(), out IReadOnlyCollection<PolygonM>? polygons, out _) ? polygons : throw new InvalidGeometryTypeException();

    /// <inheritdoc/>
    public override IReadOnlyCollection<PolygonZM> GetMultiPolygonZM() => Altemiq.Buffers.Text.WktParser.TryParse(this.GetWellKnownText(), out IReadOnlyCollection<PolygonZM>? polygons, out _) ? polygons : throw new InvalidGeometryTypeException();

    /// <inheritdoc/>
    public override object GetGeometry() => Altemiq.Buffers.Text.WktParser.TryParse(this.GetWellKnownText(), out object? geometry, out _) ? geometry : throw new InvalidGeometryTypeException();

    private static int GetSrid(ReadOnlySpan<char> span)
    {
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

    private ReadOnlySpan<byte> GetWellKnownText()
    {
        var span = this.Wkt.AsSpan();
        var index = span.IndexOf(';');
        if (index is -1)
        {
            return System.Runtime.InteropServices.MemoryMarshal.Cast<char, byte>(span);
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

        return System.Runtime.InteropServices.MemoryMarshal.Cast<char, byte>(span.Slice(start, end - start + 1));
    }
}