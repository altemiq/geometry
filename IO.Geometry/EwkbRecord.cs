// -----------------------------------------------------------------------
// <copyright file="EwkbRecord.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

/// <summary>
/// Represents an extended <see cref="WkbRecord"/>.
/// </summary>
public class EwkbRecord : Data.Common.BinaryGeometryRecord, Data.ISridGeometryRecord
{
    /// <summary>
    /// Initialises a new instance of the <see cref="EwkbRecord"/> class.
    /// </summary>
    /// <param name="bytes">The byte array to read from.</param>
    public EwkbRecord(byte[] bytes)
        : base(bytes)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="EwkbRecord"/> class.
    /// </summary>
    /// <param name="bytes">The byte array to read from.</param>
    /// <param name="index">The index in <paramref name="bytes"/> at which the stream begins.</param>
    /// <param name="count">The length of the stream in bytes.</param>
    public EwkbRecord(byte[] bytes, int index, int count)
        : base(bytes, index, count)
    {
    }

    /// <inheritdoc />
    public override Point GetPoint() => Altemiq.Buffers.Binary.EwkbPrimitives.ReadPoint(this.AsSpan());

    /// <inheritdoc />
    public override PointZ GetPointZ() => Altemiq.Buffers.Binary.EwkbPrimitives.ReadPointZ(this.AsSpan());

    /// <inheritdoc />
    public override PointM GetPointM() => Altemiq.Buffers.Binary.EwkbPrimitives.ReadPointM(this.AsSpan());

    /// <inheritdoc />
    public override PointZM GetPointZM() => Altemiq.Buffers.Binary.EwkbPrimitives.ReadPointZM(this.AsSpan());

    /// <inheritdoc/>
    public override IReadOnlyCollection<Point> GetMultiPoint() => Altemiq.Buffers.Binary.EwkbPrimitives.ReadMultiPoint(this.AsSpan());

    /// <inheritdoc/>
    public override IReadOnlyCollection<PointZ> GetMultiPointZ() => Altemiq.Buffers.Binary.EwkbPrimitives.ReadMultiPointZ(this.AsSpan());

    /// <inheritdoc/>
    public override IReadOnlyCollection<PointM> GetMultiPointM() => Altemiq.Buffers.Binary.EwkbPrimitives.ReadMultiPointM(this.AsSpan());

    /// <inheritdoc/>
    public override IReadOnlyCollection<PointZM> GetMultiPointZM() => Altemiq.Buffers.Binary.EwkbPrimitives.ReadMultiPointZM(this.AsSpan());

    /// <inheritdoc />
    public override Polyline GetLineString() => Altemiq.Buffers.Binary.EwkbPrimitives.ReadLineString(this.AsSpan());

    /// <inheritdoc />
    public override PolylineZ GetLineStringZ() => Altemiq.Buffers.Binary.EwkbPrimitives.ReadLineStringZ(this.AsSpan());

    /// <inheritdoc />
    public override PolylineM GetLineStringM() => Altemiq.Buffers.Binary.EwkbPrimitives.ReadLineStringM(this.AsSpan());

    /// <inheritdoc />
    public override PolylineZM GetLineStringZM() => Altemiq.Buffers.Binary.EwkbPrimitives.ReadLineStringZM(this.AsSpan());

    /// <inheritdoc />
    public override IReadOnlyCollection<Polyline> GetMultiLineString() => Altemiq.Buffers.Binary.EwkbPrimitives.ReadMultiLineString(this.AsSpan());

    /// <inheritdoc />
    public override IReadOnlyCollection<PolylineZ> GetMultiLineStringZ() => Altemiq.Buffers.Binary.EwkbPrimitives.ReadMultiLineStringZ(this.AsSpan());

    /// <inheritdoc />
    public override IReadOnlyCollection<PolylineM> GetMultiLineStringM() => Altemiq.Buffers.Binary.EwkbPrimitives.ReadMultiLineStringM(this.AsSpan());

    /// <inheritdoc />
    public override IReadOnlyCollection<PolylineZM> GetMultiLineStringZM() => Altemiq.Buffers.Binary.EwkbPrimitives.ReadMultiLineStringZM(this.AsSpan());

    /// <inheritdoc />
    public override Polygon GetPolygon() => Altemiq.Buffers.Binary.EwkbPrimitives.ReadPolygon(this.AsSpan());

    /// <inheritdoc />
    public override PolygonZ GetPolygonZ() => Altemiq.Buffers.Binary.EwkbPrimitives.ReadPolygonZ(this.AsSpan());

    /// <inheritdoc />
    public override PolygonM GetPolygonM() => Altemiq.Buffers.Binary.EwkbPrimitives.ReadPolygonM(this.AsSpan());

    /// <inheritdoc />
    public override PolygonZM GetPolygonZM() => Altemiq.Buffers.Binary.EwkbPrimitives.ReadPolygonZM(this.AsSpan());

    /// <inheritdoc />
    public override IReadOnlyCollection<Polygon> GetMultiPolygon() => Altemiq.Buffers.Binary.EwkbPrimitives.ReadMultiPolygon(this.AsSpan());

    /// <inheritdoc />
    public override IReadOnlyCollection<PolygonZ> GetMultiPolygonZ() => Altemiq.Buffers.Binary.EwkbPrimitives.ReadMultiPolygonZ(this.AsSpan());

    /// <inheritdoc />
    public override IReadOnlyCollection<PolygonM> GetMultiPolygonM() => Altemiq.Buffers.Binary.EwkbPrimitives.ReadMultiPolygonM(this.AsSpan());

    /// <inheritdoc />
    public override IReadOnlyCollection<PolygonZM> GetMultiPolygonZM() => Altemiq.Buffers.Binary.EwkbPrimitives.ReadMultiPolygonZM(this.AsSpan());

    /// <inheritdoc/>
    public override object? GetGeometry() => Altemiq.Buffers.Binary.EwkbPrimitives.ReadGeometry(this.AsSpan());

    /// <inheritdoc/>
    public int GetSrid() => Altemiq.Buffers.Binary.EwkbPrimitives.ReadSrid(this.AsSpan());

    /// <inheritdoc />
    public override bool IsNull()
    {
        var span = this.AsSpan();
        return span.Length <= 9;
    }
}