// -----------------------------------------------------------------------
// <copyright file="WkbWriter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

using static Altemiq.Buffers.Binary.WkbPrimitives;

/// <summary>
/// Represents an implementation of <see cref="Data.IGeometryWriter"/> that writes Well-Known Binary.
/// </summary>
public class WkbWriter : Data.Common.BinaryGeometryWriter
{
    /// <summary>
    /// Initialises a new instance of the <see cref="WkbWriter"/> class.
    /// </summary>
    /// <param name="bytes">The byte array to write to.</param>
    public WkbWriter(byte[] bytes)
        : base(bytes)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="WkbWriter"/> class.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    public WkbWriter(Stream stream)
        : base(stream)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="WkbWriter"/> class.
    /// </summary>
    /// <param name="bytes">The byte array to write to.</param>
    /// <param name="littleEndian">Set to <see langword="true"/> to treat the data as little endian.</param>
    public WkbWriter(byte[] bytes, bool littleEndian)
        : base(bytes, littleEndian)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="WkbWriter"/> class.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="littleEndian">Set to <see langword="true"/> to treat the data as little endian.</param>
    public WkbWriter(Stream stream, bool littleEndian)
        : base(stream, littleEndian)
    {
    }

    private delegate int GetMaxSize<in T>(T value);

    private delegate int WriteValue<in T>(Span<byte> destination, T value);

    /// <inheritdoc/>
    public override void Write(Point point) => this.Write(point, GetMaxSize, WritePointBigEndian, WritePointLittleEndian);

    /// <inheritdoc/>
    public override void Write(PointZ point) => this.Write(point, GetMaxSize, WritePointZBigEndian, WritePointZLittleEndian);

    /// <inheritdoc/>
    public override void Write(PointM point) => this.Write(point, GetMaxSize, WritePointMBigEndian, WritePointMLittleEndian);

    /// <inheritdoc/>
    public override void Write(PointZM point) => this.Write(point, GetMaxSize, WritePointZMBigEndian, WritePointZMLittleEndian);

    /// <inheritdoc/>
    public override void Write(IEnumerable<Point> points) => this.Write(ToCollection(points), GetMaxSize, WritePointBigEndian, WritePointLittleEndian);

    /// <inheritdoc/>
    public override void Write(IEnumerable<PointZ> points) => this.Write(ToCollection(points), GetMaxSize, WritePointZBigEndian, WritePointZLittleEndian);

    /// <inheritdoc/>
    public override void Write(IEnumerable<PointM> points) => this.Write(ToCollection(points), GetMaxSize, WritePointMBigEndian, WritePointMLittleEndian);

    /// <inheritdoc/>
    public override void Write(IEnumerable<PointZM> points) => this.Write(ToCollection(points), GetMaxSize, WritePointZMBigEndian, WritePointZMLittleEndian);

    /// <inheritdoc/>
    public override void Write(Polyline<Point> polyline) => this.Write(polyline, GetMaxSize, WriteLineStringBigEndian, WriteLineStringLittleEndian);

    /// <inheritdoc/>
    public override void Write(Polyline<PointZ> polyline) => this.Write(polyline, GetMaxSize, WriteLineStringZBigEndian, WriteLineStringZLittleEndian);

    /// <inheritdoc/>
    public override void Write(Polyline<PointM> polyline) => this.Write(polyline, GetMaxSize, WriteLineStringMBigEndian, WriteLineStringMLittleEndian);

    /// <inheritdoc/>
    public override void Write(Polyline<PointZM> polyline) => this.Write(polyline, GetMaxSize, WriteLineStringZMBigEndian, WriteLineStringZMLittleEndian);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polyline<Point>> polylines) => this.Write(ToCollection(polylines), GetMaxSize, WriteLineStringBigEndian, WriteLineStringLittleEndian);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polyline<PointZ>> polylines) => this.Write(ToCollection(polylines), GetMaxSize, WriteLineStringZBigEndian, WriteLineStringZLittleEndian);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polyline<PointM>> polylines) => this.Write(ToCollection(polylines), GetMaxSize, WriteLineStringMBigEndian, WriteLineStringMLittleEndian);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polyline<PointZM>> polylines) => this.Write(ToCollection(polylines), GetMaxSize, WriteLineStringZMBigEndian, WriteLineStringZMLittleEndian);

    /// <inheritdoc/>
    public override void Write(Polygon<Point> polygon) => this.Write(polygon, GetMaxSize, WritePolygonBigEndian, WritePolygonLittleEndian);

    /// <inheritdoc/>
    public override void Write(Polygon<PointZ> polygon) => this.Write(polygon, GetMaxSize, WritePolygonZBigEndian, WritePolygonZLittleEndian);

    /// <inheritdoc/>
    public override void Write(Polygon<PointM> polygon) => this.Write(polygon, GetMaxSize, WritePolygonMBigEndian, WritePolygonMLittleEndian);

    /// <inheritdoc/>
    public override void Write(Polygon<PointZM> polygon) => this.Write(polygon, GetMaxSize, WritePolygonZMBigEndian, WritePolygonZMLittleEndian);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polygon<Point>> polygons) => this.Write(ToCollection(polygons), GetMaxSize, WritePolygonBigEndian, WritePolygonLittleEndian);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polygon<PointZ>> polygons) => this.Write(ToCollection(polygons), GetMaxSize, WritePolygonZBigEndian, WritePolygonZLittleEndian);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polygon<PointM>> polygons) => this.Write(ToCollection(polygons), GetMaxSize, WritePolygonMBigEndian, WritePolygonMLittleEndian);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polygon<PointZM>> polygons) => this.Write(ToCollection(polygons), GetMaxSize, WritePolygonZMBigEndian, WritePolygonZMLittleEndian);

    private static ICollection<T> ToCollection<T>(IEnumerable<T> enumerable) => [.. enumerable];

    private void Write<T>(T value, GetMaxSize<T> getMaxSize, WriteValue<T> writeBigEndian, WriteValue<T> writeLittleEndian)
    {
        var size = getMaxSize(value);
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        Span<byte> span = stackalloc byte[size];
#else
        var a = new byte[size];
        var span = a.AsSpan();
#endif

        var written = this.IsLittleEndian
            ? writeLittleEndian(span, value)
            : writeBigEndian(span, value);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        this.BaseStream.Write(span[..written]);
#else
        this.BaseStream.Write(a, 0, written);
#endif
    }
}