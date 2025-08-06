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
    public Point GetPoint() => Altemiq.Buffers.Binary.TwkbPrimitives.ReadPoint(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public PointZ GetPointZ() => Altemiq.Buffers.Binary.TwkbPrimitives.ReadPointZ(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public PointM GetPointM() => Altemiq.Buffers.Binary.TwkbPrimitives.ReadPointM(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public PointZM GetPointZM() => Altemiq.Buffers.Binary.TwkbPrimitives.ReadPointZM(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public IReadOnlyCollection<Point> GetMultiPoint() => Altemiq.Buffers.Binary.TwkbPrimitives.ReadMultiPoint(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public IReadOnlyCollection<PointZ> GetMultiPointZ() => Altemiq.Buffers.Binary.TwkbPrimitives.ReadMultiPointZ(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public IReadOnlyCollection<PointM> GetMultiPointM() => Altemiq.Buffers.Binary.TwkbPrimitives.ReadMultiPointM(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public IReadOnlyCollection<PointZM> GetMultiPointZM() => Altemiq.Buffers.Binary.TwkbPrimitives.ReadMultiPointZM(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public Polyline GetLineString() => Altemiq.Buffers.Binary.TwkbPrimitives.ReadLineString(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public PolylineZ GetLineStringZ() => Altemiq.Buffers.Binary.TwkbPrimitives.ReadLineStringZ(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public PolylineM GetLineStringM() => Altemiq.Buffers.Binary.TwkbPrimitives.ReadLineStringM(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public PolylineZM GetLineStringZM() => Altemiq.Buffers.Binary.TwkbPrimitives.ReadLineStringZM(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public IReadOnlyCollection<Polyline> GetMultiLineString() => Altemiq.Buffers.Binary.TwkbPrimitives.ReadMultiLineString(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public IReadOnlyCollection<PolylineZ> GetMultiLineStringZ() => Altemiq.Buffers.Binary.TwkbPrimitives.ReadMultiLineStringZ(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public IReadOnlyCollection<PolylineM> GetMultiLineStringM() => Altemiq.Buffers.Binary.TwkbPrimitives.ReadMultiLineStringM(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public IReadOnlyCollection<PolylineZM> GetMultiLineStringZM() => Altemiq.Buffers.Binary.TwkbPrimitives.ReadMultiLineStringZM(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public Polygon GetPolygon() => Altemiq.Buffers.Binary.TwkbPrimitives.ReadPolygon(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public PolygonZ GetPolygonZ() => Altemiq.Buffers.Binary.TwkbPrimitives.ReadPolygonZ(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public PolygonM GetPolygonM() => Altemiq.Buffers.Binary.TwkbPrimitives.ReadPolygonM(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public PolygonZM GetPolygonZM() => Altemiq.Buffers.Binary.TwkbPrimitives.ReadPolygonZM(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public IReadOnlyCollection<Polygon> GetMultiPolygon() => Altemiq.Buffers.Binary.TwkbPrimitives.ReadMultiPolygon(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public IReadOnlyCollection<PolygonZ> GetMultiPolygonZ() => Altemiq.Buffers.Binary.TwkbPrimitives.ReadMultiPolygonZ(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public IReadOnlyCollection<PolygonM> GetMultiPolygonM() => Altemiq.Buffers.Binary.TwkbPrimitives.ReadMultiPolygonM(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public IReadOnlyCollection<PolygonZM> GetMultiPolygonZM() => Altemiq.Buffers.Binary.TwkbPrimitives.ReadMultiPolygonZM(this.ReadStreamAsSpan());

    /// <inheritdoc/>
    public object GetGeometry() => Altemiq.Buffers.Binary.TwkbPrimitives.ReadGeometry(this.ReadStreamAsSpan());

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