// -----------------------------------------------------------------------
// <copyright file="WkbRecord.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

using static Altemiq.Buffers.Binary.WkbPrimitives;

/// <summary>
/// Represents an implementation of <see cref="Data.IGeometryRecord"/> that reads Well-Known Binary.
/// </summary>
public class WkbRecord : Data.Common.BinaryGeometryRecord
{
    /// <summary>
    /// Initialises a new instance of the <see cref="WkbRecord"/> class.
    /// </summary>
    /// <param name="bytes">The byte array to read from.</param>
    public WkbRecord(byte[] bytes)
        : base(bytes)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="WkbRecord"/> class.
    /// </summary>
    /// <param name="bytes">The byte array to read from.</param>
    /// <param name="index">The index in <paramref name="bytes"/> at which the stream begins.</param>
    /// <param name="count">The length of the stream in bytes.</param>
    public WkbRecord(byte[] bytes, int index, int count)
        : base(bytes, index, count)
    {
    }

    /// <inheritdoc />
    public override Point GetPoint() => ReadPoint(this.AsSpan());

    /// <inheritdoc />
    public override PointZ GetPointZ() => ReadPointZ(this.AsSpan());

    /// <inheritdoc />
    public override PointM GetPointM() => ReadPointM(this.AsSpan());

    /// <inheritdoc />
    public override PointZM GetPointZM() => ReadPointZM(this.AsSpan());

    /// <inheritdoc/>
    public override IMultiGeometry<Point> GetMultiPoint() => ReadMultiPoint(this.AsSpan());

    /// <inheritdoc/>
    public override IMultiGeometry<PointZ> GetMultiPointZ() => ReadMultiPointZ(this.AsSpan());

    /// <inheritdoc/>
    public override IMultiGeometry<PointM> GetMultiPointM() => ReadMultiPointM(this.AsSpan());

    /// <inheritdoc/>
    public override IMultiGeometry<PointZM> GetMultiPointZM() => ReadMultiPointZM(this.AsSpan());

    /// <inheritdoc />
    public override Polyline GetLineString() => ReadLineString(this.AsSpan());

    /// <inheritdoc />
    public override PolylineZ GetLineStringZ() => ReadLineStringZ(this.AsSpan());

    /// <inheritdoc />
    public override PolylineM GetLineStringM() => ReadLineStringM(this.AsSpan());

    /// <inheritdoc />
    public override PolylineZM GetLineStringZM() => ReadLineStringZM(this.AsSpan());

    /// <inheritdoc />
    public override IMultiGeometry<Polyline> GetMultiLineString() => ReadMultiLineString(this.AsSpan());

    /// <inheritdoc />
    public override IMultiGeometry<PolylineZ> GetMultiLineStringZ() => ReadMultiLineStringZ(this.AsSpan());

    /// <inheritdoc />
    public override IMultiGeometry<PolylineM> GetMultiLineStringM() => ReadMultiLineStringM(this.AsSpan());

    /// <inheritdoc />
    public override IMultiGeometry<PolylineZM> GetMultiLineStringZM() => ReadMultiLineStringZM(this.AsSpan());

    /// <inheritdoc />
    public override Polygon GetPolygon() => ReadPolygon(this.AsSpan());

    /// <inheritdoc />
    public override PolygonZ GetPolygonZ() => ReadPolygonZ(this.AsSpan());

    /// <inheritdoc />
    public override PolygonM GetPolygonM() => ReadPolygonM(this.AsSpan());

    /// <inheritdoc />
    public override PolygonZM GetPolygonZM() => ReadPolygonZM(this.AsSpan());

    /// <inheritdoc />
    public override IMultiGeometry<Polygon> GetMultiPolygon() => ReadMultiPolygon(this.AsSpan());

    /// <inheritdoc />
    public override IMultiGeometry<PolygonZ> GetMultiPolygonZ() => ReadMultiPolygonZ(this.AsSpan());

    /// <inheritdoc />
    public override IMultiGeometry<PolygonM> GetMultiPolygonM() => ReadMultiPolygonM(this.AsSpan());

    /// <inheritdoc />
    public override IMultiGeometry<PolygonZM> GetMultiPolygonZM() => ReadMultiPolygonZM(this.AsSpan());

    /// <inheritdoc/>
    public override IGeometry GetGeometry() => ReadGeometry(this.AsSpan());

    /// <inheritdoc />
    public override bool IsNull()
    {
        var span = this.AsSpan();
        return span.Length <= 5;
    }
}