// -----------------------------------------------------------------------
// <copyright file="EwktWriter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

/// <summary>
/// The WKT writer.
/// </summary>
/// <param name="writer">The text writer to write to.</param>
/// <param name="bufferSize">The buffer size.</param>
public sealed class EwktWriter(TextWriter writer, int bufferSize = 1024) : WktWriter(writer, bufferSize)
{
    /// <summary>
    /// Initialises a new instance of the <see cref="EwktWriter"/> class.
    /// </summary>
    /// <param name="sb">The string builder to write to.</param>
    public EwktWriter(System.Text.StringBuilder sb)
        : this(new StringWriter(sb))
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="EwktWriter"/> class.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    public EwktWriter(Stream stream)
        : this(new StreamWriter(stream, System.Text.Encoding.UTF8, 1024, leaveOpen: true))
    {
    }

    private delegate bool TryFormat<in T>(T value, int srid, Span<byte> destination, out int written);

    /// <inheritdoc/>
    public override void Write(Point point) => this.Write(point, default);

    /// <inheritdoc/>
    public void Write(Point point, int srid) => this.Write(point, srid, Buffers.Text.EwktFormatter.TryFormat);

    /// <inheritdoc/>
    public override void Write(PointZ point) => this.Write(point, default);

    /// <inheritdoc/>
    public void Write(PointZ point, int srid) => this.Write(point, srid, Buffers.Text.EwktFormatter.TryFormat);

    /// <inheritdoc/>
    public override void Write(PointM point) => this.Write(point, default);

    /// <inheritdoc/>
    public void Write(PointM point, int srid) => this.Write(point, srid, Buffers.Text.EwktFormatter.TryFormat);

    /// <inheritdoc/>
    public override void Write(PointZM point) => this.Write(point, default);

    /// <inheritdoc/>
    public void Write(PointZM point, int srid) => this.Write(point, srid, Buffers.Text.EwktFormatter.TryFormat);

    /// <inheritdoc/>
    public override void Write(IEnumerable<Point> points) => this.Write(points, default);

    /// <inheritdoc/>
    public void Write(IEnumerable<Point> points, int srid) => this.Write(points, srid, Buffers.Text.EwktFormatter.TryFormat);

    /// <inheritdoc/>
    public override void Write(IEnumerable<PointZ> points) => this.Write(points, default);

    /// <inheritdoc/>
    public void Write(IEnumerable<PointZ> points, int srid) => this.Write(points, srid, Buffers.Text.EwktFormatter.TryFormat);

    /// <inheritdoc/>
    public override void Write(IEnumerable<PointM> points) => this.Write(points, default);

    /// <inheritdoc/>
    public void Write(IEnumerable<PointM> points, int srid) => this.Write(points, srid, Buffers.Text.EwktFormatter.TryFormat);

    /// <inheritdoc/>
    public override void Write(IEnumerable<PointZM> points) => this.Write(points, default);

    /// <inheritdoc/>
    public void Write(IEnumerable<PointZM> points, int srid) => this.Write(points, srid, Buffers.Text.EwktFormatter.TryFormat);

    /// <inheritdoc/>
    public override void Write(Polyline<Point> polyline) => this.Write(polyline, default);

    /// <inheritdoc/>
    public void Write(Polyline<Point> polyline, int srid) => this.Write(polyline, srid, Buffers.Text.EwktFormatter.TryFormat);

    /// <inheritdoc/>
    public override void Write(Polyline<PointZ> polyline) => this.Write(polyline, default);

    /// <inheritdoc/>
    public void Write(Polyline<PointZ> polyline, int srid) => this.Write(polyline, srid, Buffers.Text.EwktFormatter.TryFormat);

    /// <inheritdoc/>
    public override void Write(Polyline<PointM> polyline) => this.Write(polyline, default);

    /// <inheritdoc/>
    public void Write(Polyline<PointM> polyline, int srid) => this.Write(polyline, srid, Buffers.Text.EwktFormatter.TryFormat);

    /// <inheritdoc/>
    public override void Write(Polyline<PointZM> polyline) => this.Write(polyline, default);

    /// <inheritdoc/>
    public void Write(Polyline<PointZM> polyline, int srid) => this.Write(polyline, srid, Buffers.Text.EwktFormatter.TryFormat);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polyline<Point>> polylines) => this.Write(polylines, default);

    /// <inheritdoc/>
    public void Write(IEnumerable<Polyline<Point>> polylines, int srid) => this.Write(polylines, srid, Buffers.Text.EwktFormatter.TryFormat);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polyline<PointZ>> polylines) => this.Write(polylines, default);

    /// <inheritdoc/>
    public void Write(IEnumerable<Polyline<PointZ>> polylines, int srid) => this.Write(polylines, srid, Buffers.Text.EwktFormatter.TryFormat);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polyline<PointM>> polylines) => this.Write(polylines, default);

    /// <inheritdoc/>
    public void Write(IEnumerable<Polyline<PointM>> polylines, int srid) => this.Write(polylines, srid, Buffers.Text.EwktFormatter.TryFormat);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polyline<PointZM>> polylines) => this.Write(polylines, default);

    /// <inheritdoc/>
    public void Write(IEnumerable<Polyline<PointZM>> polylines, int srid) => this.Write(polylines, srid, Buffers.Text.EwktFormatter.TryFormat);

    /// <inheritdoc/>
    public override void Write(Polygon<Point> polygon) => this.Write(polygon, default);

    /// <inheritdoc/>
    public void Write(Polygon<Point> polygon, int srid) => this.Write(polygon, srid, Buffers.Text.EwktFormatter.TryFormat);

    /// <inheritdoc/>
    public override void Write(Polygon<PointZ> polygon) => this.Write(polygon, default);

    /// <inheritdoc/>
    public void Write(Polygon<PointZ> polygon, int srid) => this.Write(polygon, srid, Buffers.Text.EwktFormatter.TryFormat);

    /// <inheritdoc/>
    public override void Write(Polygon<PointM> polygon) => this.Write(polygon, default);

    /// <inheritdoc/>
    public void Write(Polygon<PointM> polygon, int srid) => this.Write(polygon, srid, Buffers.Text.EwktFormatter.TryFormat);

    /// <inheritdoc/>
    public override void Write(Polygon<PointZM> polygon) => this.Write(polygon, default);

    /// <inheritdoc/>
    public void Write(Polygon<PointZM> polygon, int srid) => this.Write(polygon, srid, Buffers.Text.EwktFormatter.TryFormat);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polygon<Point>> polygons) => this.Write(polygons, default);

    /// <inheritdoc/>
    public void Write(IEnumerable<Polygon<Point>> polygons, int srid) => this.Write(polygons, srid, Buffers.Text.EwktFormatter.TryFormat);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polygon<PointZ>> polygons) => this.Write(polygons, default);

    /// <inheritdoc/>
    public void Write(IEnumerable<Polygon<PointZ>> polygons, int srid) => this.Write(polygons, srid, Buffers.Text.EwktFormatter.TryFormat);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polygon<PointM>> polygons) => this.Write(polygons, default);

    /// <inheritdoc/>
    public void Write(IEnumerable<Polygon<PointM>> polygons, int srid) => this.Write(polygons, srid, Buffers.Text.EwktFormatter.TryFormat);

    /// <inheritdoc/>
    public override void Write(params IEnumerable<Polygon<PointZM>> polygons) => this.Write(polygons, default);

    /// <inheritdoc/>
    public void Write(IEnumerable<Polygon<PointZM>> polygons, int srid) => this.Write(polygons, srid, Buffers.Text.EwktFormatter.TryFormat);

    private void Write<T>(T value, int srid, TryFormat<T> tryFormat) => this.Write(value, (v, b) => tryFormat(v, srid, b, out int written) ? written : throw new InvalidGeometryTypeException());
}