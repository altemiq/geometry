// -----------------------------------------------------------------------
// <copyright file="HeaderBlock.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.MapInfo;

/// <summary>
/// The header block.
/// </summary>
/// <param name="data">The data.</param>
internal readonly struct HeaderBlock(byte[] data)
{
    private const int MagicCookie = 42424242;

    private readonly byte[] data = data[0] is 0 && System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(0x100)) is MagicCookie ? data : throw new InvalidOperationException();

    /// <summary>
    /// Gets the map version number.
    /// </summary>
    public short MapVersionNumber => System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(this.data.AsSpan(0x104));

    /// <summary>
    /// Gets the regular block size.
    /// </summary>
    public short RegularBlockSize => System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(this.data.AsSpan(0x106));

    /// <summary>
    /// Gets the coordinate system 2 distance units.
    /// </summary>
    public double Coordsys2DistUnits => System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(this.data.AsSpan(0x108));

    /// <summary>
    /// Gets the minimum x-value.
    /// </summary>
    public int MinX => System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(this.data.AsSpan(0x110));

    /// <summary>
    /// Gets the minimum y-value.
    /// </summary>
    public int MinY => System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(this.data.AsSpan(0x114));

    /// <summary>
    /// Gets the maximum x-value.
    /// </summary>
    public int MaxX => System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(this.data.AsSpan(0x118));

    /// <summary>
    /// Gets the maximum y-value.
    /// </summary>
    public int MaxY => System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(this.data.AsSpan(0x11C));

    /// <summary>
    /// Gets the first index block.
    /// </summary>
    public int FirstIndexBlock => System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(this.data.AsSpan(0x130));

    /// <summary>
    /// Gets the first garbage block.
    /// </summary>
    public int FirstGarbageBlock => System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(this.data.AsSpan(0x134));

    /// <summary>
    /// Gets the first tool block.
    /// </summary>
    public int FirstToolBlock => System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(this.data.AsSpan(0x138));

    /// <summary>
    /// Gets the point objects.
    /// </summary>
    public int PointObjects => System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(this.data.AsSpan(0x13C));

    /// <summary>
    /// Gets the line objects.
    /// </summary>
    public int LineObjects => System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(this.data.AsSpan(0x140));

    /// <summary>
    /// Gets the region objects.
    /// </summary>
    public int RegionObjects => System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(this.data.AsSpan(0x144));

    /// <summary>
    /// Gets the text objects.
    /// </summary>
    public int TextObjects => System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(this.data.AsSpan(0x148));

    /// <summary>
    /// Gets the maximum coordinate buffer size.
    /// </summary>
    public int MaxCoordBufSize => System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(this.data.AsSpan(0x14C));

    /// <summary>
    /// Gets the distance units code.
    /// </summary>
    public byte DistUnitsCode => this.data[0x15E];

    /// <summary>
    /// Gets the maximum SP index depth.
    /// </summary>
    public byte MaxSpIndexDepth => this.data[0x15F];

    /// <summary>
    /// Gets the coordinate precision.
    /// </summary>
    public byte CoordPrecision => this.data[0x160];

    /// <summary>
    /// Gets the coordinate origin quadrant.
    /// </summary>
    public byte CoordOriginQuadrant => this.data[0x161];

    /// <summary>
    /// Gets the reflection x-axis coordinate.
    /// </summary>
    public byte ReflectXAxisCoord => this.data[0x162];

    /// <summary>
    /// Gets the maximum object length array ID.
    /// </summary>
    public byte MaxObjLenArrayId => this.data[0x163];

    /// <summary>
    /// Gets the pen definitions.
    /// </summary>
    public byte PenDefs => this.data[0x164];

    /// <summary>
    /// Gets the brush definitions.
    /// </summary>
    public byte BrushDefs => this.data[0x165];

    /// <summary>
    /// Gets the symbol definitions.
    /// </summary>
    public byte SymbolDefs => this.data[0x166];

    /// <summary>
    /// Gets the font definitions.
    /// </summary>
    public byte FontDefs => this.data[0x167];

    /// <summary>
    /// Gets the map tool blocks.
    /// </summary>
    public byte MapToolBlocks => this.data[0x168];

    /// <summary>
    /// Gets the datum ID.
    /// </summary>
    public short DatumId => this.MapVersionNumber >= 500 ? System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(this.data.AsSpan(0x16A)) : default;

    /// <summary>
    /// Gets the projection ID.
    /// </summary>
    public byte ProjId => this.data[0x16D];

    /// <summary>
    /// Gets the ellipsoid ID.
    /// </summary>
    public byte EllipsoidId => this.data[0x16E];

    /// <summary>
    /// Gets the unit ID.
    /// </summary>
    public byte UnitsId => this.data[0x16F];

    /// <summary>
    /// Gets the scale for the x-values.
    /// </summary>
    public double ScaleX => System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(this.data.AsSpan(0x170));

    /// <summary>
    /// Gets the scale for the y-values.
    /// </summary>
    public double ScaleY => System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(this.data.AsSpan(0x178));

    /// <summary>
    /// Gets the displacement for the x-values.
    /// </summary>
    public double DisplX => System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(this.data.AsSpan(0x180));

    /// <summary>
    /// Gets the displacement for the y-values.
    /// </summary>
    public double DisplY => System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(this.data.AsSpan(0x188));

    /// <summary>
    /// Gets the projection parameters.
    /// </summary>
    public double[] ProjParams =>
    [
        System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(this.data.AsSpan(0x190)),
        System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(this.data.AsSpan(0x198)),
        System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(this.data.AsSpan(0x1A0)),
        System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(this.data.AsSpan(0x1A8)),
        System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(this.data.AsSpan(0x1B0)),
        this.ProjId == 35 && this.MapVersionNumber > 500 && this.data.Length > (0x268 + 8) ? System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(this.data.AsSpan(0x268)) : System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(this.data.AsSpan(0x1B8)),
    ];

    /// <summary>
    /// Gets the datum shift for the x-values.
    /// </summary>
    public double DatumShiftX => System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(this.data.AsSpan(0x1C0));

    /// <summary>
    /// Gets the datum shift for the y-values.
    /// </summary>
    public double DatumShiftY => System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(this.data.AsSpan(0x1C8));

    /// <summary>
    /// Gets the datum shift for the z-values.
    /// </summary>
    public double DatumShiftZ => System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(this.data.AsSpan(0x1D0));

    /// <summary>
    /// Gets the datum parameters.
    /// </summary>
    public double[] DatumParams =>
    [
        System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(this.data.AsSpan(0x1D8)),
        System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(this.data.AsSpan(0x1E0)),
        System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(this.data.AsSpan(0x1E8)),
        System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(this.data.AsSpan(0x1F0)),
        System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(this.data.AsSpan(0x1F8)),
    ];

    /// <summary>
    /// Gets the affine flag.
    /// </summary>
    public byte AffineFlag => this.MapVersionNumber >= 500 && this.data.Length > TabReader.TabMinBlockSize && this.data[0x200] is not 0 ? (byte)1 : default;

    /// <summary>
    /// Gets the affine units.
    /// </summary>
    public byte AffineUnits => this.MapVersionNumber >= 500 && this.data.Length > TabReader.TabMinBlockSize ? this.data[0x201] : default;

    /// <summary>
    /// Gets the affine parameters.
    /// </summary>
    public double[] AffineParamA
    {
        get
        {
            if (this.MapVersionNumber >= 500 && this.data.Length > TabReader.TabMinBlockSize)
            {
                return [
                    System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(this.data.AsSpan(0x208)),
                    System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(this.data.AsSpan(0x210)),
                    System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(this.data.AsSpan(0x218)),
                    System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(this.data.AsSpan(0x220)),
                    System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(this.data.AsSpan(0x228)),
                    System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(this.data.AsSpan(0x230)),
                ];
            }

            return new double[6];
        }
    }

    /// <summary>
    /// Gets the precision for the x-values.
    /// </summary>
    public double PrecisionX => Math.Pow(10, Math.Round(Math.Log10(this.ScaleX), MidpointRounding.ToEven));

    /// <summary>
    /// Gets the precision for the y-values.
    /// </summary>
    public double PrecisionY => Math.Pow(10, Math.Round(Math.Log10(this.ScaleY), MidpointRounding.ToEven));
}