// -----------------------------------------------------------------------
// <copyright file="ShpRecordHeader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

/// <summary>
/// The SHP record header.
/// </summary>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly struct ShpRecordHeader : IEquatable<ShpRecordHeader>
{
    /// <summary>
    /// The size of this struct.
    /// </summary>
    public const int Size = sizeof(int) + sizeof(uint);

    /// <summary>
    /// Initialises a new instance of the <see cref="ShpRecordHeader"/> struct.
    /// </summary>
    /// <param name="recordNumber">The record number.</param>
    /// <param name="contentLength">The content length.</param>
    public ShpRecordHeader(int recordNumber, uint contentLength) => (this.RecordNumber, this.ContentLength) = (recordNumber, contentLength);

    /// <summary>
    /// Gets the record number.
    /// </summary>
    public int RecordNumber { get; }

    /// <summary>
    /// Gets the content length.
    /// </summary>
    public uint ContentLength { get; }

    /// <summary>
    /// Implements the equals operator.
    /// </summary>
    /// <param name="left">The left-hand side.</param>
    /// <param name="right">The right-hand side.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise <see langword="false"/>.</returns>
    public static bool operator ==(ShpRecordHeader left, ShpRecordHeader right) => left.Equals(right);

    /// <summary>
    /// Implements the not-equals operator.
    /// </summary>
    /// <param name="left">The left-hand side.</param>
    /// <param name="right">The right-hand side.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise <see langword="false"/>.</returns>
    public static bool operator !=(ShpRecordHeader left, ShpRecordHeader right) => !(left == right);

    /// <summary>
    /// Reads the SHP record header.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>The SHP record header.</returns>
    public static ShpRecordHeader Read(Stream stream)
    {
#if NETSTANDARD2_1_OR_GREATER
        Span<byte> span = stackalloc byte[8];
        _ = stream.Read(span);
#else
        var bytes = new byte[8];
        _ = stream.Read(bytes, 0, bytes.Length);
        ReadOnlySpan<byte> span = bytes;
#endif
        return Read(span);
    }

    /// <summary>
    /// Reads the SHP record header.
    /// </summary>
    /// <param name="source">The source bytes.</param>
    /// <returns>The SHP record header.</returns>
    public static ShpRecordHeader Read(ReadOnlySpan<byte> source)
    {
        var recordNumber = System.Buffers.Binary.BinaryPrimitives.ReadInt32BigEndian(source[..4]);
        var contentLength = System.Buffers.Binary.BinaryPrimitives.ReadUInt32BigEndian(source[4..8]);
        return new(recordNumber, contentLength);
    }

    /// <inheritdoc/>
    public override bool Equals(object obj) => obj is ShpRecordHeader record && this.Equals(record);

    /// <inheritdoc/>
    public bool Equals(ShpRecordHeader other) => this.RecordNumber == other.RecordNumber && this.ContentLength == other.ContentLength;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(this.RecordNumber, this.ContentLength);

    /// <inheritdoc/>
    public override string ToString()
    {
        FormattableString formattableString = $"({nameof(this.RecordNumber)}: {this.RecordNumber}, {nameof(this.ContentLength)}: {this.ContentLength}";
        return formattableString.ToString(System.Globalization.CultureInfo.CurrentCulture);
    }
}