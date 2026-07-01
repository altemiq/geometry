// -----------------------------------------------------------------------
// <copyright file="WktWriterHelper.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.Geodesy;

/// <summary>
/// The WKT writer helper class.
/// </summary>
internal static class WktWriterHelper
{
    /// <summary>
    /// Writes the indentation to the specified buffer.
    /// </summary>
    /// <param name="buffer">The buffer.</param>
    /// <param name="indent">The indent.</param>
    /// <param name="indentByte">The indent byte.</param>
    public static void WriteIndentation(Span<byte> buffer, int indent, byte indentByte)
    {
        System.Diagnostics.Debug.Assert(buffer.Length >= indent, "Buffer is too small");

        // Based on perf tests, the break-even point where vectorized Fill is faster
        // than explicitly writing the space in a loop is 8.
        if (indent < 8)
        {
            var i = 0;
            while (i + 1 < indent)
            {
                buffer[i++] = indentByte;
                buffer[i++] = indentByte;
            }

            if (i < indent)
            {
                buffer[i] = indentByte;
            }
        }
        else
        {
            buffer[..indent].Fill(indentByte);
        }
    }
}