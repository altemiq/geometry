// -----------------------------------------------------------------------
// <copyright file="BinaryGeometryRecord.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data.Common;

/// <summary>
/// The abstract binary reader.
/// </summary>
public abstract class BinaryGeometryRecord : IGeometryRecord
{
    private readonly byte[] bytes;

    private readonly int startLocation;

    private readonly int length;

    /// <summary>
    /// Initialises a new instance of the <see cref="BinaryGeometryRecord"/> class.
    /// </summary>
    /// <param name="bytes">The byte array to read from.</param>
    protected BinaryGeometryRecord(byte[] bytes)
        : this(bytes, 0, bytes.Length)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="BinaryGeometryRecord"/> class.
    /// </summary>
    /// <param name="bytes">The byte array to read from.</param>
    /// <param name="start">The index in <paramref name="bytes"/> at which the stream begins.</param>
    /// <param name="length">The length of the stream in bytes.</param>
    protected BinaryGeometryRecord(byte[] bytes, int start, int length)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(bytes);
#else
        if (bytes is null)
        {
            throw new ArgumentNullException(nameof(bytes));
        }
#endif

        if (start < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(start));
        }

        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        if (bytes.Length < start + length)
        {
            throw new ArgumentOutOfRangeException(nameof(bytes), string.Format(Properties.Resources.Culture, Properties.Resources.MustBeEqualToOrGreaterThan, $"{nameof(bytes)}.{nameof(bytes.Length)}", $"{nameof(start)} + {nameof(length)}"));
        }

        System.Diagnostics.Contracts.Contract.EndContractBlock();
        this.bytes = bytes;
        this.startLocation = start;
        this.length = length;
    }

    /// <inheritdoc/>
    public abstract Point GetPoint();

    /// <inheritdoc/>
    public abstract PointZ GetPointZ();

    /// <inheritdoc/>
    public abstract PointM GetPointM();

    /// <inheritdoc/>
    public abstract PointZM GetPointZM();

    /// <inheritdoc/>
    public abstract IReadOnlyCollection<Point> GetMultiPoint();

    /// <inheritdoc/>
    public abstract IReadOnlyCollection<PointZ> GetMultiPointZ();

    /// <inheritdoc/>
    public abstract IReadOnlyCollection<PointM> GetMultiPointM();

    /// <inheritdoc/>
    public abstract IReadOnlyCollection<PointZM> GetMultiPointZM();

    /// <inheritdoc/>
    public abstract Polyline GetLineString();

    /// <inheritdoc/>
    public abstract PolylineZ GetLineStringZ();

    /// <inheritdoc/>
    public abstract PolylineM GetLineStringM();

    /// <inheritdoc/>
    public abstract PolylineZM GetLineStringZM();

    /// <inheritdoc/>
    public abstract IReadOnlyCollection<Polyline> GetMultiLineString();

    /// <inheritdoc/>
    public abstract IReadOnlyCollection<PolylineZ> GetMultiLineStringZ();

    /// <inheritdoc/>
    public abstract IReadOnlyCollection<PolylineM> GetMultiLineStringM();

    /// <inheritdoc/>
    public abstract IReadOnlyCollection<PolylineZM> GetMultiLineStringZM();

    /// <inheritdoc/>
    public abstract Polygon GetPolygon();

    /// <inheritdoc/>
    public abstract PolygonZ GetPolygonZ();

    /// <inheritdoc/>
    public abstract PolygonM GetPolygonM();

    /// <inheritdoc/>
    public abstract PolygonZM GetPolygonZM();

    /// <inheritdoc/>
    public abstract IReadOnlyCollection<Polygon> GetMultiPolygon();

    /// <inheritdoc/>
    public abstract IReadOnlyCollection<PolygonZ> GetMultiPolygonZ();

    /// <inheritdoc/>
    public abstract IReadOnlyCollection<PolygonM> GetMultiPolygonM();

    /// <inheritdoc/>
    public abstract IReadOnlyCollection<PolygonZM> GetMultiPolygonZM();

    /// <inheritdoc/>
    public abstract object? GetGeometry();

    /// <inheritdoc/>
    public abstract bool IsNull();

    /// <summary>
    /// Gets this instance as a span, start at the specified value.
    /// </summary>
    /// <param name="start">The value to start at.</param>
    /// <returns>The span instance.</returns>
    protected ReadOnlySpan<byte> AsSpan(int start = default) => new(this.bytes, this.startLocation + start, this.length - start);
}