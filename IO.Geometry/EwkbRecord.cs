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
    public override Point GetPoint() => Buffers.Binary.EwkbPrimitives.ReadPoint(this.AsSpan());

    /// <inheritdoc />
    public override PointZ GetPointZ() => Buffers.Binary.EwkbPrimitives.ReadPointZ(this.AsSpan());

    /// <inheritdoc />
    public override PointM GetPointM() => Buffers.Binary.EwkbPrimitives.ReadPointM(this.AsSpan());

    /// <inheritdoc />
    public override PointZM GetPointZM() => Buffers.Binary.EwkbPrimitives.ReadPointZM(this.AsSpan());

    /// <inheritdoc/>
    public override IMultiGeometry<Point> GetMultiPoint() => Buffers.Binary.EwkbPrimitives.ReadMultiPoint(this.AsSpan());

    /// <inheritdoc/>
    public override IMultiGeometry<PointZ> GetMultiPointZ() => Buffers.Binary.EwkbPrimitives.ReadMultiPointZ(this.AsSpan());

    /// <inheritdoc/>
    public override IMultiGeometry<PointM> GetMultiPointM() => Buffers.Binary.EwkbPrimitives.ReadMultiPointM(this.AsSpan());

    /// <inheritdoc/>
    public override IMultiGeometry<PointZM> GetMultiPointZM() => Buffers.Binary.EwkbPrimitives.ReadMultiPointZM(this.AsSpan());

    /// <inheritdoc />
    public override Polyline GetLineString() => Buffers.Binary.EwkbPrimitives.ReadLineString(this.AsSpan());

    /// <inheritdoc />
    public override PolylineZ GetLineStringZ() => Buffers.Binary.EwkbPrimitives.ReadLineStringZ(this.AsSpan());

    /// <inheritdoc />
    public override PolylineM GetLineStringM() => Buffers.Binary.EwkbPrimitives.ReadLineStringM(this.AsSpan());

    /// <inheritdoc />
    public override PolylineZM GetLineStringZM() => Buffers.Binary.EwkbPrimitives.ReadLineStringZM(this.AsSpan());

    /// <inheritdoc />
    public override IMultiGeometry<Polyline> GetMultiLineString() => Buffers.Binary.EwkbPrimitives.ReadMultiLineString(this.AsSpan());

    /// <inheritdoc />
    public override IMultiGeometry<PolylineZ> GetMultiLineStringZ() => Buffers.Binary.EwkbPrimitives.ReadMultiLineStringZ(this.AsSpan());

    /// <inheritdoc />
    public override IMultiGeometry<PolylineM> GetMultiLineStringM() => Buffers.Binary.EwkbPrimitives.ReadMultiLineStringM(this.AsSpan());

    /// <inheritdoc />
    public override IMultiGeometry<PolylineZM> GetMultiLineStringZM() => Buffers.Binary.EwkbPrimitives.ReadMultiLineStringZM(this.AsSpan());

    /// <inheritdoc />
    public override Polygon GetPolygon() => Buffers.Binary.EwkbPrimitives.ReadPolygon(this.AsSpan());

    /// <inheritdoc />
    public override PolygonZ GetPolygonZ() => Buffers.Binary.EwkbPrimitives.ReadPolygonZ(this.AsSpan());

    /// <inheritdoc />
    public override PolygonM GetPolygonM() => Buffers.Binary.EwkbPrimitives.ReadPolygonM(this.AsSpan());

    /// <inheritdoc />
    public override PolygonZM GetPolygonZM() => Buffers.Binary.EwkbPrimitives.ReadPolygonZM(this.AsSpan());

    /// <inheritdoc />
    public override IMultiGeometry<Polygon> GetMultiPolygon() => Buffers.Binary.EwkbPrimitives.ReadMultiPolygon(this.AsSpan());

    /// <inheritdoc />
    public override IMultiGeometry<PolygonZ> GetMultiPolygonZ() => Buffers.Binary.EwkbPrimitives.ReadMultiPolygonZ(this.AsSpan());

    /// <inheritdoc />
    public override IMultiGeometry<PolygonM> GetMultiPolygonM() => Buffers.Binary.EwkbPrimitives.ReadMultiPolygonM(this.AsSpan());

    /// <inheritdoc />
    public override IMultiGeometry<PolygonZM> GetMultiPolygonZM() => Buffers.Binary.EwkbPrimitives.ReadMultiPolygonZM(this.AsSpan());

    /// <inheritdoc/>
    public override IGeometry GetGeometry() => Buffers.Binary.EwkbPrimitives.ReadGeometry(this.AsSpan());

    /// <inheritdoc/>
    public int GetSrid() => Buffers.Binary.EwkbPrimitives.ReadSrid(this.AsSpan());

    /// <inheritdoc />
    public override bool IsNull()
    {
        var span = this.AsSpan();
        return span.Length <= 9;
    }
}