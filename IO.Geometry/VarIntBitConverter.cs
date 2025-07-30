// -----------------------------------------------------------------------
// <copyright file="VarIntBitConverter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

/// <summary>
/// The <c>varint</c> bit converter.
/// </summary>
internal static class VarIntBitConverter
{
    /// <summary>
    /// Returns the specified 32-bit unsigned value as a variable int encoded array of bytes.
    /// </summary>
    /// <param name="value">32-bit unsigned value.</param>
    /// <returns>An array of bytes.</returns>
    public static byte[] GetBytes(uint value) => GetBytes((ulong)value);

    /// <summary>
    /// Returns the specified 64-bit signed value as a variable encoded array of bytes.
    /// </summary>
    /// <param name="value">64-bit signed value.</param>
    /// <returns>An array of bytes.</returns>
    public static byte[] GetBytes(long value) => GetBytes((ulong)EncodeZigZag(value, 64));

    /// <summary>
    /// Returns the specified 64-bit unsigned value as a variable int encoded array of bytes.
    /// </summary>
    /// <param name="value">64-bit unsigned value.</param>
    /// <returns>An int array of bytes.</returns>
    public static byte[] GetBytes(ulong value)
    {
        Span<byte> buffer = stackalloc byte[10];
        var pos = 0;
        do
        {
            var byteVal = value & 0x7F;
            value >>= 7;

            if (value is not 0)
            {
                byteVal |= 0x80;
            }

            buffer[pos++] = (byte)byteVal;
        }
        while (value is not 0);

        return buffer[..pos].ToArray();
    }

    /// <summary>
    /// Returns 64-bit signed value from a variable int encoded array of bytes.
    /// </summary>
    /// <param name="bytes">The encoded array of bytes.</param>
    /// <returns>64-bit signed value.</returns>
    public static long ToInt64(ReadOnlySpan<byte> bytes) => DecodeZigZag(ToTarget(bytes, 64));

    /// <summary>
    /// Returns 64-bit unsigned value from a variable int encoded array of bytes.
    /// </summary>
    /// <param name="bytes">The encoded array of bytes.</param>
    /// <returns>64-bit unsigned value.</returns>
    public static ulong ToUInt64(ReadOnlySpan<byte> bytes) => ToTarget(bytes, 64);

    /// <summary>
    /// Encodes the zgp-zag value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="bitLength">The bit length.</param>
    /// <returns>The encoded value.</returns>
    public static long EncodeZigZag(long value, int bitLength) => (value << 1) ^ (value >> (bitLength - 1));

    /// <summary>
    /// Decodes the zig-zag value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The decoded value.</returns>
    public static long DecodeZigZag(ulong value) => (value & 0x1) is 0x1
        ? -1 * ((long)(value >> 1) + 1)
        : (long)(value >> 1);

    private static ulong ToTarget(ReadOnlySpan<byte> bytes, int sizeBites)
    {
        var shift = 0;
        var result = 0UL;

        foreach (ulong byteValue in bytes)
        {
            var temp = byteValue & 0x7f;
            result |= temp << shift;

            if (shift > sizeBites)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes), Properties.Resources.ByteArrayTooLarge);
            }

            if ((byteValue & 0x80) is not 0x80)
            {
                return result;
            }

            shift += 7;
        }

        throw new ArgumentException(Properties.Resources.CannotDecodeVarInt, nameof(bytes));
    }
}