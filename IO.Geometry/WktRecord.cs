// -----------------------------------------------------------------------
// <copyright file="WktRecord.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

/// <summary>
/// Represents an implementation of <see cref="Data.IGeometryRecord"/> that reads Well-Known Text.
/// </summary>
/// <remarks>
/// Initialises a new instance of the <see cref="WktRecord"/> class.
/// </remarks>
/// <param name="wkt">The well-known text.</param>
public class WktRecord(string wkt) : Data.IGeometryRecord
{
    /// <summary>
    /// The try parse delegate.
    /// </summary>
    /// <typeparam name="T">The type of object to parse.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="value">The value.</param>
    /// <param name="bytesConsumed">The bytes consumed.</param>
    /// <returns>The value from the method.</returns>
    protected delegate bool TryParse<T>(ReadOnlySpan<byte> source, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out T? value, out int bytesConsumed);

    /// <summary>
    /// Gets the Well-Known Text.
    /// </summary>
    protected string Wkt { get; } = wkt;

    /// <inheritdoc/>
    public virtual Point GetPoint() => this.GetValue<Point>(Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual PointZ GetPointZ() => this.GetValue<PointZ>(Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual PointM GetPointM() => this.GetValue<PointM>(Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual PointZM GetPointZM() => this.GetValue<PointZM>(Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual IMultiGeometry<Point> GetMultiPoint() => this.GetValue<IMultiGeometry<Point>>(Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual IMultiGeometry<PointZ> GetMultiPointZ() => this.GetValue<IMultiGeometry<PointZ>>(Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual IMultiGeometry<PointM> GetMultiPointM() => this.GetValue<IMultiGeometry<PointM>>(Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual IMultiGeometry<PointZM> GetMultiPointZM() => this.GetValue<IMultiGeometry<PointZM>>(Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual Polyline GetLineString() => this.GetValue<Polyline>(Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual PolylineZ GetLineStringZ() => this.GetValue<PolylineZ>(Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual PolylineM GetLineStringM() => this.GetValue<PolylineM>(Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual PolylineZM GetLineStringZM() => this.GetValue<PolylineZM>(Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual IMultiGeometry<Polyline> GetMultiLineString() => this.GetValue<IMultiGeometry<Polyline>>(Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual IMultiGeometry<PolylineZ> GetMultiLineStringZ() => this.GetValue<IMultiGeometry<PolylineZ>>(Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual IMultiGeometry<PolylineM> GetMultiLineStringM() => this.GetValue<IMultiGeometry<PolylineM>>(Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual IMultiGeometry<PolylineZM> GetMultiLineStringZM() => this.GetValue<IMultiGeometry<PolylineZM>>(Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual Polygon GetPolygon() => this.GetValue<Polygon>(Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual PolygonZ GetPolygonZ() => this.GetValue<PolygonZ>(Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual PolygonM GetPolygonM() => this.GetValue<PolygonM>(Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual PolygonZM GetPolygonZM() => this.GetValue<PolygonZM>(Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual IMultiGeometry<Polygon> GetMultiPolygon() => this.GetValue<IMultiGeometry<Polygon>>(Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual IMultiGeometry<PolygonZ> GetMultiPolygonZ() => this.GetValue<IMultiGeometry<PolygonZ>>(Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual IMultiGeometry<PolygonM> GetMultiPolygonM() => this.GetValue<IMultiGeometry<PolygonM>>(Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual IMultiGeometry<PolygonZM> GetMultiPolygonZM() => this.GetValue<IMultiGeometry<PolygonZM>>(Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual IGeometry GetGeometry() => this.GetValue<IGeometry>(Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public bool IsNull() => string.IsNullOrEmpty(this.Wkt);

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <typeparam name="T">The type of object.</typeparam>
    /// <param name="tryParse">The try parse method.</param>
    /// <returns>The parsed object.</returns>
    /// <exception cref="InvalidGeometryTypeException"><paramref name="tryParse"/> failed.</exception>
    protected virtual T GetValue<T>(TryParse<T> tryParse)
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        Span<byte> bytes = stackalloc byte[System.Text.Encoding.UTF8.GetByteCount(this.Wkt)];
        var count = System.Text.Encoding.UTF8.GetBytes(this.Wkt.AsSpan(), bytes);
        ReadOnlySpan<byte> span = bytes[..count];
#else
        ReadOnlySpan<byte> span = System.Text.Encoding.UTF8.GetBytes(this.Wkt);
#endif

        if (tryParse(span, out var result, out _))
        {
            return result;
        }

        throw new InvalidGeometryTypeException();
    }
}