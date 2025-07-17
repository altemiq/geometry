// -----------------------------------------------------------------------
// <copyright file="QixWriter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

using static System.Buffers.Binary.BinaryPrimitives;

/// <summary>
/// The QIZ writer.
/// </summary>
public class QixWriter : IDisposable
{
    private static readonly char[] Signature = ['S', 'Q', 'T'];

    private readonly Stream stream;

    private readonly bool leaveOpen;

    private readonly bool isLittleEndian;

    private bool disposedValue;

    /// <summary>
    /// Initialises a new instance of the <see cref="QixWriter"/> class.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="isLittleEndian"><see langword="true"/> to write the data as little endian.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="QixWriter"/> object is disposed; otherwise, <see langword="false"/>.</param>
    public QixWriter(Stream stream, bool isLittleEndian = true, bool leaveOpen = false)
    {
        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        System.Diagnostics.Contracts.Contract.EndContractBlock();
        (this.stream, this.isLittleEndian, this.leaveOpen) = (stream, isLittleEndian, leaveOpen);
    }

    /// <summary>
    /// Writes the <see cref="QixNode"/> to the stream.
    /// </summary>
    /// <param name="node">The node to write.</param>
    public void Write(QixNode node)
    {
        const byte LittleEndian = 1;
        const byte BigEndian = 2;

        Span<byte> span = stackalloc byte[16];

        // signature
        span[0] = (byte)Signature[0];
        span[1] = (byte)Signature[1];
        span[2] = (byte)Signature[2];

        // byte order.
        span[3] = this.isLittleEndian ? LittleEndian : BigEndian;

        // version
        span[4] = 1;

        var (count, depth) = GetCountAndMaxDepth(node, 1);

        if (this.isLittleEndian)
        {
            System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(span[8..12], count);
            System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(span[8..12], depth);
        }
        else
        {
            System.Buffers.Binary.BinaryPrimitives.WriteInt32BigEndian(span[8..12], count);
            System.Buffers.Binary.BinaryPrimitives.WriteInt32BigEndian(span[8..12], depth);
        }

        this.stream.Write(span);

        WriteNode(this.stream, this.isLittleEndian, node);

        static (int Count, int Depth) GetCountAndMaxDepth(QixNode node, int depth)
        {
            var count = node.Shapes.Count;
            var maxDepth = depth;
            if (node.Nodes is { } nodes)
            {
                foreach (var subNode in nodes)
                {
                    var (nodeCount, nodeDepth) = GetCountAndMaxDepth(subNode, depth + 1);
                    count += nodeCount;
                    if (maxDepth < nodeDepth)
                    {
                        maxDepth = nodeDepth;
                    }
                }
            }

            return (count, maxDepth);
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the resources for this instance.
    /// </summary>
    /// <param name="disposing">Set to <see langword="true"/> to dispose of managed resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing && !this.leaveOpen)
            {
                this.stream.Dispose();
            }

            this.disposedValue = true;
        }
    }

    private static void WriteNode(Stream stream, bool isLittleEndian, QixNode node)
    {
        var offset = GetSubNodeOffset(node);
        Span<byte> span = stackalloc byte[36];

        WriteInt32(span[..4], offset, isLittleEndian);
        WriteDouble(span[4..12], node.Extents.Left, isLittleEndian);
        WriteDouble(span[12..20], node.Extents.Bottom, isLittleEndian);
        WriteDouble(span[20..28], node.Extents.Right, isLittleEndian);
        WriteDouble(span[28..36], node.Extents.Top, isLittleEndian);
        stream.Write(span);

        WriteShapes(stream, isLittleEndian, node.Shapes);

        if (node.Nodes is { Count: > 0 } nodes)
        {
            WriteInt32(span, nodes.Count, isLittleEndian);
            stream.Write(span[..4]);
            foreach (var subNode in nodes)
            {
                WriteNode(stream, isLittleEndian, subNode);
            }
        }
        else
        {
            WriteInt32(span, 0, isLittleEndian);
            stream.Write(span[..4]);
        }

        static int GetSubNodeOffset(QixNode node)
        {
            var offset = 0;

            if (node.Nodes is { Count: > 0 } nodes)
            {
                foreach (var subNode in nodes)
                {
                    offset += (4 * sizeof(double)) + ((subNode.Shapes.Count + 3) * sizeof(int));
                    offset += GetSubNodeOffset(subNode);
                }
            }

            return offset;
        }

        static void WriteShapes(Stream stream, bool isLittleEndian, IReadOnlyCollection<int> shapes)
        {
            if (shapes is { Count: > 0 })
            {
                Span<byte> span = stackalloc byte[(shapes.Count + 1) * sizeof(int)];
                WriteInt32(span[..4], shapes.Count, isLittleEndian);
                var idx = sizeof(int);
                if (isLittleEndian)
                {
                    foreach (var shape in shapes)
                    {
                        WriteInt32LittleEndian(span[idx..], shape);
                        idx += sizeof(int);
                    }
                }
                else
                {
                    foreach (var shape in shapes)
                    {
                        WriteInt32BigEndian(span[idx..], shape);
                        idx += sizeof(int);
                    }
                }

                stream.Write(span);
            }
            else
            {
                Span<byte> span = stackalloc byte[sizeof(int)];
                WriteInt32(span[..4], 0, isLittleEndian);
                stream.Write(span);
            }
        }

        static void WriteInt32(Span<byte> destination, int value, bool isLittleEndian)
        {
            if (isLittleEndian)
            {
                WriteInt32LittleEndian(destination, value);
            }
            else
            {
                WriteInt32BigEndian(destination, value);
            }
        }

        static void WriteDouble(Span<byte> destination, double value, bool isLittleEndian)
        {
            if (isLittleEndian)
            {
                WriteInt64LittleEndian(destination, BitConverter.DoubleToInt64Bits(value));
            }
            else
            {
                WriteInt64BigEndian(destination, BitConverter.DoubleToInt64Bits(value));
            }
        }
    }
}