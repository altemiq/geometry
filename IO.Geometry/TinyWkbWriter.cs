// -----------------------------------------------------------------------
// <copyright file="TinyWkbWriter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

using Altemiq.Buffers.Binary;

/// <summary>
/// Represents an implementation of <see cref="Data.IGeometryWriter"/> that writes Tiny Well-Known Binary.
/// </summary>
/// <remarks>
/// Initialises a new instance of the <see cref="TinyWkbWriter"/> class based on the specified writer.
/// </remarks>
/// <param name="writer">The output writer.</param>
public class TinyWkbWriter(BinaryWriter writer) : Data.IGeometryWriter, IDisposable
{
    /// <summary>
    /// Initialises a new instance of the <see cref="TinyWkbWriter"/> class based on the specified stream, and optionally leaves the stream open.
    /// </summary>
    /// <param name="stream">The output stream.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="TinyWkbWriter"/> object is disposed; otherwise, <see langword="false"/>.</param>
    public TinyWkbWriter(Stream stream, bool leaveOpen = true)
        : this(new(stream, System.Text.Encoding.UTF8, leaveOpen))
    {
    }

    private delegate int GetMaxSize<in T>(T value);

    private delegate int Write2DValue<in T>(Span<byte> destination, T value, int precision, bool boundingBox = false);

    private delegate int Write3DValue<in T>(Span<byte> destination, T value, int precisionXY, int precision, bool boundingBox = false);

    private delegate int Write4DValue<in T>(Span<byte> destination, T value, int precisionXY, int precisionZ, int precisionM, bool boundingBox = false);

    /// <inheritdoc/>
    public void Write(Point point) => this.Write(point, default);

    /// <inheritdoc cref="Write(Point)"/>
    public void Write(Point point, bool boundingBox) => this.Write(point, precision: default, boundingBox);

    /// <inheritdoc cref="Write(Point)"/>
    public void Write(Point point, int precision, bool boundingBox = false) => this.Write2DCore(point, precision, boundingBox, WkbPrimitives.GetMaxSize, TwkbPrimitives.WritePoint);

    /// <inheritdoc/>
    public void Write(PointZ point) => this.Write(point, default);

    /// <inheritdoc cref="Write(PointZ)"/>
    public void Write(PointZ point, bool boundingBox) => this.Write(point, default, default, boundingBox);

    /// <inheritdoc cref="Write(PointZ)"/>
    public void Write(PointZ point, int precisionXY, int precisionZ, bool boundingBox = false) => this.Write3DCore(point, precisionXY, precisionZ, boundingBox, WkbPrimitives.GetMaxSize, TwkbPrimitives.WritePointZ);

    /// <inheritdoc/>
    public void Write(PointM point) => this.Write(point, default);

    /// <inheritdoc cref="Write(PointM)"/>
    public void Write(PointM point, bool boundingBox) => this.Write(point, default, default, boundingBox);

    /// <inheritdoc cref="Write(PointM)"/>
    public void Write(PointM point, int precisionXY, int precisionM, bool boundingBox = false) => this.Write3DCore(point, precisionXY, precisionM, boundingBox, WkbPrimitives.GetMaxSize, TwkbPrimitives.WritePointM);

    /// <inheritdoc/>
    public void Write(PointZM point) => this.Write(point, default);

    /// <inheritdoc cref="Write(PointZM)"/>
    public void Write(PointZM point, bool boundingBox) => this.Write(point, default, default, default, boundingBox);

    /// <inheritdoc cref="Write(PointZM)"/>
    public void Write(PointZM point, int precisionXY, int precisionZ, int precisionM, bool boundingBox = false) => this.Write4DCore(point, precisionXY, precisionZ, precisionM, boundingBox, WkbPrimitives.GetMaxSize, TwkbPrimitives.WritePointZM);

    /// <inheritdoc/>
    public void Write(IEnumerable<Point> points) => this.Write(points, default);

    /// <inheritdoc cref="Write(IEnumerable{Point})"/>
    public void Write(IEnumerable<Point> points, bool boundingBox) => this.Write(points, default, boundingBox);

    /// <inheritdoc cref="Write(IEnumerable{Point})"/>
    public void Write(IEnumerable<Point> points, int precision, bool boundingBox = false) => this.Write2DCore(ToCollection(points), precision, boundingBox, WkbPrimitives.GetMaxSize, TwkbPrimitives.WritePoint);

    /// <inheritdoc/>
    public void Write(IEnumerable<PointZ> points) => this.Write(points, default);

    /// <inheritdoc cref="Write(IEnumerable{PointZ})"/>
    public void Write(IEnumerable<PointZ> points, bool boundingBox) => this.Write(points, default, default, boundingBox);

    /// <inheritdoc cref="Write(IEnumerable{PointZ})"/>
    public void Write(IEnumerable<PointZ> points, int precisionXY, int precisionZ, bool boundingBox = false) => this.Write3DCore(ToCollection(points), precisionXY, precisionZ, boundingBox, WkbPrimitives.GetMaxSize, TwkbPrimitives.WritePointZ);

    /// <inheritdoc/>
    public void Write(IEnumerable<PointM> points) => this.Write(points, default);

    /// <inheritdoc cref="Write(IEnumerable{PointM})"/>
    public void Write(IEnumerable<PointM> points, bool boundingBox) => this.Write(points, default, default, boundingBox);

    /// <inheritdoc cref="Write(IEnumerable{PointM})"/>
    public void Write(IEnumerable<PointM> points, int precisionXY, int precisionM, bool boundingBox = false) => this.Write3DCore(ToCollection(points), precisionXY, precisionM, boundingBox, WkbPrimitives.GetMaxSize, TwkbPrimitives.WritePointM);

    /// <inheritdoc/>
    public void Write(IEnumerable<PointZM> points) => this.Write(points, default);

    /// <inheritdoc cref="Write(IEnumerable{PointZM})"/>
    public void Write(IEnumerable<PointZM> points, bool boundingBox) => this.Write(points, default, default, default, boundingBox);

    /// <inheritdoc cref="Write(IEnumerable{PointZM})"/>
    public void Write(IEnumerable<PointZM> points, int precisionXY, int precisionZ, int precisionM, bool boundingBox = false) => this.Write4DCore(ToCollection(points), precisionXY, precisionZ, precisionM, boundingBox, WkbPrimitives.GetMaxSize, TwkbPrimitives.WritePointZM);

    /// <inheritdoc/>
    public void Write(Polyline<Point> polyline) => this.Write(polyline, boundingBox: default);

    /// <inheritdoc cref="Write(Polyline{Point})"/>
    public void Write(Polyline<Point> polyline, bool boundingBox) => this.Write(polyline, default, boundingBox);

    /// <inheritdoc cref="Write(Polyline{Point})"/>
    public void Write(Polyline<Point> polyline, int precision, bool boundingBox = false) => this.Write2DCore(polyline, precision, boundingBox, WkbPrimitives.GetMaxSize, TwkbPrimitives.WriteLineString);

    /// <inheritdoc/>
    public void Write(Polyline<PointZ> polyline) => this.Write(polyline, boundingBox: default);

    /// <inheritdoc cref="Write(Polyline{PointZ})"/>
    public void Write(Polyline<PointZ> polyline, bool boundingBox) => this.Write(polyline, default, default, boundingBox);

    /// <inheritdoc cref="Write(Polyline{PointZ})"/>
    public void Write(Polyline<PointZ> polyline, int precisionXY, int precisionZ, bool boundingBox = false) => this.Write3DCore(polyline, precisionXY, precisionZ, boundingBox, WkbPrimitives.GetMaxSize, TwkbPrimitives.WriteLineStringZ);

    /// <inheritdoc/>
    public void Write(Polyline<PointM> polyline) => this.Write(polyline, boundingBox: default);

    /// <inheritdoc cref="Write(Polyline{PointM})"/>
    public void Write(Polyline<PointM> polyline, bool boundingBox) => this.Write(polyline, default, default, boundingBox);

    /// <inheritdoc cref="Write(Polyline{PointM})"/>
    public void Write(Polyline<PointM> polyline, int precisionXY, int precisionM, bool boundingBox = false) => this.Write3DCore(polyline, precisionXY, precisionM, boundingBox, WkbPrimitives.GetMaxSize, TwkbPrimitives.WriteLineStringM);

    /// <inheritdoc/>
    public void Write(Polyline<PointZM> polyline) => this.Write(polyline, boundingBox: default);

    /// <inheritdoc cref="Write(Polyline{PointZM})"/>
    public void Write(Polyline<PointZM> polyline, bool boundingBox) => this.Write(polyline, default, default, default, boundingBox);

    /// <inheritdoc cref="Write(Polyline{PointZM})"/>
    public void Write(Polyline<PointZM> polyline, int precisionXY, int precisionZ, int precisionM, bool boundingBox = false) => this.Write4DCore(polyline, precisionXY, precisionZ, precisionM, boundingBox, WkbPrimitives.GetMaxSize, TwkbPrimitives.WriteLineStringZM);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polyline<Point>> polylines) => this.Write(polylines, default);

    /// <inheritdoc cref="Write(IEnumerable{Polyline{Point}})"/>
    public void Write(IEnumerable<Polyline<Point>> polylines, bool boundingBox) => this.Write(polylines, default, boundingBox);

    /// <inheritdoc cref="Write(IEnumerable{Polyline{Point}})"/>
    public void Write(IEnumerable<Polyline<Point>> polylines, int precision, bool boundingBox = false) => this.Write2DCore(ToCollection(polylines), precision, boundingBox, WkbPrimitives.GetMaxSize, TwkbPrimitives.WriteLineString);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polyline<PointZ>> polylines) => this.Write(polylines, default);

    /// <inheritdoc cref="Write(IEnumerable{Polyline{PointZ}})"/>
    public void Write(IEnumerable<Polyline<PointZ>> polylines, bool boundingBox) => this.Write(polylines, default, default, boundingBox);

    /// <inheritdoc cref="Write(IEnumerable{Polyline{PointZ}})"/>
    public void Write(IEnumerable<Polyline<PointZ>> polylines, int precisionXY, int precisionZ, bool boundingBox = false) => this.Write3DCore(ToCollection(polylines), precisionXY, precisionZ, boundingBox, WkbPrimitives.GetMaxSize, TwkbPrimitives.WriteLineStringZ);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polyline<PointM>> polylines) => this.Write(polylines, default);

    /// <inheritdoc cref="Write(IEnumerable{Polyline{PointM}})"/>
    public void Write(IEnumerable<Polyline<PointM>> polylines, bool boundingBox) => this.Write(polylines, default, default, boundingBox);

    /// <inheritdoc cref="Write(IEnumerable{Polyline{PointM}})"/>
    public void Write(IEnumerable<Polyline<PointM>> polylines, int precisionXY, int precisionM, bool boundingBox = false) => this.Write3DCore(ToCollection(polylines), precisionXY, precisionM, boundingBox, WkbPrimitives.GetMaxSize, TwkbPrimitives.WriteLineStringM);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polyline<PointZM>> polylines) => this.Write(polylines, default);

    /// <inheritdoc cref="Write(IEnumerable{Polyline{PointZM}})"/>
    public void Write(IEnumerable<Polyline<PointZM>> polylines, bool boundingBox) => this.Write(polylines, default, default, default, boundingBox);

    /// <inheritdoc cref="Write(IEnumerable{Polyline{PointZM}})"/>
    public void Write(IEnumerable<Polyline<PointZM>> polylines, int precisionXY, int precisionZ, int precisionM, bool boundingBox = false) => this.Write4DCore(ToCollection(polylines), precisionXY, precisionZ, precisionM, boundingBox, WkbPrimitives.GetMaxSize, TwkbPrimitives.WriteLineStringZM);

    /// <inheritdoc/>
    public void Write(Polygon<Point> polygon) => this.Write(polygon, boundingBox: default);

    /// <inheritdoc cref="Write(Polygon{Point})"/>
    public void Write(Polygon<Point> polygon, bool boundingBox) => this.Write(polygon, default, boundingBox);

    /// <inheritdoc cref="Write(Polygon{Point})"/>
    public void Write(Polygon<Point> polygon, int precision, bool boundingBox = false) => this.Write2DCore(polygon, precision, boundingBox, WkbPrimitives.GetMaxSize, TwkbPrimitives.WritePolygon);

    /// <inheritdoc/>
    public void Write(Polygon<PointZ> polygon) => this.Write(polygon, boundingBox: default);

    /// <inheritdoc cref="Write(Polygon{PointZ})"/>
    public void Write(Polygon<PointZ> polygon, bool boundingBox) => this.Write(polygon, default, default, boundingBox);

    /// <inheritdoc cref="Write(Polygon{PointZ})"/>
    public void Write(Polygon<PointZ> polygon, int precisionXY, int precisionZ, bool boundingBox = false) => this.Write3DCore(polygon, precisionXY, precisionZ, boundingBox, WkbPrimitives.GetMaxSize, TwkbPrimitives.WritePolygonZ);

    /// <inheritdoc/>
    public void Write(Polygon<PointM> polygon) => this.Write(polygon, boundingBox: default);

    /// <inheritdoc cref="Write(Polygon{PointM})"/>
    public void Write(Polygon<PointM> polygon, bool boundingBox) => this.Write(polygon, default, default, boundingBox);

    /// <inheritdoc cref="Write(Polygon{PointM})"/>
    public void Write(Polygon<PointM> polygon, int precisionXY, int precisionM, bool boundingBox = false) => this.Write3DCore(polygon, precisionXY, precisionM, boundingBox, WkbPrimitives.GetMaxSize, TwkbPrimitives.WritePolygonM);

    /// <inheritdoc/>
    public void Write(Polygon<PointZM> polygon) => this.Write(polygon, boundingBox: default);

    /// <inheritdoc cref="Write(Polygon{PointZM})"/>
    public void Write(Polygon<PointZM> polygon, bool boundingBox) => this.Write(polygon, default, default, default, boundingBox);

    /// <inheritdoc cref="Write(Polygon{PointZM})"/>
    public void Write(Polygon<PointZM> polygon, int precisionXY, int precisionZ, int precisionM, bool boundingBox = false) => this.Write4DCore(polygon, precisionXY, precisionZ, precisionM, boundingBox, WkbPrimitives.GetMaxSize, TwkbPrimitives.WritePolygonZM);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polygon<Point>> polygons) => this.Write(polygons, default);

    /// <inheritdoc cref="Write(IEnumerable{Polygon{Point}})"/>
    public void Write(IEnumerable<Polygon<Point>> polygons, bool boundingBox) => this.Write(polygons, default, boundingBox);

    /// <inheritdoc cref="Write(IEnumerable{Polygon{Point}})"/>
    public void Write(IEnumerable<Polygon<Point>> polygons, int precision, bool boundingBox = false) => this.Write2DCore(ToCollection(polygons), precision, boundingBox, WkbPrimitives.GetMaxSize, TwkbPrimitives.WritePolygon);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polygon<PointZ>> polygons) => this.Write(polygons, default);

    /// <inheritdoc cref="Write(IEnumerable{Polygon{PointZ}})"/>
    public void Write(IEnumerable<Polygon<PointZ>> polygons, bool boundingBox) => this.Write(polygons, default, default, boundingBox);

    /// <inheritdoc cref="Write(IEnumerable{Polygon{PointZ}})"/>
    public void Write(IEnumerable<Polygon<PointZ>> polygons, int precisionXY, int precisionZ, bool boundingBox = false) => this.Write3DCore(ToCollection(polygons), precisionXY, precisionZ, boundingBox, WkbPrimitives.GetMaxSize, TwkbPrimitives.WritePolygonZ);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polygon<PointM>> polygons) => this.Write(polygons, default);

    /// <inheritdoc cref="Write(IEnumerable{Polygon{PointM}})"/>
    public void Write(IEnumerable<Polygon<PointM>> polygons, bool boundingBox) => this.Write(polygons, default, default, boundingBox);

    /// <inheritdoc cref="Write(IEnumerable{Polygon{PointM}})"/>
    public void Write(IEnumerable<Polygon<PointM>> polygons, int precisionXY, int precisionM, bool boundingBox = false) => this.Write3DCore(ToCollection(polygons), precisionXY, precisionM, boundingBox, WkbPrimitives.GetMaxSize, TwkbPrimitives.WritePolygonM);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polygon<PointZM>> polygons) => this.Write(polygons, default);

    /// <inheritdoc cref="Write(IEnumerable{Polygon{PointZM}})"/>
    public void Write(IEnumerable<Polygon<PointZM>> polygons, bool boundingBox) => this.Write(polygons, default, default, default, boundingBox);

    /// <inheritdoc cref="Write(IEnumerable{Polygon{PointZM}})"/>
    public void Write(IEnumerable<Polygon<PointZM>> polygons, int precisionXY, int precisionZ, int precisionM, bool boundingBox = false) => this.Write4DCore(ToCollection(polygons), precisionXY, precisionZ, precisionM, boundingBox, WkbPrimitives.GetMaxSize, TwkbPrimitives.WritePolygonZM);

    /// <inheritdoc/>
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            writer.Dispose();
        }
    }

    private static ICollection<T> ToCollection<T>(IEnumerable<T> enumerable) => [.. enumerable];

    private void Write2DCore<T>(T value, int precision, bool boundingBox, GetMaxSize<T> getMaxSize, Write2DValue<T> write)
    {
        var size = getMaxSize(value);
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        Span<byte> span = stackalloc byte[size];
#else
        var a = new byte[size];
        var span = a.AsSpan();
#endif

        var written = write(span, value, precision, boundingBox);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        writer.Write(span[..written]);
#else
        writer.Write(a, 0, written);
#endif
    }

    private void Write3DCore<T>(T value, int precisionXY, int precision, bool boundingBox, GetMaxSize<T> getMaxSize, Write3DValue<T> write)
    {
        var size = getMaxSize(value);
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        Span<byte> span = stackalloc byte[size];
#else
        var a = new byte[size];
        var span = a.AsSpan();
#endif

        var written = write(span, value, precisionXY, precision, boundingBox);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        writer.Write(span[..written]);
#else
        writer.Write(a, 0, written);
#endif
    }

    private void Write4DCore<T>(T value, int precisionXY, int precisionZ, int precisionM, bool boundingBox, GetMaxSize<T> getMaxSize, Write4DValue<T> write)
    {
        var size = getMaxSize(value);
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        Span<byte> span = stackalloc byte[size];
#else
        var a = new byte[size];
        var span = a.AsSpan();
#endif

        var written = write(span, value, precisionXY, precisionZ, precisionM, boundingBox);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        writer.Write(span[..written]);
#else
        writer.Write(a, 0, written);
#endif
    }
}