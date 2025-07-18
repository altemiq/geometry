// -----------------------------------------------------------------------
// <copyright file="QixHeader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

/// <summary>
/// The QIX header.
/// </summary>
public class QixHeader
{
    /// <summary>
    /// The header size.
    /// </summary>
    public const int Size = 8;

    /// <summary>
    /// The file code.
    /// </summary>
    public const string FileCode = "SQT";

    /// <summary>
    /// The version.
    /// </summary>
    public const byte Version = 1;

    private static readonly byte[] FileCodeBytes = System.Text.Encoding.UTF8.GetBytes(FileCode);

    /// <summary>
    /// Gets a value indicating whether the multibyte types are little endian.
    /// </summary>
    public bool IsLittleEndian { get; init; }

    /// <summary>
    /// Gets the number of shapes in the tree.
    /// </summary>
    public int Count { get; init; }

    /// <summary>
    /// Gets the depth of the tree.
    /// </summary>
    public int Depth { get; init; }

    /// <summary>
    /// Reads the QIX header.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="readFileCode">Set to <see langword="false"/> to ignore the file code.</param>
    /// <returns>The QIX header.</returns>
    public static QixHeader ReadFrom(Stream stream, bool readFileCode = true)
    {
        if (readFileCode)
        {
            if (stream.Length - stream.Position < Size)
            {
                throw new Data.InsufficientDataException
                {
                    RequiredDataLength = Size,
                    ActualDataLength = stream.Length - stream.Position,
                };
            }

            var fileCodeBytes = new byte[3];
            if (stream.Read(fileCodeBytes, 0, fileCodeBytes.Length) is not 3 || !fileCodeBytes.SequenceEqual(FileCodeBytes))
            {
                throw new System.IO.InvalidDataException { Data = { { "Name", nameof(FileCode) }, { "Expected", FileCode }, { "Actual", System.Text.Encoding.UTF8.GetString(fileCodeBytes) } } };
            }
        }
        else if (stream.Length - stream.Position < Size - FileCode.Length)
        {
            throw new Data.InsufficientDataException
            {
                RequiredDataLength = Size - FileCode.Length,
                ActualDataLength = stream.Length - stream.Position,
            };
        }

        var isLittleEndian = stream.ReadByte() switch
        {
            1 => true,
            2 => false,
            0 when BitConverter.IsLittleEndian => true,
            0 => false,
            _ => throw new InvalidOperationException(),
        };

        var version = stream.ReadByte();
        if (version is not Version)
        {
            throw new InvalidDataException { Data = { { "Name", nameof(Version) }, { "Expected", Version }, { "Actual", version } } };
        }

        if (stream.CanSeek)
        {
            stream.Position += 3;
        }
        else
        {
            var reserved = new byte[3];
            _ = stream.Read(reserved, 0, reserved.Length);
        }

#if NETSTANDARD2_1_OR_GREATER
        Span<byte> span = stackalloc byte[8];
        _ = stream.Read(span);
#else
        var data = new byte[8];
        _ = stream.Read(data, 0, 8);
        ReadOnlySpan<byte> span = data;
#endif

        var count = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[..4]);
        var depth = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[4..8]);

        return new()
        {
            IsLittleEndian = isLittleEndian,
            Count = count,
            Depth = depth,
        };
    }
}