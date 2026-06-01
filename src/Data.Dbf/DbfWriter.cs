// -----------------------------------------------------------------------
// <copyright file="DbfWriter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data.Dbf;

/// <summary>
/// This class represents a <see cref="Dbf"/> writer. You can create new and save <see cref="Dbf"/> files using this class and supporting classes.
/// </summary>
/// <param name="stream">The stream.</param>
/// <param name="options">The options.</param>
/// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="DbfWriter"/> object is disposed; otherwise, <see langword="false"/>.</param>
public class DbfWriter(Stream stream, DbfWriterOptions? options = default, bool leaveOpen = false) : IDisposable
{
    private const byte TrueByte = 0x54; // 'T'

    private const byte FalseByte = 0x46; // 'F'

    private const byte EndOfFile = 0x1A;

    // array used to clear decimals, we can clear up to 40 decimals which is much more than is allowed under DBF spec anyway.
    // Note: 48 is ASCII code for 0.
    private const byte DecimalClear = 0x30;

    private const byte DecimalNull = 0x2A;

    private readonly Stream stream = stream ?? throw new ArgumentNullException(nameof(stream));

    private readonly DbfWriterOptions options = options ?? DbfWriterOptions.Default;

    private bool disposedValue;

    private long headerPosition;

    /// <summary>
    /// Gets the header.
    /// </summary>
    internal DbfHeader? Header { get; private set; }

    /// <summary>
    /// Writes the header.
    /// </summary>
    /// <param name="columns">The columns.</param>
    public void Write(params IEnumerable<DbfColumn> columns)
    {
        var header = new DbfHeader();
        header.AddRange(columns);
        this.Write(header);
    }

    /// <summary>
    /// Writes the header.
    /// </summary>
    /// <param name="header">The header.</param>
    /// <param name="writeDataAddress">Set to <see langword="true"/> to write the <see cref="DbfColumn.DataAddress"/> value.</param>
    public void Write(DbfHeader header, bool writeDataAddress = true)
    {
        this.headerPosition = this.stream.Position;
        header.CopyTo(this.stream, writeDataAddress);
        this.Header = header;
    }

    /// <summary>
    /// Writes the values to the file.
    /// </summary>
    /// <param name="values">The values.</param>
    public void Write(params IEnumerable<object?> values)
    {
        var data = this.CreateRecordBytes();
        foreach (var (column, value) in this.Header.Zip(values, (header, value) => (header, value)))
        {
            this.WriteTo(column, value, data);
        }

        this.stream.Write(data, 0, data.Length);
    }

    /// <summary>
    /// Updates the header.
    /// </summary>
    /// <param name="recordCount">The record count.</param>
    /// <param name="writeDataAddress">Set to <see langword="true"/> to write the <see cref="DbfColumn.DataAddress"/> value.</param>
    public void Update(int recordCount, bool writeDataAddress = true)
    {
        if (this.Header is null)
        {
            throw new InvalidOperationException();
        }

        this.Header.RecordCount = (uint)recordCount;
        var currentPosition = this.stream.Position;
        this.stream.Position = this.headerPosition;
        this.Header.CopyTo(this.stream, writeDataAddress);
        this.stream.Position = currentPosition;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Creates the record bytes.
    /// </summary>
    /// <returns>The record bytes.</returns>
    /// <exception cref="InvalidOperationException">Header has not been initialised.</exception>
    [System.Diagnostics.CodeAnalysis.MemberNotNull(nameof(Header))]
    internal byte[] CreateRecordBytes() => this.Header is { RecordLength: var recordLength }
        ? new byte[recordLength]
        : throw new InvalidOperationException();

    /// <summary>
    /// Writes the values to the file.
    /// </summary>
    /// <param name="column">The column.</param>
    /// <param name="value">The value.</param>
    /// <param name="bytes">The data to write to.</param>
    internal void WriteTo(DbfColumn column, object? value, byte[] bytes) => this.WriteTo(column.DbfType, column.DataAddress, column.ColumnSize ?? 0, column.NumericPrecision, value, bytes);

    /// <summary>
    /// Writes the values to the file.
    /// </summary>
    /// <param name="columnType">The column type.</param>
    /// <param name="dataAddress">The data address.</param>
    /// <param name="columnSize">The column size.</param>
    /// <param name="numericPrecision">The numeric precision.</param>
    /// <param name="value">The value.</param>
    /// <param name="bytes">The data to write to.</param>
    internal void WriteTo(DbfColumn.DbfColumnType columnType, int dataAddress, int columnSize, int? numericPrecision, object? value, byte[] bytes)
    {
        if (this.Header is null)
        {
            throw new InvalidOperationException();
        }

        bytes[0] = DbfRecord.VacantByte;

        var encoding = this.Header.GetEncodingOrDefault();

        if (value is null)
        {
            if (columnType is DbfColumn.DbfColumnType.Number or DbfColumn.DbfColumnType.Float)
            {
                var byteToFillWith = this.options.WriteNullNumberAsSpace
                    ? DbfRecord.VacantByte // copy in '*' values
                    : DecimalNull; // copy in ' ' values
                Array.Fill(
                    bytes,
                    byteToFillWith,
                    dataAddress,
                    columnSize);
            }
            else if (columnType is DbfColumn.DbfColumnType.Date)
            {
                // copy in '0' values
                Array.Fill(bytes, DecimalClear, dataAddress, columnSize);
            }
            else if (columnType is DbfColumn.DbfColumnType.Boolean)
            {
                bytes[dataAddress] = 0x3F;
            }
            else
            {
                // this is like NULL data, set it to empty. SAS DBF output when a null value exists
                // and empty data are output. we get the same result, so this looks good.
                Array.Fill(bytes, DbfRecord.VacantByte, dataAddress, columnSize);
            }
        }
        else if (columnType is DbfColumn.DbfColumnType.Character && value is string @string)
        {
            if (!this.options.AllowStringTruncate && @string.Length > columnSize)
            {
                throw new DbfDataTruncateException($"Value not set. String truncation would occur and {nameof(this.options.AllowStringTruncate)} flag is set to false. To suppress this exception change {nameof(this.options.AllowStringTruncate)} to true.");
            }

            Array.Fill(bytes, DbfRecord.VacantByte, dataAddress, columnSize);
            encoding.GetBytes(@string, 0, Math.Min(@string.Length, columnSize), bytes, dataAddress);
        }
        else if (columnType is DbfColumn.DbfColumnType.Number && !numericPrecision.HasValue)
        {
            var stringValue = value switch
            {
                short shortNumber => shortNumber.ToString(System.Globalization.CultureInfo.InvariantCulture),
                int intNumber => intNumber.ToString(System.Globalization.CultureInfo.InvariantCulture),
                long longNumber => longNumber.ToString(System.Globalization.CultureInfo.InvariantCulture),
                _ => throw new NotSupportedException(),
            };

            // throw an exception if integer overflow would occur
            if (!this.options.AllowIntegerTruncate && stringValue.Length > columnSize)
            {
                throw new DbfDataTruncateException($"Value not set. Integer does not fit and would be truncated. {nameof(this.options.AllowIntegerTruncate)} is set to false. To suppress this exception set {nameof(this.options.AllowIntegerTruncate)} to true, although that is not recommended.");
            }

            // clear all numbers, set to [space].
            // -----------------------------------------------------
            Array.Fill(bytes, DbfRecord.VacantByte, dataAddress, columnSize);

            // set integer part, CAREFUL not to overflow buffer! (truncate instead)
            // -----------------------------------------------------------------------
            var numberLength = Math.Min(stringValue.Length, columnSize);
            encoding.GetBytes(stringValue, 0, numberLength, bytes, dataAddress + columnSize - numberLength);
        }
        else if (columnType is DbfColumn.DbfColumnType.Number && numericPrecision.HasValue && value is double or float or decimal)
        {
            // force this to decimal
            var decimalValue = value switch
            {
                double @double => (decimal)@double,
                float @float => (decimal)@float,
                decimal @decimal => @decimal,
                _ => throw new InvalidCastException(),
            };

            //------------------------------------------------------------------------------------------------------------------
            // NUMERIC TYPE
            //------------------------------------------------------------------------------------------------------------------
            var (decimalTruncation, integerTruncation) = WriteDecimal(
                bytes,
                dataAddress,
                decimalValue,
                this.options,
                columnSize,
                numericPrecision.Value,
                encoding);
            if ((decimalTruncation || integerTruncation) && WriteExponential(bytes, dataAddress, decimalValue, this.options, columnSize, numericPrecision.Value, encoding) is (false, false))
            {
                // Only set the truncation values if the exponential passes.
                // Otherwise, we want to return the original decimal failure.
                decimalTruncation = integerTruncation = false;
            }

            if (decimalTruncation)
            {
                throw new DbfDataTruncateException($"Value not set. Decimal does not fit and would be truncated. {nameof(this.options.AllowDecimalTruncate)} is set to false. To suppress this exception set {nameof(this.options.AllowDecimalTruncate)} to true.");
            }

            if (integerTruncation)
            {
                throw new DbfDataTruncateException($"Value not set. Integer does not fit and would be truncated. {nameof(this.options.AllowIntegerTruncate)} is set to false. To suppress this exception set {nameof(this.options.AllowIntegerTruncate)} to true, although that is not recommended.");
            }

            static (bool, bool) WriteDecimal(
                byte[] bytes,
                int dataAddress,
                decimal value,
                DbfWriterOptions options,
                int columnSize,
                int numericPrecision,
                System.Text.Encoding encoding)
            {
                var (decimalTruncation, integerTruncation, stringValue) = CheckTruncation(value.ToString(GetDecimalFormat(options, numericPrecision), System.Globalization.CultureInfo.InvariantCulture), options, columnSize, numericPrecision);
                if (!decimalTruncation && !integerTruncation)
                {
                    var (decimalPortion, integerPortion) = GetPortions(stringValue);

                    // try to format this as an 'e' value
                    // clear all decimals, set to 0.
                    //-----------------------------------------------------
                    Array.Fill(bytes, DecimalClear, dataAddress + columnSize - numericPrecision, numericPrecision);

                    // clear all numbers, set to [space].
                    Array.Fill(bytes, DbfRecord.VacantByte, dataAddress, columnSize);

                    // set decimal numbers, CAREFUL not to overflow buffer! (truncate instead)
                    //-----------------------------------------------------------------------
                    if (decimalPortion.Length > 0)
                    {
                        var decimalPortionLength = Math.Min(decimalPortion.Length, numericPrecision);
                        encoding.GetBytes(decimalPortion, 0, decimalPortionLength, bytes, dataAddress + columnSize - numericPrecision);
                    }

                    // set integer part, CAREFUL not to overflow buffer! (truncate instead)
                    //-----------------------------------------------------------------------
                    var integerPortionLength = Math.Min(integerPortion.Length, columnSize - numericPrecision - 1);
                    encoding.GetBytes(integerPortion, 0, integerPortionLength, bytes, dataAddress + columnSize - numericPrecision - integerPortionLength - 1);

                    // set decimal point
                    //-----------------------------------------------------------------------
                    bytes[dataAddress + columnSize - numericPrecision - 1] = 0x2E; // '.'
                }

                return (decimalTruncation, integerTruncation);

                static string GetDecimalFormat(DbfWriterOptions options, int numericPrecision)
                {
                    return $"0.{new string(options.WriteTrailingDecimals ? '0' : '#', numericPrecision)}";
                }

                static (char[] DecimalPortion, char[] IntegerPortion) GetPortions(string value)
                {
                    // break value down into integer and decimal portions
                    //--------------------------------------------------------------------------
                    var decimalPlace = value.IndexOf(System.Globalization.CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator, StringComparison.Ordinal);

                    return decimalPlace > -1
                        ? (value[(decimalPlace + 1)..].Trim().ToCharArray(), value[..decimalPlace].ToCharArray())
                        : ((char[] DecimalPortion, char[] IntegerPortion))([], value.ToCharArray());
                }
            }

            static (bool, bool) WriteExponential(byte[] bytes, int dataAddress, decimal value, DbfWriterOptions options, int columnSize, int numericPrecision, System.Text.Encoding encoding)
            {
                var (decimalTruncation, integerTruncation, stringValue) = CheckTruncation(value.ToString("e", System.Globalization.CultureInfo.InvariantCulture), options, columnSize, numericPrecision);
                if (!decimalTruncation && !integerTruncation)
                {
                    // check size, throw exception if value won't fit:
                    if (stringValue.Length > columnSize)
                    {
                        throw new DbfDataTruncateException(Properties.Resources.FloatValueTruncated);
                    }

                    // clear value that was present previously
                    Array.Fill(bytes, DecimalClear, dataAddress, columnSize);

                    // copy new value at location
                    var valueAsCharArray = stringValue.ToCharArray();
                    encoding.GetBytes(valueAsCharArray, 0, valueAsCharArray.Length, bytes, dataAddress);
                }

                return (decimalTruncation, integerTruncation);
            }

            static (bool DecimalTruncation, bool IntegerTruncation, string Value) CheckTruncation(string value, DbfWriterOptions options, int columnSize, int? numericPrecision)
            {
                var decimalPlace = value.IndexOf(System.Globalization.CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator, StringComparison.Ordinal);
                var decimalPortionLength = decimalPlace < 0 ? 0 : value.Length - decimalPlace - 1;
                var integerPortionLength = decimalPlace < 0 ? value.Length : decimalPlace;

                var decimalTruncation = false;
                var integerTruncation = false;
                if (!options.AllowDecimalTruncate && decimalPortionLength > numericPrecision)
                {
                    decimalTruncation = true;
                }

                if (!options.AllowIntegerTruncate && integerPortionLength > columnSize - numericPrecision!.Value - 1)
                {
                    integerTruncation = true;
                }

                return (decimalTruncation, integerTruncation, value);
            }
        }
        else if (columnType is DbfColumn.DbfColumnType.Float && value is double @float)
        {
            //------------------------------------------------------------------------------------------------------------------
            // FLOAT TYPE
            // example:   value=" 2.40000000000e+001"  Length=19   Decimal-Count=11
            //------------------------------------------------------------------------------------------------------------------
            var stringValue = @float.ToString(FormattableString.Invariant($"e{numericPrecision}"), System.Globalization.CultureInfo.InvariantCulture);

            // check size, throw exception if value won't fit:
            if (stringValue.Length > columnSize)
            {
                throw new DbfDataTruncateException(Properties.Resources.FloatValueTruncated);
            }

            // clear value that was present previously
            Array.Fill(bytes, DecimalClear, dataAddress, columnSize);

            // copy new value at location
            var valueAsCharArray = stringValue.ToCharArray();
            encoding.GetBytes(valueAsCharArray, 0, valueAsCharArray.Length, bytes, dataAddress);
        }
        else if (columnType is DbfColumn.DbfColumnType.Boolean && value is bool boolean)
        {
            bytes[dataAddress] = boolean ? TrueByte : FalseByte;
        }
        else if (columnType is DbfColumn.DbfColumnType.Date && value is DateTime dateTime)
        {
            encoding.GetBytes(dateTime.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture), 0, columnSize, bytes, dataAddress);
        }
        else
        {
            throw new InvalidOperationException(Properties.Resources.NotSupportedColumn);
        }
    }

    /// <summary>
    /// Writes the data.
    /// </summary>
    /// <param name="data">The data to write.</param>
    internal void Write(byte[] data) => this.stream.Write(data, 0, data.Length);

    /// <summary>
    /// Disposes the resources for this instance.
    /// </summary>
    /// <param name="disposing">Set to <see langword="true"/> to dispose of managed resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                // write out the end of file character
                this.stream.Write([EndOfFile], 0, 1);

                if (!leaveOpen)
                {
                    this.stream.Dispose();
                }
            }

            this.disposedValue = true;
        }
    }
}