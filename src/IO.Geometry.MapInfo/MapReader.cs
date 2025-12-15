// -----------------------------------------------------------------------
// <copyright file="MapReader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.MapInfo;

/// <summary>
/// The <c>MAP</c> reader.
/// </summary>
public class MapReader : IDisposable
{
    private readonly Stream stream;

    private readonly bool leaveOpen;

    private readonly int blockSize;

    private readonly HeaderBlock headerBlock;

    private bool disposedValue;

    private ObjectBlock currentObjectBlock;

    private CoordBlock? currentCoordBlock;

    private long currentOffset;

    /// <summary>
    /// Initialises a new instance of the <see cref="MapReader"/> class.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="MapReader"/> object is disposed; otherwise, <see langword="false"/>.</param>
    public MapReader(Stream stream, bool leaveOpen = false)
    {
        ArgumentNullException.ThrowIfNull(stream);

        (this.stream, this.leaveOpen) = (stream, leaveOpen);

        // read the header block
        var bytes = this.ReadBytes(TabReader.TabMinBlockSize);

        // get the type
        var header = new HeaderBlock(bytes);
        if (header.MapVersionNumber >= 500)
        {
            Array.Resize(ref bytes, 2 * TabReader.TabMinBlockSize);
            if (this.stream.Read(bytes, TabReader.TabMinBlockSize, TabReader.TabMinBlockSize) is not TabReader.TabMinBlockSize)
            {
                throw new InvalidOperationException();
            }

            header = new(bytes);
        }

        this.blockSize = header.RegularBlockSize;
        this.headerBlock = header;

        if (this.ReadBytes(this.blockSize) is { Length: not 0 } objectBlockBytes)
        {
            this.currentObjectBlock = new(objectBlockBytes, this.currentOffset);
        }
    }

    /// <summary>
    /// Reads the object.
    /// </summary>
    /// <param name="fileOffset">The file offset.</param>
    /// <returns>The object.</returns>
    public MapRecord Read(long fileOffset)
    {
        this.EnsureObjectBlock(fileOffset);

        var current = GetOffset(this.currentObjectBlock, fileOffset);

        var data = this.currentObjectBlock.GetData();
        var byVal = data[current];
        var geomType = Enum.IsDefined(typeof(TabGeomType), byVal)
            ? (TabGeomType)byVal
            : TabGeomType.Unset;

        var featureId = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(current + 1));

        return new(
            featureId,
            data,
            current + 5,
            geomType,
            this.headerBlock,
            this.currentObjectBlock,
            this);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Gets the coordinate block.
    /// </summary>
    /// <param name="fileOffset">The file offset.</param>
    /// <returns>The coordinate block.</returns>
    internal CoordBlock GetCoordBlock(long fileOffset)
    {
        var coordBlock = this.currentCoordBlock;
        this.EnsureBlock(ref coordBlock, fileOffset, bytes => new(bytes, this.currentOffset));
        this.currentCoordBlock = coordBlock;
        while (coordBlock.NextCoordBlock is not 0 && coordBlock.Next is null)
        {
            var nextCoordBlock = coordBlock;
            this.EnsureBlock(ref nextCoordBlock, nextCoordBlock.NextCoordBlock, bytes => new(bytes, this.currentOffset));
            coordBlock.SetNext(nextCoordBlock);
            coordBlock = nextCoordBlock;
        }

        return this.currentCoordBlock;
    }

    /// <summary>
    /// Disposes the resources for this instance.
    /// </summary>
    /// <param name="disposing">Set to <see langword="true"/> to dispose of managed resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (this.disposedValue)
        {
            return;
        }

        if (disposing && !this.leaveOpen)
        {
            this.stream.Dispose();
        }

        this.disposedValue = true;
    }

    private static int GetOffset<T>(T value, long offset)
        where T : IRawBlock => (int)(offset - value.Offset);

    private byte[] ReadBytes(int size)
    {
        var bytes = new byte[size];
        var actualSize = this.stream.Read(bytes, 0, size);
        if (actualSize != size)
        {
            Array.Resize(ref bytes, actualSize);
        }

        this.currentOffset += actualSize;
        return bytes;
    }

    private void EnsureObjectBlock(long fileOffset) => this.EnsureBlock(ref this.currentObjectBlock, fileOffset, bytes => new(bytes, this.currentOffset));

    private void EnsureBlock<T>([System.Diagnostics.CodeAnalysis.NotNull] ref T? current, long fileOffset, Func<byte[], T> func)
        where T : IRawBlock
    {
        if (current is not null && current.ContainsOffset(fileOffset))
        {
            return;
        }

        byte[] bytes;
        do
        {
            bytes = this.ReadBytes(this.blockSize);
        }
        while (fileOffset >= (this.currentOffset + this.blockSize));

        current = func(bytes);
    }
}