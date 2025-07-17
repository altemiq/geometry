// -----------------------------------------------------------------------
// <copyright file="BinaryGeometryWriter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data.Common;

/// <summary>
/// The abstract binary writer.
/// </summary>
public abstract class BinaryGeometryWriter : IGeometryWriter, IDisposable
{
    private bool disposedValue;

    /// <summary>
    /// Initialises a new instance of the <see cref="BinaryGeometryWriter"/> class.
    /// </summary>
    /// <param name="bytes">The byte array to write to.</param>
    protected BinaryGeometryWriter(byte[] bytes)
        : this(bytes, BitConverter.IsLittleEndian)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="BinaryGeometryWriter"/> class.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    protected BinaryGeometryWriter(Stream stream)
        : this(stream, BitConverter.IsLittleEndian)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="BinaryGeometryWriter"/> class.
    /// </summary>
    /// <param name="bytes">The byte array to write to.</param>
    /// <param name="littleEndian">Set to <see langword="true"/> to treat the data as little endian.</param>
    protected BinaryGeometryWriter(byte[] bytes, bool littleEndian)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(bytes);
#else
        if (bytes is null)
        {
            throw new ArgumentNullException(nameof(bytes));
        }
#endif

        System.Diagnostics.Contracts.Contract.EndContractBlock();
        this.BaseStream = new MemoryStream(bytes, writable: true);
        this.IsLittleEndian = littleEndian;
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="BinaryGeometryWriter"/> class.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="littleEndian">Set to <see langword="true"/> to treat the data as little endian.</param>
    protected BinaryGeometryWriter(Stream stream, bool littleEndian)
    {
        this.BaseStream = stream ?? throw new ArgumentNullException(nameof(stream));
        this.IsLittleEndian = littleEndian;
    }

    /// <summary>
    /// Gets the base stream.
    /// </summary>
    public Stream BaseStream { get; }

    /// <summary>
    /// Gets a value indicating whether this instance is little endian.
    /// </summary>
    protected bool IsLittleEndian { get; }

    /// <inheritdoc/>
    public abstract void Write(Point point);

    /// <inheritdoc/>
    public abstract void Write(PointZ point);

    /// <inheritdoc/>
    public abstract void Write(PointM point);

    /// <inheritdoc/>
    public abstract void Write(PointZM point);

    /// <inheritdoc/>
    public abstract void Write(IEnumerable<Point> points);

    /// <inheritdoc/>
    public abstract void Write(IEnumerable<PointZ> points);

    /// <inheritdoc/>
    public abstract void Write(IEnumerable<PointM> points);

    /// <inheritdoc/>
    public abstract void Write(IEnumerable<PointZM> points);

    /// <inheritdoc/>
    public abstract void Write(Polyline<Point> polyline);

    /// <inheritdoc/>
    public abstract void Write(Polyline<PointZ> polyline);

    /// <inheritdoc/>
    public abstract void Write(Polyline<PointM> polyline);

    /// <inheritdoc/>
    public abstract void Write(Polyline<PointZM> polyline);

    /// <inheritdoc/>
    public abstract void Write(params IEnumerable<Polyline<Point>> polylines);

    /// <inheritdoc/>
    public abstract void Write(params IEnumerable<Polyline<PointZ>> polylines);

    /// <inheritdoc/>
    public abstract void Write(params IEnumerable<Polyline<PointM>> polylines);

    /// <inheritdoc/>
    public abstract void Write(params IEnumerable<Polyline<PointZM>> polylines);

    /// <inheritdoc/>
    public abstract void Write(Polygon<Point> polygon);

    /// <inheritdoc/>
    public abstract void Write(Polygon<PointZ> polygon);

    /// <inheritdoc/>
    public abstract void Write(Polygon<PointM> polygon);

    /// <inheritdoc/>
    public abstract void Write(Polygon<PointZM> polygon);

    /// <inheritdoc/>
    public abstract void Write(params IEnumerable<Polygon<Point>> polygons);

    /// <inheritdoc/>
    public abstract void Write(params IEnumerable<Polygon<PointZ>> polygons);

    /// <inheritdoc/>
    public abstract void Write(params IEnumerable<Polygon<PointM>> polygons);

    /// <inheritdoc/>
    public abstract void Write(params IEnumerable<Polygon<PointZM>> polygons);

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
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.BaseStream.Dispose();
            }

            this.disposedValue = true;
        }
    }
}