// -----------------------------------------------------------------------
// <copyright file="QixReader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

/// <summary>
/// The QIX tree reader.
/// </summary>
public class QixReader : IDisposable
{
    private readonly Stream stream;

    private readonly bool leaveOpen;

    private bool disposedValue;

    /// <summary>
    /// Initialises a new instance of the <see cref="QixReader"/> class.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="QixReader"/> object is disposed; otherwise, <see langword="false"/>.</param>
    public QixReader(Stream stream, bool leaveOpen = false)
    {
        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        System.Diagnostics.Contracts.Contract.EndContractBlock();
        (this.stream, this.leaveOpen, this.Header) = (stream, leaveOpen, QixHeader.ReadFrom(stream));
    }

    /// <summary>
    /// Gets the header.
    /// </summary>
    public QixHeader Header { get; }

    /// <summary>
    /// Gets the number of nodes.
    /// </summary>
    public int Count => this.Header.Depth;

    /// <summary>
    /// Reads the next node.
    /// </summary>
    /// <returns>The next node.</returns>
    public QixNode Read() => (this.stream.Position + 40) > this.stream.Length
        ? default
        : QixNode.ReadFrom(this.stream, this.Header.IsLittleEndian);

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
}