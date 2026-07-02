// -----------------------------------------------------------------------
// <copyright file="WktSerializer.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.Geodesy;

/// <summary>
/// The WKT serializer.
/// </summary>
public static class WktSerializer
{
    /// <summary>
    /// Reads one WKT value from the provided stream into a <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TValue">The target type of the WKT value.</typeparam>
    /// <param name="stream">The stream containing the WKT.</param>
    /// <param name="options">Options to control the behavior during parsing.</param>
    /// <returns>A <typeparamref name="TValue"/> representation of the WKT value.</returns>
    public static TValue? Deserialize<TValue>(Stream stream, WktSerializerOptions? options = default)
    {
        var bytes = System.Buffers.ArrayPool<byte>.Shared.Rent((int)stream.Length);

        try
        {
            var bytesRead = stream.Read(bytes, 0, bytes.Length);
            return Deserialize<TValue>(bytes.AsSpan(0, bytesRead), options);
        }
        finally
        {
            System.Buffers.ArrayPool<byte>.Shared.Return(bytes);
        }
    }

    /// <summary>
    /// Reads one WKT value from the provided string into a <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TValue">The target type of the WKT value.</typeparam>
    /// <param name="wkt">The WKT text to parse.</param>
    /// <param name="options">Options to control the behavior during parsing.</param>
    /// <returns>A <typeparamref name="TValue"/> representation of the WKT value.</returns>
    public static TValue? Deserialize<TValue>(string wkt, WktSerializerOptions? options = default) => Deserialize<TValue>(wkt.AsSpan(), options);

    /// <summary>
    /// Reads one WKT value from the provided span into a <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TValue">The target type of the WKT value.</typeparam>
    /// <param name="wkt">The WKT text to parse.</param>
    /// <param name="options">Options to control the behavior during parsing.</param>
    /// <returns>A <typeparamref name="TValue"/> representation of the WKT value.</returns>
    public static TValue? Deserialize<TValue>(ReadOnlySpan<char> wkt, WktSerializerOptions? options = default)
    {
        options ??= WktSerializerOptions.Default;

        return options.GetConverter(typeof(TValue)) switch
        {
            Serialization.WktConverter<TValue> converter => Deserialize(converter, wkt, options),
            null => throw new InvalidOperationException($"No converter found for type {typeof(TValue)}"),
            _ => throw new InvalidOperationException($"Converter for type {typeof(TValue)} is not a WktConverter<{typeof(TValue)}>"),
        };

        static TValue? Deserialize(Serialization.WktConverter<TValue> converter, ReadOnlySpan<char> wkt, WktSerializerOptions options)
        {
            var array = System.Buffers.ArrayPool<byte>.Shared.Rent(System.Text.Encoding.UTF8.GetMaxByteCount(wkt.Length));

            try
            {
                System.Text.Encoding.UTF8.GetBytes(wkt, array);

                var reader = new Text.Geodesy.Utf8WktReader(array);

                return reader.Read()
                    ? converter.Read(ref reader, typeof(TValue), options)
                    : default;
            }
            finally
            {
                System.Buffers.ArrayPool<byte>.Shared.Return(array);
            }
        }
    }

    /// <summary>
    /// Reads one WKT value from the provided span into a <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TValue">The target type of the WKT value.</typeparam>
    /// <param name="wkt">The WKT text to parse.</param>
    /// <param name="options">Options to control the behavior during parsing.</param>
    /// <returns>A <typeparamref name="TValue"/> representation of the WKT value.</returns>
    public static TValue? Deserialize<TValue>(ReadOnlySpan<byte> wkt, WktSerializerOptions? options = default)
    {
        options ??= WktSerializerOptions.Default;

        return options.GetConverter(typeof(TValue)) switch
        {
            Serialization.WktConverter<TValue> converter => Deserialize(converter, wkt, options),
            null => throw new InvalidOperationException($"No converter found for type {typeof(TValue)}"),
            _ => throw new InvalidOperationException($"Converter for type {typeof(TValue)} is not a WktConverter<{typeof(TValue)}>"),
        };

        static TValue? Deserialize(Serialization.WktConverter<TValue> converter, ReadOnlySpan<byte> wkt, WktSerializerOptions options)
        {
            var reader = new Text.Geodesy.Utf8WktReader(wkt);
            return reader.Read() ? converter.Read(ref reader, typeof(TValue), options) : default;
        }
    }

    /// <summary>
    /// Converts the provided value into a <see cref="string"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <returns>A <see cref="string"/> representation of the value.</returns>
    public static string Serialize<TValue>(TValue value, WktSerializerOptions? options = default)
    {
        options ??= WktSerializerOptions.Default;

        return options.GetConverter(typeof(TValue)) switch
        {
            Serialization.WktConverter<TValue> converter => Serialize(converter, value, options),
            null => throw new InvalidOperationException($"No converter found for type {typeof(TValue)}"),
            _ => throw new InvalidOperationException($"Converter for type {typeof(TValue)} is not a WktConverter<{typeof(TValue)}>"),
        };

        static string Serialize(Serialization.WktConverter<TValue> converter, TValue value, WktSerializerOptions options)
        {
            using var memoryStream = new MemoryStream();
            var writer = new Text.Geodesy.Utf8WktWriter(memoryStream, new Text.Geodesy.WktWriterOptions
            {
                Indented = options.WriteIndented,
            });

            converter.Write(writer, value, options);

            return System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
        }
    }
}