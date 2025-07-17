// -----------------------------------------------------------------------
// <copyright file="ShxRecord.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

/// <summary>
/// The SHX record.
/// </summary>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly struct ShxRecord : IEquatable<ShxRecord>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="ShxRecord"/> struct.
    /// </summary>
    /// <param name="offset">The offset.</param>
    /// <param name="contentLength">The content length.</param>
    public ShxRecord(uint offset, uint contentLength) => (this.Offset, this.ContentLength) = (offset, contentLength);

    /// <summary>
    /// Gets the offset.
    /// </summary>
    /// <remarks>
    /// The offset of a record in the main file is the number of 16-bit words from the start of the main file to the first byte of the record header for the record.
    /// Thus, the offset for the first record in the main file is 50, given the 100-byte header.
    /// </remarks>
    public uint Offset { get; }

    /// <summary>
    /// Gets the content length.
    /// </summary>
    /// <remarks>
    /// The content length stored in the index record is the same as the value stored in the main file record header.
    /// </remarks>
    public uint ContentLength { get; }

    /// <summary>
    /// Implements the equals operator.
    /// </summary>
    /// <param name="left">The left-hand side.</param>
    /// <param name="right">The right-hand side.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise <see langword="false"/>.</returns>
    public static bool operator ==(ShxRecord left, ShxRecord right) => left.Equals(right);

    /// <summary>
    /// Implements the not-equals operator.
    /// </summary>
    /// <param name="left">The left-hand side.</param>
    /// <param name="right">The right-hand side.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise <see langword="false"/>.</returns>
    public static bool operator !=(ShxRecord left, ShxRecord right) => !(left == right);

    /// <inheritdoc/>
    public override bool Equals(object obj) => obj is ShxRecord record && this.Equals(record);

    /// <inheritdoc/>
    public bool Equals(ShxRecord other) => this.Offset == other.Offset && this.ContentLength == other.ContentLength;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(this.Offset, this.ContentLength);

    /// <inheritdoc/>
    public override string ToString()
    {
        FormattableString formattableString = $"({nameof(this.Offset)}: {this.Offset}, {nameof(this.ContentLength)}: {this.ContentLength}";
        return formattableString.ToString(System.Globalization.CultureInfo.CurrentCulture);
    }
}