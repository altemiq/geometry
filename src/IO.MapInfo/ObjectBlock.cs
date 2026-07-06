// -----------------------------------------------------------------------
// <copyright file="ObjectBlock.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapInfo;

/// <summary>
/// The object block.
/// </summary>
/// <param name="data">The data.</param>
public readonly struct ObjectBlock(byte[] data) : IRawBlock
{
    private const int MapObjectHeaderSize = 20;

    private readonly byte[] data = data[0] is 2 ? data : throw new InvalidOperationException();

    private readonly long dataOffset;

    /// <summary>
    /// Initialises a new instance of the <see cref="ObjectBlock"/> struct.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <param name="offset">The offset.</param>
    internal ObjectBlock(byte[] data, long offset)
        : this(data) => this.dataOffset = offset;

    /// <summary>
    /// Gets the center-x value.
    /// </summary>
    public int CenterX => System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(this.data.AsSpan(0x04));

    /// <summary>
    /// Gets the center-y value.
    /// </summary>
    public int CenterY => System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(this.data.AsSpan(0x08));

    /// <summary>
    /// Gets the first coordinate block.
    /// </summary>
    public int FirstCoordBlock => System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(this.data.AsSpan(0x0C));

    /// <summary>
    /// Gets the last coordinate block.
    /// </summary>
    public int LastCoordBlock => System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(this.data.AsSpan(0x10));

    /// <inheritdoc/>
    long IRawBlock.Offset => this.dataOffset;

    /// <inheritdoc/>
    int IRawBlock.BytesUsed => this.DataBytes + MapObjectHeaderSize;

    private short DataBytes => System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(this.data.AsSpan(0x02));

    /// <inheritdoc/>
    bool IRawBlock.ContainsOffset(long offset) => this.dataOffset <= offset && this.dataOffset + this.DataBytes + MapObjectHeaderSize > offset;

    /// <summary>
    /// Gets the data.
    /// </summary>
    /// <returns>The data.</returns>
    internal byte[] GetData() => this.data;
}