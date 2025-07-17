// -----------------------------------------------------------------------
// <copyright file="QixNode.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

using static System.Buffers.Binary.BinaryPrimitives;

/// <summary>
/// The QIX tree node.
/// </summary>
public readonly struct QixNode : IEquatable<QixNode>
{
    /// <summary>
    /// The split ratio.
    /// </summary>
    public const double SplitRatio = 0.55;

    /// <summary>
    /// Represents an empty node.
    /// </summary>
    public static readonly QixNode Empty = new() { Extents = default };

    /// <summary>
    /// Initialises a new instance of the <see cref="QixNode"/> struct.
    /// </summary>
    public QixNode()
    {
    }

    /// <summary>
    /// Gets the extents.
    /// </summary>
    public required Envelope Extents { get; init; }

    /// <summary>
    /// Gets the shapes.
    /// </summary>
    public IReadOnlyCollection<int> Shapes { get; init; } = [];

    /// <summary>
    /// Gets the nodes.
    /// </summary>
    public IReadOnlyCollection<QixNode> Nodes { get; init; } = [];

    /// <summary>
    /// Determines equality of the two nodes.
    /// </summary>
    /// <param name="left">The LHS.</param>
    /// <param name="right">The RHS.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator ==(QixNode left, QixNode right) => left.Equals(right);

    /// <summary>
    /// Determines in-equality of the two nodes.
    /// </summary>
    /// <param name="left">The LHS.</param>
    /// <param name="right">The RHS.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator !=(QixNode left, QixNode right) => !(left == right);

    /// <summary>
    /// Reads the QIX node.
    /// </summary>
    /// <param name="stream">The binary reader.</param>
    /// <param name="isLittleEndian">The byte order.</param>
    /// <returns>The QIX node.</returns>
    public static QixNode ReadFrom(Stream stream, bool isLittleEndian)
    {
        if ((stream.Length - stream.Position) < 40L)
        {
            return QixNode.Empty;
        }

#if NETSTANDARD2_1_OR_GREATER
        Span<byte> span = stackalloc byte[40];
        _ = stream.Read(span);
#else
        var bytes = new byte[40];
        _ = stream.Read(bytes, 0, bytes.Length);
        ReadOnlySpan<byte> span = bytes;
#endif

        _ = ReadInt32(span[..4], isLittleEndian);
        var minX = ReadDouble(span[4..12], isLittleEndian);
        var minY = ReadDouble(span[12..20], isLittleEndian);
        var maxX = ReadDouble(span[20..28], isLittleEndian);
        var maxY = ReadDouble(span[28..36], isLittleEndian);
        var shapeCount = ReadInt32(span[36..40], isLittleEndian);

        var ids = ReadNodes(stream, shapeCount, isLittleEndian);

#if NETSTANDARD2_1_OR_GREATER
        _ = stream.Read(span[..4]);
#else
        _ = stream.Read(bytes, 0, 4);
#endif
        var nodeCount = ReadInt32(span, isLittleEndian);
        var nodes = new QixNode[nodeCount];
        for (var i = 0; i < nodeCount; i++)
        {
            nodes[i] = ReadFrom(stream, isLittleEndian);
        }

        return new()
        {
            Extents = new(minX, minY, maxX, maxY),
            Shapes = ids,
            Nodes = nodes,
        };

        static int ReadInt32(ReadOnlySpan<byte> span, bool isLittleEndian)
        {
            return isLittleEndian ? ReadInt32LittleEndian(span) : ReadInt32BigEndian(span);
        }

        static double ReadDouble(ReadOnlySpan<byte> span, bool isLittleEndian)
        {
            return BitConverter.Int64BitsToDouble(isLittleEndian ? ReadInt64LittleEndian(span) : ReadInt64BigEndian(span));
        }

        static int[] ReadNodes(Stream stream, int shapeCount, bool isLittleEndian)
        {
#if NETSTANDARD2_1_OR_GREATER
            Span<byte> span = stackalloc byte[shapeCount * sizeof(int)];
            _ = stream.Read(span);
#else
            var bytes = new byte[shapeCount * sizeof(int)];
            _ = stream.Read(bytes, 0, bytes.Length);
            ReadOnlySpan<byte> span = bytes;
#endif

            var ids = new int[shapeCount];
            for (var i = 0; i < shapeCount; i++)
            {
                ids[i] = ReadInt32(span, isLittleEndian);
            }

            return ids;
        }
    }

    /// <summary>
    /// Creates a new instance of <see cref="QixNode"/> based on the shapefile reader, and depth.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="depth">The depth to use. If this is zero, then the depth will be calculated.</param>
    /// <returns>The <see cref="QixNode"/>.</returns>
    public static QixNode FromShapefile(ShapefileReader reader, int depth = 0) => FromShapefile(reader.GetShpReader(), reader.GetShxReader(), depth);

    /// <summary>
    /// Creates a new instance of <see cref="QixNode"/> based on the shapefile readers, and depth.
    /// </summary>
    /// <param name="shpReader">The SHP reader.</param>
    /// <param name="shxReader">The SHX reader.</param>
    /// <param name="depth">The depth to use. If this is zero, then the depth will be calculated.</param>
    /// <returns>The <see cref="QixNode"/>.</returns>
    public static QixNode FromShapefile(ShpReader shpReader, ShxReader shxReader, int depth = 0)
    {
        if (depth == default)
        {
            depth = CalculateDepth(shxReader.Count);
        }

        var rootNode = new MutableNode(new(shpReader.Header.Extents.Left, shpReader.Header.Extents.Bottom, shpReader.Header.Extents.Right, shpReader.Header.Extents.Top));

        while (GetRecord() is { } record)
        {
            _ = AddShapeToNode(rootNode, (record.RecordHeader.RecordNumber, record.GetExtents()), depth);
        }

        _ = Trim(rootNode);

        return rootNode.AsReadOnly();

        ShpRecord? GetRecord()
        {
            var shxRecord = shxReader.Read();
            return shxRecord.HasValue ? shpReader.Read(shxRecord.Value) : default;
        }

        static bool AddShapeToNode(MutableNode node, (int Id, Envelope Extents) record, int maxDepth)
        {
            if (maxDepth > 1 && node.Nodes.Count > 0)
            {
                if (node.Nodes.Find(subNode => NodeContains(subNode.Extents, record.Extents)) is { } subNode)
                {
                    return AddShapeToNode(subNode, record, maxDepth - 1);
                }
            }
            else if (maxDepth > 1 && node.Nodes.Count is 0)
            {
                var (tempFirst, tempSecond) = Split(node.Extents);
                var (first, second) = Split(tempFirst);
                var (third, forth) = Split(tempSecond);

                if (NodeContains(first, record.Extents)
                    || NodeContains(second, record.Extents)
                    || NodeContains(third, record.Extents)
                    || NodeContains(forth, record.Extents))
                {
                    node.Nodes.Add(new(first));
                    node.Nodes.Add(new(second));
                    node.Nodes.Add(new(third));
                    node.Nodes.Add(new(forth));

                    // recurse back on this node now that it has subnodes
                    return AddShapeToNode(node, record, maxDepth);
                }
            }

            node.Shapes.Add(record.Id - 1);

            return true;

            static bool NodeContains(Envelope nodeExtents, Envelope recordExtents)
            {
                return recordExtents.Left >= nodeExtents.Left && recordExtents.Right <= nodeExtents.Right && recordExtents.Bottom >= nodeExtents.Bottom && recordExtents.Top <= nodeExtents.Top;
            }
        }
    }

    /// <summary>
    /// Calculates the depth, based on the number of items.
    /// </summary>
    /// <param name="reader">The shapefile reader.</param>
    /// <returns>The calculated depth.</returns>
    public static int CalculateDepth(ShapefileReader reader) => CalculateDepth(reader.Count);

    /// <summary>
    /// Calculates the depth, based on the number of items.
    /// </summary>
    /// <param name="reader">The SHX reader.</param>
    /// <returns>The calculated depth.</returns>
    public static int CalculateDepth(ShxReader reader) => CalculateDepth(reader.Count);

    /// <summary>
    /// Calculates the depth, based on the number of items.
    /// </summary>
    /// <param name="count">The count.</param>
    /// <returns>The calculated depth.</returns>
    public static int CalculateDepth(int count)
    {
        var maxNodeCount = 1;
        var maxDepth = 0;

        while (maxNodeCount * 4 < count)
        {
            maxDepth++;
            maxNodeCount *= 2;
        }

        return maxDepth;
    }

    /// <summary>
    /// Gets the IDs for the specified envelope.
    /// </summary>
    /// <param name="envelope">The envelope.</param>
    /// <returns>The IDs for envelope.</returns>
    public readonly IEnumerable<int> GetIds(Envelope envelope) => this.Extents.IntersectsWith(envelope)
        ? this.Shapes.Concat(this.Nodes.SelectMany(node => node.GetIds(envelope)))
        : [];

    /// <inheritdoc/>
    public override readonly bool Equals([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] object? obj) => obj is QixNode node && this.Equals(node);

    /// <inheritdoc/>
    public readonly bool Equals(QixNode other) => this.Shapes.Count == other.Shapes.Count
        && this.Nodes.Count == other.Nodes.Count
        && this.Extents == other.Extents
        && this.Shapes.SequenceEqual(other.Shapes)
        && this.Nodes.SequenceEqual(other.Nodes);

    /// <inheritdoc/>
    public override readonly int GetHashCode() => HashCode.Combine(this.Extents, this.Shapes, this.Nodes);

    private static bool Trim(MutableNode node)
    {
        // Trim subtrees, and free subnodes that come back empty.
        var i = 0;
        while (i < node.Nodes.Count)
        {
            if (Trim(node.Nodes[i]))
            {
                var last = node.Nodes.Count - 1;
                node.Nodes[i] = node.Nodes[last];
                node.Nodes.RemoveAt(last);
                continue;
            }

            i++;
        }

        // If the current node has 1 subnode and no shapes, promote that subnode to the current node position.
        if (node.Nodes.Count is 1 && node.Shapes.Count is 0)
        {
            var subNode = node.Nodes[0];
            node.UpdateExtents(subNode.Extents);
            node.Shapes.Clear();
            node.Shapes.AddRange(subNode.Shapes);
            node.Nodes.Clear();
            node.Nodes.AddRange(subNode.Nodes);
        }

        // We should be trimmed if we have no subnodes, and no shapes.
        return node.Nodes.Count is 0 && node.Shapes.Count is 0;
    }

    private static (Envelope First, Envelope Second) Split(Envelope envelope)
    {
        if (envelope.Width > envelope.Height)
        {
            var range = envelope.Width;
            return (
                new(envelope.Left, envelope.Bottom, envelope.Left + (range * SplitRatio), envelope.Top),
                new(envelope.Right - (range * SplitRatio), envelope.Bottom, envelope.Right, envelope.Top));
        }
        else
        {
            var range = envelope.Height;
            return (
                new(envelope.Left, envelope.Bottom, envelope.Right, envelope.Bottom + (range * SplitRatio)),
                new(envelope.Left, envelope.Top - (range * SplitRatio), envelope.Right, envelope.Top));
        }
    }

    private sealed record MutableNode(Envelope Extents)
    {
        public Envelope Extents { get; private set; } = Extents;

        public List<int> Shapes { get; } = [];

        public List<MutableNode> Nodes { get; } = [];

        public QixNode AsReadOnly() => new()
        {
            Extents = this.Extents,
            Shapes = this.Shapes.AsReadOnly(),
            Nodes = this.Nodes.ConvertAll(static node => node.AsReadOnly()).AsReadOnly(),
        };

        public void UpdateExtents(Envelope extents) => this.Extents = extents;
    }
}