// -----------------------------------------------------------------------
// <copyright file="DBaseReader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Dbf;

/// <summary>
/// The dBASE reader.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1010:Generic interface should also be implemented", Justification = "This follows the base class")]
public class DBaseReader : System.Data.Common.DbDataReader
{
    private readonly DbfReader dbfReader;

    private readonly DbtReader? dbtReader;

    /// <summary>
    /// Initialises a new instance of the <see cref="DBaseReader"/> class.
    /// </summary>
    /// <param name="path">The file path.</param>
    public DBaseReader(string path)
        : this(File.OpenRead(path), Dbt.OpenRead(path))
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="DBaseReader"/> class.
    /// </summary>
    /// <param name="dbfReader">The <c>DBF</c> reader.</param>
    /// <param name="dbtReader">The <c>DBT</c> reader.</param>
    public DBaseReader(DbfReader dbfReader, DbtReader? dbtReader)
    {
        this.dbfReader = dbfReader;
        this.dbtReader = dbtReader;
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="DBaseReader"/> class.
    /// </summary>
    /// <param name="dbfStream">The <c>DBF</c> stream.</param>
    /// <param name="dbtStream">The <c>DBT</c> stream.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the streams open after the <see cref="DBaseReader"/> object is disposed; otherwise, <see langword="false"/>.</param>
    public DBaseReader(Stream dbfStream, Stream? dbtStream = default, bool leaveOpen = false)
    {
        this.dbfReader = new(dbfStream, leaveOpen);
        if (dbtStream is not null)
        {
            this.dbtReader = new(dbtStream, this.dbfReader.Header.GetEncodingOrDefault(), leaveOpen);
        }
    }

    /// <inheritdoc cref="DbfReader.Header"/>
    public DbfHeader Header => this.dbfReader.Header;

    /// <inheritdoc cref="DbfReader.FileName"/>
    public string? FileName => this.dbfReader.FileName;

    /// <inheritdoc/>
    public override int Depth => 1;

    /// <inheritdoc/>
    public override int FieldCount => this.dbfReader.FieldCount;

    /// <inheritdoc/>
    public override bool HasRows => this.dbfReader.HasRows;

    /// <inheritdoc/>
    public override bool IsClosed => this.dbfReader.IsClosed;

    /// <inheritdoc/>
    public override int RecordsAffected => this.dbfReader.RecordsAffected;

    /// <inheritdoc/>
    public override object this[int ordinal] => this.GetValue(ordinal);

    /// <inheritdoc/>
    public override object this[string name] => this.GetValue(this.dbfReader.GetOrdinal(name));

    /// <summary>
    /// Open a dBASE from a FileStream. This can be a file or an internet connection stream. Make sure that it is positioned at start of DBF file.
    /// Reading a DBF over the internet we can not determine size of the file, so we support HasMore(), ReadNext() interface.
    /// RecordCount information in header can not be trusted always, since some packages store 0 there.
    /// </summary>
    /// <param name="dbfStream">The <c>DBF</c> stream.</param>
    /// <param name="dbtStream">The <c>DBT</c> stream.</param>
    /// <returns>The dBASE reader.</returns>
    public static DBaseReader Open(Stream dbfStream, Stream? dbtStream) => Open(dbfStream, dbtStream, leaveOpen: true);

    /// <summary>
    /// Open a dBASE from a FileStream. This can be a file or an internet connection stream. Make sure that it is positioned at start of DBF file.
    /// Reading a DBdBASE over the internet we can not determine size of the file, so we support HasMore(), ReadNext() interface.
    /// RecordCount information in header can not be trusted always, since some packages store 0 there.
    /// </summary>
    /// <param name="dbfStream">The <c>DBF</c> stream.</param>
    /// <param name="dbtStream">The <c>DBT</c> stream.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="DBaseReader"/> object is disposed; otherwise, <see langword="false"/>.</param>
    /// <returns>The dBASE reader.</returns>
    public static DBaseReader Open(Stream dbfStream, Stream? dbtStream, bool leaveOpen) => new(dbfStream, dbtStream, leaveOpen);

    /// <summary>
    /// Open a DBF file or create a new one.
    /// </summary>
    /// <param name="path">The file to open.</param>
    /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
    /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the file.</param>
    /// <param name="share">A <see cref="FileShare"/> value specifying the type of access other threads have to the file.</param>
    /// <returns>The DBF file.</returns>
    public static DBaseReader Open(string path, FileMode mode, FileAccess access, FileShare share) => Open(File.Open(path, mode, access, share), Dbt.Open(path, mode, access, share), leaveOpen: false);

    /// <summary>
    /// Open a DBF file or create a new one.
    /// </summary>
    /// <param name="path">The file to open.</param>
    /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
    /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the file.</param>
    /// <returns>The DBF file.</returns>
    public static DBaseReader Open(string path, FileMode mode, FileAccess access) => Open(File.Open(path, mode, access), Dbt.Open(path, mode, access), leaveOpen: false);

    /// <summary>
    /// Open a DBF file.
    /// </summary>
    /// <param name="path">The file to open.</param>
    /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
    /// <returns>The DBF file.</returns>
    public static DBaseReader Open(string path, FileMode mode) => Open(File.Open(path, mode), Dbt.Open(path, mode), leaveOpen: false);

    /// <summary>
    /// Open a DBF file.
    /// </summary>
    /// <param name="path">The file to open.</param>
    /// <returns>The DBF file.</returns>
    public static DBaseReader OpenRead(string path) => Open(File.OpenRead(path), Dbt.OpenRead(path), leaveOpen: false);

    /// <inheritdoc/>
    public override bool GetBoolean(int ordinal) => this.dbfReader.GetBoolean(ordinal);

    /// <inheritdoc/>
    public override byte GetByte(int ordinal) => this.dbfReader.GetByte(ordinal);

    /// <inheritdoc/>
    public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length) => this.dbfReader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);

    /// <inheritdoc/>
    public override char GetChar(int ordinal) => this.dbfReader.GetChar(ordinal);

    /// <inheritdoc/>
    public override long GetChars(int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length) => this.dbfReader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);

    /// <inheritdoc/>
    public override string GetDataTypeName(int ordinal) => this.dbfReader.GetDataTypeName(ordinal);

    /// <inheritdoc/>
    public override DateTime GetDateTime(int ordinal) => this.dbfReader.GetDateTime(ordinal);

    /// <inheritdoc/>
    public override decimal GetDecimal(int ordinal) => this.dbfReader.GetDecimal(ordinal);

    /// <inheritdoc/>
    public override double GetDouble(int ordinal) => this.dbfReader.GetDouble(ordinal);

    /// <inheritdoc/>
    public override System.Collections.IEnumerator GetEnumerator() => new System.Data.Common.DbEnumerator(this, closeReader: false);

    /// <inheritdoc/>
    public override Type GetFieldType(int ordinal) => this.dbfReader.GetFieldType(ordinal);

    /// <inheritdoc/>
    public override float GetFloat(int ordinal) => this.dbfReader.GetFloat(ordinal);

    /// <inheritdoc/>
    public override Guid GetGuid(int ordinal) => this.dbfReader.GetGuid(ordinal);

    /// <inheritdoc/>
    public override short GetInt16(int ordinal) => this.dbfReader.GetInt16(ordinal);

    /// <inheritdoc/>
    public override int GetInt32(int ordinal) => this.dbfReader.GetInt32(ordinal);

    /// <inheritdoc/>
    public override long GetInt64(int ordinal) => this.dbfReader.GetInt64(ordinal);

    /// <inheritdoc/>
    public override string GetName(int ordinal) => this.dbfReader.GetName(ordinal);

    /// <inheritdoc/>
    public override int GetOrdinal(string name) => this.dbfReader.GetOrdinal(name);

    /// <inheritdoc/>
    public override string GetString(int ordinal) => (this.dbfReader.Header[ordinal], this.dbtReader) switch
    {
        ({ DbfType: DbfColumn.DbfColumnType.Memo }, { } reader) => reader.GetString(this.dbfReader.GetInt32(ordinal)),
        ({ DbfType: DbfColumn.DbfColumnType.Memo }, null) => throw new InvalidOperationException(),
        _ => this.dbfReader.GetString(ordinal),
    };

    /// <inheritdoc/>
    public override object GetValue(int ordinal) => (this.dbfReader[ordinal], this.dbfReader.Header[ordinal], this.dbtReader) switch
    {
        (int index, { DbfType: DbfColumn.DbfColumnType.Memo }, { } reader) => reader.GetString(index),
        (int, { DbfType: DbfColumn.DbfColumnType.Memo }, null) => throw new InvalidOperationException(),
        ({ } v, _, _) => v,
    };

    /// <inheritdoc/>
    public override int GetValues(object[] values) => this.dbfReader.GetValues(values);

    /// <inheritdoc/>
    public override bool IsDBNull(int ordinal) => this.dbfReader.IsDBNull(ordinal);

    /// <inheritdoc/>
    public override bool NextResult() => false;

    /// <inheritdoc/>
    public override bool Read() => this.dbfReader.Read();

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S3218:Inner class members should not shadow outer class \"static\" or type members", Justification = "This is by design.")]
    private static class Dbt
    {
        private const string DefaultExtension = ".dbt";

        private static readonly string[] Extensions = [DefaultExtension, ".DBT"];

        public static FileStream? Open(string path, FileMode mode) => File.Open(GetPath(path, mode), mode);

        public static FileStream? Open(string path, FileMode mode, FileAccess access) => File.Open(GetPath(path, mode), mode, access);

        public static FileStream? Open(string path, FileMode mode, FileAccess access, FileShare share) => File.Open(GetPath(path, mode), mode, access, share);

        public static FileStream? OpenRead(string path) => GetPath(path).Select(File.OpenRead).FirstOrDefault();

        private static IEnumerable<string> GetPath(string path) => Extensions.Select(extension => Path.ChangeExtension(path, extension)).Where(File.Exists);

        private static string GetPath(string path, FileMode mode)
        {
            return GetPath(path).FirstOrDefault(File.Exists) is { } found
                ? found
                : GetDefaultPath(path, mode);

            static string GetDefaultPath(string path, FileMode mode)
            {
                return mode is FileMode.Open ? throw new FileNotFoundException() : Path.ChangeExtension(path, DefaultExtension);
            }
        }
    }
}