// -----------------------------------------------------------------------
// <copyright file="EwktWriter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

/// <summary>
/// The Extended WKB writer.
/// </summary>
public sealed class EwkbWriter : WkbWriter
{
    /// <summary>
    /// Initialises a new instance of the <see cref="EwkbWriter"/> class.
    /// </summary>
    /// <param name="bytes">The byte array to write to.</param>
    public EwkbWriter(byte[] bytes)
        : base(bytes)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="EwkbWriter"/> class.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    public EwkbWriter(Stream stream)
        : base(stream)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="EwkbWriter"/> class.
    /// </summary>
    /// <param name="bytes">The byte array to write to.</param>
    /// <param name="littleEndian">Set to <see langword="true"/> to treat the data as little endian.</param>
    public EwkbWriter(byte[] bytes, bool littleEndian)
        : base(bytes, littleEndian)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="EwkbWriter"/> class.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="littleEndian">Set to <see langword="true"/> to treat the data as little endian.</param>
    public EwkbWriter(Stream stream, bool littleEndian)
        : base(stream, littleEndian)
    {
    }

    private delegate int GetMaxSize<in T>(T value);

    private delegate int WriteValue<in T>(Span<byte> destination, T value, int srid);

    /// <inheritdoc/>
    public override void Write(Point point) => this.Write(point, default);

    /// <inheritdoc/>
    public void Write(Point point, int srid) => this.Write(point, srid, Buffers.Binary.WkbPrimitives.GetMaxSize, Buffers.Binary.EwkbPrimitives.WritePointLittleEndian, Buffers.Binary.EwkbPrimitives.WritePointBigEndian);

    /// <inheritdoc/>
    public override void Write(PointZ point) => this.Write(point, default);

    /// <inheritdoc/>
    public void Write(PointZ point, int srid) => this.Write(point, srid, Buffers.Binary.WkbPrimitives.GetMaxSize, Buffers.Binary.EwkbPrimitives.WritePointZLittleEndian, Buffers.Binary.EwkbPrimitives.WritePointZBigEndian);

    /// <inheritdoc/>
    public override void Write(PointM point) => this.Write(point, default);

    /// <inheritdoc/>
    public void Write(PointM point, int srid) => this.Write(point, srid, Buffers.Binary.WkbPrimitives.GetMaxSize, Buffers.Binary.EwkbPrimitives.WritePointMLittleEndian, Buffers.Binary.EwkbPrimitives.WritePointMBigEndian);

    /// <inheritdoc/>
    public override void Write(PointZM point) => this.Write(point, default);

    /// <inheritdoc/>
    public void Write(PointZM point, int srid) => this.Write(point, srid, Buffers.Binary.WkbPrimitives.GetMaxSize, Buffers.Binary.EwkbPrimitives.WritePointZMLittleEndian, Buffers.Binary.EwkbPrimitives.WritePointZMBigEndian);

    /// <inheritdoc/>
    public override void Write(IEnumerable<Point> points) => this.Write(points, default);

    /// <inheritdoc/>
    public void Write(IEnumerable<Point> points, int srid) => this.Write(ToCollection(points), srid, Buffers.Binary.WkbPrimitives.GetMaxSize, Buffers.Binary.EwkbPrimitives.WritePointLittleEndian, Buffers.Binary.EwkbPrimitives.WritePointBigEndian);

    /// <inheritdoc/>
    public override void Write(IEnumerable<PointZ> points) => this.Write(points, default);

    /// <inheritdoc/>
    public void Write(IEnumerable<PointZ> points, int srid) => this.Write(ToCollection(points), srid, Buffers.Binary.WkbPrimitives.GetMaxSize, Buffers.Binary.EwkbPrimitives.WritePointZLittleEndian, Buffers.Binary.EwkbPrimitives.WritePointZBigEndian);

    /// <inheritdoc/>
    public override void Write(IEnumerable<PointM> points) => this.Write(points, default);

    /// <inheritdoc/>
    public void Write(IEnumerable<PointM> points, int srid) => this.Write(ToCollection(points), srid, Buffers.Binary.WkbPrimitives.GetMaxSize, Buffers.Binary.EwkbPrimitives.WritePointMLittleEndian, Buffers.Binary.EwkbPrimitives.WritePointMBigEndian);

    /// <inheritdoc/>
    public override void Write(IEnumerable<PointZM> points) => this.Write(points, default);

    /// <inheritdoc/>
    public void Write(IEnumerable<PointZM> points, int srid) => this.Write(ToCollection(points), srid, Buffers.Binary.WkbPrimitives.GetMaxSize, Buffers.Binary.EwkbPrimitives.WritePointZMLittleEndian, Buffers.Binary.EwkbPrimitives.WritePointZMBigEndian);

    /// <inheritdoc/>
    public override void Write(Polyline<Point> polyline) => this.Write(polyline, default);

    /// <inheritdoc/>
    public void Write(Polyline<Point> polyline, int srid) => this.Write(polyline, srid, Buffers.Binary.WkbPrimitives.GetMaxSize, Buffers.Binary.EwkbPrimitives.WriteLineStringLittleEndian, Buffers.Binary.EwkbPrimitives.WriteLineStringBigEndian);

    /// <inheritdoc/>
    public override void Write(Polyline<PointZ> polyline) => this.Write(polyline, default);

    /// <inheritdoc/>
    public void Write(Polyline<PointZ> polyline, int srid) => this.Write(polyline, srid, Buffers.Binary.WkbPrimitives.GetMaxSize, Buffers.Binary.EwkbPrimitives.WriteLineStringZLittleEndian, Buffers.Binary.EwkbPrimitives.WriteLineStringZBigEndian);

    /// <inheritdoc/>
    public override void Write(Polyline<PointM> polyline) => this.Write(polyline, default);

    /// <inheritdoc/>
    public void Write(Polyline<PointM> polyline, int srid) => this.Write(polyline, srid, Buffers.Binary.WkbPrimitives.GetMaxSize, Buffers.Binary.EwkbPrimitives.WriteLineStringMLittleEndian, Buffers.Binary.EwkbPrimitives.WriteLineStringMBigEndian);

    /// <inheritdoc/>
    public override void Write(Polyline<PointZM> polyline) => this.Write(polyline, default);

    /// <inheritdoc/>
    public void Write(Polyline<PointZM> polyline, int srid) => this.Write(polyline, srid, Buffers.Binary.WkbPrimitives.GetMaxSize, Buffers.Binary.EwkbPrimitives.WriteLineStringZMLittleEndian, Buffers.Binary.EwkbPrimitives.WriteLineStringZMBigEndian);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polyline<Point>> polylines) => this.Write(polylines, default);

    /// <inheritdoc/>
    public void Write(IEnumerable<Polyline<Point>> polylines, int srid) => this.Write(ToCollection(polylines), srid, Buffers.Binary.WkbPrimitives.GetMaxSize, Buffers.Binary.EwkbPrimitives.WriteLineStringLittleEndian, Buffers.Binary.EwkbPrimitives.WriteLineStringBigEndian);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polyline<PointZ>> polylines) => this.Write(polylines, default);

    /// <inheritdoc/>
    public void Write(IEnumerable<Polyline<PointZ>> polylines, int srid) => this.Write(ToCollection(polylines), srid, Buffers.Binary.WkbPrimitives.GetMaxSize, Buffers.Binary.EwkbPrimitives.WriteLineStringZLittleEndian, Buffers.Binary.EwkbPrimitives.WriteLineStringZBigEndian);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polyline<PointM>> polylines) => this.Write(polylines, default);

    /// <inheritdoc/>
    public void Write(IEnumerable<Polyline<PointM>> polylines, int srid) => this.Write(ToCollection(polylines), srid, Buffers.Binary.WkbPrimitives.GetMaxSize, Buffers.Binary.EwkbPrimitives.WriteLineStringMLittleEndian, Buffers.Binary.EwkbPrimitives.WriteLineStringMBigEndian);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polyline<PointZM>> polylines) => this.Write(polylines, default);

    /// <inheritdoc/>
    public void Write(IEnumerable<Polyline<PointZM>> polylines, int srid) => this.Write(ToCollection(polylines), srid, Buffers.Binary.WkbPrimitives.GetMaxSize, Buffers.Binary.EwkbPrimitives.WriteLineStringZMLittleEndian, Buffers.Binary.EwkbPrimitives.WriteLineStringZMBigEndian);

    /// <inheritdoc/>
    public override void Write(Polygon<Point> polygon) => this.Write(polygon, default);

    /// <inheritdoc/>
    public void Write(Polygon<Point> polygon, int srid) => this.Write(polygon, srid, Buffers.Binary.WkbPrimitives.GetMaxSize, Buffers.Binary.EwkbPrimitives.WritePolygonLittleEndian, Buffers.Binary.EwkbPrimitives.WritePolygonBigEndian);

    /// <inheritdoc/>
    public override void Write(Polygon<PointZ> polygon) => this.Write(polygon, default);

    /// <inheritdoc/>
    public void Write(Polygon<PointZ> polygon, int srid) => this.Write(polygon, srid, Buffers.Binary.WkbPrimitives.GetMaxSize, Buffers.Binary.EwkbPrimitives.WritePolygonZLittleEndian, Buffers.Binary.EwkbPrimitives.WritePolygonZBigEndian);

    /// <inheritdoc/>
    public override void Write(Polygon<PointM> polygon) => this.Write(polygon, default);

    /// <inheritdoc/>
    public void Write(Polygon<PointM> polygon, int srid) => this.Write(polygon, srid, Buffers.Binary.WkbPrimitives.GetMaxSize, Buffers.Binary.EwkbPrimitives.WritePolygonMLittleEndian, Buffers.Binary.EwkbPrimitives.WritePolygonMBigEndian);

    /// <inheritdoc/>
    public override void Write(Polygon<PointZM> polygon) => this.Write(polygon, default);

    /// <inheritdoc/>
    public void Write(Polygon<PointZM> polygon, int srid) => this.Write(polygon, srid, Buffers.Binary.WkbPrimitives.GetMaxSize, Buffers.Binary.EwkbPrimitives.WritePolygonZMLittleEndian, Buffers.Binary.EwkbPrimitives.WritePolygonZMBigEndian);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polygon<Point>> polygons) => this.Write(polygons, default);

    /// <inheritdoc/>
    public void Write(IEnumerable<Polygon<Point>> polygons, int srid) => this.Write(ToCollection(polygons), srid, Buffers.Binary.WkbPrimitives.GetMaxSize, Buffers.Binary.EwkbPrimitives.WritePolygonLittleEndian, Buffers.Binary.EwkbPrimitives.WritePolygonBigEndian);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polygon<PointZ>> polygons) => this.Write(polygons, default);

    /// <inheritdoc/>
    public void Write(IEnumerable<Polygon<PointZ>> polygons, int srid) => this.Write(ToCollection(polygons), srid, Buffers.Binary.WkbPrimitives.GetMaxSize, Buffers.Binary.EwkbPrimitives.WritePolygonZLittleEndian, Buffers.Binary.EwkbPrimitives.WritePolygonZBigEndian);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polygon<PointM>> polygons) => this.Write(polygons, default);

    /// <inheritdoc/>
    public void Write(IEnumerable<Polygon<PointM>> polygons, int srid) => this.Write(ToCollection(polygons), srid, Buffers.Binary.WkbPrimitives.GetMaxSize, Buffers.Binary.EwkbPrimitives.WritePolygonMLittleEndian, Buffers.Binary.EwkbPrimitives.WritePolygonMBigEndian);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polygon<PointZM>> polygons) => this.Write(polygons, default);

    /// <inheritdoc/>
    public void Write(IEnumerable<Polygon<PointZM>> polygons, int srid) => this.Write(ToCollection(polygons), srid, Buffers.Binary.WkbPrimitives.GetMaxSize, Buffers.Binary.EwkbPrimitives.WritePolygonZMLittleEndian, Buffers.Binary.EwkbPrimitives.WritePolygonZMBigEndian);

    private static ICollection<T> ToCollection<T>(IEnumerable<T> enumerable) => [.. enumerable];

    private void Write<T>(T value, int srid, GetMaxSize<T> getMaxSize, WriteValue<T> writeLittleEndian, WriteValue<T> writeBigEndian)
    {
        var size = getMaxSize(value) + sizeof(int);
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        Span<byte> span = stackalloc byte[size];
#else
        var a = new byte[size];
        var span = a.AsSpan();
#endif

        var written = this.IsLittleEndian
            ? writeLittleEndian(span, value, srid)
            : writeBigEndian(span, value, srid);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        this.BaseStream.Write(span[..written]);
#else
        this.BaseStream.Write(a, 0, written);
#endif
    }
}