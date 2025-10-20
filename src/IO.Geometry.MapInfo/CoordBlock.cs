// -----------------------------------------------------------------------
// <copyright file="CoordBlock.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.MapInfo;

/// <summary>
/// The object block.
/// </summary>
/// <param name="data">The data.</param>
public class CoordBlock(byte[] data) : IRawBlock
{
    private const int MapCoordHeaderSize = 8;

    private readonly byte[] data = data[0] is 3 ? data : throw new InvalidOperationException();

    private readonly long dataOffset;

    /// <summary>
    /// Initialises a new instance of the <see cref="CoordBlock"/> class.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <param name="offset">The offset.</param>
    internal CoordBlock(byte[] data, long offset)
        : this(data) => this.dataOffset = offset;

    /// <summary>
    /// Gets the next <see cref="CoordBlock"/>.
    /// </summary>
    public CoordBlock? Next { get; private set; }

    /// <inheritdoc/>
    long IRawBlock.Offset => this.dataOffset;

    /// <inheritdoc/>
    int IRawBlock.BytesUsed => this.DataBytes + MapCoordHeaderSize;

    /// <summary>
    /// Gets the next coord block.
    /// </summary>
    internal int NextCoordBlock => System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(this.data.AsSpan(0x04));

    private short DataBytes => System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(this.data.AsSpan(0x02));

    /// <summary>
    /// Reads an <see cref="short"/> from the coord block.
    /// </summary>
    /// <param name="offset">The offset.</param>
    /// <returns>The value.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is invalid.</exception>
    public short ReadInt16(int offset) => this.Read(offset, static (coordBlock, actualOffset) => System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(coordBlock.data.AsSpan(actualOffset)));

    /// <summary>
    /// Reads an <see cref="int"/> from the coordinate block.
    /// </summary>
    /// <param name="offset">The offset.</param>
    /// <returns>The value.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is invalid.</exception>
    public int ReadInt32(int offset) => this.Read(offset, static (coordBlock, actualOffset) => System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(coordBlock.data.AsSpan(actualOffset)));

    /// <summary>
    /// Gets the coordinate block for the specified offset.
    /// </summary>
    /// <param name="offset">The file offset.</param>
    /// <returns>The coordinate block.</returns>
    /// <exception cref="ArgumentOutOfRangeException">No <see cref="CoordBlock"/> contains <paramref name="offset"/>.</exception>
    public CoordBlock GetCoordBlock(int offset)
    {
        if (!Contains(this, offset))
        {
            if (this.Next is { } next)
            {
                return next.GetCoordBlock(offset);
            }

            throw new ArgumentOutOfRangeException(nameof(offset));
        }

        return this;
    }

    /// <summary>
    /// Tries to get the coordinate block for the specified offset, returning whether it was found.
    /// </summary>
    /// <param name="offset">The file offset.</param>
    /// <param name="coordBlock">The <see cref="CoordBlock"/> if found; otherwise <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if a <see cref="CoordBlock"/> was found containing <paramref name="offset"/>; otherwise <see langword="false"/>.</returns>
    public bool TryGetCoordBlock(int offset, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out CoordBlock? coordBlock)
    {
        if (!Contains(this, offset))
        {
            if (this.Next is { } next)
            {
                return next.TryGetCoordBlock(offset, out coordBlock);
            }

            coordBlock = default;
            return false;
        }

        coordBlock = this;
        return true;
    }

    /// <summary>
    /// Reads the coordinate section headers.
    /// </summary>
    /// <param name="offset">The offset.</param>
    /// <param name="count">The count.</param>
    /// <param name="version">The version.</param>
    /// <returns>The coordinate section headers.</returns>
    public IEnumerable<CoordSectionHeader> ReadCoordSectionHeaders(int offset, int count, int version)
    {
        var sectionSize = version >= 450 ? 28 : 24;
        for (var i = 0; i < count; i++)
        {
            var (sectionData, start) = this.GetData(offset + (sectionSize * i));
            yield return new(sectionData, start, sectionSize, version);
        }
    }

    /// <summary>
    /// Reads the coordinate section headers.
    /// </summary>
    /// <param name="offset">The offset.</param>
    /// <param name="count">The count.</param>
    /// <param name="version">The version.</param>
    /// <param name="centerX">The center-x.</param>
    /// <param name="centerY">The center-y.</param>
    /// <returns>The coordinate section headers.</returns>
    public IEnumerable<CoordSectionHeader> ReadCoordSectionHeaders(int offset, int count, int version, int centerX, int centerY)
    {
        var sectionSize = version >= 450 ? 20 : 16;
        for (var i = 0; i < count; i++)
        {
            var (sectionData, start) = this.GetData(offset + (sectionSize * i));
            yield return new(sectionData, start, version, centerX, centerY);
        }
    }

    /// <inheritdoc/>
    bool IRawBlock.ContainsOffset(long offset) => (this.dataOffset <= offset && this.dataOffset + this.DataBytes + MapCoordHeaderSize > offset) || (this.Next is IRawBlock next && next.ContainsOffset(offset));

    /// <summary>
    /// Sets the <see cref="Next"/> value.
    /// </summary>
    /// <param name="coordBlock">The coord block.</param>
    internal void SetNext(CoordBlock coordBlock) => this.Next = coordBlock;

    private static bool Contains<T>(T block, int offset)
        where T : CoordBlock, IRawBlock => (int)(offset - block.dataOffset) < block.BytesUsed;

    private T Read<T>(int offset, Func<CoordBlock, int, T> func)
    {
        var coordBlock = this.GetCoordBlock(offset);
        return func(coordBlock, (int)(offset - coordBlock.dataOffset));
    }

    private (byte[] Data, int Offset) GetData(long offset)
    {
        var actualOffset = (int)(offset - this.dataOffset);
        if (actualOffset >= this.data.Length)
        {
            if (this.Next is { } next)
            {
                return next.GetData(offset);
            }

            throw new ArgumentOutOfRangeException(nameof(offset));
        }

        return (this.data, actualOffset);
    }
}