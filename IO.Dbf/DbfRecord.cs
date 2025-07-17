// -----------------------------------------------------------------------
// <copyright file="DbfRecord.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Dbf;

/// <summary>
/// <para>Use this class to create a record and write it to a <see cref="Dbf"/> file.</para>
/// <para>You can use one record object to write all records! It was designed for this kind of use. You can do this by clearing the record of all data (call Clear() method) or setting values to all fields again, then write to <see cref="Dbf"/> file.</para>
/// <para>This eliminates creating and destroying objects and optimizes memory use.</para>
/// <para>Once you create a record the header can no longer be modified, since modifying the header would make a corrupt <see cref="Dbf"/> file.</para>
/// </summary>
public class DbfRecord : System.Data.IDataRecord
{
    /// <summary>
    /// The vacant character.
    /// </summary>
    internal const char VacantChar = ' ';

    /// <summary>
    /// The vacant byte.
    /// </summary>
    internal const byte VacantByte = (byte)VacantChar;

    private const char DeletedChar = '*';

    private const byte DeletedByte = (byte)DeletedChar;

    private const char ZeroChar = '0';

    private const byte ZeroByte = (byte)ZeroChar;

    private const char QuestionChar = '?';

    private const byte QuestionByte = (byte)QuestionChar;

    private readonly byte[] data;

    /// <summary>
    /// Initialises a new instance of the <see cref="DbfRecord"/> class.
    /// </summary>
    /// <param name="header"><see cref="DbfHeader"/> will be locked once a record is created since the record size is fixed and if the header was modified it would corrupt the DBF file.</param>
    public DbfRecord(DbfHeader header)
    {
        this.Header = header;
        this.Header.Locked = true;

        // create a buffer to hold all record data. We will reuse this buffer to write all data to the file.
        this.data = new byte[this.Header.RecordLength];

        // Make sure mData[0] correctly represents 'not deleted'
        this.IsDeleted = false;
    }

    /// <summary>
    /// Gets or sets a zero based record index. This information is not directly stored in DBF.
    /// It is the location of this record within the DBF.
    /// </summary>
    /// <remarks>
    /// This property is managed from outside this object,
    /// CDbfFile object updates it when records are read. The reason we don't set it in the Read()
    /// function within this object is that the stream can be forward-only so the Position property
    /// is not available and there is no way to figure out what index the record was unless you
    /// count how many records were read, and that's exactly what CDbfFile does.
    /// </remarks>
    [System.ComponentModel.DefaultValue(-1L)]
    public long RecordIndex { get; set; } = -1L;

    /// <summary>
    /// Gets or sets a value indicating whether this record was tagged deleted.
    /// </summary>
    [System.ComponentModel.DefaultValue(false)]
    public bool IsDeleted
    {
        get => this.data[0] is DeletedByte;
        set => this.data[0] = value ? DeletedByte : VacantByte;
    }

    /// <summary>
    /// Gets or sets a value indicating whether strings can be truncated.
    /// </summary>
    /// <remarks>
    /// <para>If <see langword="false"/> and string is longer than can fit in the field, an exception is thrown.</para>
    /// <para>Default is <see langword="true"/>.</para>
    /// </remarks>
    [System.ComponentModel.DefaultValue(true)]
    public bool AllowStringTruncate { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to allow the decimal portion of numbers to be truncated.
    /// </summary>
    /// <remarks>
    /// <para>If <see langword="false"/> and decimal digits overflow the field, an exception is thrown.</para>
    /// <para>Default is <see langword="false"/>.</para>
    /// </remarks>
    [System.ComponentModel.DefaultValue(false)]
    public bool AllowDecimalTruncate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether integer portion of numbers can be truncated.
    /// </summary>
    /// <remarks>
    /// <para>If <see langword="false"/> and integer digits overflow the field, an exception is thrown.</para>
    /// <para>Default is <see langword="false"/>.</para>
    /// </remarks>
    [System.ComponentModel.DefaultValue(false)]
    public bool AllowIntegerTruncate { get; set; }

    /// <summary>
    /// Gets the header object associated with this record.
    /// </summary>
    public DbfHeader Header { get; }

    /// <inheritdoc/>
    public int FieldCount => this.Header.FieldCount;

    /// <inheritdoc/>
    public object this[string name] => this[this.Header.FindColumn(name)];

    /// <inheritdoc/>
    public object this[int i] => this.GetValue(i);

    /// <summary>
    /// Gets the column from the specified <see cref="DbfColumn"/>.
    /// </summary>
    /// <param name="column">The column.</param>
    /// <returns>The column defined by <paramref name="column"/> as an <see cref="object"/>.</returns>
    public object this[DbfColumn column] => this.GetValue(column);

    /// <summary>
    /// Get column by index.
    /// </summary>
    /// <param name="index">The column index.</param>
    /// <returns>The column.</returns>
    public DbfColumn Column(int index) => this.Header[index];

    /// <summary>
    /// Get column by name.
    /// </summary>
    /// <param name="name">The column name.</param>
    /// <returns>The column.</returns>
    public DbfColumn? Column(string name) => this.Header[name];

    /// <summary>
    /// Clears all data in the record.
    /// </summary>
    public void Clear()
    {
#if NETSTANDARD2_1_OR_GREATER
        Array.Fill(this.data, VacantByte, 0, this.data.Length);
#else
        var emptyRecord = this.Header.EmptyDataRecord;
        Buffer.BlockCopy(emptyRecord, 0, this.data, 0, emptyRecord.Length);
#endif
        this.RecordIndex = -1;
    }

    /// <inheritdoc/>
    public override string ToString() => this.Header.GetEncodingOrDefault().GetString(this.data);

    /// <inheritdoc/>
    public bool GetBoolean(int i) =>
        this.Header[i] is { DbfType: DbfColumn.DbfColumnType.Boolean } column
            ? this.GetBoolean(column)
            : throw new InvalidOperationException();

    /// <inheritdoc/>
    public byte GetByte(int i) => throw new NotSupportedException();

    /// <inheritdoc/>
    public long GetBytes(int i, long fieldOffset, byte[]? buffer, int bufferoffset, int length)
    {
        var column = this.Header[i];
        var columnSize = column.ColumnSize ?? 0;
        if (buffer is null)
        {
            // just return the required length
            return columnSize - fieldOffset;
        }

        // copy the data in
        length = Math.Min(length, (int)(columnSize - fieldOffset));
        Array.Copy(this.data, fieldOffset, buffer, bufferoffset, length);
        return length;
    }

    /// <inheritdoc/>
    public char GetChar(int i) =>
        this.Header[i] is { DbfType: DbfColumn.DbfColumnType.Character } column
            ? this.GetChar(column)
            : throw new InvalidOperationException();

    /// <inheritdoc/>
    public long GetChars(int i, long fieldoffset, char[]? buffer, int bufferoffset, int length) => throw new NotSupportedException();

    /// <inheritdoc/>
    public System.Data.IDataReader GetData(int i) => throw new NotSupportedException();

    /// <inheritdoc/>
    public string GetDataTypeName(int i) => this.Header[i].DataTypeName;

    /// <inheritdoc/>
    public DateTime GetDateTime(int i) => this.GetDateTime(this.Header[i]);

    /// <inheritdoc/>
    public decimal GetDecimal(int i) =>
        this.Header[i] is { DbfType: DbfColumn.DbfColumnType.Number, NumericPrecision: > 15 } column
            ? this.GetDecimal(column)
            : throw new InvalidOperationException();

    /// <inheritdoc/>
    public double GetDouble(int i) => this.GetDouble(this.Header[i]);

    /// <inheritdoc/>
    public Type GetFieldType(int i) => this.Header[i].DataType;

    /// <inheritdoc/>
    public float GetFloat(int i) => this.Header[i] switch
    {
        { DbfType: DbfColumn.DbfColumnType.Float } column => this.GetFloat(column),
        { DbfType: DbfColumn.DbfColumnType.Number, NumericPrecision: <= 7 } column => this.GetFloat(column),
        _ => throw new InvalidOperationException(),
    };

    /// <inheritdoc/>
    public Guid GetGuid(int i) => throw new NotSupportedException();

    /// <inheritdoc/>
    public short GetInt16(int i) => throw new NotSupportedException();

    /// <inheritdoc/>
    public int GetInt32(int i) => this.Header[i] switch
    {
        { DbfType: DbfColumn.DbfColumnType.Number, NumericPrecision: null, ColumnSize: < 10 } column => this.GetInt32(column),
        _ => throw new InvalidOperationException(),
    };

    /// <inheritdoc/>
    public long GetInt64(int i) => this.Header[i] switch
    {
        { DbfType: DbfColumn.DbfColumnType.Number, NumericPrecision: null, ColumnSize: >= 10 } column => this.GetInt64(column),
        _ => throw new InvalidOperationException(),
    };

    /// <inheritdoc/>
    public string GetName(int i) => this.Header[i].ColumnName;

    /// <inheritdoc/>
    public int GetOrdinal(string name) => this.Header.FindColumn(name);

    /// <inheritdoc/>
    public string GetString(int i) => this.GetString(this.Header[i]);

    /// <inheritdoc/>
    public object GetValue(int i) => this.GetValue(this.Header[i]);

    /// <inheritdoc/>
    public int GetValues(object?[] values)
    {
        var count = Math.Min(values.Length, this.FieldCount);
        for (var i = 0; i < count; i++)
        {
            values[i] = this.GetValue(i);
        }

        return count;
    }

    /// <inheritdoc/>
    public bool IsDBNull(int i) => IsDBNull(this.data, this.Header[i]);

    /// <summary>
    /// Gets a span for the bytes.
    /// </summary>
    /// <param name="i">The ordinal index.</param>
    /// <returns>The span representing the bytes.</returns>
    public ReadOnlySpan<byte> GetSpan(int i)
    {
        var column = this.Header[i];
        var length = column.ColumnSize ?? 0;
        return new(this.data, column.DataAddress, length);
    }

    /// <summary>
    /// Writes data to stream. Make sure stream is positioned correctly because we simply write out data to it, and clear the record.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="clearRecordAfterWrite">Set to <see langword="true"/> to clear the records after write.</param>
    protected internal void CopyTo(Stream stream, bool clearRecordAfterWrite = false)
    {
        stream.Write(this.data, 0, this.data.Length);

        if (clearRecordAfterWrite)
        {
            this.Clear();
        }
    }

    /// <summary>
    /// Read record from stream.
    /// </summary>
    /// <param name="stream">The stream to read from.</param>
    /// <returns><see langword="true"/> if record read completely; otherwise <see langword="false"/>.</returns>
    protected internal bool ReadFrom(Stream stream) => stream.Read(this.data, 0, this.data.Length) >= this.data.Length;

    private static bool GetBoolean(string value) => string.Equals(value, "T", StringComparison.OrdinalIgnoreCase);

#if NETSTANDARD2_1_OR_GREATER
    private static int GetInt32(ReadOnlySpan<char> value) => int.Parse(value, provider: System.Globalization.CultureInfo.InvariantCulture);

    private static long GetInt64(ReadOnlySpan<char> value) => long.Parse(value, provider: System.Globalization.CultureInfo.InvariantCulture);

    private static float GetFloat(ReadOnlySpan<char> value) => float.Parse(value, provider: System.Globalization.CultureInfo.InvariantCulture);

    private static double GetDouble(ReadOnlySpan<char> value) => double.Parse(value, provider: System.Globalization.CultureInfo.InvariantCulture);

    private static decimal GetDecimal(ReadOnlySpan<char> value) => decimal.Parse(value, provider: System.Globalization.CultureInfo.InvariantCulture);

    private static DateTime GetDateTime(ReadOnlySpan<char> value) => DateTime.ParseExact(value, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
#else

    private static int GetInt32(string value) => int.Parse(value, System.Globalization.CultureInfo.InvariantCulture);

    private static long GetInt64(string value) => long.Parse(value, System.Globalization.CultureInfo.InvariantCulture);

    private static float GetFloat(string value) => float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);

    private static double GetDouble(string value) => double.Parse(value, System.Globalization.CultureInfo.InvariantCulture);

    private static decimal GetDecimal(string value) => decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);

    private static DateTime GetDateTime(string value) => DateTime.ParseExact(value, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
#endif

    private static bool IsDBNull(byte[] bytes, DbfColumn column)
    {
        return IsDBNullCore(bytes, column.DataAddress, column.ColumnSize ?? 0, column.DbfType);

        static bool IsDBNullCore(byte[] bytes, int start, int length, DbfColumn.DbfColumnType columnType)
        {
            return length is 0 || CheckColumn(bytes, start, length, columnType);

            static bool CheckColumn(byte[] bytes, int start, int length, DbfColumn.DbfColumnType columnType)
            {
                return columnType switch
                {
                    // We accept all asterisks or all blanks as NULL though according to the spec I think it should be all asterisks.
                    DbfColumn.DbfColumnType.Number or DbfColumn.DbfColumnType.Float => IsAll(bytes, start, length, DeletedByte) || IsAll(bytes, start, length, VacantByte),

                    // NULL date fields have value "00000000"
                    DbfColumn.DbfColumnType.Date => IsAll(bytes, start, length, ZeroByte) || IsAll(bytes, start, length, VacantByte),
                    DbfColumn.DbfColumnType.Boolean => bytes[start] is QuestionByte or VacantByte,
                    DbfColumn.DbfColumnType.Memo => IsAll(bytes, start, 10, 0),
                    _ => IsNullOrWhiteSpace(bytes, start, length),
                };

                static bool IsAll(byte[] bytes, int start, int length, byte value)
                {
                    var end = start + length;
                    for (var i = start; i < end; i++)
                    {
                        var @byte = bytes[i];
                        if (value is not 0 && @byte is 0)
                        {
                            break;
                        }

                        if (@byte != value)
                        {
                            return false;
                        }
                    }

                    return true;
                }

                static bool IsNullOrWhiteSpace(byte[] bytes, int start, int length)
                {
                    var end = start + length;
                    for (var i = start; i < end; i++)
                    {
                        var @byte = bytes[i];
                        if (@byte is 0)
                        {
                            return i != start;
                        }

                        if (!char.IsWhiteSpace((char)@byte))
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }
        }
    }

    private object GetValue(DbfColumn column)
    {
        return IsDBNull(this.data, column)
            ? DBNull.Value
            : GetValueCore(column);

        object GetValueCore(DbfColumn valueColumn)
        {
            return valueColumn switch
            {
                { DbfType: DbfColumn.DbfColumnType.Character } => this.GetString(valueColumn),
                { DbfType: DbfColumn.DbfColumnType.Number, NumericPrecision: null, ColumnSize: >= 10 } => this.GetInt64(valueColumn),
                { DbfType: DbfColumn.DbfColumnType.Number, NumericPrecision: null } => this.GetInt32(valueColumn),
                { DbfType: DbfColumn.DbfColumnType.Number, NumericPrecision: > 15 } => this.GetDecimal(valueColumn),
                { DbfType: DbfColumn.DbfColumnType.Number, NumericPrecision: <= 7 } => this.GetFloat(valueColumn),
                { DbfType: DbfColumn.DbfColumnType.Number } => this.GetDouble(valueColumn),
                { DbfType: DbfColumn.DbfColumnType.Boolean } => this.GetBoolean(valueColumn),
                { DbfType: DbfColumn.DbfColumnType.Date } => this.GetDateTime(valueColumn),
                { DbfType: DbfColumn.DbfColumnType.Float } => this.GetFloat(valueColumn),
                { DbfType: DbfColumn.DbfColumnType.Memo } => this.GetInt32(valueColumn),
                _ => throw new NotSupportedException(),
            };
        }
    }

    private string GetString(DbfColumn column) => column.DbfType switch
    {
        DbfColumn.DbfColumnType.Character or DbfColumn.DbfColumnType.Date or DbfColumn.DbfColumnType.Number or DbfColumn.DbfColumnType.Float or DbfColumn.DbfColumnType.Boolean => this.GetString(column.DataAddress, column.ColumnSize ?? 0),
        DbfColumn.DbfColumnType.Memo => this.GetString(column.DataAddress, 10),
        _ => throw new InvalidOperationException(),
    };

    private string GetString(int index, int count)
    {
        return this.Header.GetEncodingOrDefault().GetString(this.data, index, GetCount(this.data, index, count));

        static int GetCount(byte[] data, int index, int count)
        {
            var end = index + count;
            for (var i = index; i < end; i++)
            {
                if (data[i] is 0)
                {
                    return i - index;
                }
            }

            return count;
        }
    }

    private bool GetBoolean(DbfColumn column) => GetBoolean(this.GetString(column));

    private char GetChar(DbfColumn header) => this.GetString(header) is { Length: 1 } stringValue ? stringValue[0] : throw new InvalidOperationException();

    private int GetInt32(DbfColumn column) => column.DbfType switch
    {
        DbfColumn.DbfColumnType.Number or DbfColumn.DbfColumnType.Memo => GetInt32(this.GetString(column)),
        _ => throw new InvalidOperationException(),
    };

    private long GetInt64(DbfColumn column) => GetInt64(this.GetString(column));

    private float GetFloat(DbfColumn column) => GetFloat(this.GetString(column));

    private double GetDouble(DbfColumn column) => column.DbfType switch
    {
        DbfColumn.DbfColumnType.Number => GetDouble(this.GetString(column)),
        _ => throw new InvalidOperationException(),
    };

    private decimal GetDecimal(DbfColumn column) => GetDecimal(this.GetString(column));

    private DateTime GetDateTime(DbfColumn column) => column.DbfType switch
    {
        DbfColumn.DbfColumnType.Date => GetDateTime(this.GetString(column)),
        _ => throw new InvalidOperationException(string.Format(Properties.Resources.Culture, Properties.Resources.NotADateColumn, column.ColumnName)),
    };
}