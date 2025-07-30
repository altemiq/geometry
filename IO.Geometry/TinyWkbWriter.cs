// -----------------------------------------------------------------------
// <copyright file="TinyWkbWriter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

using static System.Math;

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

    /// <inheritdoc/>
    public void Write(Point point) => this.Write(point, default);

    /// <inheritdoc/>
    public void Write(PointZ point) => this.Write(point, default);

    /// <inheritdoc/>
    public void Write(PointM point) => this.Write(point, default);

    /// <inheritdoc/>
    public void Write(PointZM point) => this.Write(point, default);

    /// <inheritdoc cref="Write(Point)"/>
    public void Write(Point point, bool boundingBox) => this.Write(point, precision: default, boundingBox);

    /// <inheritdoc cref="Write(Point)"/>
    public void Write(Point point, int precision, bool boundingBox = false) => this.WriteCore(point, precision, default, default, boundingBox, GetXY, GetZ, GetM, IsNullOrEmpty);

    /// <inheritdoc cref="Write(PointZ)"/>
    public void Write(PointZ point, bool boundingBox) => this.Write(point, default, default, boundingBox);

    /// <inheritdoc cref="Write(PointZ)"/>
    public void Write(PointZ point, int precisionXY, int precisionZ, bool boundingBox = false) => this.WriteCore(point, precisionXY, precisionZ, default, boundingBox, GetXY, GetZ, GetM, IsNullOrEmpty);

    /// <inheritdoc cref="Write(PointM)"/>
    public void Write(PointM point, bool boundingBox) => this.Write(point, default, default, boundingBox);

    /// <inheritdoc cref="Write(PointM)"/>
    public void Write(PointM point, int precisionXY, int precisionM, bool boundingBox = false) => this.WriteCore(point, precisionXY, default, precisionM, boundingBox, GetXY, GetZ, GetM, IsNullOrEmpty);

    /// <inheritdoc cref="Write(PointZM)"/>
    public void Write(PointZM point, bool boundingBox) => this.Write(point, default, default, default, boundingBox);

    /// <inheritdoc cref="Write(PointZM)"/>
    public void Write(PointZM point, int precisionXY, int precisionZ, int precisionM, bool boundingBox = false) => this.WriteCore(point, precisionXY, precisionZ, precisionM, boundingBox, GetXY, GetZ, GetM, IsNullOrEmpty);

    /// <inheritdoc/>
    public void Write(IEnumerable<Point> points) => this.Write(points, default);

    /// <inheritdoc cref="Write(IEnumerable{Point})"/>
    public void Write(IEnumerable<Point> points, bool boundingBox) => this.Write(points, default, boundingBox);

    /// <inheritdoc cref="Write(IEnumerable{Point})"/>
    public void Write(IEnumerable<Point> points, int precision, bool boundingBox = false) => this.WriteCore([.. points], precision, default, default, boundingBox, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public void Write(IEnumerable<PointZ> points) => this.Write(points, default);

    /// <inheritdoc cref="Write(IEnumerable{PointZ})"/>
    public void Write(IEnumerable<PointZ> points, bool boundingBox) => this.Write(points, default, default, boundingBox);

    /// <inheritdoc cref="Write(IEnumerable{PointZ})"/>
    public void Write(IEnumerable<PointZ> points, int precisionXY, int precisionZ, bool boundingBox = false) => this.WriteCore([.. points], precisionXY, precisionZ, default, boundingBox, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public void Write(IEnumerable<PointM> points) => this.Write(points, default);

    /// <inheritdoc cref="Write(IEnumerable{PointM})"/>
    public void Write(IEnumerable<PointM> points, bool boundingBox) => this.Write(points, default, default, boundingBox);

    /// <inheritdoc cref="Write(IEnumerable{PointM})"/>
    public void Write(IEnumerable<PointM> points, int precisionXY, int precisionM, bool boundingBox = false) => this.WriteCore([.. points], precisionXY, default, precisionM, boundingBox, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public void Write(IEnumerable<PointZM> points) => this.Write(points, default);

    /// <inheritdoc cref="Write(IEnumerable{PointZM})"/>
    public void Write(IEnumerable<PointZM> points, bool boundingBox) => this.Write(points, default, default, default, boundingBox);

    /// <inheritdoc cref="Write(IEnumerable{PointZM})"/>
    public void Write(IEnumerable<PointZM> points, int precisionXY, int precisionZ, int precisionM, bool boundingBox = false) => this.WriteCore([.. points], precisionXY, precisionZ, precisionM, boundingBox, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public void Write(Polyline<Point> polyline) => this.Write(polyline, boundingBox: default);

    /// <inheritdoc cref="Write(Polyline{Point})"/>
    public void Write(Polyline<Point> points, bool boundingBox) => this.Write(points, default, boundingBox);

    /// <inheritdoc cref="Write(Polyline{Point})"/>
    public void Write(Polyline<Point> points, int precision, bool boundingBox = false) => this.WriteCore(points, precision, default, default, boundingBox, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public void Write(Polyline<PointZ> polyline) => this.Write(polyline, boundingBox: default);

    /// <inheritdoc cref="Write(Polyline{PointZ})"/>
    public void Write(Polyline<PointZ> polyline, bool boundingBox) => this.Write(polyline, default, default, boundingBox);

    /// <inheritdoc cref="Write(Polyline{PointZ})"/>
    public void Write(Polyline<PointZ> polyline, int precisionXY, int precisionZ, bool boundingBox = false) => this.WriteCore(polyline, precisionXY, precisionZ, default, boundingBox, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public void Write(Polyline<PointM> polyline) => this.Write(polyline, boundingBox: default);

    /// <inheritdoc cref="Write(Polyline{PointM})"/>
    public void Write(Polyline<PointM> polyline, bool boundingBox) => this.Write(polyline, default, default, boundingBox);

    /// <inheritdoc cref="Write(Polyline{PointM})"/>
    public void Write(Polyline<PointM> polyline, int precisionXY, int precisionM, bool boundingBox = false) => this.WriteCore(polyline, precisionXY, default, precisionM, boundingBox, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public void Write(Polyline<PointZM> polyline) => this.Write(polyline, boundingBox: default);

    /// <inheritdoc cref="Write(Polyline{PointZM})"/>
    public void Write(Polyline<PointZM> polyline, bool boundingBox) => this.Write(polyline, default, default, default, boundingBox);

    /// <inheritdoc cref="Write(Polyline{PointZM})"/>
    public void Write(Polyline<PointZM> polyline, int precisionXY, int precisionZ, int precisionM, bool boundingBox = false) => this.WriteCore(polyline, precisionXY, precisionZ, precisionM, boundingBox, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polyline<Point>> polylines) => this.Write(polylines, default);

    /// <inheritdoc cref="Write(IEnumerable{Polyline{Point}})"/>
    public void Write(IEnumerable<Polyline<Point>> polylines, bool boundingBox) => this.Write(polylines, default, boundingBox);

    /// <inheritdoc cref="Write(IEnumerable{Polyline{Point}})"/>
    public void Write(IEnumerable<Polyline<Point>> polylines, int precision, bool boundingBox = false) => this.WriteCore([.. polylines], precision, default, default, boundingBox, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polyline<PointZ>> polylines) => this.Write(polylines, default);

    /// <inheritdoc cref="Write(IEnumerable{Polyline{PointZ}})"/>
    public void Write(IEnumerable<Polyline<PointZ>> polylines, bool boundingBox) => this.Write(polylines, default, default, boundingBox);

    /// <inheritdoc cref="Write(IEnumerable{Polyline{PointZ}})"/>
    public void Write(IEnumerable<Polyline<PointZ>> polylines, int precisionXY, int precisionZ, bool boundingBox = false) => this.WriteCore([.. polylines], precisionXY, precisionZ, default, boundingBox, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polyline<PointM>> polylines) => this.Write(polylines, default);

    /// <inheritdoc cref="Write(IEnumerable{Polyline{PointM}})"/>
    public void Write(IEnumerable<Polyline<PointM>> polylines, bool boundingBox) => this.Write(polylines, default, default, boundingBox);

    /// <inheritdoc cref="Write(IEnumerable{Polyline{PointM}})"/>
    public void Write(IEnumerable<Polyline<PointM>> polylines, int precisionXY, int precisionM, bool boundingBox = false) => this.WriteCore([.. polylines], precisionXY, default, precisionM, boundingBox, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polyline<PointZM>> polylines) => this.Write(polylines, default);

    /// <inheritdoc cref="Write(IEnumerable{Polyline{PointZM}})"/>
    public void Write(IEnumerable<Polyline<PointZM>> polylines, bool boundingBox) => this.Write(polylines, default, default, default, boundingBox);

    /// <inheritdoc cref="Write(IEnumerable{Polyline{PointZM}})"/>
    public void Write(IEnumerable<Polyline<PointZM>> polylines, int precisionXY, int precisionZ, int precisionM, bool boundingBox = false) =>
        this.WriteCore([.. polylines], precisionXY, precisionZ, precisionM, boundingBox, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public void Write(Polygon<Point> polygon) => this.Write(polygon, boundingBox: default);

    /// <inheritdoc cref="Write(Polygon{Point})"/>
    public void Write(Polygon<Point> polygon, bool boundingBox) => this.Write(polygon, default, boundingBox);

    /// <inheritdoc cref="Write(Polygon{Point})"/>
    public void Write(Polygon<Point> polygon, int precision, bool boundingBox = false) => this.WriteCore(polygon, precision, default, default, boundingBox, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public void Write(Polygon<PointZ> polygon) => this.Write(polygon, boundingBox: default);

    /// <inheritdoc cref="Write(Polygon{PointZ})"/>
    public void Write(Polygon<PointZ> polygon, bool boundingBox) => this.Write(polygon, default, default, boundingBox);

    /// <inheritdoc cref="Write(Polygon{PointZ})"/>
    public void Write(Polygon<PointZ> polygon, int precisionXY, int precisionZ, bool boundingBox = false) => this.WriteCore(polygon, precisionXY, precisionZ, default, boundingBox, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public void Write(Polygon<PointM> polygon) => this.Write(polygon, boundingBox: default);

    /// <inheritdoc cref="Write(Polygon{PointM})"/>
    public void Write(Polygon<PointM> polygon, bool boundingBox) => this.Write(polygon, default, default, boundingBox);

    /// <inheritdoc cref="Write(Polygon{PointM})"/>
    public void Write(Polygon<PointM> polygon, int precisionXY, int precisionM, bool boundingBox = false) => this.WriteCore(polygon, precisionXY, default, precisionM, boundingBox, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public void Write(Polygon<PointZM> polygon) => this.Write(polygon, boundingBox: default);

    /// <inheritdoc cref="Write(Polygon{PointZM})"/>
    public void Write(Polygon<PointZM> polygon, bool boundingBox) => this.Write(polygon, default, default, default, boundingBox);

    /// <inheritdoc cref="Write(Polygon{PointZM})"/>
    public void Write(Polygon<PointZM> polygon, int precisionXY, int precisionZ, int precisionM, bool boundingBox = false) => this.WriteCore(polygon, precisionXY, precisionZ, precisionM, boundingBox, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polygon<Point>> polygons) => this.Write(polygons, default);

    /// <inheritdoc cref="Write(IEnumerable{Polygon{Point}})"/>
    public void Write(IEnumerable<Polygon<Point>> polygons, bool boundingBox) => this.Write(polygons, default, boundingBox);

    /// <inheritdoc cref="Write(IEnumerable{Polygon{Point}})"/>
    public void Write(IEnumerable<Polygon<Point>> polygons, int precision, bool boundingBox = false) => this.WriteCore([.. polygons], precision, default, default, boundingBox, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polygon<PointZ>> polygons) => this.Write(polygons, default);

    /// <inheritdoc cref="Write(IEnumerable{Polygon{PointZ}})"/>
    public void Write(IEnumerable<Polygon<PointZ>> polygons, bool boundingBox) => this.Write(polygons, default, default, boundingBox);

    /// <inheritdoc cref="Write(IEnumerable{Polygon{PointZ}})"/>
    public void Write(IEnumerable<Polygon<PointZ>> polygons, int precisionXY, int precisionZ, bool boundingBox = false) => this.WriteCore([.. polygons], precisionXY, precisionZ, default, boundingBox, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polygon<PointM>> polygons) => this.Write(polygons, default);

    /// <inheritdoc cref="Write(IEnumerable{Polygon{PointM}})"/>
    public void Write(IEnumerable<Polygon<PointM>> polygons, bool boundingBox) => this.Write(polygons, default, default, boundingBox);

    /// <inheritdoc cref="Write(IEnumerable{Polygon{PointM}})"/>
    public void Write(IEnumerable<Polygon<PointM>> polygons, int precisionXY, int precisionM, bool boundingBox = false) => this.WriteCore([.. polygons], precisionXY, default, precisionM, boundingBox, GetXY, GetZ, GetM);

    /// <inheritdoc/>
    public void Write(params IEnumerable<Polygon<PointZM>> polygons) => this.Write(polygons, default);

    /// <inheritdoc cref="Write(IEnumerable{Polygon{PointZM}})"/>
    public void Write(IEnumerable<Polygon<PointZM>> polygons, bool boundingBox) => this.Write(polygons, default, default, default, boundingBox);

    /// <inheritdoc cref="Write(IEnumerable{Polygon{PointZM}})"/>
    public void Write(IEnumerable<Polygon<PointZM>> polygons, int precisionXY, int precisionZ, int precisionM, bool boundingBox = false) => this.WriteCore([.. polygons], precisionXY, precisionZ, precisionM, boundingBox, GetXY, GetZ, GetM);

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

    private static bool IsNullOrEmpty(Point point) => point is { IsEmpty: true };

    private static bool IsNullOrEmpty(PointZ point) => point is { IsEmpty: true };

    private static bool IsNullOrEmpty(PointM point) => point is { IsEmpty: true };

    private static bool IsNullOrEmpty(PointZM point) => point is { IsEmpty: true };

    private static (double X, double Y) GetXY(Point point) => (point.X, point.Y);

    private static (double X, double Y) GetXY(PointZ point) => (point.X, point.Y);

    private static (double X, double Y) GetXY(PointM point) => (point.X, point.Y);

    private static (double X, double Y) GetXY(PointZM point) => (point.X, point.Y);

    private static double GetZ(Point point) => throw new InvalidGeometryTypeException();

    private static double GetZ(PointZ point) => point.Z;

    private static double GetZ(PointM point) => throw new InvalidGeometryTypeException();

    private static double GetZ(PointZM point) => point.Z;

    private static double GetM(Point point) => throw new InvalidGeometryTypeException();

    private static double GetM(PointZ point) => throw new InvalidGeometryTypeException();

    private static double GetM(PointM point) => point.Measurement;

    private static double GetM(PointZM point) => point.Measurement;

    private void WriteCore<T>(T point, int precisionXY, int? precisionZ, int? precisionM, bool boundingBox, Func<T, (double X, double Y)> getXY, Func<T, double> getZ, Func<T, double> getM, Func<T?, bool> isNullOrEmpty)
    {
        var header = new TinyWkbRecordHeader(
            TinyWkbGeometryType.Point,
            precisionXY,
            hasEmptyGeometry: isNullOrEmpty(point),
            hasBoundingBox: boundingBox,
            precisionZ: precisionZ,
            precisionM: precisionM);
        var internalWriter = Writer.Initialise(writer, header);
        if (isNullOrEmpty(point))
        {
            return;
        }

        internalWriter.WritePoint(point, getXY, getZ, getM);
    }

    private void WriteCore<T>(IReadOnlyCollection<T> points, int precisionXY, int? precisionZ, int? precisionM, bool boundingBox, Func<T, (double X, double Y)> getXY, Func<T, double> getZ, Func<T, double> getM)
    {
        var count = points.Count;
        var header = new TinyWkbRecordHeader(
            TinyWkbGeometryType.MultiPoint,
            precisionXY,
            hasEmptyGeometry: count is 0,
            hasBoundingBox: boundingBox,
            precisionZ: precisionZ,
            precisionM: precisionM);
        Writer
            .Initialise(writer, header)
            .WritePoints(points, count, getXY, getZ, getM);
    }

    private void WriteCore<T>(Polyline<T> points, int precisionXY, int? precisionZ, int? precisionM, bool boundingBox, Func<T, (double X, double Y)> getXY, Func<T, double> getZ, Func<T, double> getM)
    {
        var count = points.Count;
        var header = new TinyWkbRecordHeader(
            TinyWkbGeometryType.Linestring,
            precisionXY,
            hasEmptyGeometry: count is 0,
            hasBoundingBox: boundingBox,
            precisionZ: precisionZ,
            precisionM: precisionM);
        Writer
            .Initialise(writer, header)
            .WritePoints(points, count, getXY, getZ, getM);
    }

    private void WriteCore<T>(IReadOnlyCollection<Polyline<T>> lines, int precisionXY, int? precisionZ, int? precisionM, bool boundingBox, Func<T, (double X, double Y)> getXY, Func<T, double> getZ, Func<T, double> getM)
    {
        var count = lines.Count;
        var header = new TinyWkbRecordHeader(
            TinyWkbGeometryType.MultiLinestring,
            precisionXY,
            hasEmptyGeometry: count is 0,
            hasBoundingBox: boundingBox,
            precisionZ: precisionZ,
            precisionM: precisionM);
        Writer
            .Initialise(writer, header)
            .WritePointCollections(lines, count, getXY, getZ, getM);
    }

    private void WriteCore<T>(Polygon<T> polygon, int precisionXY, int? precisionZ, int? precisionM, bool boundingBox, Func<T, (double X, double Y)> getXY, Func<T, double> getZ, Func<T, double> getM)
        where T : struct
    {
        var count = polygon.Count;
        var header = new TinyWkbRecordHeader(
            TinyWkbGeometryType.Polygon,
            precisionXY,
            hasEmptyGeometry: count is 0,
            hasBoundingBox: boundingBox,
            precisionZ: precisionZ,
            precisionM: precisionM);
        Writer
            .Initialise(writer, header)
            .WritePointCollections(polygon, count, getXY, getZ, getM);
    }

    private void WriteCore<T>(IReadOnlyCollection<Polygon<T>> polygons, int precisionXY, int? precisionZ, int? precisionM, bool boundingBox, Func<T, (double X, double Y)> getXY, Func<T, double> getZ, Func<T, double> getM)
        where T : struct
    {
        var count = polygons.Count;
        var header = new TinyWkbRecordHeader(
            TinyWkbGeometryType.MultiPolygon,
            precisionXY,
            hasEmptyGeometry: count is 0,
            hasBoundingBox: boundingBox,
            precisionZ: precisionZ,
            precisionM: precisionM);
        var internalWriter = Writer.Initialise(writer, header);
        if (count is 0)
        {
            return;
        }

        internalWriter.WriteCount((ulong)count);

        foreach (var polygon in polygons)
        {
            internalWriter.WritePointCollections(polygon, polygon.Count, getXY, getZ, getM);
        }
    }

    private sealed class Writer
    {
        private readonly BinaryWriter writer;

        private readonly double scaleXY;

        private readonly bool hasZ;
        private readonly double scaleZ;

        private readonly bool hasM;
        private readonly double scaleM;

        private long previousX;
        private long previousY;
        private long previousZ;
        private long previousM;

        private Writer(BinaryWriter writer, TinyWkbRecordHeader header)
        {
            this.writer = writer;

            var precisionXY = header.PrecisionXY;
            this.scaleXY = precisionXY.Scale();

            this.hasZ = header.HasZ;
            var precisionZ = this.hasZ ? header.PrecisionZ : 0;
            this.scaleZ = precisionZ.Scale();

            this.hasM = header.HasM;
            var precisionM = this.hasM ? header.PrecisionM : 0;
            this.scaleM = precisionM.Scale();
        }

        public static Writer Initialise(BinaryWriter writer, TinyWkbRecordHeader header)
        {
            var returnWriter = new Writer(writer, header);
            TinyWkbRecordHeader.Write(writer, header);
            return returnWriter;
        }

        public void WritePoint<T>(T point, Func<T, (double X, double Y)> getXY, Func<T, double> getZ, Func<T, double> getM)
        {
            var (x, y) = getXY(point);
            this.writer.Write(VarIntBitConverter.GetBytes(Encode(x, this.scaleXY, ref this.previousX)));
            this.writer.Write(VarIntBitConverter.GetBytes(Encode(y, this.scaleXY, ref this.previousY)));

            if (this.hasZ)
            {
                this.writer.Write(VarIntBitConverter.GetBytes(Encode(getZ(point), this.scaleZ, ref this.previousZ)));
            }

            if (this.hasM)
            {
                this.writer.Write(VarIntBitConverter.GetBytes(Encode(getM(point), this.scaleM, ref this.previousM)));
            }

            static long Encode(double value, double scale, ref long lastScaledValue)
            {
                var longValue = (long)Round(value * scale, 0, MidpointRounding.AwayFromZero);
                var encodedValue = longValue - lastScaledValue;
                lastScaledValue = longValue;
                return encodedValue;
            }
        }

        public void WritePoints<T>(IEnumerable<T>? points, int count, Func<T, (double X, double Y)> getXY, Func<T, double> getZ, Func<T, double> getM)
        {
            if (count is 0 || points is null)
            {
                return;
            }

            this.WriteCount((ulong)count);

            foreach (var point in points)
            {
                this.WritePoint(point, getXY, getZ, getM);
            }
        }

        public void WritePointCollections<T>(IEnumerable<IReadOnlyCollection<T>>? pointCollections, int count, Func<T, (double X, double Y)> getXY, Func<T, double> getZ, Func<T, double> getM)
        {
            if (count is 0 || pointCollections is null)
            {
                return;
            }

            this.WriteCount((ulong)count);

            foreach (var pointCollection in pointCollections)
            {
                this.WritePoints(pointCollection, pointCollection.Count, getXY, getZ, getM);
            }
        }

        public void WriteCount(ulong count) => this.writer.Write(VarIntBitConverter.GetBytes(count));
    }
}