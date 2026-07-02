// -----------------------------------------------------------------------
// <copyright file="PrjReader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

/// <summary>
/// The PRJ reader.
/// </summary>
public class PrjReader : IDisposable
{
    private readonly Stream stream;

    private readonly bool leaveOpen;

    private bool disposedValue;

    /// <summary>
    /// Initialises a new instance of the <see cref="PrjReader"/> class.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="ShxReader"/> object is disposed; otherwise, <see langword="false"/>.</param>
    public PrjReader(Stream stream, bool leaveOpen = false)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (this.stream, this.leaveOpen) = (stream, leaveOpen);
    }

    /// <summary>
    /// Gets the well known ID from the WKT.
    /// </summary>
    /// <param name="wkt">The well known text.</param>
    /// <returns>The well known ID.</returns>
    public static int GetWellKnownId(string wkt) => GetWellKnownId(wkt.AsSpan());

    /// <summary>
    /// Gets the well known ID from the WKT.
    /// </summary>
    /// <param name="wkt">The well known text.</param>
    /// <returns>The well known ID.</returns>
    public static int GetWellKnownId(ReadOnlySpan<char> wkt)
    {
        var byteCount = System.Text.Encoding.UTF8.GetByteCount(wkt);
        Span<byte> bytes = stackalloc byte[byteCount];
        var actualByteCount = System.Text.Encoding.UTF8.GetBytes(wkt, bytes);

        return GetWellKnownId(bytes[..actualByteCount]);
    }

    /// <summary>
    /// Gets the well known ID from the WKT.
    /// </summary>
    /// <param name="wkt">The well known text.</param>
    /// <returns>The well known ID.</returns>
    public static int GetWellKnownId(ReadOnlySpan<byte> wkt)
    {
        var reader = new Altemiq.Text.Geodesy.Utf8WktReader(wkt);

        while (reader.Read())
        {
            if (reader.TokenType is Altemiq.Text.Geodesy.WktTokenType.Keyword)
            {
                var keyword = reader.GetString();

                if (reader.Read()
                    && reader.TokenType is Altemiq.Text.Geodesy.WktTokenType.StartObject
                    && reader.Read()
                    && reader.TokenType is Altemiq.Text.Geodesy.WktTokenType.String)
                {
                    var name = reader.GetString();
                    return keyword switch
                    {
                        PrjConstants.ProjCSKeyword when TryGetWkidFromManifestStreamName(PrjConstants.ProjCSJson, PrjConstants.ProjectedCoordinateSystems, name, out var id) => id,
                        PrjConstants.GeogCSKeyword when TryGetWkidFromManifestStreamName(PrjConstants.GeogCSJson, PrjConstants.GeographicCoordinateSystems, name, out var id) => id,
                        _ => throw new KeyNotFoundException(),
                    };
                }
            }
        }

        throw new ArgumentException(Properties.Resources.FailedToFindNameOfWkt, nameof(wkt));
    }

    /// <summary>
    /// Tries to get the well known ID from the specified well-known text node.
    /// </summary>
    /// <param name="wkt">The well-known text node.</param>
    /// <param name="wkid">The well-known ID.</param>
    /// <returns><see langword="true"/> if <paramref name="wkt"/> represents a valid well-known ID; otherwise <see langword="false" />.</returns>
    public static bool TryGetWellKnownId(string wkt, out int wkid) => TryGetWellKnownId(wkt.AsSpan(), out wkid);

    /// <summary>
    /// Tries to get the well known ID from the specified well-known text node.
    /// </summary>
    /// <param name="wkt">The well-known text node.</param>
    /// <param name="wkid">The well-known ID.</param>
    /// <returns><see langword="true"/> if <paramref name="wkt"/> represents a valid well-known ID; otherwise <see langword="false" />.</returns>
    public static bool TryGetWellKnownId(ReadOnlySpan<char> wkt, out int wkid)
    {
        Span<byte> bytes = stackalloc byte[wkt.Length * 3];
        var byteCount = System.Text.Encoding.UTF8.GetBytes(wkt, bytes);
        return TryGetWellKnownId(bytes[..byteCount], out wkid);
    }

    /// <summary>
    /// Tries to get the well known ID from the specified well-known text node.
    /// </summary>
    /// <param name="wkt">The well-known text node.</param>
    /// <param name="wkid">The well-known ID.</param>
    /// <returns><see langword="true"/> if <paramref name="wkt"/> represents a valid well-known ID; otherwise <see langword="false" />.</returns>
    public static bool TryGetWellKnownId(ReadOnlySpan<byte> wkt, out int wkid)
    {
        var reader = new Altemiq.Text.Geodesy.Utf8WktReader(wkt);

        while (reader.Read())
        {
            if (reader.TokenType is Altemiq.Text.Geodesy.WktTokenType.Keyword
                && reader.TryGetString(out var keyword)
                && reader.Read()
                && reader.TokenType is Altemiq.Text.Geodesy.WktTokenType.StartObject
                && reader.Read()
                && reader.TokenType is Altemiq.Text.Geodesy.WktTokenType.String
                && reader.TryGetString(out var name))
            {
                if (keyword is PrjConstants.ProjCSKeyword)
                {
                    return TryGetWkidFromManifestStreamName(PrjConstants.ProjCSJson, PrjConstants.ProjectedCoordinateSystems, name, out wkid);
                }

                if (keyword is PrjConstants.GeogCSKeyword)
                {
                    return TryGetWkidFromManifestStreamName(PrjConstants.GeogCSJson, PrjConstants.GeographicCoordinateSystems, name, out wkid);
                }
            }
        }

        wkid = default;
        return default;
    }

    /// <summary>
    /// Reads the well known text.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>The well known text.</returns>
    public static Text.Geodesy.WktElement Read(Stream stream)
    {
        return Text.Geodesy.WktElement.Parse(ReadAllText(stream));

        static ReadOnlySpan<byte> ReadAllText(Stream stream)
        {
            var buffer = new byte[stream.Length];
            var length = stream.Read(buffer, 0, buffer.Length);

            return buffer.AsSpan(0, length);
        }
    }

    /// <summary>
    /// Reads the well known text.
    /// </summary>
    /// <returns>The well known text.</returns>
    public Text.Geodesy.WktElement Read() => Read(this.stream);

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

    private static bool TryGetWkidFromManifestStreamName(string streamName, string propertyName, string name, out int wkid)
    {
        using var compressedStream = new System.IO.Compression.GZipStream(typeof(PrjWriter).Assembly.GetManifestResourceStream(typeof(PrjWriter), streamName + ".gz")!, System.IO.Compression.CompressionMode.Decompress, leaveOpen: false);

        return TryGetWkidFromStream(compressedStream, propertyName, name, out wkid);

        static bool TryGetWkidFromStream(Stream stream, string propertyName, string name, out int wkid)
        {
            var json = System.Text.Json.JsonDocument.Parse(stream);
            var coordinateSystems = json.RootElement.GetProperty(propertyName);
            foreach (var element in coordinateSystems.EnumerateArray())
            {
                if (element.TryGetProperty(PrjConstants.NameKeyword, out var nameElement)
                    && string.Equals(nameElement.GetString(), name, StringComparison.Ordinal))
                {
                    if (element.TryGetProperty(PrjConstants.WkIdKeyword, out var wkidElement)
                        && wkidElement.TryGetInt32(out wkid))
                    {
                        return true;
                    }

                    if (element.TryGetProperty(PrjConstants.LatestWkIdKeyword, out wkidElement)
                        && wkidElement.TryGetInt32(out wkid))
                    {
                        return true;
                    }

                    wkid = 0;
                    return false;
                }
            }

            wkid = 0;
            return default;
        }
    }
}