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
    public virtual Point GetPoint() => this.GetValue<Point>(Altemiq.Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual PointZ GetPointZ() => this.GetValue<PointZ>(Altemiq.Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual PointM GetPointM() => this.GetValue<PointM>(Altemiq.Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual PointZM GetPointZM() => this.GetValue<PointZM>(Altemiq.Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<Point> GetMultiPoint() => this.GetValue<IReadOnlyCollection<Point>>(Altemiq.Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<PointZ> GetMultiPointZ() => this.GetValue<IReadOnlyCollection<PointZ>>(Altemiq.Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<PointM> GetMultiPointM() => this.GetValue<IReadOnlyCollection<PointM>>(Altemiq.Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<PointZM> GetMultiPointZM() => this.GetValue<IReadOnlyCollection<PointZM>>(Altemiq.Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual Polyline GetLineString() => this.GetValue<Polyline>(Altemiq.Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual PolylineZ GetLineStringZ() => this.GetValue<PolylineZ>(Altemiq.Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual PolylineM GetLineStringM() => this.GetValue<PolylineM>(Altemiq.Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual PolylineZM GetLineStringZM() => this.GetValue<PolylineZM>(Altemiq.Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<Polyline> GetMultiLineString() => this.GetValue<IReadOnlyCollection<Polyline>>(Altemiq.Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<PolylineZ> GetMultiLineStringZ() => this.GetValue<IReadOnlyCollection<PolylineZ>>(Altemiq.Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<PolylineM> GetMultiLineStringM() => this.GetValue<IReadOnlyCollection<PolylineM>>(Altemiq.Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<PolylineZM> GetMultiLineStringZM() => this.GetValue<IReadOnlyCollection<PolylineZM>>(Altemiq.Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual Polygon GetPolygon() => this.GetValue<Polygon>(Altemiq.Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual PolygonZ GetPolygonZ() => this.GetValue<PolygonZ>(Altemiq.Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual PolygonM GetPolygonM() => this.GetValue<PolygonM>(Altemiq.Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual PolygonZM GetPolygonZM() => this.GetValue<PolygonZM>(Altemiq.Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<Polygon> GetMultiPolygon() => this.GetValue<IReadOnlyCollection<Polygon>>(Altemiq.Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<PolygonZ> GetMultiPolygonZ() => this.GetValue<IReadOnlyCollection<PolygonZ>>(Altemiq.Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<PolygonM> GetMultiPolygonM() => this.GetValue<IReadOnlyCollection<PolygonM>>(Altemiq.Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<PolygonZM> GetMultiPolygonZM() => this.GetValue<IReadOnlyCollection<PolygonZM>>(Altemiq.Buffers.Text.WktParser.TryParse);

    /// <inheritdoc/>
    public virtual object GetGeometry() => this.GetValue<object>(Altemiq.Buffers.Text.WktParser.TryParse);

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