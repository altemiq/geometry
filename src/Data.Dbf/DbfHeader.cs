// -----------------------------------------------------------------------
// <copyright file="DbfHeader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data.Dbf;

/// <summary>
/// This class represents a DBF IV file header.
/// </summary>
/// <remarks>
/// <para>DBF files are really wasteful on space but this legacy format lives on because it's really, really simple.
/// It lacks much in features though.
/// </para>
/// <para>
/// Thanks to Erik Bachmann for providing the DBF file structure information (http://www.clicketyclick.dk/databases/xbase/format/dbf.html).
/// </para>
/// <code>
///           _______________________  _______
/// 00h /   0| Version number        |  ^
///          |-----------------------|  |
/// 01h /   1| Date of last update   |  |
/// 02h /   2|      YYMMDD           |  |
/// 03h /   3|                       |  |
///          |-----------------------|  |
/// 04h /   4| Number of records     | Record
/// 05h /   5| in data file          | header
/// 06h /   6| ( 32 bits )           |  |
/// 07h /   7|                       |  |
///          |-----------------------|  |
/// 08h /   8| Length of header      |  |
/// 09h /   9| structure ( 16 bits ) |  |
///          |-----------------------|  |
/// 0Ah /  10| Length of each record |  |
/// 0Bh /  11| ( 16 bits )           |  |
///          |-----------------------|  |
/// 0Ch /  12| ( Reserved )          |  |
/// 0Dh /  13|                       |  |
///          |-----------------------|  |
/// 0Eh /  14| Incomplete transac.   |  |
///          |-----------------------|  |
/// 0Fh /  15| Encryption flag       |  |
///          |-----------------------|  |
/// 10h /  16| Free record thread    |  |
/// 11h /  17| ( reserved for LAN    |  |
/// 12h /  18|  only )               |  |
/// 13h /  19|                       |  |
///          |-----------------------|  |
/// 14h /  20| ( Reserved for        |  |            _        |=======================| ______
///          |   multi-user dBASE )  |  |           / 00h /  0| Field name in ASCII   |  ^
///          : ( dBASE III+ - )      :  |          /          : (terminated by 00h)   :  |
///          :                       :  |         |           |                       |  |
/// 1Bh /  27|                       |  |         |   0Ah / 10|                       |  |
///          |-----------------------|  |         |           |-----------------------| For
/// 1Ch /  28| MDX flag (dBASE IV)   |  |         |   0Bh / 11| Field type (ASCII)    | each
///          |-----------------------|  |         |           |-----------------------| field
/// 1Dh /  29| Language driver       |  |        /    0Ch / 12| Field data address    |  |
///          |-----------------------|  |       /             |                       |  |
/// 1Eh /  30| ( Reserved )          |  |      /              | (in memory !!!)       |  |
/// 1Fh /  31|                       |  |     /       0Fh / 15| (dBASE III+)          |  |
///          |=======================|__|____/                |-----------------------|  |  -
/// 20h /  32|                       |  |  ^          10h / 16| Field length          |  |   |
///          |- - - - - - - - - - - -|  |  |                  |-----------------------|  |   |
///          |                       |  |  |          11h / 17| Decimal count         |  |   |
///          |- - - - - - - - - - - -|  |  Field              |-----------------------|  |  -
///          |                       |  | Descriptor  12h / 18| ( Reserved for        |  |
///          :. . . . . . . . . . . .:  |  |array     13h / 19|   multi-user dBASE)   |  |
///          :                       :  |  |                  |-----------------------|  |
///       n  |                       |__|__v_         14h / 20| Work area ID          |  |
///          |-----------------------|  |    \                |-----------------------|  |
///       n+1| Terminator (0Dh)      |  |     \       15h / 21| ( Reserved for        |  |
///          |=======================|  |      \      16h / 22|   multi-user dBASE )  |  |
///       m  | Database Container    |  |       \             |-----------------------|  |
///          :                       :  |        \    17h / 23| Flag for SET FIELDS   |  |
///          :                       :  |         |           |-----------------------|  |
///     / m+263                      |  |         |   18h / 24| ( Reserved )          |  |
///          |=======================|__v_ ___    |           :                       :  |
///          :                       :    ^       |           :                       :  |
///          :                       :    |       |           :                       :  |
///          :                       :    |       |   1Eh / 30|                       |  |
///          | Record structure      |    |       |           |-----------------------|  |
///          |                       |    |        \  1Fh / 31| Index field flag      |  |
///          |                       |    |         \_        |=======================| _v_____
///          |                       | Records
///          |-----------------------|    |
///          |                       |    |          _        |=======================| _______
///          |                       |    |         / 00h /  0| Record deleted flag   |  ^
///          |                       |    |        /          |-----------------------|  |
///          |                       |    |       /           | Data                  |  One
///          |                       |    |      /            : (ASCII)               : record
///          |                       |____|_____/             |                       |  |
///          :                       :    |                   |                       | _v_____
///          :                       :____|_____              |=======================|
///          :                       :    |
///          |                       |    |
///          |                       |    |
///          |                       |    |
///          |                       |    |
///          |                       |    |
///          |=======================|    |
///          |__End_of_File__________| ___v____  End of file ( 1Ah )
/// </code>
/// </remarks>
public class DbfHeader : IList<DbfColumn>, ICloneable
{
    /// <summary>
    /// Header file descriptor size is 33 bytes (32 bytes + 1 terminator byte), followed by column metadata which is 32 bytes each.
    /// </summary>
    public const int FileDescriptorSize = 33;

    /// <summary>
    /// Field or DBF Column descriptor is 32 bytes long.
    /// </summary>
    public const int ColumnDescriptorSize = 32;

    private const byte EndFieldDefinitions = 0x0D;

    private static System.Text.Encoding? defaultEncoding;

    private readonly DateTime updateDate;

    private readonly List<DbfColumn> fields;

    private uint recordCount;

    private Dictionary<string, int>? columnNameIndex;

    /// <summary>
    /// Initialises a new instance of the <see cref="DbfHeader"/> class.
    /// </summary>
    public DbfHeader()
        : this(DbfVersion.DBase3WithoutMemo)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="DbfHeader"/> class.
    /// </summary>
    /// <param name="encoding">The encoding for the header.</param>
    public DbfHeader(System.Text.Encoding encoding)
        : this(DbfVersion.DBase3WithoutMemo, encoding)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="DbfHeader"/> class with the specified initial column capacity.
    /// </summary>
    /// <param name="fieldCapacity">The field capacity.</param>
    public DbfHeader(int fieldCapacity)
        : this(DbfVersion.DBase3WithoutMemo, fieldCapacity)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="DbfHeader"/> class.
    /// </summary>
    /// <param name="version">The version.</param>
    public DbfHeader(DbfVersion version)
        : this(version, GetDefaultEncoding())
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="DbfHeader"/> class.
    /// </summary>
    /// <param name="version">The version.</param>
    /// <param name="encoding">The encoding for the header.</param>
    public DbfHeader(DbfVersion version, System.Text.Encoding encoding)
        : this(version, encoding, default)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="DbfHeader"/> class with the specified initial column capacity.
    /// </summary>
    /// <param name="version">The version.</param>
    /// <param name="fieldCapacity">The field capacity.</param>
    public DbfHeader(DbfVersion version, int fieldCapacity)
        : this(version, GetDefaultEncoding(), fieldCapacity)
    {
    }

    private DbfHeader(DbfVersion version, System.Text.Encoding? encoding, int? fieldCapacity)
        : this(version, encoding, DateTime.UtcNow.Date, fieldCapacity.HasValue ? new List<DbfColumn>(fieldCapacity.Value) : [])
    {
    }

    private DbfHeader(DbfVersion version, System.Text.Encoding? encoding, DateTime updateDate, List<DbfColumn> fields, bool isReadOnly = false, bool isDirty = false)
    {
        this.Version = version;
        this.Encoding = encoding;
        this.updateDate = updateDate;
        this.fields = fields;
        this.IsReadOnly = isReadOnly;
        this.IsDirty = isDirty;
    }

    /// <summary>
    /// Gets the version.
    /// </summary>
    public DbfVersion Version { get; }

    /// <summary>
    /// Gets header length.
    /// </summary>
    public ushort HeaderLength { get; private set; } = FileDescriptorSize;

    /// <inheritdoc />
    public int Count => this.fields.Count;

    /// <summary>
    /// Gets the size of one record in bytes. All fields + 1 byte delete flag.
    /// </summary>
    public int RecordLength { get; private set; } = 1;

    /// <summary>
    /// Gets or sets the number of records in the DBF.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The reason we allow client to set <see cref="RecordCount"/> is that in certain streams, like internet streams,
    /// we can not update record count as we write out the records.
    /// </para>
    /// <para>We have to set it in advance, so client has to be able to modify this property.</para>
    /// </remarks>
    public uint RecordCount
    {
        get => this.recordCount;
        set
        {
            this.recordCount = value;

            // set the dirty bit
            this.IsDirty = true;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this object is modified after read or write.
    /// </summary>
    public bool IsDirty { get; set; }

    /// <inheritdoc/>
    public bool IsReadOnly { get; internal set; }

    /// <summary>
    /// Gets the encoding.
    /// </summary>
    internal System.Text.Encoding? Encoding { get; }

    /// <inheritdoc/>
    public DbfColumn this[int index]
    {
        get => this.fields[index];
        set
        {
            this.ThrowIfReadOnly();
            this.fields[index] = value;
            this.UpdateColumns();
        }
    }

    /// <summary>
    /// Gets the column by case-insensitive name.
    /// </summary>
    /// <param name="name">The column name.</param>
    public DbfColumn? this[string name]
    {
        get
        {
            var index = this.IndexOf(name);
            return index > -1 ? this.fields[index] : default;
        }
    }

    /// <inheritdoc/>
    void IList<DbfColumn>.Insert(int index, DbfColumn item)
    {
        this.fields.Insert(index, item);
        this.UpdateColumns();
    }

    /// <summary>
    /// Adds new columns to the DBF header.
    /// </summary>
    /// <param name="columns">The columns to add.</param>
    public void AddRange(params IEnumerable<DbfColumn> columns)
    {
        // throw exception if the header is locked
        if (this.IsReadOnly)
        {
            throw new InvalidOperationException(Properties.Resources.HeaderLocked);
        }

        this.fields.AddRange(columns);

        this.UpdateColumns();
    }

    /// <inheritdoc />
    public void Add(DbfColumn column)
    {
        // throw exception if the header is locked
        if (this.IsReadOnly)
        {
            throw new InvalidOperationException(Properties.Resources.HeaderLocked);
        }

        // since we are breaking the spec rules about max number of fields, we should at least
        // check that the record length stays within a number that can be recorded in the header!
        // we have 2 unsigned bytes for record length for a maximum of 65535.
        if (this.RecordLength + column.ColumnSize > 65535)
        {
            throw new ArgumentOutOfRangeException(nameof(column), Properties.Resources.RecordTooLarge);
        }

        // add the column
        this.fields.Add(column);

        this.UpdateColumns();
    }

    /// <summary>
    /// Create and add a new column with specified name and type.
    /// </summary>
    /// <param name="name">Field name. Uniqueness is not enforced.</param>
    /// <param name="type">The type.</param>
    public void Add(string name, DbfColumn.DbfColumnType type) => this.Add(new(name, type));

    /// <summary>
    /// Create and add a new column with specified name, type, length, and decimal precision.
    /// </summary>
    /// <param name="name">Field name. Uniqueness is not enforced.</param>
    /// <param name="type">The type.</param>
    /// <param name="length">Length of the field including decimal point and decimal numbers.</param>
    /// <param name="decimals">Number of decimal places to keep.</param>
    public void Add(string name, DbfColumn.DbfColumnType type, int length, int decimals) => this.Add(new(name, type, length, decimals));

    /// <inheritdoc/>
    bool ICollection<DbfColumn>.Remove(DbfColumn item)
    {
        if (this.fields.IndexOf(item) is not (>= 0 and var index))
        {
            return false;
        }

        this.RemoveAt(index);
        return true;
    }

    /// <inheritdoc />
    public void RemoveAt(int index)
    {
        this.ThrowIfReadOnly();

        var columnToRemove = this.fields[index];
        this.fields.RemoveAt(index);

        columnToRemove.DataAddress = 0;
        if (columnToRemove.ColumnSize.HasValue)
        {
            this.RecordLength -= columnToRemove.IsLong == true ? 10 : columnToRemove.ColumnSize.Value;
        }

        this.HeaderLength -= ColumnDescriptorSize;

        // if you remove a column offset shift for each of the columns
        // following the one removed, we need to update those offsets.
        var removedColumnLength = columnToRemove.ColumnSize ?? 0;
        for (var i = index; i < this.fields.Count; i++)
        {
            this.fields[i].DataAddress -= removedColumnLength;
        }

        // set dirty bit
        this.IsDirty = true;
        this.columnNameIndex = null;
    }

    /// <inheritdoc/>
    void ICollection<DbfColumn>.Clear()
    {
        while (this.Count > 0)
        {
            this.RemoveAt(0);
        }
    }

    /// <inheritdoc/>
    bool ICollection<DbfColumn>.Contains(DbfColumn item) => this.fields.Contains(item);

    /// <summary>
    /// Use this method with caution. Headers are locked for a reason, to prevent DBF from becoming corrupt.
    /// </summary>
    public void Unlock() => this.IsReadOnly = false;

    /// <inheritdoc/>
    public object Clone() => this.MemberwiseClone();

    /// <inheritdoc/>
    public void CopyTo(DbfColumn[] array, int arrayIndex) => this.fields.CopyTo(array, arrayIndex);

    /// <summary>
    /// Copies this instance to the specified stream.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="writeDataAddress">Set to <see langword="true"/> to write the <see cref="DbfColumn.DataAddress"/> value.</param>
    public void CopyTo(Stream stream, bool writeDataAddress = true)
    {
        using var binaryWriter = new BinaryWriter(stream, this.GetEncodingOrDefault(), leaveOpen: true);
        this.CopyTo(binaryWriter, writeDataAddress);
    }

    /// <summary>
    /// Encoding must be ASCII for this binary writer.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="writeDataAddress">Set to <see langword="true"/> to write the <see cref="DbfColumn.DataAddress"/> value.</param>
    /// <remarks>See class remarks for DBF file structure.</remarks>
    public void CopyTo(BinaryWriter writer, bool writeDataAddress = true)
    {
        // write the header
        // write the output file type.
        writer.Write(this.Version);

        // Update date format is YYMMDD, which is different from the column Date type (YYYYDDMM)
        writer.Write((byte)(this.updateDate.Year - 1900));
        writer.Write((byte)this.updateDate.Month);
        writer.Write((byte)this.updateDate.Day);

        // write the number of records in the datafile. (32-bit number, little-endian unsigned)
        writer.Write(this.recordCount);

        // write the length of the header structure.
        writer.Write(this.HeaderLength);

        // write the length of a record
        writer.Write((ushort)this.RecordLength);

        // write the reserved bytes in the header
        for (var i = 0; i < 17; i++)
        {
            writer.Write(byte.MinValue);
        }

        writer.Write(GetLcidFromEncoding(this.Encoding));

        for (var i = 0; i < 2; i++)
        {
            writer.Write(byte.MinValue);
        }

        // write all the header records
        var byteReserved = new byte[14];
        foreach (var field in this.fields)
        {
            // write the field name
            writer.Write(field.ColumnName.PadRight(11, '\0').ToCharArray());

            // write the field type
            writer.Write(field.DbfTypeChar);

            // write the field data address, offset from the start of the record.
            writer.Write(writeDataAddress ? field.DataAddress : 0);

            // get the column size.
            // if the field is long, then set to the pointer size.
            var columnSize = field.IsLong == true ? 10 : field.ColumnSize ?? 0;

            // write the length of the field.
            // if char field is longer than 255 bytes, then we use the decimal field as part of the field length.
            if (field.DbfType is DbfColumn.DbfColumnType.Character)
            {
                // treat decimal count as high byte of field length, this extends char field max to 65535
                writer.Write((ushort)columnSize);
            }
            else
            {
                // write the length of the field.
                writer.Write((byte)columnSize);

                // write the decimal count.
                writer.Write((byte)(field.NumericPrecision ?? 0));
            }

            // write the reserved bytes.
            writer.Write(byteReserved);
        }

        // write the end of the field definitions marker
        writer.Write(EndFieldDefinitions);
        writer.Flush();

        // clear dirty bit
        this.IsDirty = false;

        // lock the header so it can not be modified any longer, we could actually postpone this until first record is written!
        this.IsReadOnly = true;

        static byte GetLcidFromEncoding(System.Text.Encoding? encoding)
        {
            return encoding switch
            {
                null => default,
                { HeaderName: { } headerName } when string.Equals(headerName, GetDefaultEncoding().HeaderName, StringComparison.OrdinalIgnoreCase) => 87,
                { CodePage: var codePage } => GetLcidFromCodePage(codePage),
            };

            static byte GetLcidFromCodePage(int codePage)
            {
                return codePage switch
                {
                    437 => 1,
                    620 => 105,
                    737 => 106,
                    850 => 2,
                    852 => 31,
                    857 => 107,
                    860 => 36,
                    861 => 103,
                    863 => 28,
                    865 => 8,
                    866 => 38,
                    874 => 80,
                    895 => 104,
                    932 => 19,
                    936 => 77,
                    949 => 78,
                    950 => 79,
                    1250 => 200,
                    1251 => 201,
                    1252 => 3,
                    1253 => 203,
                    1254 => 202,
                    1257 => 204,
                    10000 => 4,
                    10007 => 150,
                    10029 => 151,
                    _ => 0,
                };
            }
        }
    }

    /// <inheritdoc/>
    public IEnumerator<DbfColumn> GetEnumerator() => this.fields.GetEnumerator();

    /// <inheritdoc/>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.fields.GetEnumerator();

    /// <inheritdoc/>
    public int IndexOf(DbfColumn item) => item.ColumnOrdinal ?? this.IndexOf(item.ColumnName);

    /// <summary>
    /// Finds a column index by using a fast dictionary lookup-- creates column dictionary on first use. Returns -1 if not found.
    /// </summary>
    /// <param name="name">Column name (case insensitive comparison).</param>
    /// <returns>column index (0 based) or -1 if not found.</returns>
    public int IndexOf(string name)
    {
        this.columnNameIndex ??= Create(this.fields);
        return this.columnNameIndex.GetValueOrDefault(name, -1);

        static Dictionary<string, int> Create(IList<DbfColumn> fields)
        {
            var names = new Dictionary<string, int>(fields.Count, StringComparer.OrdinalIgnoreCase);

            // create a new index
            for (var i = 0; i < fields.Count; i++)
            {
                names.Add(fields[i].ColumnName, i);
            }

            return names;
        }
    }

    /// <summary>
    /// Read header data, make sure the stream is positioned at the start of the file to read the header otherwise you will get an exception.
    /// When this function is done the position will be the first record.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>The <see cref="DbfHeader"/>.</returns>
    /// <exception cref="NotSupportedException">Unsupported DBF reader Type.</exception>
    internal static DbfHeader ReadFrom(Stream stream)
    {
        DbfVersion version;
        DateTime updateDateField;
        uint recordCountField;
        ushort headerLength;
        short recordLength;
        System.Text.Encoding? encoding;
        using (var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true))
        {
            // type of reader.
            var fileType = (DbfVersion)reader.ReadByte();
            if (!DbfVersion.IsDefined(fileType))
            {
                throw new NotSupportedException($"Unsupported DBF reader Type {fileType}");
            }

            version = fileType;

            // parse the update date information.
            var year = (int)reader.ReadByte();
            var month = (int)reader.ReadByte();
            var day = (int)reader.ReadByte();
            updateDateField = new DateTime(year + 1900, month, day, default, default, default, DateTimeKind.Unspecified);

            // read the number of records.
            recordCountField = reader.ReadUInt32();

            // read the length of the header structure.
            headerLength = reader.ReadUInt16();

            // read the length of a record
            recordLength = reader.ReadInt16();

            // skip the reserved bytes in the header.
            _ = reader.ReadBytes(17);

            var languageDriver = reader.ReadByte();
            encoding = GetEncodingFromLcid(languageDriver);

            _ = reader.ReadBytes(2);
        }

        // calculate the number of Fields in the header
        var fieldCount = (headerLength - FileDescriptorSize) / ColumnDescriptorSize;

        // offset from start of record, start at 1 because that's the delete flag.
        var dataOffset = 1;

        // read all the header records
        var columns = new List<DbfColumn>(fieldCount);
        var nameEncoding = GetEncodingOrDefault(encoding);
        var fieldBuffer = new byte[ColumnDescriptorSize];
        for (var i = 0; i < fieldCount; i++)
        {
            _ = stream.Read(fieldBuffer, 0, 1);
            if (fieldBuffer[0] == '\x0d')
            {
                break;
            }

            _ = stream.Read(fieldBuffer, 1, ColumnDescriptorSize - 1);

            // read the field name
            var fieldName = nameEncoding.GetString(fieldBuffer, 0, 10);
            var nullIndex = fieldName.IndexOf('\0', StringComparison.Ordinal);
            if (nullIndex is not -1)
            {
                fieldName = fieldName[..nullIndex];
            }

            // read the field type
            var dbaseType = (char)fieldBuffer[11];

            // read the field data address, offset from the start of the record.
            // this is computed, and so is ignored.

            // read the field length in bytes
            // if field type is char, then read FieldLength and Decimal count as one number to allow char fields to be
            // longer than 256 bytes (ASCII char). This is the way Clipper and FoxPro do it, and there is really no downside
            // since for char fields decimal count should be zero for other versions that do not support this extended functionality.
            //-----------------------------------------------------------------------------------------------------------------------
            int fieldLength;
            var decimals = 0;
            if (dbaseType is 'C' or 'c')
            {
                // treat decimal count as high byte
                fieldLength = System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(fieldBuffer.AsSpan(16));
            }
            else
            {
                // read field length as an unsigned byte.
                fieldLength = fieldBuffer[16];

                // read decimal count as one byte
                decimals = fieldBuffer[17];
            }

            // Create and add field to collection
            var column = new DbfColumn(fieldName, dbaseType, fieldLength, decimals, dataOffset);
            column.SetColumnOrdinal(columns.Count);
            columns.Add(column);

            // add up address information, you can not trust the address recorded in the DBF file
            dataOffset += fieldLength;
        }

        // read any extra header bytes...move to first record
        // equivalent to reader.BaseStream.Seek(mHeaderLength, SeekOrigin.Begin) except that we are not using the seek function since
        // we need to support streams that can not seek like web connections.
        if (stream.CanSeek)
        {
            stream.Position = headerLength;
        }
        else
        {
            var extraReadBytes = headerLength - stream.Position;
            if (extraReadBytes > 0)
            {
                var buffer = new byte[extraReadBytes];
                _ = stream.Read(buffer, 0, buffer.Length);
            }
        }

        // If the stream is not forward-only, calculate number of records using file size as sometimes the header does not contain the correct record count.
        // If we are reading the file from the web, we have to use ReadNext() functions anyway so the number of records is not so important,
        // and we can trust the DBF to have it stored correctly.
        if (stream.CanSeek
            && recordCountField is 0
            && recordLength > 0)
        {
            // notice here that we subtract file end byte which is supposed to be 0x1A,
            // but some DBF files are incorrectly written without this byte, so we round off to nearest integer.
            // that gives a correct result with or without ending byte.
            recordCountField = (uint)Math.Round((double)(stream.Length - headerLength - 1) / recordLength, MidpointRounding.AwayFromZero);
        }

        return new(version, encoding, updateDateField, columns, isReadOnly: true) { HeaderLength = headerLength, RecordLength = recordLength, RecordCount = recordCountField, };

        static System.Text.Encoding? GetEncodingFromLcid(byte lcid)
        {
            return lcid switch
            {
                0 => default,
                87 => GetDefaultEncoding(),
                1 or 11 or 13 or 15 or 17 or 21 or 24 or 25 or 27 => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(437),
                105 => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(620),
                106 or 134 => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(737),
                2 or 10 or 14 or 16 or 18 or 20 or 22 or 26 or 29 or 37 or 55 => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(850),
                31 or 34 or 35 or 64 or 100 or 135 => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(852),
                107 or 136 => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(857),
                36 => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(860),
                103 => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(861),
                28 or 108 => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(863),
                8 or 23 or 102 => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(865),
                38 or 101 => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(866),
                80 or 124 => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(874),
                104 => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(895),
                19 or 123 => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(962),
                77 or 122 => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(936),
                78 or 121 => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(949),
                79 or 120 => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(950),
                200 => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(1250),
                201 => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(1251),
                3 or 88 or 89 => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(1252),
                203 => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(1253),
                202 => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(1254),
                204 => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(1257),
                4 => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(10000),
                150 => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(10007),
                151 => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(10029),
                _ => System.Text.CodePagesEncodingProvider.Instance.GetEncoding(1252),
            };
        }
    }

    /// <summary>
    /// Gets the encoding or default.
    /// </summary>
    /// <returns>Either the value from <see cref="Encoding"/>, or the default encoding.</returns>
    internal System.Text.Encoding GetEncodingOrDefault() => GetEncodingOrDefault(this.Encoding);

    private static System.Text.Encoding GetEncodingOrDefault(System.Text.Encoding? encoding) => encoding ?? GetDefaultEncoding();

    private static System.Text.Encoding GetDefaultEncoding() => defaultEncoding ??= System.Text.Encoding.GetEncoding("ISO-8859-1");

    private void ThrowIfReadOnly()
    {
        // throw exception if the header is locked
        if (this.IsReadOnly)
        {
            throw new InvalidOperationException(Properties.Resources.HeaderLocked);
        }
    }

    private void UpdateColumns()
    {
        // update offset bits, record and header lengths
        var recordLength = 1;
        var columnOrdinal = 1;
        foreach (var column in this.fields)
        {
            column.SetColumnOrdinal(columnOrdinal);
            column.DataAddress = recordLength;
            var columnLength = column switch
            {
                { IsLong: true } => 10,
                { ColumnSize: { } size } => size,
                _ => throw new InvalidOperationException(),
            };

            recordLength += columnLength;
            columnOrdinal++;
        }

        this.RecordLength = recordLength;

        this.HeaderLength = (ushort)(FileDescriptorSize + (this.Count * ColumnDescriptorSize));

        // set dirty bit
        this.IsDirty = true;
        this.columnNameIndex = null;
    }
}