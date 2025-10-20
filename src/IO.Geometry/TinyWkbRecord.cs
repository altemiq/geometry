// -----------------------------------------------------------------------
// <copyright file="TinyWkbRecord.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

/// <summary>
/// Represents an implementation of <see cref="Data.IGeometryRecord"/> that reads Tiny Well-Known Binary.
/// </summary>
public class TinyWkbRecord : Data.IGeometryRecord, IDisposable
{
    private readonly Stream stream;

    /// <summary>
    /// Initialises a new instance of the <see cref="TinyWkbRecord"/> class.
    /// </summary>
    /// <param name="bytes">The byte array to read from.</param>
    public TinyWkbRecord(byte[] bytes)
        : this(new MemoryStream(bytes ?? throw new ArgumentNullException(nameof(bytes)), 0, bytes.Length, writable: false, publiclyVisible: true))
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="TinyWkbRecord"/> class.
    /// </summary>
    /// <param name="stream">The stream to read from.</param>
    public TinyWkbRecord(Stream stream)
    {
        this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
        System.Diagnostics.Contracts.Contract.EndContractBlock();
    }

    /// <inheritdoc/>
    public Point GetPoint() => Buffers.Binary.TwkbPrimitives.ReadPoint(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public PointZ GetPointZ() => Buffers.Binary.TwkbPrimitives.ReadPointZ(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public PointM GetPointM() => Buffers.Binary.TwkbPrimitives.ReadPointM(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public PointZM GetPointZM() => Buffers.Binary.TwkbPrimitives.ReadPointZM(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public IMultiGeometry<Point> GetMultiPoint() => Buffers.Binary.TwkbPrimitives.ReadMultiPoint(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public IMultiGeometry<PointZ> GetMultiPointZ() => Buffers.Binary.TwkbPrimitives.ReadMultiPointZ(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public IMultiGeometry<PointM> GetMultiPointM() => Buffers.Binary.TwkbPrimitives.ReadMultiPointM(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public IMultiGeometry<PointZM> GetMultiPointZM() => Buffers.Binary.TwkbPrimitives.ReadMultiPointZM(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public Polyline GetLineString() => Buffers.Binary.TwkbPrimitives.ReadLineString(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public PolylineZ GetLineStringZ() => Buffers.Binary.TwkbPrimitives.ReadLineStringZ(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public PolylineM GetLineStringM() => Buffers.Binary.TwkbPrimitives.ReadLineStringM(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public PolylineZM GetLineStringZM() => Buffers.Binary.TwkbPrimitives.ReadLineStringZM(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public IMultiGeometry<Polyline> GetMultiLineString() => Buffers.Binary.TwkbPrimitives.ReadMultiLineString(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public IMultiGeometry<PolylineZ> GetMultiLineStringZ() => Buffers.Binary.TwkbPrimitives.ReadMultiLineStringZ(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public IMultiGeometry<PolylineM> GetMultiLineStringM() => Buffers.Binary.TwkbPrimitives.ReadMultiLineStringM(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public IMultiGeometry<PolylineZM> GetMultiLineStringZM() => Buffers.Binary.TwkbPrimitives.ReadMultiLineStringZM(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public Polygon GetPolygon() => Buffers.Binary.TwkbPrimitives.ReadPolygon(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public PolygonZ GetPolygonZ() => Buffers.Binary.TwkbPrimitives.ReadPolygonZ(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public PolygonM GetPolygonM() => Buffers.Binary.TwkbPrimitives.ReadPolygonM(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public PolygonZM GetPolygonZM() => Buffers.Binary.TwkbPrimitives.ReadPolygonZM(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public IMultiGeometry<Polygon> GetMultiPolygon() => Buffers.Binary.TwkbPrimitives.ReadMultiPolygon(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public IMultiGeometry<PolygonZ> GetMultiPolygonZ() => Buffers.Binary.TwkbPrimitives.ReadMultiPolygonZ(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public IMultiGeometry<PolygonM> GetMultiPolygonM() => Buffers.Binary.TwkbPrimitives.ReadMultiPolygonM(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public IMultiGeometry<PolygonZM> GetMultiPolygonZM() => Buffers.Binary.TwkbPrimitives.ReadMultiPolygonZM(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public IGeometry GetGeometry() => Buffers.Binary.TwkbPrimitives.ReadGeometry(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public bool IsNull() => this.stream.Position >= this.stream.Length;

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
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
        if (disposing && this.stream is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    private ReadOnlySpan<byte> ReadStreamAsSpan()
    {
        if (this.stream is MemoryStream memoryStream)
        {
            return memoryStream.GetBuffer().AsSpan();
        }

        throw new InvalidOperationException();
    }
}