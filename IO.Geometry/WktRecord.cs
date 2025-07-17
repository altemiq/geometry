// -----------------------------------------------------------------------
// <copyright file="WktRecord.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

/// <summary>
/// Represents an implementation of <see cref="Data.IGeometryRecord"/> that reads Well-Known Text.
/// </summary>
/// <remarks>
/// Initialises a new instance of the <see cref="WktRecord"/> class.
/// </remarks>
/// <param name="wkt">The well-known text.</param>
public class WktRecord(string wkt) : Data.IGeometryRecord
{
    /// <summary>
    /// Gets the Well-Known Text.
    /// </summary>
    protected string Wkt { get; } = wkt;

    /// <inheritdoc/>
    public virtual Point GetPoint() => GetPoint(this.Wkt.AsMemory());

    /// <inheritdoc/>
    public virtual PointZ GetPointZ() => GetPointZ(this.Wkt.AsMemory());

    /// <inheritdoc/>
    public virtual PointM GetPointM() => GetPointM(this.Wkt.AsMemory());

    /// <inheritdoc/>
    public virtual PointZM GetPointZM() => GetPointZM(this.Wkt.AsMemory());

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<Point> GetMultiPoint() => GetMultiPoint(this.Wkt.AsMemory());

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<PointZ> GetMultiPointZ() => GetMultiPointZ(this.Wkt.AsMemory());

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<PointM> GetMultiPointM() => GetMultiPointM(this.Wkt.AsMemory());

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<PointZM> GetMultiPointZM() => GetMultiPointZM(this.Wkt.AsMemory());

    /// <inheritdoc/>
    public virtual Polyline GetLineString() => GetLineString(this.Wkt.AsMemory());

    /// <inheritdoc/>
    public virtual PolylineZ GetLineStringZ() => GetLineStringZ(this.Wkt.AsMemory());

    /// <inheritdoc/>
    public virtual PolylineM GetLineStringM() => GetLineStringM(this.Wkt.AsMemory());

    /// <inheritdoc/>
    public virtual PolylineZM GetLineStringZM() => GetLineStringZM(this.Wkt.AsMemory());

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<Polyline> GetMultiLineString() => [.. GetMultiLineString(this.Wkt.AsMemory())];

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<PolylineZ> GetMultiLineStringZ() => [.. GetMultiLineStringZ(this.Wkt.AsMemory())];

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<PolylineM> GetMultiLineStringM() => [.. GetMultiLineStringM(this.Wkt.AsMemory())];

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<PolylineZM> GetMultiLineStringZM() => [.. GetMultiLineStringZM(this.Wkt.AsMemory())];

    /// <inheritdoc/>
    public virtual Polygon GetPolygon() => GetPolygon(this.Wkt.AsMemory());

    /// <inheritdoc/>
    public virtual PolygonZ GetPolygonZ() => GetPolygonZ(this.Wkt.AsMemory());

    /// <inheritdoc/>
    public virtual PolygonM GetPolygonM() => GetPolygonM(this.Wkt.AsMemory());

    /// <inheritdoc/>
    public virtual PolygonZM GetPolygonZM() => GetPolygonZM(this.Wkt.AsMemory());

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<Polygon> GetMultiPolygon() => [..GetMultiPolygon(this.Wkt.AsMemory())];

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<PolygonZ> GetMultiPolygonZ() => [..GetMultiPolygonZ(this.Wkt.AsMemory())];

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<PolygonM> GetMultiPolygonM() => [..GetMultiPolygonM(this.Wkt.AsMemory())];

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<PolygonZM> GetMultiPolygonZM() => [..GetMultiPolygonZM(this.Wkt.AsMemory())];

    /// <inheritdoc/>
    public virtual object GetGeometry() => GetGeometry(this.Wkt.AsMemory());

    /// <inheritdoc/>
    public bool IsNull() => string.IsNullOrEmpty(this.Wkt);

    /// <inheritdoc cref="GetGeometry()"/>
    protected static object GetGeometry(ReadOnlyMemory<char> wkt) => GetTypeAndDimensions(wkt, out var parser) switch
    {
        (GeometryTypes.Point, false, false, 2) => GetPoint(2, parser, GetPoint),
        (GeometryTypes.Point, true, false, 3) => GetPoint(3, parser, GetPointZ),
        (GeometryTypes.Point, false, true, 3) => GetPoint(3, parser, GetPointM),
        (GeometryTypes.Point, true, true, 4) => GetPoint(4, parser, GetPointZM),
        (GeometryTypes.LineString, false, false, 2) => GetLineString(2, parser, GetPoint, GetLineString),
        (GeometryTypes.LineString, true, false, 3) => GetLineString(3, parser, GetPointZ, GetLineStringZ),
        (GeometryTypes.LineString, false, true, 3) => GetLineString(3, parser, GetPointM, GetLineStringM),
        (GeometryTypes.LineString, true, true, 4) => GetLineString(4, parser, GetPointZM, GetLineStringZM),
        (GeometryTypes.Polygon, false, false, 2) => GetPolygon(2, parser, GetPoint, GetPolygon),
        (GeometryTypes.Polygon, true, false, 3) => GetPolygon(3, parser, GetPointZ, GetPolygonZ),
        (GeometryTypes.Polygon, false, true, 3) => GetPolygon(3, parser, GetPointM, GetPolygonM),
        (GeometryTypes.Polygon, true, true, 4) => GetPolygon(4, parser, GetPointZM, GetPolygonZM),
        (GeometryTypes.MultiPoint, false, false, 2) => GetMultiPoint(2, parser, GetPoint),
        (GeometryTypes.MultiPoint, true, false, 3) => GetMultiPoint(3, parser, GetPointZ),
        (GeometryTypes.MultiPoint, false, true, 3) => GetMultiPoint(3, parser, GetPointM),
        (GeometryTypes.MultiPoint, true, true, 4) => GetMultiPoint(4, parser, GetPointZM),
        (GeometryTypes.MultiLineString, false, false, 2) => GetMultiLineString(2, parser, GetPoint, GetLineString),
        (GeometryTypes.MultiLineString, true, false, 3) => GetMultiLineString(3, parser, GetPointZ, GetLineStringZ),
        (GeometryTypes.MultiLineString, false, true, 3) => GetMultiLineString(3, parser, GetPointM, GetLineStringM),
        (GeometryTypes.MultiLineString, true, true, 4) => GetMultiLineString(4, parser, GetPointZM, GetLineStringZM),
        (GeometryTypes.MultiPolygon, false, false, 2) => GetMultiPolygon(2, parser, GetPoint, GetPolygon),
        (GeometryTypes.MultiPolygon, true, false, 3) => GetMultiPolygon(3, parser, GetPointZ, GetPolygonZ),
        (GeometryTypes.MultiPolygon, false, true, 3) => GetMultiPolygon(3, parser, GetPointM, GetPolygonM),
        (GeometryTypes.MultiPolygon, true, true, 4) => GetMultiPolygon(4, parser, GetPointZM, GetPolygonZM),
        _ => throw new InvalidGeometryTypeException(),
    };

    /// <inheritdoc cref="GetLineString()"/>
    protected static Polyline GetLineString(ReadOnlyMemory<char> wkt) => GetTypeAndDimensions(wkt, out var parser) switch
    {
        (GeometryTypes.LineString, false, false, 2) => [.. GetLineString(2, parser, GetPoint, GetLineString)],
        _ => throw new InvalidGeometryTypeException(),
    };

    /// <inheritdoc cref="GetLineStringZ()"/>
    protected static PolylineZ GetLineStringZ(ReadOnlyMemory<char> wkt) => GetTypeAndDimensions(wkt, out var parser) switch
    {
        (GeometryTypes.LineString, true, false, 3) => [.. GetLineString(3, parser, GetPointZ, GetLineStringZ)],
        _ => throw new InvalidGeometryTypeException(),
    };

    /// <inheritdoc cref="GetLineStringM()"/>
    protected static PolylineM GetLineStringM(ReadOnlyMemory<char> wkt) => GetTypeAndDimensions(wkt, out var parser) switch
    {
        (GeometryTypes.LineString, false, true, 3) => [.. GetLineString(3, parser, GetPointM, GetLineStringM)],
        _ => throw new InvalidGeometryTypeException(),
    };

    /// <inheritdoc cref="GetLineStringZM()"/>
    protected static PolylineZM GetLineStringZM(ReadOnlyMemory<char> wkt) => GetTypeAndDimensions(wkt, out var parser) switch
    {
        (GeometryTypes.LineString, true, true, 4) => [.. GetLineString(4, parser, GetPointZM, GetLineStringZM)],
        _ => throw new InvalidGeometryTypeException(),
    };

    /// <inheritdoc cref="GetMultiLineString()"/>
    protected static IEnumerable<Polyline> GetMultiLineString(ReadOnlyMemory<char> wkt) => GetTypeAndDimensions(wkt, out var parser) switch
    {
        (GeometryTypes.MultiLineString, false, false, 2) => [.. GetMultiLineString(2, parser, GetPoint, GetLineString)],
        _ => throw new InvalidGeometryTypeException(),
    };

    /// <inheritdoc cref="GetMultiLineStringZ()"/>
    protected static IEnumerable<PolylineZ> GetMultiLineStringZ(ReadOnlyMemory<char> wkt) => GetTypeAndDimensions(wkt, out var parser) switch
    {
        (GeometryTypes.MultiLineString, true, false, 3) => [.. GetMultiLineString(3, parser, GetPointZ, GetLineStringZ)],
        _ => throw new InvalidGeometryTypeException(),
    };

    /// <inheritdoc cref="GetMultiLineStringM()"/>
    protected static IEnumerable<PolylineM> GetMultiLineStringM(ReadOnlyMemory<char> wkt) => GetTypeAndDimensions(wkt, out var parser) switch
    {
        (GeometryTypes.MultiLineString, false, true, 3) => [.. GetMultiLineString(3, parser, GetPointM, GetLineStringM)],
        _ => throw new InvalidGeometryTypeException(),
    };

    /// <inheritdoc cref="GetMultiLineStringZM()"/>
    protected static IEnumerable<PolylineZM> GetMultiLineStringZM(ReadOnlyMemory<char> wkt) => GetTypeAndDimensions(wkt, out var parser) switch
    {
        (GeometryTypes.MultiLineString, true, true, 4) => [.. GetMultiLineString(4, parser, GetPointZM, GetLineStringZM)],
        _ => throw new InvalidGeometryTypeException(),
    };

    /// <inheritdoc cref="GetMultiPolygon()"/>
    protected static IEnumerable<Polygon> GetMultiPolygon(ReadOnlyMemory<char> wkt) => GetTypeAndDimensions(wkt, out var parser) switch
    {
        (GeometryTypes.MultiPolygon, false, false, 2) => GetMultiPolygon(2, parser, GetPoint, GetPolygon),
        _ => throw new InvalidGeometryTypeException(),
    };

    /// <inheritdoc cref="GetMultiPolygon()"/>
    protected static IEnumerable<PolygonZ> GetMultiPolygonZ(ReadOnlyMemory<char> wkt) => GetTypeAndDimensions(wkt, out var parser) switch
    {
        (GeometryTypes.MultiPolygon, true, false, 3) => GetMultiPolygon(3, parser, GetPointZ, GetPolygonZ),
        _ => throw new InvalidGeometryTypeException(),
    };

    /// <inheritdoc cref="GetMultiPolygon()"/>
    protected static IEnumerable<PolygonM> GetMultiPolygonM(ReadOnlyMemory<char> wkt) => GetTypeAndDimensions(wkt, out var parser) switch
    {
        (GeometryTypes.MultiPolygon, false, true, 3) => GetMultiPolygon(3, parser, GetPointM, GetPolygonM),
        _ => throw new InvalidGeometryTypeException(),
    };

    /// <inheritdoc cref="GetMultiPolygon()"/>
    protected static IEnumerable<PolygonZM> GetMultiPolygonZM(ReadOnlyMemory<char> wkt) => GetTypeAndDimensions(wkt, out var parser) switch
    {
        (GeometryTypes.MultiPolygon, true, true, 4) => GetMultiPolygon(4, parser, GetPointZM, GetPolygonZM),
        _ => throw new InvalidGeometryTypeException(),
    };

    /// <inheritdoc cref="GetPoint()"/>
    protected static Point GetPoint(ReadOnlyMemory<char> wkt) => GetTypeAndDimensions(wkt, out var parser) switch
    {
        (GeometryTypes.Point, false, false, 2) => GetPoint(2, parser, GetPoint),
        _ => throw new InvalidGeometryTypeException(),
    };

    /// <inheritdoc cref="GetPointZ()"/>
    protected static PointZ GetPointZ(ReadOnlyMemory<char> wkt) => GetTypeAndDimensions(wkt, out var parser) switch
    {
        (GeometryTypes.Point, true, false, 3) => GetPoint(3, parser, GetPointZ),
        _ => throw new InvalidGeometryTypeException(),
    };

    /// <inheritdoc cref="GetPointM()"/>
    protected static PointM GetPointM(ReadOnlyMemory<char> wkt) => GetTypeAndDimensions(wkt, out var parser) switch
    {
        (GeometryTypes.Point, false, true, 3) => GetPoint(3, parser, GetPointM),
        _ => throw new InvalidGeometryTypeException(),
    };

    /// <inheritdoc cref="GetPointZM()"/>
    protected static PointZM GetPointZM(ReadOnlyMemory<char> wkt) => GetTypeAndDimensions(wkt, out var parser) switch
    {
        (GeometryTypes.Point, true, true, 4) => GetPoint(4, parser, GetPointZM),
        _ => throw new InvalidGeometryTypeException(),
    };

    /// <inheritdoc cref="GetMultiPoint()"/>
    protected static IReadOnlyCollection<Point> GetMultiPoint(ReadOnlyMemory<char> wkt) => GetTypeAndDimensions(wkt, out var parser) switch
    {
        (GeometryTypes.MultiPoint, false, false, 2) => [.. GetMultiPoint(2, parser, GetPoint)],
        _ => throw new InvalidGeometryTypeException(),
    };

    /// <inheritdoc cref="GetMultiPointZ()"/>
    protected static IReadOnlyCollection<PointZ> GetMultiPointZ(ReadOnlyMemory<char> wkt) => GetTypeAndDimensions(wkt, out var parser) switch
    {
        (GeometryTypes.MultiPoint, true, false, 3) => [.. GetMultiPoint(3, parser, GetPointZ)],
        _ => throw new InvalidGeometryTypeException(),
    };

    /// <inheritdoc cref="GetMultiPointZ()"/>
    protected static IReadOnlyCollection<PointM> GetMultiPointM(ReadOnlyMemory<char> wkt) => GetTypeAndDimensions(wkt, out var parser) switch
    {
        (GeometryTypes.MultiPoint, false, true, 3) => [.. GetMultiPoint(3, parser, GetPointM)],
        _ => throw new InvalidGeometryTypeException(),
    };

    /// <inheritdoc cref="GetMultiPointZM()"/>
    protected static IReadOnlyCollection<PointZM> GetMultiPointZM(ReadOnlyMemory<char> wkt) => GetTypeAndDimensions(wkt, out var parser) switch
    {
        (GeometryTypes.MultiPoint, true, true, 4) => [.. GetMultiPoint(4, parser, GetPointZM)],
        _ => throw new InvalidGeometryTypeException(),
    };

    /// <inheritdoc cref="GetPolygon()"/>
    protected static Polygon GetPolygon(ReadOnlyMemory<char> wkt) => GetTypeAndDimensions(wkt, out var parser) switch
    {
        (GeometryTypes.Polygon, false, false, 2) => GetPolygon(2, parser, GetPoint, GetPolygon),
        _ => throw new InvalidGeometryTypeException(),
    };

    /// <inheritdoc cref="GetPolygonZ()"/>
    protected static PolygonZ GetPolygonZ(ReadOnlyMemory<char> wkt) => GetTypeAndDimensions(wkt, out var parser) switch
    {
        (GeometryTypes.Polygon, true, false, 3) => GetPolygon(3, parser, GetPointZ, GetPolygonZ),
        _ => throw new InvalidGeometryTypeException(),
    };

    /// <inheritdoc cref="GetPolygonM()"/>
    protected static PolygonM GetPolygonM(ReadOnlyMemory<char> wkt) => GetTypeAndDimensions(wkt, out var parser) switch
    {
        (GeometryTypes.Polygon, false, true, 3) => GetPolygon(3, parser, GetPointM, GetPolygonM),
        _ => throw new InvalidGeometryTypeException(),
    };

    /// <inheritdoc cref="GetPolygonZM()"/>
    protected static PolygonZM GetPolygonZM(ReadOnlyMemory<char> wkt) => GetTypeAndDimensions(wkt, out var parser) switch
    {
        (GeometryTypes.Polygon, true, true, 4) => GetPolygon(4, parser, GetPointZM, GetPolygonZM),
        _ => throw new InvalidGeometryTypeException(),
    };

    private static (string Type, bool HasZ, bool HasM, int Dimensions) GetTypeAndDimensions(ReadOnlyMemory<char> wkt, out WktParser reader)
    {
        reader = WktParser.Create(wkt);
        if (!reader.Read() || reader.TokenType is not WktParser.WktTokenType.Type)
        {
            return default;
        }

        var type = reader.GetValue();
        if (!reader.Read() || reader.TokenType is not WktParser.WktTokenType.Dimensions)
        {
            return (type.ToString(), false, false, 2);
        }

        var (hasZ, hasM, dimensions) = reader.GetValue() switch
        {
            "ZM" or "zM" or "Zm" or "zm" => (true, true, 4),
            "Z" or "z" => (true, false, 3),
            "M" or "m" => (false, true, 3),
            _ => (false, false, 2),
        };

        _ = reader.Read();
        return (type.ToString(), hasZ, hasM, dimensions);
    }

    private static T GetPoint<T>(int values, WktParser parser, Func<double[], T> createPoint)
        where T : struct => parser.TokenType switch
    {
        WktParser.WktTokenType.Empty => default,
        _ => GetCoordinates(values, parser, createPoint).Single(),
    };

    private static Point GetPoint(double[] coordinates) => new(coordinates);

    private static PointZ GetPointZ(double[] coordinates) => new(coordinates);

    private static PointM GetPointM(double[] coordinates) => new(coordinates);

    private static PointZM GetPointZM(double[] coordinates) => new(coordinates);

    private static IEnumerable<T> GetMultiPoint<T>(int values, WktParser parser, Func<double[], T> createPoint)
    {
        if (parser is { TokenType: WktParser.WktTokenType.Empty })
        {
            // this is empty
            yield break;
        }

        if (!parser.Read())
        {
            yield break;
        }

        switch (parser)
        {
            case { TokenType: WktParser.WktTokenType.OpenParenthesis }:
                foreach (var points in GetMultiCoordinatesWithoutRead(values, parser, createPoint))
                {
                    yield return points.Single();
                }

                break;

            case { TokenType: WktParser.WktTokenType.Number }:
                foreach (var point in GetCoordinatesWithoutRead(values, parser, createPoint))
                {
                    yield return point;
                }

                break;
        }
    }

    private static TPolyLine GetLineString<TPolyLine, TPoint>(int values, WktParser parser, Func<double[], TPoint> createPoint, Func<IEnumerable<TPoint>, TPolyLine> createPolyline)
        where TPolyLine : Polyline<TPoint>, new()
        where TPoint : struct => parser is { TokenType: WktParser.WktTokenType.Empty } ? [] : createPolyline(GetCoordinates(values, parser, createPoint));

    private static Polyline GetLineString(IEnumerable<Point> points) => [.. points];

    private static PolylineZ GetLineStringZ(IEnumerable<PointZ> points) => [.. points];

    private static PolylineM GetLineStringM(IEnumerable<PointM> points) => [.. points];

    private static PolylineZM GetLineStringZM(IEnumerable<PointZM> points) => [.. points];

    private static IEnumerable<TPolyLine> GetMultiLineString<TPolyLine, TPoint>(int values, WktParser parser, Func<double[], TPoint> createPoint, Func<IEnumerable<TPoint>, TPolyLine> createPolyline)
        where TPolyLine : Polyline<TPoint>, new()
        where TPoint : struct
    {
        if (parser is { TokenType: WktParser.WktTokenType.Empty })
        {
            // this is empty
            yield break;
        }

        foreach (var points in GetMultiCoordinates(values, parser, createPoint))
        {
            yield return createPolyline(points);
        }
    }

    private static TPolygon GetPolygon<TPolygon, TPoint>(int values, WktParser parser, Func<double[], TPoint> createPoint, Func<IEnumerable<IEnumerable<TPoint>>, TPolygon> createPolygon)
        where TPolygon : Polygon<TPoint>, new()
        where TPoint : struct => parser.TokenType switch
    {
        WktParser.WktTokenType.Empty => [],
        _ => createPolygon(GetMultiCoordinates(values, parser, createPoint)),
    };

    private static Polygon GetPolygon(IEnumerable<IEnumerable<Point>> points) => [.. points.Select(static p => new LinearRing<Point>(p))];

    private static PolygonZ GetPolygonZ(IEnumerable<IEnumerable<PointZ>> points) => [.. points.Select(static p => new LinearRing<PointZ>(p))];

    private static PolygonM GetPolygonM(IEnumerable<IEnumerable<PointM>> points) => [.. points.Select(static p => new LinearRing<PointM>(p))];

    private static PolygonZM GetPolygonZM(IEnumerable<IEnumerable<PointZM>> points) => [.. points.Select(static p => new LinearRing<PointZM>(p))];

    private static IEnumerable<TPolygon> GetMultiPolygon<TPolygon, TPoint>(int values, WktParser parser, Func<double[], TPoint> createPoint, Func<IEnumerable<IEnumerable<TPoint>>, TPolygon> createPolygon)
        where TPolygon : Polygon<TPoint>, new()
        where TPoint : struct
    {
        if (parser is { TokenType: WktParser.WktTokenType.Empty })
        {
            // this is empty
            yield break;
        }

        while (parser.Read())
        {
            if (parser is { TokenType: WktParser.WktTokenType.CloseParenthesis })
            {
                yield break;
            }

            if (parser is { TokenType: WktParser.WktTokenType.OpenParenthesis })
            {
                yield return GetPolygon(values, parser, createPoint, createPolygon);
            }
        }
    }

    private static IEnumerable<T> GetCoordinates<T>(int values, WktParser parser, Func<double[], T> createPoint) => parser.Read() ? GetCoordinatesWithoutRead(values, parser, createPoint) : [];

    private static IEnumerable<T> GetCoordinatesWithoutRead<T>(int values, WktParser parser, Func<double[], T> createPoint)
    {
        var coordinates = new double[values];
        var current = 0;
        do
        {
            if (parser is { TokenType: WktParser.WktTokenType.Comma })
            {
                yield return createPoint(coordinates);
                coordinates = new double[values];
                current = 0;
            }
            else if (parser is { TokenType: WktParser.WktTokenType.CloseParenthesis })
            {
                yield return createPoint(coordinates);
                yield break;
            }

            if (parser is { TokenType: WktParser.WktTokenType.Number })
            {
                coordinates[current] =
#if NETSTANDARD2_1
                    double.Parse(parser.GetValue(), provider: System.Globalization.CultureInfo.InvariantCulture);
#else
                    double.Parse(parser.GetValue().ToString(), System.Globalization.CultureInfo.InvariantCulture);
#endif
                current++;
            }
        }
        while (parser.Read());
    }

    private static IEnumerable<IEnumerable<T>> GetMultiCoordinates<T>(int values, WktParser parser, Func<double[], T> createPoint) => parser.Read() ? GetMultiCoordinatesWithoutRead(values, parser, createPoint) : [];

    private static IEnumerable<IEnumerable<T>> GetMultiCoordinatesWithoutRead<T>(int values, WktParser parser, Func<double[], T> createPoint)
    {
        do
        {
            switch (parser.TokenType)
            {
                // we've got to the end
                case WktParser.WktTokenType.CloseParenthesis:
                    yield break;
                case WktParser.WktTokenType.OpenParenthesis:
                    yield return GetCoordinates(values, parser, createPoint);
                    break;
            }
        }
        while (parser.Read());
    }

    private static class GeometryTypes
    {
        public const string Point = "POINT";

        public const string LineString = "LINESTRING";

        public const string Polygon = "POLYGON";

        public const string MultiPoint = "MULTIPOINT";

        public const string MultiLineString = "MULTILINESTRING";

        public const string MultiPolygon = "MULTIPOLYGON";
    }

    private static class DimensionTypes
    {
        public const char Z = 'Z';

        public const char M = 'M';
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    private sealed class WktParser
    {
        private readonly ReadOnlyMemory<char> wkt;

        private int position;

        private WktParser(ReadOnlyMemory<char> wkt) => this.wkt = wkt;

        public enum WktTokenType
        {
            Start,
            Type,
            Dimensions,
            Empty,
            Comma,
            OpenParenthesis,
            CloseParenthesis,
            WhiteSpace,
            Number,
            EOF,
        }

        public WktTokenType TokenType { get; private set; }

        public static WktParser Create(ReadOnlyMemory<char> wkt) => new(wkt);

        public bool Read()
        {
            if (this.TokenType is WktTokenType.Start)
            {
                this.position = 0;
                this.TokenType = WktTokenType.Type;
                return true;
            }

            if (this.TokenType is WktTokenType.Type)
            {
                // read the dimensions
                this.position = this.GetDimensionIndex();
                this.TokenType = this.IsToken()
                    ? this.GetTokenType()
                    : WktTokenType.Dimensions;

                return true;
            }

            this.TokenType = this.MoveToNextToken();
            return this.TokenType is not WktTokenType.EOF;
        }

        public ReadOnlySpan<char> GetValue()
        {
            if (this.TokenType is WktTokenType.Type)
            {
                // read the type
                return this.ReadType();
            }

            var endIndex = this.GetNextToken();
            return this.wkt[this.position..endIndex].Span.Trim();
        }

        private static bool IsToken(char value) => value is ',' or '(' or ')' or 'E';

        private static bool IsTokenOrWhiteSpace(char value) => IsToken(value) || char.IsWhiteSpace(value);

        private static bool IsTokenOrDigit(char value) => IsToken(value) || IsDigit(value);

        private static bool IsDigit(char value) => char.IsDigit(value) || value is '-' || value is '+';

        private ReadOnlySpan<char> ReadType()
        {
            var endIndex = this.GetDimensionIndex();
            return this.wkt[this.position..endIndex].Span;
        }

        private int GetDimensionIndex()
        {
            var span = this.wkt.Span;
            return FindEnd(span, GetIndex(span, this.position)) + 1;

            static int GetIndex(ReadOnlySpan<char> wkt, int index)
            {
                for (var i = index; i < wkt.Length; i++)
                {
                    if (wkt[i] is ' ' or '(')
                    {
                        return i - 1;
                    }
                }

                return -1;
            }

            static int FindEnd(ReadOnlySpan<char> wkt, int index)
            {
                if (IsNotDimension(ref wkt, index))
                {
                    return index;
                }

                for (var i = index - 1; i >= 0; i--)
                {
                    if (IsNotDimension(ref wkt, i))
                    {
                        return i;
                    }
                }

                return index;

                static bool IsNotDimension(ref ReadOnlySpan<char> wkt, int k)
                {
                    return wkt[k] is not DimensionTypes.Z and not DimensionTypes.M;
                }
            }
        }

        private WktTokenType MoveToNextToken()
        {
            this.position = this.GetNextToken();
            return this.position == this.wkt.Length ? WktTokenType.EOF : this.GetTokenType();
        }

        private int GetNextToken()
        {
            return GetNextTokenCore(
                this.wkt.Span,
                this.position,
                this.TokenType is WktTokenType.Number ? IsTokenOrWhiteSpace : IsTokenOrDigit);

            static int GetNextTokenCore(ReadOnlySpan<char> wkt, int position, Func<char, bool> isToken)
            {
                for (var i = position + 1; i < wkt.Length; i++)
                {
                    if (isToken(wkt[i]))
                    {
                        return i;
                    }
                }

                return wkt.Length;
            }
        }

        private bool IsToken() => IsToken(this.wkt.Span[this.position]);

        private WktTokenType GetTokenType() => this.wkt.Span[this.position] switch
        {
            ',' => WktTokenType.Comma,
            '(' => WktTokenType.OpenParenthesis,
            ')' => WktTokenType.CloseParenthesis,
            'E' => WktTokenType.Empty,
            var x when char.IsWhiteSpace(x) => WktTokenType.WhiteSpace,
            var x when IsDigit(x) => WktTokenType.Number,
            _ => throw new InvalidOperationException("Not a valid token"),
        };
    }
}