// -----------------------------------------------------------------------
// <copyright file="NdxReader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Dbf;

/// <summary>
/// The <c>NDX</c> reader.
/// </summary>
public class NdxReader : IDisposable
{
    private const int Size = 512;

    private readonly Stream stream;
    private readonly bool leaveOpen;

    private bool disposedValue;

    /// <summary>
    /// Initialises a new instance of the <see cref="NdxReader"/> class.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="NdxReader"/> object is disposed; otherwise, <see langword="false"/>.</param>
    public NdxReader(Stream stream, bool leaveOpen = false)
    {
        this.stream = stream;
        this.leaveOpen = leaveOpen;
        var bytes = new byte[Size];
        if (this.stream.Read(bytes, 0, Size) == Size)
        {
            var span = bytes.AsSpan();
            this.StartingPageNumber = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[0..4]);
            this.TotalNoOfPages = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[4..8]);
            this.KeyLength = System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(span[12..14]);
            this.NumberOfKeysPerPage = System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(span[14..16]);
            this.KeyType = System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(span[16..18]);
            this.SizeOfKeyRecord = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[18..22]);
            this.UniqueFlag = span[23] != 0;

            var index = SpaceIndex(span[24..]);
#if NETSTANDARD2_1_OR_GREATER
            this.StringDefiningTheKey = System.Text.Encoding.UTF8.GetString(span[24..(24 + index)]);
#else
            this.StringDefiningTheKey = System.Text.Encoding.UTF8.GetString(bytes, 24, index);
#endif

            // get the index
            static int SpaceIndex(ReadOnlySpan<byte> span)
            {
                for (var idx = 0; idx < span.Length; idx++)
                {
                    if (span[idx] == ' ')
                    {
                        return idx;
                    }
                }

                return -1;
            }
        }
    }

    /// <summary>
    /// Gets the starting page number.
    /// </summary>
    public int StartingPageNumber { get; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalNoOfPages { get; }

    /// <summary>
    /// Gets the key length.
    /// </summary>
    public short KeyLength { get; }

    /// <summary>
    /// Gets the number of keys per page.
    /// </summary>
    public short NumberOfKeysPerPage { get; }

    /// <summary>
    /// Gets the key type.
    /// </summary>
    public short KeyType { get; }

    /// <summary>
    /// Gets the size of the key record.
    /// </summary>
    public int SizeOfKeyRecord { get; }

    /// <summary>
    /// Gets a value indicating whether the values are unique.
    /// </summary>
    public bool UniqueFlag { get; }

    /// <summary>
    /// Gets the string defining the key.
    /// </summary>
    public string? StringDefiningTheKey { get; }

    /// <inheritdoc/>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Reads the next page.
    /// </summary>
    /// <returns>The page if found; otherwise <see langword="null"/>.</returns>
    public Page? Read()
    {
        if (this.stream.Position + Size > this.stream.Length)
        {
            return default;
        }

        var bytes = new byte[Size];
        return this.stream.Read(bytes, 0, Size) != Size ? throw new InvalidOperationException() : new Page(bytes, this.SizeOfKeyRecord);
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
                this.stream.Close();
                this.stream.Dispose();
            }

            this.disposedValue = true;
        }
    }

    /// <summary>
    /// The key entry.
    /// </summary>
    public readonly struct Entry
    {
        private readonly byte[] bytes;
        private readonly int start;

        /// <summary>
        /// Initialises a new instance of the <see cref="Entry"/> struct.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="start">The start.</param>
        internal Entry(byte[] bytes, int start)
        {
            (this.bytes, this.start) = (bytes, start);
            var span = this.Span;
            this.PointerToLowerLevel = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[0..4]);
            this.RecordNumberInDataFile = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span[4..8]);
        }

        /// <summary>
        /// Gets the pointer to the lower level.
        /// </summary>
        public int PointerToLowerLevel { get; }

        /// <summary>
        /// Gets the record number in the data file.
        /// </summary>
        public int RecordNumberInDataFile { get; }

        private readonly ReadOnlySpan<byte> Span => this.bytes.AsSpan()[this.start..];

        /// <summary>
        /// Reads the key value as a <see cref="short"/>.
        /// </summary>
        /// <returns>The <see cref="short"/> value.</returns>
        public short ReadInt16() => System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(this.Span[8..]);

        /// <summary>
        /// Reads the key value as a <see cref="int"/>.
        /// </summary>
        /// <returns>The <see cref="int"/> value.</returns>
        public int ReadInt32() => System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(this.Span[8..]);

        /// <summary>
        /// Reads the key value as a <see cref="long"/>.
        /// </summary>
        /// <returns>The <see cref="long"/> value.</returns>
        public long ReadInt64() => System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(this.Span[8..]);
    }

    /// <summary>
    /// The index page.
    /// </summary>
    public class Page : IReadOnlyList<Entry>
    {
        private readonly byte[] bytes;

        private readonly int sizeOfKeyRecord;

        /// <summary>
        /// Initialises a new instance of the <see cref="Page"/> class.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="sizeOfKeyRecord">The size of key record.</param>
        public Page(byte[] bytes, int sizeOfKeyRecord)
        {
            if (bytes.Length != Size)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes));
            }

            this.bytes = bytes;
            this.Count = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan());
            this.sizeOfKeyRecord = sizeOfKeyRecord;
        }

        /// <inheritdoc/>
        public int Count { get; }

        /// <inheritdoc/>
        public Entry this[int index] => index < 0 || index >= this.Count
            ? throw new ArgumentOutOfRangeException(nameof(index))
            : new Entry(this.bytes, this.sizeOfKeyRecord * index);

        /// <inheritdoc/>
        public System.Collections.IEnumerator GetEnumerator() => new Enumerator(this);

        /// <inheritdoc/>
        IEnumerator<Entry> IEnumerable<Entry>.GetEnumerator() => new Enumerator(this);

        private struct Enumerator(Page? page) : IEnumerator<Entry>
        {
            private int index = -1;

            public Entry Current { get; private set; }

            readonly object System.Collections.IEnumerator.Current => this.Current;

            readonly void IDisposable.Dispose()
            {
            }

            bool System.Collections.IEnumerator.MoveNext()
            {
                if (page is null)
                {
                    return false;
                }

                this.index++;
                if (this.index > page.Count)
                {
                    return false;
                }

                this.Current = page[this.index];
                return true;
            }

            void System.Collections.IEnumerator.Reset() => this.index = -1;
        }
    }
}