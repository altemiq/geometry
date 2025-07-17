// -----------------------------------------------------------------------
// <copyright file="PrjWriter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

/// <summary>
/// The PRJ writer.
/// </summary>
public class PrjWriter : IDisposable
{
    private readonly Stream stream;

    private readonly bool leaveOpen;

    private bool disposedValue;

    /// <summary>
    /// Initialises a new instance of the <see cref="PrjWriter"/> class.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="ShxWriter"/> object is disposed; otherwise, <see langword="false"/>.</param>
    public PrjWriter(Stream stream, bool leaveOpen = false)
    {
        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        System.Diagnostics.Contracts.Contract.EndContractBlock();
        (this.stream, this.leaveOpen) = (stream, leaveOpen);
    }

    /// <summary>
    /// Writes the coordinate system to the specified stream.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="wkid">The well known ID.</param>
    public static void WriteTo(Stream stream, int wkid) => WriteTo(stream, wkid, 1);

    /// <summary>
    /// Writes the coordinate system to the specified stream.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="wkid">The well known ID.</param>
    /// <param name="version">The well known text version.</param>
    public static void WriteTo(Stream stream, int wkid, int version)
    {
        var name = version switch
        {
            2 => "wkt2",
            1 => "wkt",
            _ => throw new ArgumentOutOfRangeException(nameof(version), "Invalid WKT version"),
        };

        var wkt = GetWkt(wkid, name);
        var bytes = System.Text.Encoding.UTF8.GetBytes(wkt);
        stream.Write(bytes, 0, bytes.Length);
    }

    /// <summary>
    /// Writes the coordinate system.
    /// </summary>
    /// <param name="wkid">The well known ID.</param>
    public void Write(int wkid) => WriteTo(this.stream, wkid);

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

    private static string GetWkt(int wkid, string name)
    {
        return GetWktFromManifestStreamName(PrjConstants.ProjCSJson, PrjConstants.ProjectedCoordinateSystems, wkid, name)
            ?? GetWktFromManifestStreamName(PrjConstants.GeogCSJson, PrjConstants.GeographicCoordinateSystems, wkid, name)
            ?? throw new KeyNotFoundException();

        static string? GetWktFromManifestStreamName(string streamName, string propertyName, int wkid, string name)
        {
            using var compressedStream = new System.IO.Compression.GZipStream(typeof(PrjWriter).Assembly.GetManifestResourceStream(typeof(PrjWriter), streamName + ".gz")!, System.IO.Compression.CompressionMode.Decompress, leaveOpen: false);

            return GetWktFromStream(compressedStream, propertyName, wkid, name);

            static string? GetWktFromStream(Stream stream, string propertyName, int wkid, string name)
            {
                var json = System.Text.Json.JsonDocument.Parse(stream);
                var coordinateSystems = json.RootElement.GetProperty(propertyName);
                foreach (var element in coordinateSystems.EnumerateArray())
                {
                    if ((IsValid(element, PrjConstants.WkIdKeyword, wkid) || IsValid(element, PrjConstants.LatestWkIdKeyword, wkid))
                        && element.TryGetProperty(name, out var wktElement))
                    {
                        return wktElement.GetString();
                    }
                }

                return default;

                static bool IsValid(System.Text.Json.JsonElement element, string propertyName, int value)
                {
                    return element.TryGetProperty(propertyName, out var propertyElement)
                        && propertyElement.TryGetInt32(out var propertyValue)
                        && propertyValue == value;
                }
            }
        }
    }
}