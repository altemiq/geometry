// -----------------------------------------------------------------------
// <copyright file="WktWriter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

/// <summary>
/// The WKT writer.
/// </summary>
/// <param name="writer">The text writer to write to.</param>
public class WktWriter(TextWriter writer) : Data.IGeometryWriter, IDisposable
{
    private bool disposedValue;

    /// <summary>
    /// Initialises a new instance of the <see cref="WktWriter"/> class.
    /// </summary>
    /// <param name="sb">The string builder to write to.</param>
    public WktWriter(System.Text.StringBuilder sb)
        : this(new StringWriter(sb))
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="WktWriter"/> class.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    public WktWriter(Stream stream)
        : this(new StreamWriter(stream, System.Text.Encoding.UTF8, 1024, leaveOpen: true))
    {
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    public void Write(Point point) => this.Write(string.Empty, point, static p => p.IsEmpty, WriteWithoutType);

    /// <inheritdoc/>
    public void Write(PointZ point) => this.Write("Z", point, static p => p.IsEmpty, WriteWithoutType);

    /// <inheritdoc/>
    public void Write(PointM point) => this.Write("M", point, static p => p.IsEmpty, WriteWithoutType);

    /// <inheritdoc/>
    public void Write(PointZM point) => this.Write("ZM", point, static p => p.IsEmpty, WriteWithoutType);

    /// <inheritdoc/>
    public void Write(IEnumerable<Point> points) => this.Write(string.Empty, points, WriteWithoutBrackets);

    /// <inheritdoc/>
    public void Write(IEnumerable<PointZ> points) => this.Write("Z", points, WriteWithoutBrackets);

    /// <inheritdoc/>
    public void Write(IEnumerable<PointM> points) => this.Write("M", points, WriteWithoutBrackets);

    /// <inheritdoc/>
    public void Write(IEnumerable<PointZM> points) => this.Write("ZM", points, WriteWithoutBrackets);

    /// <inheritdoc/>
    public void Write(Polyline<Point> polyline) => this.Write(string.Empty, polyline, WriteWithoutBrackets);

    /// <inheritdoc/>
    public void Write(Polyline<PointZ> polyline) => this.Write(string.Empty, polyline, WriteWithoutBrackets);

    /// <inheritdoc/>
    public void Write(Polyline<PointM> polyline) => this.Write(string.Empty, polyline, WriteWithoutBrackets);

    /// <inheritdoc/>
    public void Write(Polyline<PointZM> polyline) => this.Write(string.Empty, polyline, WriteWithoutBrackets);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polyline<Point>> polylines) => this.Write(string.Empty, polylines, WriteWithoutType);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polyline<PointZ>> polylines) => this.Write(string.Empty, polylines, WriteWithoutType);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polyline<PointM>> polylines) => this.Write(string.Empty, polylines, WriteWithoutType);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polyline<PointZM>> polylines) => this.Write(string.Empty, polylines, WriteWithoutType);

    /// <inheritdoc/>
    public void Write(Polygon<Point> polygon) => this.Write(string.Empty, polygon, WriteWithoutType);

    /// <inheritdoc/>
    public void Write(Polygon<PointZ> polygon) => this.Write(string.Empty, polygon, WriteWithoutType);

    /// <inheritdoc/>
    public void Write(Polygon<PointM> polygon) => this.Write(string.Empty, polygon, WriteWithoutType);

    /// <inheritdoc/>
    public void Write(Polygon<PointZM> polygon) => this.Write(string.Empty, polygon, WriteWithoutType);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polygon<Point>> polygons) => this.Write(string.Empty, polygons, WriteWithoutType);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polygon<PointZ>> polygons) => this.Write(string.Empty, polygons, WriteWithoutType);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polygon<PointM>> polygons) => this.Write(string.Empty, polygons, WriteWithoutType);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polygon<PointZM>> polygons) => this.Write(string.Empty, polygons, WriteWithoutType);

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                writer.Dispose();
            }

            this.disposedValue = true;
        }
    }

    private static void WriteWithoutType<T>(TextWriter writer, T item, Action<TextWriter, T> write)
    {
        writer.Write("(");
        write(writer, item);
        writer.Write(")");
    }

    private static void WriteWithoutType<T>(TextWriter writer, IEnumerable<T> items, Action<TextWriter, T> write)
    {
        writer.Write("(");

        bool first = true;
        foreach (var item in items)
        {
            if (!first)
            {
                writer.Write(", ");
            }

            first = false;
            write(writer, item);
        }

        writer.Write(")");
    }

    private static void WriteWithoutType<T>(TextWriter writer, IEnumerable<IEnumerable<T>> parts, Action<TextWriter, T> write)
    {
        writer.Write("(");

        bool first = true;
        foreach (var part in parts)
        {
            if (!first)
            {
                writer.Write(", ");
            }

            first = false;

            WriteWithoutType(writer, part, write);
        }

        writer.Write(")");
    }

    private static void WriteWithoutType(TextWriter writer, Point point) => WriteWithoutType(writer, point, WriteWithoutBrackets);

    private static void WriteWithoutType(TextWriter writer, PointZ point) => WriteWithoutType(writer, point, WriteWithoutBrackets);

    private static void WriteWithoutType(TextWriter writer, PointM point) => WriteWithoutType(writer, point, WriteWithoutBrackets);

    private static void WriteWithoutType(TextWriter writer, PointZM point) => WriteWithoutType(writer, point, WriteWithoutBrackets);

    private static void WriteWithoutType(TextWriter writer, IEnumerable<Point> points) => WriteWithoutType(writer, points, WriteWithoutBrackets);

    private static void WriteWithoutType(TextWriter writer, IEnumerable<PointZ> points) => WriteWithoutType(writer, points, WriteWithoutBrackets);

    private static void WriteWithoutType(TextWriter writer, IEnumerable<PointM> points) => WriteWithoutType(writer, points, WriteWithoutBrackets);

    private static void WriteWithoutType(TextWriter writer, IEnumerable<PointZM> points) => WriteWithoutType(writer, points, WriteWithoutBrackets);

    private static void WriteWithoutType(TextWriter writer, IEnumerable<IEnumerable<Point>> points) => WriteWithoutType(writer, points, WriteWithoutBrackets);

    private static void WriteWithoutType(TextWriter writer, IEnumerable<IEnumerable<PointZ>> points) => WriteWithoutType(writer, points, WriteWithoutBrackets);

    private static void WriteWithoutType(TextWriter writer, IEnumerable<IEnumerable<PointM>> points) => WriteWithoutType(writer, points, WriteWithoutBrackets);

    private static void WriteWithoutType(TextWriter writer, IEnumerable<IEnumerable<PointZM>> points) => WriteWithoutType(writer, points, WriteWithoutBrackets);

    private static void WriteWithoutBrackets(TextWriter writer, Point point)
    {
        writer.Write(point.X);
        writer.Write(" ");
        writer.Write(point.Y);
    }

    private static void WriteWithoutBrackets(TextWriter writer, PointZ point)
    {
        writer.Write(point.X);
        writer.Write(" ");
        writer.Write(point.Y);
        writer.Write(" ");
        writer.Write(point.Z);
    }

    private static void WriteWithoutBrackets(TextWriter writer, PointM point)
    {
        writer.Write(point.X);
        writer.Write(" ");
        writer.Write(point.Y);
        writer.Write(" ");
        writer.Write(point.Measurement);
    }

    private static void WriteWithoutBrackets(TextWriter writer, PointZM point)
    {
        writer.Write(point.X);
        writer.Write(" ");
        writer.Write(point.Y);
        writer.Write(" ");
        writer.Write(point.Z);
        writer.Write(" ");
        writer.Write(point.Measurement);
    }

    private void Write<T>(string type, T point, Func<T, bool> isEmpty, Action<TextWriter, T> write) => this.Write("POINT", type, point, isEmpty, write);

    private void Write<T>(string type, IEnumerable<T> points, Action<TextWriter, T> write) => this.Write("MULTIPOINT", type, points, write);

    private void Write<T>(string type, Polyline<T> points, Action<TextWriter, T> write) => this.Write("LINESTRING", type, points, write);

    private void Write<T>(string type, IEnumerable<Polyline<T>> points, Action<TextWriter, Polyline<T>> write) => this.Write("MULTILINESTRING", type, points, write);

    private void Write<T>(string type, Polygon<T> points, Action<TextWriter, LinearRing<T>> write)
        where T : struct => this.Write("POLYGON", type, points, write);

    private void Write<T>(string type, IEnumerable<Polygon<T>> points, Action<TextWriter, Polygon<T>> write)
        where T : struct => this.Write("MULTIPOLYGON", type, points, write);

    private void Write<T>(string geometry, string type, T element, Func<T, bool> isEmpty, Action<TextWriter, T> write)
    {
        writer.Write(geometry);
        writer.Write(" ");
        if (type.Length is not 0)
        {
            writer.Write(type);
            writer.Write(" ");
        }

        if (isEmpty(element))
        {
            writer.Write("EMPTY");
            return;
        }

        write(writer, element);
    }

    private void Write<T>(string geometry, string type, IEnumerable<T> elements, Action<TextWriter, T> write)
    {
        writer.Write(geometry);
        writer.Write(" ");
        if (type.Length is not 0)
        {
            writer.Write(type);
            writer.Write(" ");
        }

        using var enumerator = elements.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            writer.Write("EMPTY");
            return;
        }

        writer.Write("(");

        bool first = true;
        do
        {
            if (!first)
            {
                writer.Write(", ");
            }

            first = false;
            write(writer, enumerator.Current);
        }
        while (enumerator.MoveNext());

        writer.Write(")");
    }
}