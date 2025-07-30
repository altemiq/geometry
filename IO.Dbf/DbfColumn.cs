// -----------------------------------------------------------------------
// <copyright file="DbfColumn.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Dbf;

/// <summary>
/// This class represents a <see cref="Dbf"/> Column.
/// </summary>
/// <remarks>
/// <para>Note that certain properties can not be modified after creation of the object.</para>
/// <para>This is because we are locking the header object after creation of a data row, and columns are part of the header so either we have to have a lock field for each column, or make it so that certain properties such as length can only be set during creation of a column.</para>
/// <para>Otherwise, a user of this object could modify a column that belongs to a locked header and thus corrupt the <see cref="Dbf"/> file.</para>
/// </remarks>
public class DbfColumn : System.Data.Common.DbColumn, ICloneable
{
    /// <summary>
    /// Initialises a new instance of the <see cref="DbfColumn"/> class and sets all relevant fields.
    /// </summary>
    /// <param name="name">The column name.</param>
    /// <param name="type">The type of field.</param>
    /// <param name="length">The field length including decimal places and decimal point if any.</param>
    /// <param name="decimals">The decimal places.</param>
    public DbfColumn(string name, DbfColumnType type, int length, int decimals = default)
    {
        decimals = type is DbfColumnType.Number or DbfColumnType.Float
            ? decimals
            : 0;

        // Perform some simple integrity checks...
        // -------------------------------------------

        // decimal precision: we could also fix the length property with a statement like this: <c>length = decimalCount + 2</c>
        if (decimals > 0 && length - decimals <= 1)
        {
            throw new ArgumentException(Properties.Resources.DecimalPrecisionVsLength, nameof(decimals));
        }

        length = type switch
        {
            DbfColumnType.Boolean => 1,
            DbfColumnType.Date => 8,
            DbfColumnType.Memo => 10,
            _ => length,
        };

        // field length:
        if (length <= 0)
        {
            throw new ArgumentException(Properties.Resources.FieldLengthGreaterThenZero, nameof(length));
        }

        if (type is not DbfColumnType.Character && length > 255)
        {
            throw new ArgumentException(Properties.Resources.FieldLengthGreaterThanByte, nameof(length));
        }

        if (type is DbfColumnType.Character && length > 65535)
        {
            throw new ArgumentException(Properties.Resources.FieldLengthGreaterThanShort, nameof(length));
        }

        System.Diagnostics.Contracts.Contract.EndContractBlock();

        if (decimals is not 0)
        {
            this.NumericPrecision = decimals;
        }

        // set the DbColumn values
        this.ColumnName = name switch
        {
            null or { Length: 0 } => throw new ArgumentNullException(nameof(name), Properties.Resources.FieldNameTooShort),
            { Length: > 11 } => throw new ArgumentOutOfRangeException(nameof(name), Properties.Resources.FieldNameTooLong),
            _ => name,
        };

        this.DbfType = type;
        this.DataType = (type, decimals, length) switch
        {
            (DbfColumnType.Character or DbfColumnType.Memo, _, _) => typeof(string),
            (DbfColumnType.Number, > 0, > 15) => typeof(decimal),
            (DbfColumnType.Number, > 0, <= 7) => typeof(float),
            (DbfColumnType.Number, > 0, _) => typeof(double),
            (DbfColumnType.Number, 0, >= 10) => typeof(long),
            (DbfColumnType.Number, 0, < 10) => typeof(int),
            (DbfColumnType.Boolean, _, _) => typeof(bool),
            (DbfColumnType.Float, _, _) => typeof(float),
            (DbfColumnType.Date, _, _) => typeof(DateTime),
            _ => throw new NotSupportedException(),
        };

        this.IsLong = type is DbfColumnType.Memo;
        this.ColumnSize = this.IsLong == true ? int.MaxValue : length;
        this.DataTypeName = type.ToString();
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="DbfColumn"/> class.
    /// </summary>
    /// <param name="name">The column name.</param>
    /// <param name="type">The type of field.</param>
    public DbfColumn(string name, DbfColumnType type)
        : this(name, type, 0, 0)
    {
        if (type is DbfColumnType.Number or DbfColumnType.Character)
        {
            throw new ArgumentException(Properties.Resources.MustSpecifyLength, nameof(type));
        }

        System.Diagnostics.Contracts.Contract.EndContractBlock();
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="DbfColumn"/> class fully specifying all properties.
    /// </summary>
    /// <param name="name">The column name.</param>
    /// <param name="type">The type of field.</param>
    /// <param name="length">The field length including decimal places and decimal point if any.</param>
    /// <param name="decimals">The decimal places.</param>
    /// <param name="dataAddress">The offset from start of record.</param>
    internal DbfColumn(string name, char type, int length, int decimals, int dataAddress)
        : this(name, GetDbaseType(type), length, decimals) => this.DataAddress = dataAddress;

    /// <summary>
    /// Great information on DBF located here:
    /// http://www.clicketyclick.dk/databases/xbase/format/data_types.html
    /// http://www.clicketyclick.dk/databases/xbase/format/dbf.html
    /// also take a look at this:
    /// http://www.dbase.com/Knowledgebase/INT/db7_file_fmt.htm
    /// for more information.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "This is by design.")]
    public enum DbfColumnType
    {
        /// <summary>
        /// <para>
        /// C Character   All OEM code page characters - padded with blanks to the width of the field.
        /// Character  less than 254 length
        /// ASCII text less than 254 characters long in dBASE.
        /// </para>
        /// <para>
        /// Character fields can be up to 32 KB long (in Clipper and FoxPro) using decimal
        /// count as high byte in field length. It's possible to use up to 64KB long fields
        /// by reading length as unsigned.
        /// </para>
        /// </summary>
        Character,

        /// <summary>
        /// <para>
        /// Number Length: less than 18
        ///   ASCII text up till 18 characters long (include sign and decimal point).
        /// </para>
        /// <para>
        /// Valid characters:
        ///   "0" - "9" and "-". Number fields can be up to 20 characters long in FoxPro and Clipper.
        /// </para>
        /// </summary>
        /// <remarks>
        /// We are not enforcing this 18 char limit.
        /// </remarks>
        Number,

        /// <summary>
        /// <para>L  Logical  Length: 1    Boolean/byte (8 bit).</para>
        /// <list type="table">
        ///   <listheader>
        ///     <term>Legal value</term>
        ///     <description></description>
        ///   </listheader>
        ///   <item>
        ///     <term>?</term>
        ///     <description>Not initialised (default)</description>
        ///   </item>
        ///   <item>
        ///     <term>Y,y</term>
        ///     <description>Yes</description>
        ///   </item>
        ///   <item>
        ///     <term>N,n</term>
        ///     <description>No</description>
        ///   </item>
        ///   <item>
        ///     <term>F,f</term>
        ///     <description>False</description>
        ///   </item>
        ///   <item>
        ///     <term>T,t</term>
        ///     <description>True</description>
        ///   </item>
        /// </list>
        /// <para>Logical fields are always displayed using T/F/?. Some sources claims that space (ASCII 20h) is valid for not initialised. Space may occur, but is not defined.</para>
        /// </summary>
        Boolean,

        /// <summary>
        /// D  Date  Length: 8  Date in format YYYYMMDD. A date like 0000-00-00 is *NOT* valid.
        /// </summary>
        Date,

        /// <summary>
        /// M  Memo  Length: 10  Pointer to ASCII text field in memo file 10 digits representing a pointer to a DBT block (default is blanks).
        /// </summary>
        Memo,

        /// <summary>
        /// <para>F  Float  Number stored as a string, right justified, and padded with blanks to the width of the field.</para>
        /// <para>This type was added in DBF V4.</para>
        /// </summary>
        /// <example><c>value = " 2.40000000000e+001" Length=19  Decimal_Count=11</c>.</example>
        Float,
    }

    /// <summary>
    /// Gets the field type (C N L D or M).
    /// </summary>
    public DbfColumnType DbfType { get; }

    /// <summary>
    /// Gets the column type as a char, (as written in the DBF column header)
    /// N=number, C=char, B=binary, L=boolean, D=date, I=integer, M=memo, F=float.
    /// </summary>
    public char DbfTypeChar => this.DbfType switch
    {
        DbfColumnType.Character => 'C',
        DbfColumnType.Number => 'N',
        DbfColumnType.Boolean => 'L',
        DbfColumnType.Date => 'D',
        DbfColumnType.Memo => 'M',
        DbfColumnType.Float => 'F',
        _ => throw new InvalidCastException(Properties.Resources.UnrecognizedFieldType),
    };

    /// <summary>
    /// Gets the field data address offset from the start of the record.
    /// </summary>
    public int DataAddress { get; internal set; }

    /// <summary>
    /// Gets the variable index.
    /// </summary>
    internal int? VariableIndex { get; }

    /// <summary>
    /// Gets the nullable index.
    /// </summary>
    internal int? NullableIndex { get; }

    /// <summary>
    /// Creates a string column.
    /// </summary>
    /// <param name="name">The name of the column.</param>
    /// <param name="length">The field length.</param>
    /// <returns>The <see cref="DbfColumn"/> of <see cref="DbfColumnType.Character"/>.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "Checked")]
    public static DbfColumn String(string name, int length) => new(name, DbfColumnType.Character, length);

    /// <summary>
    /// Creates a number column.
    /// </summary>
    /// <param name="name">The name of the column.</param>
    /// <param name="length">The field length including decimal places and decimal point if any.</param>
    /// <param name="decimals">The decimal places.</param>
    /// <returns>The <see cref="DbfColumn"/> of <see cref="DbfColumnType.Number"/>.</returns>
    public static DbfColumn Number(string name, int length, int decimals) => new(name, DbfColumnType.Number, length, decimals);

    /// <summary>
    /// Creates a boolean column.
    /// </summary>
    /// <param name="name">The name of the column.</param>
    /// <returns>The <see cref="DbfColumn"/> of <see cref="DbfColumnType.Boolean"/>.</returns>
    public static DbfColumn Boolean(string name) => new(name, DbfColumnType.Boolean);

    /// <summary>
    /// Creates a date column.
    /// </summary>
    /// <param name="name">The name of the column.</param>
    /// <returns>The <see cref="DbfColumn"/> of <see cref="DbfColumnType.Date"/>.</returns>
    public static DbfColumn Date(string name) => new(name, DbfColumnType.Date);

    /// <summary>
    /// Creates a memo column.
    /// </summary>
    /// <param name="name">The name of the column.</param>
    /// <returns>The <see cref="DbfColumn"/> of <see cref="DbfColumnType.Memo"/>.</returns>
    public static DbfColumn Memo(string name) => new(name, DbfColumnType.Memo);

    /// <summary>
    /// Creates a float column.
    /// </summary>
    /// <param name="name">The name of the column.</param>
    /// <param name="length">The field length including decimal places and decimal point if any.</param>
    /// <param name="decimals">The decimal places.</param>
    /// <returns>The <see cref="DbfColumn"/> of <see cref="DbfColumnType.Float"/>.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "Checked")]
    public static DbfColumn Float(string name, int length, int decimals) => new(name, DbfColumnType.Float, length, decimals);

    /// <inheritdoc/>
    public object Clone() => this.MemberwiseClone();

    /// <summary>
    /// Gets the <see cref="System.Data.Common.DbColumn.ColumnOrdinal"/> value.
    /// </summary>
    /// <param name="i">The ordinal.</param>
    internal void SetColumnOrdinal(int i) => this.ColumnOrdinal = i;

    private static DbfColumnType GetDbaseType(char c) => c switch
    {
        'C' or 'c' => DbfColumnType.Character,
        'N' or 'n' => DbfColumnType.Number,
        'L' or 'l' => DbfColumnType.Boolean,
        'D' or 'd' => DbfColumnType.Date,
        'M' or 'm' => DbfColumnType.Memo,
        'F' or 'f' => DbfColumnType.Float,
        _ => throw new NotSupportedException($"{c} does not have a corresponding dbase type."),
    };
}