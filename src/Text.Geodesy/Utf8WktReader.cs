// -----------------------------------------------------------------------
// <copyright file="Utf8WktReader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.Geodesy;

using System.Text;

/// <summary>
/// Provides a high-performance API for forward-only, read-only access to UTF-8 encoded WKT text.
/// </summary>
public ref struct Utf8WktReader
{
    private readonly ReadOnlySpan<byte> buffer;

    /// <summary>
    /// Initialises a new instance of the <see cref="Utf8WktReader"/> struct.
    /// </summary>
    /// <param name="buffer">The buffer.</param>
    public Utf8WktReader(ReadOnlySpan<byte> buffer) => this.buffer = buffer;

    /// <summary>
    /// Gets the total number of bytes consumed so far by this instance of the <see cref="Utf8WktReader"/>.
    /// </summary>
    public int BytesConsumed { get; private set; }

    /// <summary>
    /// Gets the index that the last processed JSON token starts at (within the given UTF-8 encoded input text), skipping any white space.
    /// </summary>
    public int TokenStartIndex { get; private set; }

    /// <summary>
    /// Gets the type of the current token.
    /// </summary>
    public WktTokenType TokenType { get; private set; }

    /// <summary>
    /// Gets the value of the last processed token as a <see cref="ReadOnlySpan{Byte}"/> slice of the input payload.
    /// If the JSON is provided within a <see cref="ReadOnlySpan{Byte}"/>; and the slice that represents the token value fits in a single segment,
    /// then <see cref="ValueSpan"/> will contain the sliced value since it can be represented as a span.
    /// </summary>
    public ReadOnlySpan<byte> ValueSpan { get; private set; }

    /// <summary>
    /// Reads the next token from the input buffer.
    /// </summary>
    /// <returns>true if a token was read; false if there are no more tokens to read.</returns>
    public bool Read()
    {
        if (this.IsFinishedImpl())
        {
            this.TokenType = default;
            this.ValueSpan = default;
            return false;
        }

        // Skip whitespace or separators
        this.SkipWhitespace();

        if (this.IsFinishedImpl())
        {
            this.TokenType = default;
            this.ValueSpan = default;
            return false;
        }

        // Check what type of token we have
        var currentByte = this.buffer[this.BytesConsumed];

        if (currentByte is WktConstants.ListSeparator)
        {
            // Skip separator
            this.BytesConsumed++;
            return this.Read();
        }

        if (currentByte is WktConstants.Quote)
        {
            this.TokenStartIndex = this.BytesConsumed;

            var characters = 1;
            while (!this.IsFinishedImpl(characters))
            {
                if (this.buffer[this.TokenStartIndex + characters] is WktConstants.Quote)
                {
                    if (this.IsFinishedImpl(characters + 1))
                    {
                        break;
                    }

                    if (this.buffer[this.TokenStartIndex + characters + 1] is WktConstants.Quote)
                    {
                        characters += 2;
                        continue;
                    }

                    break;
                }

                characters++;
            }

            this.BytesConsumed += characters;

            if (!this.IsFinishedImpl())
            {
                this.BytesConsumed++;

                this.ValueSpan = this.buffer[(this.TokenStartIndex + 1)..(this.BytesConsumed - 1)];
                this.TokenType = WktTokenType.String;

                // ensure we move past the separator
                characters = 0;
                while (!this.IsFinishedImpl() && !IsEndOfValue(this.buffer[this.BytesConsumed + characters]))
                {
                    characters++;
                }

                this.BytesConsumed += characters;
                if (!this.IsFinishedImpl() && this.buffer[this.BytesConsumed] is WktConstants.ListSeparator)
                {
                    this.BytesConsumed++;
                }

                return true;
            }
        }
        else if (currentByte is WktConstants.OpenBracket)
        {
            // Start object
            this.TokenStartIndex = this.BytesConsumed;
            this.BytesConsumed++;
            this.ValueSpan = default;
            this.TokenType = WktTokenType.StartObject;
            return true;
        }
        else if (currentByte is WktConstants.CloseBracket)
        {
            // End object
            this.TokenStartIndex = this.BytesConsumed;
            this.BytesConsumed++;
            this.ValueSpan = default;
            this.TokenType = WktTokenType.EndObject;
            return true;
        }
        else if (char.IsDigit((char)currentByte) || currentByte is (byte)'-' or (byte)'+')
        {
            // Number value - read until delimiter
            this.TokenStartIndex = this.BytesConsumed;

            var digits = 1;
            while (!this.IsFinishedImpl(digits) && !IsEndOfValue(this.buffer[this.TokenStartIndex + digits]))
            {
                digits++;
            }

            this.BytesConsumed += digits;

            this.ValueSpan = TrimEnd(this.buffer[this.TokenStartIndex..this.BytesConsumed]);
            this.TokenType = WktTokenType.Number;

            if (!this.IsFinishedImpl() && this.buffer[this.BytesConsumed] is WktConstants.ListSeparator)
            {
                this.BytesConsumed++;
            }

            return true;
        }
        else
        {
            // Literal value - read until delimiter
            this.TokenStartIndex = this.BytesConsumed;

            var characters = 0;
            while (!this.IsFinishedImpl(characters) && !IsEndOfValue(this.buffer[this.TokenStartIndex + characters]))
            {
                characters++;
            }

            this.BytesConsumed += characters;
            if (this.TokenStartIndex < this.BytesConsumed)
            {
                this.ValueSpan = TrimEnd(this.buffer[this.TokenStartIndex..this.BytesConsumed]);
                this.TokenType = WktTokenType.Literal;

                if (!this.IsFinishedImpl() && this.buffer[this.BytesConsumed] is WktConstants.OpenBracket)
                {
                    this.TokenType = WktTokenType.Keyword;
                }

                if (!this.IsFinishedImpl() && this.buffer[this.BytesConsumed] is WktConstants.ListSeparator)
                {
                    this.BytesConsumed++;
                }

                return true;
            }
        }

        this.TokenType = default;
        this.ValueSpan = default;
        return false;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static bool IsEndOfValue(byte value)
        {
            return value is WktConstants.ListSeparator or WktConstants.OpenBracket or WktConstants.CloseBracket;
        }
    }

    /// <summary>
    /// Reads the next WKT token value from the source unescaped and transcodes it as a string.
    /// </summary>
    /// <returns>The token value parsed to a string.</returns>
    /// <exception cref="InvalidOperationException">The WKT token value isn't a string.</exception>
    public readonly string GetString() => this.TokenType is not WktTokenType.String and not WktTokenType.Keyword ? throw new InvalidOperationException() : UnescapeString(this.ValueSpan);

    /// <summary>
    /// Attempts to get the current token as a string value.
    /// </summary>
    /// <param name="value">The string value.</param>
    /// <returns>true if the current token can be read as a string; false otherwise.</returns>
    public readonly bool TryGetString([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out string? value)
    {
        if (this.TokenType is not WktTokenType.String and not WktTokenType.Keyword || this.ValueSpan.IsEmpty)
        {
            value = default;
            return false;
        }

        value = UnescapeString(this.ValueSpan);
        return true;
    }

    /// <summary>
    /// Reads the next WKT token value from the source and parses it to a <see cref="double"/>.
    /// </summary>
    /// <returns>The token value parsed to a <see cref="double"/>.</returns>
    /// <exception cref="InvalidOperationException">The WKT token value isn't a <see cref="WktTokenType.Number" />.</exception>
    /// <exception cref="FormatException">The WKT token value does not represents a number.</exception>
    public readonly double GetDouble()
    {
        if (this.TokenType is not WktTokenType.Number)
        {
            throw new InvalidOperationException();
        }

        if (!System.Buffers.Text.Utf8Parser.TryParse(this.ValueSpan, out double value, out var bytesConsumed)
            || bytesConsumed != this.ValueSpan.Length)
        {
            throw new FormatException();
        }

        return value;
    }

    /// <summary>
    /// Attempts to get the current token as a number value.
    /// </summary>
    /// <param name="value">The number value.</param>
    /// <returns>true if the current token can be read as a number; false otherwise.</returns>
    public readonly bool TryGetDouble(out double value)
    {
        if (this.TokenType is not WktTokenType.Number || this.ValueSpan.IsEmpty)
        {
            value = default;
            return false;
        }

        return System.Buffers.Text.Utf8Parser.TryParse(this.ValueSpan, out value, out _);
    }

    /// <summary>
    /// Reads the next WKT token value from the source unescaped.
    /// </summary>
    /// <returns>The token value parsed to a <see cref="WktLiteral"/>.</returns>
    /// <exception cref="InvalidOperationException">The WKT token value isn't a <see cref="WktLiteral"/>.</exception>
    public readonly WktLiteral GetLiteral() => this.TokenType is not WktTokenType.Literal and not WktTokenType.Keyword ? throw new InvalidOperationException() : new(this.ValueSpan);

    /// <summary>
    /// Attempts to get the current token as a literal value.
    /// </summary>
    /// <param name="value">The literal value.</param>
    /// <returns>true if the current token can be read as a literal; false otherwise.</returns>
    public readonly bool TryGetLiteral(out WktLiteral value)
    {
        if (this.TokenType is not WktTokenType.Literal and not WktTokenType.Keyword || this.ValueSpan.IsEmpty)
        {
            value = default;
            return false;
        }

        value = new(this.ValueSpan);
        return true;
    }

    /// <summary>
    /// Resets the reader to the beginning of the input buffer.
    /// </summary>
    public void Reset() => this.BytesConsumed = 0;

    private static ReadOnlySpan<byte> TrimEnd(ReadOnlySpan<byte> span)
    {
        var end = span.Length;
        while (char.IsWhiteSpace((char)span[end - 1]))
        {
            end--;
        }

        return span[0..end];
    }

    private static string UnescapeString(ReadOnlySpan<byte> span)
    {
        if (span.IndexOf([WktConstants.Quote]) is -1)
        {
            return System.Text.Encoding.UTF8.GetString(span);
        }

        var length = System.Text.Encoding.UTF8.GetCharCount(span);

        char[]? valueArray = null;

        Span<char> chars = length <= WktConstants.StackallocCharThreshold
            ? stackalloc char[WktConstants.StackallocCharThreshold]
            : RentArray(length, ref valueArray);

        try
        {
            var outputCurrent = 0;
            var inputCurrent = 0;

            for (var i = 0; i < span.Length - 2; i++)
            {
                if (span.Slice(i, 2) is [WktConstants.Quote, WktConstants.Quote])
                {
#pragma warning disable S127 // "for" loop stop conditions should be invariant
                    i++;
#pragma warning restore S127 // "for" loop stop conditions should be invariant
                    var written = System.Text.Encoding.UTF8.GetChars(span[inputCurrent..i], chars[outputCurrent..]);
                    outputCurrent += written;
                    inputCurrent = i + 1;
                }
            }

            if (inputCurrent < span.Length)
            {
                var written = System.Text.Encoding.UTF8.GetChars(span[inputCurrent..], chars[outputCurrent..]);
                outputCurrent += written;
            }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            return new string(chars[..outputCurrent]);
#else
            unsafe
            {
                // get the pointer for the span
                fixed (char* ptr = chars)
                {
                    return new string(ptr, 0, outputCurrent);
                }
            }
#endif
        }
        finally
        {
            if (valueArray is not null)
            {
                System.Buffers.ArrayPool<char>.Shared.Return(valueArray);
            }
        }

        static Span<char> RentArray(int length, ref char[]? valueArray)
        {
            valueArray = System.Buffers.ArrayPool<char>.Shared.Rent(length);
            return valueArray;
        }
    }

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private readonly bool IsFinishedImpl() => this.BytesConsumed >= this.buffer.Length;

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private readonly bool IsFinishedImpl(int current) => (this.TokenStartIndex + current) >= this.buffer.Length;

    private void SkipWhitespace()
    {
        var whitespaces = 0;
        while ((this.BytesConsumed + whitespaces) < this.buffer.Length && (char.IsWhiteSpace((char)this.buffer[this.BytesConsumed + whitespaces]) || this.buffer[this.BytesConsumed + whitespaces] is WktConstants.Tab or WktConstants.CarriageReturn or WktConstants.LineFeed))
        {
            whitespaces++;
        }

        this.BytesConsumed += whitespaces;
    }
}