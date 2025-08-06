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
/// <param name="bufferSize">The buffer size.</param>
public class WktWriter(TextWriter writer, int bufferSize = 1024) : Data.IGeometryWriter, IDisposable
{
    private readonly byte[] buffer = new byte[bufferSize];
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

    private delegate bool TryFormat<in T>(T value, Span<byte> destination, out int written);

    /// <inheritdoc/>
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    public void Write(Point point) => this.Write(point, Altemiq.Buffers.Text.WktFormatter.TryFormat);

    /// <inheritdoc/>
    public void Write(PointZ point) => this.Write(point, Altemiq.Buffers.Text.WktFormatter.TryFormat);

    /// <inheritdoc/>
    public void Write(PointM point) => this.Write(point, Altemiq.Buffers.Text.WktFormatter.TryFormat);

    /// <inheritdoc/>
    public void Write(PointZM point) => this.Write(point, Altemiq.Buffers.Text.WktFormatter.TryFormat);

    /// <inheritdoc/>
    public void Write(IEnumerable<Point> points) => this.Write(points, Altemiq.Buffers.Text.WktFormatter.TryFormat);

    /// <inheritdoc/>
    public void Write(IEnumerable<PointZ> points) => this.Write(points, Altemiq.Buffers.Text.WktFormatter.TryFormat);

    /// <inheritdoc/>
    public void Write(IEnumerable<PointM> points) => this.Write(points, Altemiq.Buffers.Text.WktFormatter.TryFormat);

    /// <inheritdoc/>
    public void Write(IEnumerable<PointZM> points) => this.Write(points, Altemiq.Buffers.Text.WktFormatter.TryFormat);

    /// <inheritdoc/>
    public void Write(Polyline<Point> polyline) => this.Write(polyline, Altemiq.Buffers.Text.WktFormatter.TryFormat);

    /// <inheritdoc/>
    public void Write(Polyline<PointZ> polyline) => this.Write(polyline, Altemiq.Buffers.Text.WktFormatter.TryFormat);

    /// <inheritdoc/>
    public void Write(Polyline<PointM> polyline) => this.Write(polyline, Altemiq.Buffers.Text.WktFormatter.TryFormat);

    /// <inheritdoc/>
    public void Write(Polyline<PointZM> polyline) => this.Write(polyline, Altemiq.Buffers.Text.WktFormatter.TryFormat);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polyline<Point>> polylines) => this.Write(polylines, Altemiq.Buffers.Text.WktFormatter.TryFormat);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polyline<PointZ>> polylines) => this.Write(polylines, Altemiq.Buffers.Text.WktFormatter.TryFormat);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polyline<PointM>> polylines) => this.Write(polylines, Altemiq.Buffers.Text.WktFormatter.TryFormat);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polyline<PointZM>> polylines) => this.Write(polylines, Altemiq.Buffers.Text.WktFormatter.TryFormat);

    /// <inheritdoc/>
    public void Write(Polygon<Point> polygon) => this.Write(polygon, Altemiq.Buffers.Text.WktFormatter.TryFormat);

    /// <inheritdoc/>
    public void Write(Polygon<PointZ> polygon) => this.Write(polygon, Altemiq.Buffers.Text.WktFormatter.TryFormat);

    /// <inheritdoc/>
    public void Write(Polygon<PointM> polygon) => this.Write(polygon, Altemiq.Buffers.Text.WktFormatter.TryFormat);

    /// <inheritdoc/>
    public void Write(Polygon<PointZM> polygon) => this.Write(polygon, Altemiq.Buffers.Text.WktFormatter.TryFormat);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polygon<Point>> polygons) => this.Write(polygons, Altemiq.Buffers.Text.WktFormatter.TryFormat);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polygon<PointZ>> polygons) => this.Write(polygons, Altemiq.Buffers.Text.WktFormatter.TryFormat);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polygon<PointM>> polygons) => this.Write(polygons, Altemiq.Buffers.Text.WktFormatter.TryFormat);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polygon<PointZM>> polygons) => this.Write(polygons, Altemiq.Buffers.Text.WktFormatter.TryFormat);

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

    private void Write<T>(T value, TryFormat<T> tryFormat)
    {
        if (!tryFormat(value, this.buffer, out int written))
        {
            throw new InvalidGeometryTypeException();
        }

        var chars = System.Text.Encoding.UTF8.GetChars(this.buffer, 0, written);
        writer.Write(chars, 0, chars.Length);
    }
}