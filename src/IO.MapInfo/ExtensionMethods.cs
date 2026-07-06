// -----------------------------------------------------------------------
// <copyright file="ExtensionMethods.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#if !NET5_0_OR_GREATER
namespace Altemiq.IO.MapInfo;

/// <summary>
/// Extension methods.
/// </summary>
internal static class ExtensionMethods
{
    extension(System.Buffers.Binary.BinaryPrimitives)
    {
        /// <summary>
        /// Reads a <see cref="double"/> from the beginning of a read-only span of bytes, as little endian.
        /// </summary>
        /// <param name="source">The read-only span to read.</param>
        /// <returns>The little endian value.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="source"/> is too small to contain a <see cref="double"/>.</exception>
        /// <remarks>Reads exactly 8 bytes from the beginning of the span.</remarks>
        public static double ReadDoubleLittleEndian(ReadOnlySpan<byte> source) => BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(source));
    }
}
#endif