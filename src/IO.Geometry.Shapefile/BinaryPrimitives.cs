// -----------------------------------------------------------------------
// <copyright file="BinaryPrimitives.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

/// <summary>
/// Reads bytes as primitives with specific endianness.
/// </summary>
internal static class BinaryPrimitives
{
    /// <summary>
    /// Writes an <see cref="double"/> into a span of bytes, as little endian, if it is not <see cref="double.NaN"/>.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <param name="defaultValue">The default value to write if <paramref name="value"/> is <see cref="double.NaN"/>.</param>
    /// <remarks>Writes exactly 8 bytes to the beginning of the span.</remarks>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain a <see cref="double"/>.</exception>
    public static void WriteDoubleLittleEndianIfNotNan(Span<byte> destination, double value, double defaultValue = Constants.NoData) => System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(destination, BitConverter.DoubleToInt64Bits(double.IsNaN(value) ? defaultValue : value));
}