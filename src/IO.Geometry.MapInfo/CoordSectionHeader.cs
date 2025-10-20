// -----------------------------------------------------------------------
// <copyright file="CoordSectionHeader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.MapInfo;

/// <summary>
/// The coordinate section header.
/// </summary>
/// <param name="data">The data.</param>
/// <param name="start">The start index within the data.</param>
/// <param name="version">The version.</param>
/// <param name="centerX">The optional center-x value.</param>
/// <param name="centerY">The optional center-y value.</param>
public readonly struct CoordSectionHeader(byte[] data, int start, int version, int centerX = default, int centerY = default)
{
    private readonly int startOfMbr = start + GetMbrOffset(version);

    /// <summary>
    /// Gets the number of vertices.
    /// </summary>
    public int NumberOfVertices => version >= 450 ? System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(start)) : System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(data.AsSpan(start));

    /// <summary>
    /// Gets the number of holes.
    /// </summary>
    public int NumberOfHoles => version >= 800 ? System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(start + 4)) : System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(data.AsSpan(start + 2));

    /// <summary>
    /// Gets the minimum x-value.
    /// </summary>
    public int MinX => centerX is 0 ? System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(this.startOfMbr)) : Point.Read(data.AsSpan(this.startOfMbr), centerX);

    /// <summary>
    /// Gets the minimum y-value.
    /// </summary>
    public int MinY => centerY is 0 ? System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(this.startOfMbr + 4)) : Point.Read(data.AsSpan(this.startOfMbr + 2), centerX);

    /// <summary>
    /// Gets the maximum x-value.
    /// </summary>
    public int MaxX => centerX is 0 ? System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(this.startOfMbr + 8)) : Point.Read(data.AsSpan(this.startOfMbr + 4), centerX);

    /// <summary>
    /// Gets the maximum y-value.
    /// </summary>
    public int MaxY => centerY is 0 ? System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(this.startOfMbr + 12)) : Point.Read(data.AsSpan(this.startOfMbr + 6), centerX);

    /// <summary>
    /// Gets the data offset.
    /// </summary>
    public int DataOffset { get => System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(field)); } = start + GetMbrOffset(version) + GetMbrSize(centerX);

    private static int GetMbrOffset(int version) => version switch
    {
        >= 800 => 8,
        >= 450 => 6,
        _ => 4,
    };

    private static int GetMbrSize(int compressed) => compressed is 0 ? 16 : 8;
}