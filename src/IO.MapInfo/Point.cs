// -----------------------------------------------------------------------
// <copyright file="Point.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapInfo;

/// <summary>
/// Reader for points.
/// </summary>
internal static class Point
{
    /// <summary>
    /// Reads a single compressed value from the span.
    /// </summary>
    /// <param name="span">The span containing the value.</param>
    /// <param name="offset">The offset value.</param>
    /// <returns>The value.</returns>
    public static int Read(ReadOnlySpan<byte> span, int offset) => SaturatedAdd(System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(span), offset);

    /// <summary>
    /// Reads a single point from the span.
    /// </summary>
    /// <param name="span">The span containing the point.</param>
    /// <returns>The point.</returns>
    public static RawPoint Read(ReadOnlySpan<byte> span) => new(System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span), System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[4..]));

    /// <summary>
    /// Reads a single compressed point from the span.
    /// </summary>
    /// <param name="span">The span containing the point.</param>
    /// <param name="offsetX">The offset for the <see cref="RawPoint.X"/> value.</param>
    /// <param name="offsetY">The offset for the <see cref="RawPoint.Y"/> value.</param>
    /// <returns>The point.</returns>
    public static RawPoint Read(ReadOnlySpan<byte> span, int offsetX, int offsetY) => new(Read(span, offsetX), Read(span[2..], offsetY));

    /// <summary>
    /// Reads a sequence of points from the <see cref="CoordBlock"/>.
    /// </summary>
    /// <param name="coordBlock">The coordinate block.</param>
    /// <param name="start">The offset at which to start reading.</param>
    /// <param name="length">The length of data to read.</param>
    /// <returns>The sequence of points.</returns>
    public static IEnumerable<RawPoint> Read(CoordBlock coordBlock, int start, int length)
    {
        var currentCoordBlock = coordBlock.GetCoordBlock(start);
        var end = start + length;
        var i = start;
        while (i < end)
        {
            var nextCoordBlock = currentCoordBlock.GetCoordBlock(i);
            if (currentCoordBlock != nextCoordBlock)
            {
                // move past the header.
                i += 8;
                end += 8;
                currentCoordBlock = nextCoordBlock;
            }

            yield return Read(currentCoordBlock, i);

            i += 2 * sizeof(int);
        }
    }

    /// <summary>
    /// Reads a sequence of compressed points from the <see cref="CoordBlock"/>.
    /// </summary>
    /// <param name="coordBlock">The coordinate block.</param>
    /// <param name="start">The offset at which to start reading.</param>
    /// <param name="dataStart">The start of the actual data.</param>
    /// <param name="length">The length of data to read.</param>
    /// <param name="offsetX">The offset for the <see cref="RawPoint.X"/> value.</param>
    /// <param name="offsetY">The offset for the <see cref="RawPoint.Y"/> value.</param>
    /// <returns>The sequence of points.</returns>
    public static IEnumerable<RawPoint> Read(CoordBlock coordBlock, int start, int dataStart, int length, int offsetX, int offsetY)
    {
        var currentCoordBlock = coordBlock.GetCoordBlock(start);
        start += dataStart;
        var end = start + length;
        var index = start;
        while (index < end)
        {
            var nextCoordBlock = currentCoordBlock.GetCoordBlock(index);
            if (currentCoordBlock != nextCoordBlock)
            {
                // move past the header.
                index += 8;
                end += 8;
                currentCoordBlock = nextCoordBlock;
            }

            yield return Read(currentCoordBlock, index, offsetX, offsetY);

            index += 2 * sizeof(short);
        }
    }

    /// <summary>
    /// Reads a single point from the <see cref="CoordBlock" />.
    /// </summary>
    /// <param name="coordBlock">The <see cref="CoordBlock" /> containing the point.</param>
    /// <param name="start">The offset at which to start reading.</param>
    /// <returns>The point.</returns>
    public static RawPoint Read(CoordBlock coordBlock, int start) => new(coordBlock.ReadInt32(start), coordBlock.ReadInt32(start + sizeof(int)));

    /// <summary>
    /// Reads a single compressed point from the <see cref="CoordBlock" />.
    /// </summary>
    /// <param name="coordBlock">The <see cref="CoordBlock" /> containing the point.</param>
    /// <param name="start">The offset at which to start reading.</param>
    /// <param name="offsetX">The offset for the <see cref="RawPoint.X"/> value.</param>
    /// <param name="offsetY">The offset for the <see cref="RawPoint.Y"/> value.</param>
    /// <returns>The point.</returns>
    public static RawPoint Read(CoordBlock coordBlock, int start, int offsetX, int offsetY) => new(SaturatedAdd(coordBlock.ReadInt16(start), offsetX), SaturatedAdd(coordBlock.ReadInt16(start + sizeof(short)), offsetY));

    private static int SaturatedAdd(short value, int addition) => addition switch
    {
        >= 0 when value > int.MaxValue - addition => int.MaxValue,
        int.MinValue when value < 0 => int.MinValue,
        not int.MinValue and < 0 when value < int.MinValue - addition => int.MinValue,
        _ => value + addition,
    };
}