// -----------------------------------------------------------------------
// <copyright file="TinyWkbRecordHeader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

/// <summary>
/// The Tiny WKT record header.
/// </summary>
public readonly struct TinyWkbRecordHeader
{
    private readonly int value;

    /// <summary>
    /// Initialises a new instance of the <see cref="TinyWkbRecordHeader"/> struct.
    /// </summary>
    /// <param name="geometryType">The geometry type.</param>
    /// <param name="precisionXY">The number of decimal places for x- and y-ordinate values. A negative value is allowed.</param>
    /// <param name="hasEmptyGeometry">A flag indicating that the geometry is empty. If <see langword="true"/> some input parameters are overridden.</param>
    /// <param name="hasBoundingBox">A flag indicating that the bounding box information should be written.</param>
    /// <param name="hasSize">A flag indicating that the size of the geometry data is part of the header.</param>
    /// <param name="hasIdList">A flag indicating that an id-list is written. This applies to multi-geometries only.</param>
    /// <param name="precisionZ">The number of decimal places for z-ordinate values.</param>
    /// <param name="precisionM">The number of decimal places for m-ordinate values.</param>
    public TinyWkbRecordHeader(
        TinyWkbGeometryType geometryType,
        int precisionXY = 7,
        bool hasEmptyGeometry = false,
        bool hasBoundingBox = true,
        bool hasSize = false,
        bool hasIdList = false,
        int? precisionZ = default,
        int? precisionM = default)
        : this()
    {
        if (precisionXY is < -7 or > 7)
        {
            throw new ArgumentOutOfRangeException(nameof(precisionXY));
        }

        if (precisionZ is < 0 or > 7)
        {
            throw new ArgumentOutOfRangeException(nameof(precisionZ));
        }

        if (precisionM is < 0 or > 7)
        {
            throw new ArgumentOutOfRangeException(nameof(precisionM));
        }

        // encode xy precision.
        var p = (int)VarIntBitConverter.EncodeZigZag(precisionXY, 4);

        // We don't write bounding boxes for points.
        if (geometryType is TinyWkbGeometryType.Point)
        {
            hasBoundingBox = false;
        }

        // No id-lists, for single instance geometries
        if (geometryType < TinyWkbGeometryType.MultiPoint)
        {
            hasIdList = false;
        }

        var metadata = 0;
        if (hasBoundingBox)
        {
            metadata |= (int)Metadata.HasBoundingBox;
        }

        if (hasSize)
        {
            metadata |= (int)Metadata.HasSize;
        }

        if (hasIdList)
        {
            metadata |= (int)Metadata.HasIdList;
        }

        if (precisionZ.HasValue || precisionM.HasValue)
        {
            metadata |= (int)Metadata.HasExtendedPrecisionInformation;
        }

        if (hasEmptyGeometry)
        {
            metadata |= (int)Metadata.HasEmptyGeometry;
        }

        if (precisionZ.HasValue)
        {
            metadata |= (int)Metadata.HasZ;
            metadata |= (0x07 & precisionZ.Value) << 18;
        }

        if (precisionM.HasValue)
        {
            metadata |= (int)Metadata.HasM;
            metadata |= (0x07 & precisionM.Value) << 21;
        }

        this.value = (int)geometryType | (p << 4) | metadata;
    }

    private TinyWkbRecordHeader(int header, int metadata, int epi = 0) => this.value = header | (metadata << 8) | (epi << 16);

    [Flags]
    private enum Metadata
    {
        None = 0,
        HasBoundingBox = 1 << 8,
        HasSize = 1 << 9,
        HasIdList = 1 << 10,
        HasExtendedPrecisionInformation = 1 << 11,
        HasEmptyGeometry = 1 << 12,
        HasZ = 1 << 16,
        HasM = 1 << 17,
    }

    /// <summary>
    /// Gets the geometry type.
    /// </summary>
    public TinyWkbGeometryType GeometryType => (TinyWkbGeometryType)(this.value & 0x0F);

    /// <summary>
    /// Gets the precision for the X, Y values.
    /// </summary>
    public int PrecisionXY => (int)VarIntBitConverter.DecodeZigZag((ulong)((this.value & 0xF0) >> 4));

    /// <summary>
    /// Gets a value indicating whether this instance has a bounding box.
    /// </summary>
    public bool HasBoundingBox => (this.value & (int)Metadata.HasBoundingBox) is not 0;

    /// <summary>
    /// Gets a value indicating whether this instance has size.
    /// </summary>
    public bool HasSize => (this.value & (int)Metadata.HasSize) is not 0;

    /// <summary>
    /// Gets a value indicating whether this instance has an ID list.
    /// </summary>
    public bool HasIdList => (this.value & (int)Metadata.HasIdList) is not 0;

    /// <summary>
    /// Gets a value indicating whether this instance has extended precision information.
    /// </summary>
    public bool HasExtendedPrecisionInformation => (this.value & (int)Metadata.HasExtendedPrecisionInformation) is not 0;

    /// <summary>
    /// Gets a value indicating whether this is empty.
    /// </summary>
    public bool HasEmptyGeometry => (this.value & (int)Metadata.HasEmptyGeometry) is not 0;

    /// <summary>
    /// Gets a value indicating whether this instance has Z values.
    /// </summary>
    public bool HasZ => (this.value & (int)Metadata.HasZ) is not 0;

    /// <summary>
    /// Gets a value indicating whether this instance has M values.
    /// </summary>
    public bool HasM => (this.value & (int)Metadata.HasM) is not 0;

    /// <summary>
    /// Gets a value for the Z precision.
    /// </summary>
    public int PrecisionZ => 0x07 & (this.value >> 18);

    /// <summary>
    /// Gets a value for the M precision.
    /// </summary>
    public int PrecisionM => 0x07 & (this.value >> 21);

    /// <summary>
    /// Reads the header from the stream.
    /// </summary>
    /// <param name="stream">The binary stream.</param>
    /// <returns>The header.</returns>
    public static TinyWkbRecordHeader Read(Stream stream)
    {
        var header = stream.ReadByte();
        var metadata = stream.ReadByte();
        return ((metadata << 8) & (int)Metadata.HasExtendedPrecisionInformation) is not 0
            ? new(header, metadata, stream.ReadByte())
            : new(header, metadata);
    }

    /// <summary>
    /// Writes the header to the specified binary writer.
    /// </summary>
    /// <param name="writer">The binary writer to use.</param>
    /// <param name="header">The header.</param>
    public static void Write(BinaryWriter writer, TinyWkbRecordHeader header)
    {
        writer.Write((ushort)(0xffff & header.value));
        if (header.HasExtendedPrecisionInformation)
        {
            writer.Write((byte)(0xFF & (header.value >> 16)));
        }
    }
}