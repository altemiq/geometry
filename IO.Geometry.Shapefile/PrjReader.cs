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
        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        System.Diagnostics.Contracts.Contract.EndContractBlock();
        (this.stream, this.leaveOpen) = (stream, leaveOpen);
    }

    /// <summary>
    /// Gets the well known ID from the WKT.
    /// </summary>
    /// <param name="wkt">The well known text.</param>
    /// <returns>The well known ID.</returns>
    public static int GetWellKnownId(string wkt) => GetWellKnownId(new Geodesy.WellKnownTextNode(wkt));

    /// <summary>
    /// Gets the well known ID from the WKT.
    /// </summary>
    /// <param name="wkt">The well known text.</param>
    /// <returns>The well known ID.</returns>
    public static int GetWellKnownId(Geodesy.WellKnownTextNode wkt)
    {
        var name = wkt.Values.Where(c => c.IsT1).Select(c => c.AsT1).FirstOrDefault() ?? throw new ArgumentException("Failed to find name of WKT", nameof(wkt));
        return wkt.Id switch
        {
            PrjConstants.ProjCSKeyword when TryGetWkidFromManifestStreamName(PrjConstants.ProjCSJson, PrjConstants.ProjectedCoordinateSystems, name, out var wkid) => wkid,
            PrjConstants.GeogCSKeyword when TryGetWkidFromManifestStreamName(PrjConstants.GeogCSJson, PrjConstants.GeographicCoordinateSystems, name, out var wkid) => wkid,
            _ => throw new KeyNotFoundException(),
        };
    }

    /// <summary>
    /// Tries to get the well known ID from the specified well-known text node.
    /// </summary>
    /// <param name="wkt">The well-known text node.</param>
    /// <param name="wkid">The well-known ID.</param>
    /// <returns><see langword="true"/> if <paramref name="wkt"/> represents an valid well-known ID; otherwise <see langword="false" />.</returns>
    public static bool TryGetWellKnownId(string wkt, out int wkid) => TryGetWellKnownId(new Geodesy.WellKnownTextNode(wkt), out wkid);

    /// <summary>
    /// Tries to get the well known ID from the specified well-known text node.
    /// </summary>
    /// <param name="wkt">The well-known text node.</param>
    /// <param name="wkid">The well-known ID.</param>
    /// <returns><see langword="true"/> if <paramref name="wkt"/> represents an valid well-known ID; otherwise <see langword="false" />.</returns>
    public static bool TryGetWellKnownId(Geodesy.WellKnownTextNode wkt, out int wkid)
    {
        if (wkt.Values.Where(c => c.IsT1).Select(c => c.AsT1).FirstOrDefault() is { } name)
        {
            if (string.Equals(wkt.Id, PrjConstants.ProjCSKeyword, StringComparison.Ordinal))
            {
                return TryGetWkidFromManifestStreamName(PrjConstants.ProjCSJson, PrjConstants.ProjectedCoordinateSystems, name, out wkid);
            }

            if (string.Equals(wkt.Id, PrjConstants.GeogCSKeyword, StringComparison.Ordinal))
            {
                return TryGetWkidFromManifestStreamName(PrjConstants.GeogCSJson, PrjConstants.GeographicCoordinateSystems, name, out wkid);
            }
        }

        wkid = default;
        return false;
    }

    /// <summary>
    /// Reads the well known text.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>The well known text.</returns>
    public static Geodesy.WellKnownTextNode Read(Stream stream)
    {
        return new(ReadAllText(stream));

        static string ReadAllText(Stream stream)
        {
            var buffer = new byte[1024];
            var length = stream.Read(buffer, 0, buffer.Length);

            if (length is 0)
            {
                return string.Empty;
            }

            var stringBuilder = new System.Text.StringBuilder();
            do
            {
                _ = stringBuilder.Append(System.Text.Encoding.UTF8.GetString(buffer, 0, length));

                length = stream.Read(buffer, 0, buffer.Length);
            }
            while (length is not 0);

            return stringBuilder.ToString();
        }
    }

    /// <summary>
    /// Reads the well known text.
    /// </summary>
    /// <returns>The well known text.</returns>
    public Geodesy.WellKnownTextNode Read() => Read(this.stream);

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