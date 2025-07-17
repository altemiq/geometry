// -----------------------------------------------------------------------
// <copyright file="Header.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

/// <summary>
/// The SHP/SHX header.
/// </summary>
public class Header
{
    /// <summary>
    /// The header size.
    /// </summary>
    public const int Size = 100;

    /// <summary>
    /// The header length.
    /// </summary>
    public const int Length = Size / 2;

    /// <summary>
    /// The file code.
    /// </summary>
    public const int FileCode = 9994;

    /// <summary>
    /// The version.
    /// </summary>
    public const int Version = 1000;

    /// <summary>
    /// Initialises a new instance of the <see cref="Header"/> class.
    /// </summary>
    /// <param name="type">The SHP type.</param>
    public Header(ShpType type)
        : this(type, 0D, 0D, 0D, 0D)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="Header"/> class.
    /// </summary>
    /// <param name="type">The SHP type.</param>
    /// <param name="minX">The minimum x-value.</param>
    /// <param name="maxX">The maximum x-value.</param>
    /// <param name="minY">The minimum y-value.</param>
    /// <param name="maxY">The maximum y-value.</param>
    /// <param name="minZ">The minimum z-value.</param>
    /// <param name="maxZ">The maximum z-value.</param>
    /// <param name="minM">The minimum m-value.</param>
    /// <param name="maxM">The maximum m-value.</param>
    public Header(ShpType type, double minX, double maxX, double minY, double maxY, double minZ = 0D, double maxZ = 0D, double minM = 0D, double maxM = 0D)
        : this(0, type, minX, maxX, minY, maxY, minZ, maxZ, minM, maxM)
    {
    }

    private Header(uint fileLength, ShpType type, double minX, double maxX, double minY, double maxY, double minZ, double maxZ, double minM, double maxM)
    {
        this.FileLength = fileLength;
        this.ShpType = type;
        this.Extents = new(minX, minY, minZ, minM, maxX, maxY, maxZ, maxM);
    }

    /// <summary>
    /// Gets the SHP type.
    /// </summary>
    public ShpType ShpType { get; }

    /// <summary>
    /// Gets the extents.
    /// </summary>
    public EnvelopeZM Extents { get; }

    /// <summary>
    /// Gets the file length.
    /// </summary>
    internal uint FileLength { get; }

    /// <summary>
    /// Reads the SHP/SHX header.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="readFileCode">Set to <see langword="false"/> to ignore the file code.</param>
    /// <returns>The SHP/SHX header.</returns>
    public static Header ReadFrom(Stream stream, bool readFileCode = true)
    {
        var size = Size;
        if (!readFileCode)
        {
            size -= sizeof(int);
        }

        var data = new byte[size];

        var bytesRead = stream.Read(data, 0, size);
        if (bytesRead != size)
        {
            throw new InsufficientDataException
            {
                RequiredDataLength = size,
                ActualDataLength = bytesRead,
            };
        }

        var span = new ReadOnlySpan<byte>(data);
        if (readFileCode)
        {
            var fileCode = System.Buffers.Binary.BinaryPrimitives.ReadInt32BigEndian(span);
            if (fileCode is not FileCode)
            {
                throw new InvalidDataException
                {
                    Name = nameof(FileCode),
                    Expected = FileCode,
                    Actual = fileCode,
                };
            }

            span = span[4..];
        }

        var fileLength = System.Buffers.Binary.BinaryPrimitives.ReadUInt32BigEndian(span[20..24]); // file length
        var version = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[24..28]);
        if (version is not Version)
        {
            throw new InvalidDataException
            {
                Name = nameof(Version),
                Expected = Version,
                Actual = version,
            };
        }

        var shapeType = (ShpType)System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[28..32]);
        var minX = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(span[32..40]));
        var minY = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(span[40..48]));
        var maxX = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(span[48..56]));
        var maxY = BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(span[56..64]));
        var minZ = CheckForNoData(BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(span[64..72])));
        var maxZ = CheckForNoData(BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(span[72..80])));
        var minM = CheckForNoData(BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(span[80..88])));
        var maxM = CheckForNoData(BitConverter.Int64BitsToDouble(System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(span[88..96])));

        return new(fileLength, shapeType, minX, maxX, minY, maxY, minZ, maxZ, minM, maxM);

        static double CheckForNoData(double value)
        {
            return value < Constants.NoDataLimit ? Constants.NoData : value;
        }
    }

    /// <summary>
    /// Copies this instance to the specified stream.
    /// </summary>
    /// <param name="stream">The stream to copy to.</param>
    public void CopyTo(Stream stream)
    {
        Span<byte> span = stackalloc byte[Size];

        System.Buffers.Binary.BinaryPrimitives.WriteInt32BigEndian(span[..4], FileCode);
        System.Buffers.Binary.BinaryPrimitives.WriteUInt32BigEndian(span[24..28], this.FileLength);
        System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(span[28..32], Version);
        System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(span[32..36], (int)this.ShpType);
        System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[36..44], BitConverter.DoubleToInt64Bits(this.Extents.Left));
        System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[44..52], BitConverter.DoubleToInt64Bits(this.Extents.Bottom));
        System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[52..60], BitConverter.DoubleToInt64Bits(this.Extents.Right));
        System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(span[60..68], BitConverter.DoubleToInt64Bits(this.Extents.Top));
        BinaryPrimitives.WriteDoubleLittleEndianIfNotNan(span[68..76], this.Extents.Front, 0D);
        BinaryPrimitives.WriteDoubleLittleEndianIfNotNan(span[76..84], this.Extents.Back, 0D);
        BinaryPrimitives.WriteDoubleLittleEndianIfNotNan(span[84..92], this.Extents.Start, 0D);
        BinaryPrimitives.WriteDoubleLittleEndianIfNotNan(span[92..100], this.Extents.End, 0D);

        stream.Write(span);
    }
}