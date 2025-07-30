// -----------------------------------------------------------------------
// <copyright file="DbfReader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Dbf;

/// <summary>
/// This class represents a <see cref="Dbf"/> reader. You can open <see cref="Dbf"/> files using this class and supporting classes.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1010:Generic interface should also be implemented", Justification = "This follows the base class")]
public class DbfReader : System.Data.Common.DbDataReader
{
    private readonly bool leaveOpen;

    private readonly Stream stream;

    private long recordsReadCount;

    private bool disposedValue;

    private DbfRecord? record;

    /// <summary>
    /// Initialises a new instance of the <see cref="DbfReader"/> class.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="DbfReader"/> object is disposed; otherwise, <see langword="false"/>.</param>
    public DbfReader(Stream stream, bool leaveOpen = false)
    {
        this.Header = new(0);
        this.leaveOpen = leaveOpen;
        this.stream = stream;

        // reset position
        this.recordsReadCount = 0;

        // read the header
        this.Header.ReadFrom(stream);

        this.IsForwardOnly = !this.stream.CanSeek;

        if (this.stream is FileStream fileStream)
        {
            this.FileName = fileStream.Name;
        }
    }

    /// <summary>
    /// Gets or sets the Access DBF header with information on columns. Use this object for faster access to header.
    /// Remove one layer of function calls by saving header reference and using it directly to access columns.
    /// </summary>
    public DbfHeader Header { get; protected set; }

    /// <summary>
    /// Gets or sets a value indicating whether we can not seek to different locations within the file, such as internet connections.
    /// </summary>
    public bool IsForwardOnly { get; protected set; }

    /// <summary>
    /// Gets or sets the name of the file stream.
    /// </summary>
    public string? FileName { get; protected set; }

    /// <inheritdoc/>
    public override int Depth => 1;

    /// <inheritdoc/>
    public override int FieldCount => this.record?.FieldCount ?? 0;

    /// <inheritdoc/>
    public override bool HasRows => this.Header.RecordCount is not 0U;

    /// <inheritdoc/>
    public override bool IsClosed => this.disposedValue;

    /// <inheritdoc/>
    public override int RecordsAffected => -1;

    private DbfRecord Record => this.record ?? throw new InvalidOperationException();

    /// <inheritdoc/>
    public override object this[string name] => this.Record[name];

    /// <inheritdoc/>
    public override object this[int ordinal] => this.Record[ordinal];

    /// <summary>
    /// Open a DBF from a FileStream. This can be a file or an internet connection stream. Make sure that it is positioned at start of DBF file.
    /// Reading a DBF over the internet we can not determine size of the file, so we support HasMore(), ReadNext() interface.
    /// RecordCount information in header can not be trusted always, since some packages store 0 there.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>The DBF file.</returns>
    public static DbfReader Open(Stream stream) => Open(stream, leaveOpen: true);

    /// <summary>
    /// Open a DBF from a FileStream. This can be a file or an internet connection stream. Make sure that it is positioned at start of DBF file.
    /// Reading a DBF over the internet we can not determine size of the file, so we support HasMore(), ReadNext() interface.
    /// RecordCount information in header can not be trusted always, since some packages store 0 there.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="DbfReader"/> object is disposed; otherwise, <see langword="false"/>.</param>
    /// <returns>The DBF file.</returns>
    public static DbfReader Open(Stream stream, bool leaveOpen) => new(stream, leaveOpen);

    /// <summary>
    /// Open a DBF file or create a new one.
    /// </summary>
    /// <param name="path">The file to open.</param>
    /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
    /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the file.</param>
    /// <param name="share">A <see cref="FileShare"/> value specifying the type of access other threads have to the file.</param>
    /// <returns>The DBF file.</returns>
    public static DbfReader Open(string path, FileMode mode, FileAccess access, FileShare share) => Open(File.Open(path, mode, access, share), leaveOpen: false);

    /// <summary>
    /// Open a DBF file or create a new one.
    /// </summary>
    /// <param name="path">The file to open.</param>
    /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
    /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the file.</param>
    /// <returns>The DBF file.</returns>
    public static DbfReader Open(string path, FileMode mode, FileAccess access) => Open(File.Open(path, mode, access), leaveOpen: false);

    /// <summary>
    /// Open a DBF file.
    /// </summary>
    /// <param name="path">The file to open.</param>
    /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
    /// <returns>The DBF file.</returns>
    public static DbfReader Open(string path, FileMode mode) => Open(File.Open(path, mode), leaveOpen: false);

    /// <summary>
    /// Open a DBF file.
    /// </summary>
    /// <param name="path">The file to open.</param>
    /// <returns>The DBF file.</returns>
    public static DbfReader OpenRead(string path) => Open(File.OpenRead(path), leaveOpen: false);

    /// <summary>
    /// Reads a record.
    /// </summary>
    /// <returns><see langword="null"/> if record can not be read, otherwise returns a new record.</returns>
    public override bool Read()
    {
        // create a new record and fill it.
        this.record ??= new(this.Header);

        if (Fill(this.record))
        {
            return true;
        }

        this.record = default;
        return false;

        bool Fill(DbfRecord record)
        {
            if (this.stream is null)
            {
                return false;
            }

            // check if we can fill this record with data. it must match record size specified by header and number of columns.
            // we are not checking whether it comes from another DBF file or not, we just need the same structure. Allow flexibility but be safe.
            if (record.Header != this.Header && (record.Header.FieldCount != this.Header.FieldCount || record.Header.RecordLength != this.Header.RecordLength))
            {
                throw new ArgumentException(Properties.Resources.InvalidRecordParameter, nameof(record));
            }

            // read next record...
            var read = record.ReadFrom(this.stream);
            if (read)
            {
                if (this.IsForwardOnly)
                {
                    // zero based index! set before incrementing count.
                    record.RecordIndex = this.recordsReadCount;
                    this.recordsReadCount++;
                }
                else
                {
                    record.RecordIndex = ((int)((this.stream.Position - this.Header.HeaderLength) / this.Header.RecordLength)) - 1;
                }
            }

            return read;
        }
    }

    /// <summary>
    /// Reads a record specified by index.
    /// This method requires the stream to be able to seek to position.
    /// If you are using a http stream, or a stream that can not stream, use <see cref="Read()"/> methods to read in all records.
    /// </summary>
    /// <param name="index">Zero based index.</param>
    /// <returns> <see langword="null"/> if record can not be read, otherwise returns a new record.</returns>
    public bool Read(long index)
    {
        // create a new record and fill it.
        this.record ??= new(this.Header);

        if (Read(this.stream, this.Header, index, this.record))
        {
            return true;
        }

        this.record = default;
        return false;

        static bool Read(Stream? stream, DbfHeader header, long index, DbfRecord record)
        {
            if (stream is null)
            {
                return false;
            }

            // check if we can fill this record with data. it must match record size specified by header and number of columns.
            // we are not checking whether it comes from another DBF file or not, we just need the same structure. Allow flexibility but be safe.
            if (record.Header != header && (record.Header.FieldCount != header.FieldCount || record.Header.RecordLength != header.RecordLength))
            {
                throw new ArgumentException(Properties.Resources.InvalidRecordParameter, nameof(record));
            }

            // move to the specified record, note that an exception will be thrown is stream is not seekable!
            // This is ok, since we provide a function to check whether the stream is seekable.
            var offset = header.HeaderLength + (index * header.RecordLength);

            // check whether requested record exists.
            // Subtract 1 from file length (there is a terminating character 1A at the end of the file) so if we hit end of file, there are no more records, so return false.
            if (index < 0 || stream.Length - 1 <= offset)
            {
                return false;
            }

            // move to record and read
            _ = stream.Seek(offset, SeekOrigin.Begin);

            // read the record
            if (record.ReadFrom(stream))
            {
                record.RecordIndex = index;
                return true;
            }

            return false;
        }
    }

    /// <inheritdoc/>
    public override bool GetBoolean(int ordinal) => this.Record.GetBoolean(ordinal);

    /// <inheritdoc/>
    public override byte GetByte(int ordinal) => this.Record.GetByte(ordinal);

    /// <inheritdoc/>
    public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length) => this.Record.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);

    /// <inheritdoc/>
    public override char GetChar(int ordinal) => this.Record.GetChar(ordinal);

    /// <inheritdoc/>
    public override long GetChars(int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length) => this.Record.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);

    /// <inheritdoc/>
    public override string GetDataTypeName(int ordinal) => this.Record.GetDataTypeName(ordinal);

    /// <inheritdoc/>
    public override DateTime GetDateTime(int ordinal) => this.Record.GetDateTime(ordinal);

    /// <inheritdoc/>
    public override decimal GetDecimal(int ordinal) => this.Record.GetDecimal(ordinal);

    /// <inheritdoc/>
    public override double GetDouble(int ordinal) => this.Record.GetDouble(ordinal);

    /// <inheritdoc/>
    public override System.Collections.IEnumerator GetEnumerator() => new System.Data.Common.DbEnumerator(this, closeReader: false);

    /// <inheritdoc/>
    public override Type GetFieldType(int ordinal) => this.Record.GetFieldType(ordinal);

    /// <inheritdoc/>
    public override float GetFloat(int ordinal) => this.Record.GetFloat(ordinal);

    /// <inheritdoc/>
    public override Guid GetGuid(int ordinal) => this.Record.GetGuid(ordinal);

    /// <inheritdoc/>
    public override short GetInt16(int ordinal) => this.Record.GetInt16(ordinal);

    /// <inheritdoc/>
    public override int GetInt32(int ordinal) => this.Record.GetInt32(ordinal);

    /// <inheritdoc/>
    public override long GetInt64(int ordinal) => this.Record.GetInt64(ordinal);

    /// <inheritdoc/>
    public override string GetName(int ordinal) => this.Record.GetName(ordinal);

    /// <inheritdoc/>
    public override int GetOrdinal(string name) => this.Record.GetOrdinal(name);

    /// <inheritdoc/>
    public override string GetString(int ordinal) => this.Record.GetString(ordinal);

    /// <inheritdoc/>
    public override object GetValue(int ordinal) => this.Record.GetValue(ordinal);

    /// <inheritdoc/>
    public override int GetValues(object[] values) => this.Record.GetValues(values);

    /// <inheritdoc/>
    public override bool IsDBNull(int ordinal) => this.Record.IsDBNull(ordinal);

    /// <inheritdoc/>
    public override bool NextResult() => false;

    /// <summary>
    /// Gets the <see cref="Dbf"/> <see cref="System.Data.IDataRecord"/>.
    /// </summary>
    /// <returns>The <see cref="Dbf"/> <see cref="System.Data.IDataRecord"/>.</returns>
    public DbfRecord GetRecord() => this.Record;

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                if (!this.leaveOpen)
                {
                    this.stream.Close();
                    this.stream.Dispose();
                }

                this.FileName = null;
            }

            this.disposedValue = true;
        }

        base.Dispose(disposing);
    }
}