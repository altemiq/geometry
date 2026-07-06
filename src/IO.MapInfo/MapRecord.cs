// -----------------------------------------------------------------------
// <copyright file="MapRecord.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapInfo;

using Altemiq.Geometry;

/// <summary>
/// The <c>MAP</c> record.
/// </summary>
/// <param name="FeatureId">The feature ID.</param>
public record MapRecord(int FeatureId) : Data.IGeometryRecord
{
    private readonly byte[]? data;
    private readonly int offset;
    private readonly HeaderBlock headerBlock;
    private readonly ObjectBlock objectBlock;
    private readonly MapReader? mapReader;

    /// <summary>
    /// Initialises a new instance of the <see cref="MapRecord"/> class.
    /// </summary>
    /// <param name="featureId">The feature ID.</param>
    /// <param name="data">The data.</param>
    /// <param name="start">The start index.</param>
    /// <param name="tabGeomType">The <see cref="TabGeomType"/>.</param>
    /// <param name="header">The header.</param>
    /// <param name="object">The object block.</param>
    /// <param name="mapReader">The map reader.</param>
    internal MapRecord(int featureId, byte[] data, int start, TabGeomType tabGeomType, HeaderBlock header, ObjectBlock @object, MapReader mapReader)
        : this(featureId)
    {
        this.data = data;
        this.offset = start;
        this.GeometryType = tabGeomType;
        this.headerBlock = header;
        this.objectBlock = @object;
        this.mapReader = mapReader;
    }

    /// <summary>
    /// Gets the geometry type.
    /// </summary>
    internal TabGeomType GeometryType { get; }

    private ReadOnlySpan<byte> Span => this.data is null ? default : new(this.data, this.offset, this.data.Length - this.offset);

    private bool IsCompressed => (int)this.GeometryType % 3 is 1;

    /// <inheritdoc/>
    public Altemiq.Geometry.Point GetPoint() => this.ReadSymbol(this.Span, this.IsCompressed).Point;

    /// <inheritdoc/>
    public PointZ GetPointZ() => throw new NotSupportedException();

    /// <inheritdoc/>
    public PointM GetPointM() => throw new NotSupportedException();

    /// <inheritdoc/>
    public PointZM GetPointZM() => throw new NotSupportedException();

    /// <inheritdoc/>
    public IMultiGeometry<Altemiq.Geometry.Point> GetMultiPoint() => throw new NotSupportedException();

    /// <inheritdoc/>
    public IMultiGeometry<PointZ> GetMultiPointZ() => throw new NotSupportedException();

    /// <inheritdoc/>
    public IMultiGeometry<PointM> GetMultiPointM() => throw new NotSupportedException();

    /// <inheritdoc/>
    public IMultiGeometry<PointZM> GetMultiPointZM() => throw new NotSupportedException();

    /// <inheritdoc/>
    public Polyline GetLineString() => this.GeometryType switch
    {
        TabGeomType.Line or TabGeomType.LineCompressed => this.ReadLine(this.Span, this.IsCompressed),
        TabGeomType.PLine or TabGeomType.PLineCompressed => this.ReadPLine(this.Span, this.IsCompressed),
        _ => throw new NotSupportedException(),
    };

    /// <inheritdoc/>
    public PolylineZ GetLineStringZ() => throw new NotSupportedException();

    /// <inheritdoc/>
    public PolylineM GetLineStringM() => throw new NotSupportedException();

    /// <inheritdoc/>
    public PolylineZM GetLineStringZM() => throw new NotSupportedException();

    /// <inheritdoc/>
    public IMultiGeometry<Polyline> GetMultiLineString() => this.ReadPLines(this.Span, this.IsCompressed);

    /// <inheritdoc/>
    public IMultiGeometry<PolylineZ> GetMultiLineStringZ() => throw new NotSupportedException();

    /// <inheritdoc/>
    public IMultiGeometry<PolylineM> GetMultiLineStringM() => throw new NotSupportedException();

    /// <inheritdoc/>
    public IMultiGeometry<PolylineZM> GetMultiLineStringZM() => throw new NotSupportedException();

    /// <inheritdoc/>
    public Polygon GetPolygon() => this.ReadRegions(this.Span, this.IsCompressed).Single();

    /// <inheritdoc/>
    public PolygonZ GetPolygonZ() => throw new NotSupportedException();

    /// <inheritdoc/>
    public PolygonM GetPolygonM() => throw new NotSupportedException();

    /// <inheritdoc/>
    public PolygonZM GetPolygonZM() => throw new NotSupportedException();

    /// <inheritdoc/>
    public IMultiGeometry<Polygon> GetMultiPolygon() => this.ReadRegions(this.Span, this.IsCompressed);

    /// <inheritdoc/>
    public IMultiGeometry<PolygonZ> GetMultiPolygonZ() => throw new NotSupportedException();

    /// <inheritdoc/>
    public IMultiGeometry<PolygonM> GetMultiPolygonM() => throw new NotSupportedException();

    /// <inheritdoc/>
    public IMultiGeometry<PolygonZM> GetMultiPolygonZM() => throw new NotSupportedException();

    /// <inheritdoc/>
    public IGeometry GetGeometry() => this.GeometryType switch
    {
        TabGeomType.Symbol or TabGeomType.SymbolCompressed => this.ReadSymbol(this.Span, this.IsCompressed),
        TabGeomType.CustomSymbol or TabGeomType.CustomSymbolCompressed => this.ReadCustomSymbol(this.Span, this.IsCompressed),
        TabGeomType.FontSymbol or TabGeomType.FontSymbolCompressed => this.ReadFontSymbol(this.Span, this.IsCompressed),
        TabGeomType.Line or TabGeomType.LineCompressed => this.ReadLine(this.Span, this.IsCompressed),
        TabGeomType.PLine or TabGeomType.PLineCompressed => this.ReadPLines(this.Span, this.IsCompressed).Single(),
        TabGeomType.MultiPLine or TabGeomType.MultiPLineCompressed => this.ReadPLines(this.Span, this.IsCompressed),
        TabGeomType.Region or TabGeomType.RegionCompressed
            or TabGeomType.V450Region or TabGeomType.V450RegionCompressed
            or TabGeomType.V800Region or TabGeomType.V800RegionCompressed => this.ReadRegions(this.Span, this.IsCompressed).Single(),
        _ => throw new InvalidGeometryTypeException(),
    };

    /// <inheritdoc />
    public bool IsNull() => this.GeometryType is TabGeomType.Unset;

    private Altemiq.Geometry.Point ReadPoint(RawPoint point)
    {
        var coordOriginQuadrant = this.headerBlock.CoordOriginQuadrant;
        var x = coordOriginQuadrant switch
        {
            0 or 2 or 3 => -1.0 * (point.X + this.headerBlock.DisplX) / this.headerBlock.ScaleX,
            _ => (point.X - this.headerBlock.DisplX) / this.headerBlock.ScaleX,
        };

        var y = coordOriginQuadrant switch
        {
            0 or 3 or 4 => -1.0 * (point.Y + this.headerBlock.DisplY) / this.headerBlock.ScaleY,
            _ => (point.Y - this.headerBlock.DisplY) / this.headerBlock.ScaleY,
        };

        if (this.headerBlock.PrecisionX is not (> 0D and var precisionX)
            || this.headerBlock.PrecisionY is not (> 0D and var precisionY))
        {
            return new(x, y);
        }

        return new(
            Math.Round(x * precisionX, MidpointRounding.ToEven) / precisionX,
            Math.Round(y * precisionY, MidpointRounding.ToEven) / precisionY);
    }

    private (RawPoint Point, int Size) ReadPoint(ReadOnlySpan<byte> span, bool compressed) => compressed switch
    {
        true => (Point.Read(span, this.objectBlock.CenterX, this.objectBlock.CenterY), 4),
        false => (Point.Read(span), 8),
    };

    private MapSymbol ReadSymbol(ReadOnlySpan<byte> span, bool compressed)
    {
        var (point, size) = this.ReadPoint(span, compressed);
        span = span[size..];

        var symbolId = span[0];

        var (x, y) = this.ReadPoint(point);
        return new() { X = x, Y = y, SymbolId = symbolId };
    }

    private MapCustomSymbol ReadCustomSymbol(ReadOnlySpan<byte> span, bool compressed)
    {
        var customStyleId = span[1];
        span = span[2..];

        var (point, size) = this.ReadPoint(span, compressed);
        span = span[size..];

        ////var symbolId = span[0];
        var fontId = span[1];

        var (x, y) = this.ReadPoint(point);
        return new() { X = x, Y = y, CustomStyleId = customStyleId, FontId = fontId };
    }

    private MapFontSymbol ReadFontSymbol(ReadOnlySpan<byte> span, bool compressed)
    {
        ////var symbolId = span[0]; // Symbol index
        ////var pointSize = span[1];
        ////var fontStyle = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[2..]); // font style
        span = span[4..];

        var r = span[0];
        var g = span[1];
        var b = span[2];
        span = span[3..];

        // skip next three
        span = span[3..];

        ////var angle = System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(span);
        span = span[2..];

        var (point, size) = this.ReadPoint(span, compressed);
        span = span[size..];

        var fontId = span[0];

        var (x, y) = this.ReadPoint(point);
        return new() { X = x, Y = y, FontId = fontId, Color = System.Drawing.Color.FromArgb(byte.MaxValue, r, g, b) };
    }

    private MapPolyline ReadLine(ReadOnlySpan<byte> span, bool compressed)
    {
        // get the points
        var (first, second, penOffset) = compressed switch
        {
            true => (Point.Read(span, this.objectBlock.CenterX, this.objectBlock.CenterY), Point.Read(span[4..], this.objectBlock.CenterX, this.objectBlock.CenterY), 8),
            false => (Point.Read(span), Point.Read(span[8..]), 16),
        };

        var penId = span[penOffset];

        // read the 2 points
        return new([this.ReadPoint(first), this.ReadPoint(second)], penId);
    }

    private MapPolyline ReadPLine(ReadOnlySpan<byte> span, bool compressed)
    {
        if (this.mapReader is null)
        {
            throw new InvalidOperationException();
        }

        var (coordBlockAddress, coordDataSize, _) = GetCoordBlockInfo(span);
        var coordBlock = this.mapReader.GetCoordBlock(coordDataSize);

        int labelX;
        int labelY;
        int minX;
        int minY;
        int maxX;
        int maxY;
        IEnumerable<Altemiq.Geometry.Point> points;
        if (compressed)
        {
            // Compressed coordinate origin (present only in compressed case!)
            var compressedOriginX = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[12..]);
            var compressedOriginY = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[16..]);

            // Region center/label point, relative to compr. coord. origin
            // No it is not relative to the Object block center
            (labelX, labelY) = Point.Read(span[8..], compressedOriginX, compressedOriginY);

            // Read MBR
            (minX, minY) = Point.Read(span[20..], compressedOriginX, compressedOriginY);
            (maxX, maxY) = Point.Read(span[24..], compressedOriginX, compressedOriginY);

            span = span[28..];

            points = this.Convert(Point.Read(coordBlock, coordBlockAddress, 0, coordDataSize, compressedOriginX, compressedOriginY));
        }
        else
        {
            labelX = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[8..]);
            labelY = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[12..]);
            minX = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[16..]);
            minY = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[20..]);
            maxX = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[24..]);
            maxY = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[28..]);

            span = span[32..];

            points = this.Convert(Point.Read(coordBlock, coordBlockAddress, coordDataSize));
        }

        var penId = span[0];

        return new(points, penId, new(labelX, labelY), new(minX, minY, maxX, maxY));
    }

    private MapMultiPolyline ReadPLines(ReadOnlySpan<byte> span, bool compressed)
    {
        if (this.mapReader is null)
        {
            throw new InvalidOperationException();
        }

        var (coordBlockAddress, coordDataSize, _) = GetCoordBlockInfo(span);
        span = span[8..];
        var coordBlock = this.mapReader.GetCoordBlock(coordDataSize);

        // get the number of line segments
        int numberOfLineSections;
        if (this.GeometryType is TabGeomType.V800MultiPLine or TabGeomType.V800MultiPLineCompressed)
        {
            numberOfLineSections = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span);
            span = span[37..];
        }
        else
        {
            numberOfLineSections = System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(span);
            span = span[2..];
        }

        int labelX;
        int labelY;
        int minX;
        int minY;
        int maxX;
        int maxY;
        Func<CoordSectionHeader, IEnumerable<RawPoint>> pointFactory;
        CoordSectionHeader[] sections;
        if (compressed)
        {
            // Compressed coordinate origin (present only in compressed case!)
            var compressedOriginX = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[4..]);
            var compressedOriginY = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[8..]);

            // Region center/label point, relative to compr. coord. origin
            // No it is not relative to the Object block center
            (labelX, labelY) = Point.Read(span, compressedOriginX, compressedOriginY);

            // Read MBR
            (minX, minY) = Point.Read(span[12..], compressedOriginX, compressedOriginY);
            (maxX, maxY) = Point.Read(span[16..], compressedOriginX, compressedOriginY);
            span = span[20..];

            sections = [.. coordBlock.ReadCoordSectionHeaders(coordBlockAddress, numberOfLineSections, this.GetVersion(), compressedOriginX, compressedOriginY)];
            var featureOffset = sections.Length * 4;

            pointFactory = section => Point.Read(coordBlock, coordBlockAddress, (section.DataOffset / 2) + featureOffset, section.NumberOfVertices * 4, compressedOriginX, compressedOriginY);
        }
        else
        {
            labelX = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[..]);
            labelY = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[4..]);
            minX = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[8..]);
            minY = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[12..]);
            maxX = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[16..]);
            maxY = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[20..]);

            span = span[24..];

            sections = [.. coordBlock.ReadCoordSectionHeaders(coordBlockAddress, numberOfLineSections, this.GetVersion())];

            pointFactory = section => Point.Read(coordBlock, coordBlockAddress + section.DataOffset, section.NumberOfVertices * 8);
        }

        var penId = span[0];

        var polylines = new Polyline[sections.Length];
        for (int i = 0; i < sections.Length; i++)
        {
            polylines[i] = [.. this.Convert(pointFactory(sections[i]))];
        }

        return new(polylines, penId, new(labelX, labelY), new(minX, minY, maxX, maxY));
    }

    private MapMultiPolygon ReadRegions(ReadOnlySpan<byte> span, bool compressed)
    {
        if (this.mapReader is null)
        {
            throw new InvalidOperationException();
        }

        var (coordBlockAddress, _, _) = GetCoordBlockInfo(span);
        span = span[8..];
        var coordBlock = this.mapReader.GetCoordBlock(coordBlockAddress);

        int numberLineSections;
        if (this.GeometryType is TabGeomType.V800Region or TabGeomType.V800RegionCompressed)
        {
            numberLineSections = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span);
            span = span[37..];
        }
        else
        {
            numberLineSections = System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(span);
            span = span[2..];
        }

        int labelX;
        int labelY;
        int minX;
        int minY;
        int maxX;
        int maxY;
        Func<CoordSectionHeader, IEnumerable<RawPoint>> pointFactory;
        CoordSectionHeader[] sections;
        if (compressed)
        {
            // Compressed coordinate origin (present only in compressed case!)
            var compressedOriginX = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[4..]);
            var compressedOriginY = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[8..]);

            // Region center/label point, relative to compr. coord. origin
            // No it is not relative to the Object block center
            (labelX, labelY) = Point.Read(span[8..], compressedOriginX, compressedOriginY);

            // Read MBR
            (minX, minY) = Point.Read(span[20..], compressedOriginX, compressedOriginY);
            (maxX, maxY) = Point.Read(span[24..], compressedOriginX, compressedOriginY);

            ////span = span[20..];

            sections = [.. coordBlock.ReadCoordSectionHeaders(coordBlockAddress, numberLineSections, this.GetVersion(), compressedOriginX, compressedOriginY)];
            var featureOffset = sections.Length * 4;

            pointFactory = section => Point.Read(coordBlock, coordBlockAddress, (section.DataOffset / 2) + featureOffset, section.NumberOfVertices * 4, compressedOriginX, compressedOriginY);
        }
        else
        {
            labelX = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[..]);
            labelY = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[4..]);
            minX = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[8..]);
            minY = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[12..]);
            maxX = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[16..]);
            maxY = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[20..]);

            ////span = span[24..];

            sections = [.. coordBlock.ReadCoordSectionHeaders(coordBlockAddress, numberLineSections, this.GetVersion())];

            pointFactory = section => Point.Read(coordBlock, coordBlockAddress + section.DataOffset, section.NumberOfVertices * 8);
        }

        var polygons = new List<Polygon>();
        List<Altemiq.Geometry.Point>? polygon = default;

        int numberOfHolesToRead = 0;
        foreach (var section in sections)
        {
            polygon ??= [];

            if (numberOfHolesToRead < 1)
            {
                numberOfHolesToRead = section.NumberOfHoles;
            }
            else
            {
                numberOfHolesToRead--;
            }

            polygon.AddRange(this.Convert(pointFactory(section)));

            if (numberOfHolesToRead < 1)
            {
                polygons.Add([with(polygon)]);
                polygon = default;
            }
        }

        return new(polygons, 0, new(labelX, labelY), new(minX, minY, maxX, maxY));
    }

    private int GetVersion() =>
        (int)this.GeometryType switch
        {
            < (int)TabGeomType.V450Region => 300,
            < (int)TabGeomType.MultiPoint => 450,
            < (int)TabGeomType.Unknown1 => 650,
            _ => 800,
        };

    private static (int CoordBlockAddress, int CoordDataSize, bool Smooth) GetCoordBlockInfo(ReadOnlySpan<byte> span)
    {
        var coordBlockAddress = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span);
        var coordDataSize = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[4..]);
        bool smooth = default;
        if ((coordDataSize & 0x80000000) is not 0)
        {
            smooth = true;
            coordDataSize &= 0x7FFFFFFF; // Take smooth flag out of the value
        }

        return (coordBlockAddress, coordDataSize, smooth);
    }

    private IEnumerable<Altemiq.Geometry.Point> Convert(IEnumerable<RawPoint> points) => points.Select(this.ReadPoint);
}