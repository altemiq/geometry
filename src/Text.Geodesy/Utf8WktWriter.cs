// -----------------------------------------------------------------------
// <copyright file="Utf8WktWriter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.Geodesy;

#pragma warning disable SA1405 // Debug.Assert should provide message text

/// <summary>
/// Provides a high-performance API for forward-only, write-only access to UTF-8 encoded WKT text.
/// </summary>
public sealed class Utf8WktWriter : IDisposable
{
    private const int DefaultGrowthSize = 4096;
    private const int InitialGrowthSize = 256;

    private System.Buffers.IBufferWriter<byte>? output;
    private Stream? stream;
    private System.Buffers.ArrayBufferWriter<byte>? arrayBufferWriter;

    private Memory<byte> memory;

    private WktTokenType tokenType;

    // The highest order bit of _currentDepth is used to discern whether we are writing the first item in a list or not.
    // if the current depth bit offset by 31 is 1, add a list separator before writing the item
    // else, no list separator is needed since we are writing the first item.
    private int currentDepth;

    // Since WktWriterOptions is a struct, use a field to avoid a copy for internal code.
    private WktWriterOptions options;

    // Cache indentation settings from WktWriterOptions to avoid recomputing them in the hot path.
    private byte indentByte;
    private int indentLength;

    // A length of 1 will emit LF for indented writes, a length of 2 will emit CRLF. Other values are invalid.
    private int newLineLength;

    /// <summary>
    /// Initialises a new instance of the <see cref="Utf8WktWriter"/> class.
    /// </summary>
    /// <param name="bufferWriter">An instance of <see cref="System.Buffers.IBufferWriter{Byte}" /> used as a destination for writing Wkt text into.</param>
    /// <param name="options">Defines the customized behavior of the <see cref="Utf8WktWriter"/>
    /// By default, the <see cref="Utf8WktWriter"/> writes WKT minimized (that is, with no extra whitespace)
    /// and validates that the WKT being written is structurally valid according to WKT RFC.</param>
    public Utf8WktWriter(System.Buffers.IBufferWriter<byte> bufferWriter, WktWriterOptions options = default)
    {
        ArgumentNullException.ThrowIfNull(bufferWriter);

        this.output = bufferWriter;
        this.SetOptions(options);
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="Utf8WktWriter"/> class.
    /// </summary>
    /// <param name="utf8Wkt">The stream to write WKT text to.</param>
    /// <param name="options">Defines the customized behavior of the <see cref="Utf8WktWriter"/>
    /// By default, the <see cref="Utf8WktWriter"/> writes WKT minimized (that is, with no extra whitespace)
    /// and validates that the WKT being written is structurally valid according to WKT RFC.</param>
    public Utf8WktWriter(Stream utf8Wkt, WktWriterOptions options = default)
    {
        ArgumentNullException.ThrowIfNull(utf8Wkt);

        this.stream = utf8Wkt;
        this.SetOptions(options);

        this.arrayBufferWriter = new();
    }

    private Utf8WktWriter()
    {
    }

    /// <summary>
    /// Gets the amount of bytes written by the <see cref="Utf8WktWriter"/> so far
    /// that have not yet been flushed to the output and committed.
    /// </summary>
    public int BytesPending { get; private set; }

    /// <summary>
    /// Gets the amount of bytes committed to the output by the <see cref="Utf8WktWriter"/> so far.
    /// </summary>
    /// <remarks>
    /// In the case of IBufferwriter, this is how much the IBufferWriter has advanced.
    /// In the case of Stream, this is how much data has been written to the stream.
    /// </remarks>
    public long BytesCommitted { get; private set; }

    /// <summary>
    /// Gets the custom behavior when writing Wkt using
    /// the <see cref="Utf8WktWriter"/> which indicates whether to format the output
    /// while writing and whether to skip structural Wkt validation or not.
    /// </summary>
    public WktWriterOptions Options => this.options;

    /// <summary>
    /// Gets the recursive depth of the nested objects / arrays within the WKT text
    /// written so far. This provides the depth of the current token.
    /// </summary>
    public int CurrentDepth => this.currentDepth & WktConstants.RemoveFlagsBitMask;

    /// <summary>
    /// Gets the token type.
    /// </summary>
    internal WktTokenType TokenType => this.tokenType;

    private int Indentation => this.CurrentDepth * this.indentLength;

    /// <summary>
    /// Writes the beginning of a JSON object with a property name as the key.
    /// </summary>
    /// <param name="propertyName">The name of the property to write.</param>
    /// <remarks>
    /// The property name is escaped before writing.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// Thrown when the specified property name is too large.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="propertyName"/> parameter is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the depth of the JSON has exceeded the maximum depth of 1000
    /// OR if this would result in invalid JSON being written (while validation is enabled).
    /// </exception>
    public void WriteStartObject(string propertyName)
    {
        ArgumentNullException.ThrowIfNull(propertyName);
        this.WriteStartObject(propertyName.AsSpan());
    }

    /// <summary>
    /// Writes the beginning of a JSON object with a property name as the key.
    /// </summary>
    /// <param name="propertyName">The name of the property to write.</param>
    /// <remarks>
    /// The property name is escaped before writing.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// Thrown when the specified property name is too large.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the depth of the JSON has exceeded the maximum depth of 1000
    /// OR if this would result in invalid JSON being written (while validation is enabled).
    /// </exception>
    public void WriteStartObject(ReadOnlySpan<char> propertyName)
    {
        Span<byte> name = stackalloc byte[propertyName.Length * 3];
        var length = System.Text.Encoding.UTF8.GetBytes(propertyName, name);
        this.WriteLiteralByOptions(name[..length], includeNewLine: this.tokenType is not WktTokenType.None);
        this.tokenType = WktTokenType.Keyword;

        this.currentDepth &= WktConstants.RemoveFlagsBitMask;

        this.WriteObjectStart();
    }

    /// <summary>
    /// Writes a WKT object start marker.
    /// </summary>
    public void WriteObjectStart()
    {
        this.WriteStart(WktConstants.OpenBracket);
        this.tokenType = WktTokenType.StartObject;
    }

    /// <summary>
    /// Writes the end of a WKT object.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this would result in invalid Wkt being written (while validation is enabled).
    /// </exception>
    public void WriteEndObject()
    {
        this.WriteEnd(WktConstants.CloseBracket);
        this.tokenType = WktTokenType.EndObject;
    }

    /// <summary>
    /// Writes the <see cref="double"/> value (as a WKT number) as an element of a WKT object.
    /// </summary>
    /// <param name="value">The value to write.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this would result in invalid WKT being written (while validation is enabled).
    /// </exception>
    /// <remarks>
    /// Writes the <see cref="double"/> using the default <see cref="System.Buffers.StandardFormat"/> on .NET Core 3 or higher and 'G15' on any other framework.
    /// </remarks>
    public void WriteNumberValue(double value)
    {
        if (this.options.Indented)
        {
            this.WriteNumberValueIndented(value, ensureDecimal: true);
        }
        else
        {
            this.WriteNumberValueMinimized(value, ensureDecimal: true);
        }

        this.SetFlagToAddListSeparatorBeforeNextItem();
        this.tokenType = WktTokenType.Number;
    }

    /// <summary>
    /// Writes the <see cref="int"/> value (as a WKT number) as an element of a WKT object.
    /// </summary>
    /// <param name="value">The value to write.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this would result in invalid WKT being written (while validation is enabled).
    /// </exception>
    public void WriteNumberValue(int value)
    {
        if (this.options.Indented)
        {
            this.WriteNumberValueIndented(value, ensureDecimal: false);
        }
        else
        {
            this.WriteNumberValueMinimized(value, ensureDecimal: false);
        }

        this.SetFlagToAddListSeparatorBeforeNextItem();
        this.tokenType = WktTokenType.Number;
    }

    /// <summary>
    /// Writes the string text value (as a Wkt string) as an element of a Wkt array.
    /// </summary>
    /// <param name="value">The value to write.</param>
    /// <exception cref="ArgumentException">Thrown when the specified value is too large.</exception>
    /// <exception cref="InvalidOperationException">Thrown if this would result in invalid Wkt being written (while validation is enabled).</exception>
    /// <remarks>The value is escaped before writing.</remarks>
    public void WriteStringValue(string value) => this.WriteStringValue(value.AsSpan());

    /// <summary>
    /// Writes the text value (as a Wkt string) as an element of a Wkt array.
    /// </summary>
    /// <param name="value">The value to write.</param>
    /// <exception cref="ArgumentException">Thrown when the specified value is too large.</exception>
    /// <exception cref="InvalidOperationException">Thrown if this would result in invalid Wkt being written (while validation is enabled).</exception>
    /// <remarks>The value is escaped before writing.</remarks>
    public void WriteStringValue(ReadOnlySpan<char> value)
    {
        this.WriteStringEscape(value);

        this.SetFlagToAddListSeparatorBeforeNextItem();
        this.tokenType = WktTokenType.String;
    }

    /// <summary>
    /// Writes the literal value as an element of a WKT object.
    /// </summary>
    /// <param name="value">The value to write.</param>
    /// <exception cref="ArgumentException">Thrown when the specified value is too large.</exception>
    /// <exception cref="InvalidOperationException">Thrown if this would result in invalid Wkt being written (while validation is enabled).</exception>
    public void WriteLiteralValue(Literal value)
    {
        this.WriteLiteralByOptions(value.Span, includeNewLine: false);
        this.SetFlagToAddListSeparatorBeforeNextItem();
        this.tokenType = WktTokenType.Literal;
    }

    /// <summary>
    /// Flushes any buffered data to the underlying stream.
    /// </summary>
    public void Flush()
    {
        this.CheckNotDisposed();

        this.memory = default;

        if (this.stream is not null)
        {
            System.Diagnostics.Debug.Assert(this.arrayBufferWriter is not null);
            if (this.BytesPending is not 0)
            {
                this.arrayBufferWriter!.Advance(this.BytesPending);
                this.BytesPending = 0;

                this.stream.Write(this.arrayBufferWriter.WrittenSpan);

                this.BytesCommitted += this.arrayBufferWriter.WrittenCount;
                this.arrayBufferWriter.Clear();
            }

            this.stream.Flush();
        }
        else
        {
            System.Diagnostics.Debug.Assert(this.output is not null);
            if (this.BytesPending is not 0)
            {
                this.output!.Advance(this.BytesPending);
                this.BytesCommitted += this.BytesPending;
                this.BytesPending = 0;
            }
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        // The conditions are ordered with stream first as that would be the most common mode
        if (this.stream is null || this.output is null)
        {
            return;
        }

        this.Flush();

        this.stream = null;
        this.arrayBufferWriter = null;
        this.output = null;
    }

    private void WriteEnd(byte token)
    {
        if (this.options.Indented)
        {
            this.WriteEndSlow(token);
        }
        else
        {
            this.WriteEndMinimized(token);
        }

        this.SetFlagToAddListSeparatorBeforeNextItem();

        // Necessary if WriteEndX is called without a corresponding WriteStartX first.
        if (this.CurrentDepth is not 0)
        {
            this.currentDepth--;
        }
    }

    private void WriteEndMinimized(byte token)
    {
        // 1 end token
        if (this.memory.Length - this.BytesPending < 1)
        {
            this.Grow(1);
        }

        Span<byte> outputSpan = this.memory.Span;
        outputSpan[this.BytesPending++] = token;
    }

    private void WriteEndSlow(byte token)
    {
        System.Diagnostics.Debug.Assert(this.options.Indented);

        if (this.options.Indented)
        {
            this.WriteEndIndented(token);
        }
        else
        {
            this.WriteEndMinimized(token);
        }
    }

    private void WriteEndIndented(byte token)
    {
        // Do not format/indent empty WKT object.
        if (this.tokenType is WktTokenType.StartObject)
        {
            this.WriteEndMinimized(token);
        }
        else
        {
            var indent = this.Indentation;

            // Necessary if WriteEndX is called without a corresponding WriteStartX first.
            if (indent is not 0)
            {
                // The end token should be at an outer indent and since we haven't updated
                // current depth yet, explicitly subtract here.
                indent -= this.indentLength;
            }

            System.Diagnostics.Debug.Assert(indent <= this.indentLength * this.options.MaxDepth);
            System.Diagnostics.Debug.Assert(this.tokenType is not WktTokenType.None);

            var maxRequired = indent + 3; // 1 end token, 1-2 bytes for new line

            if (this.memory.Length - this.BytesPending < maxRequired)
            {
                this.Grow(maxRequired);
            }

            Span<byte> outputSpan = this.memory.Span;

            outputSpan[this.BytesPending++] = token;
        }
    }

    private void WriteNumberValueMinimized(double value, bool ensureDecimal)
    {
        const int maxRequired = WktConstants.MaximumFormatDoubleLength + 1; // Optionally, 1 list separator

        if (this.memory.Length - this.BytesPending < maxRequired)
        {
            this.Grow(maxRequired);
        }

        Span<byte> outputSpan = this.memory.Span;

        if (this.currentDepth < 0)
        {
            outputSpan[this.BytesPending++] = WktConstants.ListSeparator;
        }

        bool result = System.Buffers.Text.Utf8Formatter.TryFormat(value, outputSpan[this.BytesPending..], out int bytesWritten);
        System.Diagnostics.Debug.Assert(result);

        if (ensureDecimal && outputSpan.Slice(this.BytesPending, bytesWritten).IndexOf((byte)'.') is -1)
        {
            outputSpan[this.BytesPending + bytesWritten] = (byte)'.';
            bytesWritten++;
            outputSpan[this.BytesPending + bytesWritten] = (byte)'0';
            bytesWritten++;
        }

        this.BytesPending += bytesWritten;
    }

    private void WriteNumberValueIndented(double value, bool ensureDecimal)
    {
        int indent = this.Indentation;

        System.Diagnostics.Debug.Assert(indent <= this.indentLength * this.options.MaxDepth);

        int maxRequired = indent + WktConstants.MaximumFormatDoubleLength + 1 + this.newLineLength; // Optionally, 1 list separator and 1-2 bytes for new line

        if (this.memory.Length - this.BytesPending < maxRequired)
        {
            this.Grow(maxRequired);
        }

        Span<byte> outputSpan = this.memory.Span;

        if (this.currentDepth < 0)
        {
            outputSpan[this.BytesPending++] = WktConstants.ListSeparator;
        }

        var result = System.Buffers.Text.Utf8Formatter.TryFormat(value, outputSpan[this.BytesPending..], out int bytesWritten);
        System.Diagnostics.Debug.Assert(result);

        if (ensureDecimal && outputSpan.Slice(this.BytesPending, bytesWritten).IndexOf((byte)'.') is -1)
        {
            outputSpan[this.BytesPending + bytesWritten] = (byte)'.';
            bytesWritten++;
            outputSpan[this.BytesPending + bytesWritten] = (byte)'0';
            bytesWritten++;
        }

        this.BytesPending += bytesWritten;
    }

    private void CheckNotDisposed() => ObjectDisposedException.ThrowIf(this.stream is null && this.output is null, this);

    private void WriteStart(byte token)
    {
        if (this.CurrentDepth >= this.options.MaxDepth)
        {
            throw new InvalidOperationException();
        }

        if (this.options.Indented)
        {
            this.WriteStartSlow(token);
        }
        else
        {
            this.WriteStartMinimized(token);
        }

        this.currentDepth &= WktConstants.RemoveFlagsBitMask;
        this.currentDepth++;
    }

    private void WriteStartMinimized(byte token)
    {
        // 1 start token, and optionally, 1 list separator
        if (this.memory.Length - this.BytesPending < 2)
        {
            this.Grow(2);
        }

        Span<byte> outputSpan = this.memory.Span;
        if (this.currentDepth < 0)
        {
            outputSpan[this.BytesPending++] = (byte)',';
        }

        outputSpan[this.BytesPending++] = token;
    }

    private void WriteStartSlow(byte token)
    {
        System.Diagnostics.Debug.Assert(this.options.Indented);

        if (this.options.Indented)
        {
            this.WriteStartIndented(token);
        }
        else
        {
            this.WriteStartMinimized(token);
        }
    }

    private void WriteStartIndented(byte token)
    {
        int indent = this.Indentation;

        System.Diagnostics.Debug.Assert(indent <= this.indentLength * this.options.MaxDepth);

        int minRequired = indent + 1;   // 1 start token
        int maxRequired = minRequired + 3; // Optionally, 1 list separator and 1-2 bytes for new line

        if (this.memory.Length - this.BytesPending < maxRequired)
        {
            this.Grow(maxRequired);
        }

        Span<byte> outputSpan = this.memory.Span;

        if (this.currentDepth < 0)
        {
            outputSpan[this.BytesPending++] = (byte)',';
        }

        outputSpan[this.BytesPending++] = token;
    }

    private void WriteStringEscape(ReadOnlySpan<char> value)

        // Each input char may transcode to up to 3 bytes.
        => this.WriteStringByOptions(value, value.Length * WktConstants.MaxExpansionFactorWhileTranscoding);

    private void WriteStringByOptions(ReadOnlySpan<char> value, int maxRequiredBytes)
    {
        if (this.options.Indented)
        {
            this.WriteStringIndented(value, maxRequiredBytes);
        }
        else
        {
            this.WriteStringMinimized(value, maxRequiredBytes);
        }
    }

    private void WriteStringMinimized(ReadOnlySpan<char> escapedValue, int maxRequiredBytes)
    {
        System.Diagnostics.Debug.Assert(maxRequiredBytes >= 0 && maxRequiredBytes < int.MaxValue - 3);

        // 2 quotes + optional 1 list separator, plus precomputed max bytes for the payload.
        int maxRequired = maxRequiredBytes + 3;

        if (this.memory.Length - this.BytesPending < maxRequired)
        {
            this.Grow(maxRequired);
        }

        Span<byte> outputSpan = this.memory.Span;

        if (this.currentDepth < 0)
        {
            outputSpan[this.BytesPending++] = WktConstants.ListSeparator;
        }

        outputSpan[this.BytesPending++] = WktConstants.Quote;

        this.TranscodeAndWrite(escapedValue, outputSpan);

        outputSpan[this.BytesPending++] = WktConstants.Quote;
    }

    private void WriteStringIndented(ReadOnlySpan<char> escapedValue, int maxRequiredBytes)
    {
        int indent = this.Indentation;

        System.Diagnostics.Debug.Assert(indent <= this.indentLength * this.options.MaxDepth);
        System.Diagnostics.Debug.Assert(maxRequiredBytes >= 0 && maxRequiredBytes < int.MaxValue - indent - 3 - this.newLineLength);

        // indent + 2 quotes + optional 1 list separator + 1-2 bytes for new line, plus precomputed max bytes for the payload.
        int maxRequired = indent + maxRequiredBytes + 3 + this.newLineLength;

        if (this.memory.Length - this.BytesPending < maxRequired)
        {
            this.Grow(maxRequired);
        }

        Span<byte> outputSpan = this.memory.Span;

        if (this.currentDepth < 0)
        {
            outputSpan[this.BytesPending++] = WktConstants.ListSeparator;
        }

        outputSpan[this.BytesPending++] = WktConstants.Quote;

        this.TranscodeAndWrite(escapedValue, outputSpan);

        outputSpan[this.BytesPending++] = WktConstants.Quote;
    }

    private void WriteLiteralByOptions(ReadOnlySpan<byte> utf8Value, bool includeNewLine)
    {
        if (this.options.Indented)
        {
            this.WriteLiteralIndented(utf8Value, includeNewLine);
        }
        else
        {
            this.WriteLiteralMinimized(utf8Value);
        }
    }

    private void WriteLiteralMinimized(ReadOnlySpan<byte> utf8Value)
    {
        int maxRequired = utf8Value.Length + 1; // Optionally, 1 list separator

        if (this.memory.Length - this.BytesPending < maxRequired)
        {
            this.Grow(maxRequired);
        }

        Span<byte> outputSpan = this.memory.Span;

        if (this.currentDepth < 0)
        {
            outputSpan[this.BytesPending++] = WktConstants.ListSeparator;
        }

        utf8Value.CopyTo(outputSpan[this.BytesPending..]);
        this.BytesPending += utf8Value.Length;
    }

    private void WriteLiteralIndented(ReadOnlySpan<byte> utf8Value, bool includeNewline)
    {
        int indent = this.Indentation;

        System.Diagnostics.Debug.Assert(indent <= this.indentLength * this.options.MaxDepth);

        int maxRequired = indent + utf8Value.Length + 1 + this.newLineLength; // Optionally, 1 list separator and 1-2 bytes for new line

        if (this.memory.Length - this.BytesPending < maxRequired)
        {
            this.Grow(maxRequired);
        }

        Span<byte> outputSpan = this.memory.Span;

        if (this.currentDepth < 0)
        {
            outputSpan[this.BytesPending++] = WktConstants.ListSeparator;
        }

        if (includeNewline)
        {
            this.WriteNewLine(outputSpan);
            this.WriteIndentation(outputSpan[this.BytesPending..], indent);
            this.BytesPending += indent;
        }

        utf8Value.CopyTo(outputSpan[this.BytesPending..]);
        this.BytesPending += utf8Value.Length;
    }

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private void WriteNewLine(Span<byte> output)
    {
        // Write '\r\n' OR '\n', depending on the configured new line string
        System.Diagnostics.Debug.Assert(this.newLineLength is 1 or 2, "Invalid new line length.");
        if (this.newLineLength is 2)
        {
            output[this.BytesPending++] = WktConstants.CarriageReturn;
        }

        output[this.BytesPending++] = WktConstants.LineFeed;
    }

    private void WriteIndentation(Span<byte> buffer, int indent) => WktWriterHelper.WriteIndentation(buffer, indent, this.indentByte);

    private void Grow(int requiredSize)
    {
        System.Diagnostics.Debug.Assert(requiredSize > 0);

        if (this.memory.Length is 0)
        {
            this.FirstCallToGetMemory(requiredSize);
            return;
        }

        int sizeHint = Math.Max(DefaultGrowthSize, requiredSize);

        System.Diagnostics.Debug.Assert(this.BytesPending is not 0);

        if (this.stream is not null)
        {
            System.Diagnostics.Debug.Assert(this.arrayBufferWriter is not null);

            int needed = this.BytesPending + sizeHint;
            if (needed > 0X7FEFFFFF)
            {
#pragma warning disable CA2201, MA0012, S112
                throw new OutOfMemoryException();
#pragma warning restore CA2201, MA0012, S112
            }

            this.memory = this.arrayBufferWriter!.GetMemory(needed);

            System.Diagnostics.Debug.Assert(this.memory.Length >= sizeHint);
        }
        else
        {
            System.Diagnostics.Debug.Assert(this.output is not null);

            this.output!.Advance(this.BytesPending);
            this.BytesCommitted += this.BytesPending;
            this.BytesPending = 0;

            this.memory = this.output.GetMemory(sizeHint);

            if (this.memory.Length < sizeHint)
            {
                throw new InvalidOperationException();
            }
        }
    }

    private void FirstCallToGetMemory(int requiredSize)
    {
        System.Diagnostics.Debug.Assert(this.memory.Length is 0);
        System.Diagnostics.Debug.Assert(this.BytesPending is 0);

        int sizeHint = Math.Max(InitialGrowthSize, requiredSize);

        if (this.stream is not null)
        {
            System.Diagnostics.Debug.Assert(this.arrayBufferWriter is not null);
            this.memory = this.arrayBufferWriter!.GetMemory(sizeHint);
            System.Diagnostics.Debug.Assert(this.memory.Length >= sizeHint);
        }
        else
        {
            System.Diagnostics.Debug.Assert(this.output is not null);
            this.memory = this.output!.GetMemory(sizeHint);

            if (this.memory.Length < sizeHint)
            {
                throw new InvalidOperationException();
            }
        }
    }

    private void SetFlagToAddListSeparatorBeforeNextItem() => this.currentDepth |= 1 << 31;

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private void TranscodeAndWrite(ReadOnlySpan<char> escapedPropertyName, Span<byte> output) => this.BytesPending += System.Text.Encoding.UTF8.GetBytes(escapedPropertyName, output[this.BytesPending..]);

    private void SetOptions(WktWriterOptions options)
    {
        this.options = options;
        this.indentByte = (byte)this.options.IndentCharacter;
        this.indentLength = this.options.IndentSize;

        System.Diagnostics.Debug.Assert(options.NewLine is "\n" or "\r\n", "Invalid NewLine string.");
        this.newLineLength = this.options.NewLine.Length;

        if (this.options.MaxDepth is 0)
        {
            this.options.MaxDepth = WktWriterOptions.DefaultMaxDepth; // If max depth is not set, revert to the default depth.
        }
    }
}