// -----------------------------------------------------------------------
// <copyright file="ExtensionMethods.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#if !NETSTANDARD2_1_OR_GREATER
namespace Altemiq.IO.Geometry.Shapefile;

/// <summary>
/// Extension methods.
/// </summary>
internal static class ExtensionMethods
{
    /// <summary>
    /// Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="data">A region of memory. This method copies the contents of this region to <paramref name="stream"/>.</param>
    public static void Write(this Stream stream, ReadOnlySpan<byte> data)
    {
        var array = data.ToArray();
        stream.Write(array, 0, array.Length);
    }
}
#endif