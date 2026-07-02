// -----------------------------------------------------------------------
// <copyright file="WktElement.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.Geodesy;

/// <summary>
/// The WKT element.
/// </summary>
public readonly struct WktElement
{
    private readonly WktObject nodeValue;
    private readonly double doubleValue;
    private readonly string? stringValue;
    private readonly string? literalValue;

    /// <summary>
    /// Initialises a new instance of the <see cref="WktElement"/> struct.
    /// </summary>
    /// <param name="value">The value.</param>
    internal WktElement(WktObject value)
    {
        this.nodeValue = value;
        this.ValueKind = WktValueKind.Object;
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="WktElement"/> struct.
    /// </summary>
    /// <param name="value">The value.</param>
    internal WktElement(double value)
    {
        this.doubleValue = value;
        this.ValueKind = WktValueKind.Number;
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="WktElement"/> struct.
    /// </summary>
    /// <param name="value">The value.</param>
    internal WktElement(string value)
    {
        this.stringValue = value;
        this.ValueKind = WktValueKind.String;
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="WktElement"/> struct.
    /// </summary>
    /// <param name="value">The value.</param>
    internal WktElement(WktLiteral value)
    {
        this.literalValue = value.ToString();
        this.ValueKind = WktValueKind.Literal;
    }

    /// <summary>
    /// Gets the type of the current WKT value.
    /// </summary>
    public WktValueKind ValueKind { get; }

    /// <summary>
    /// Gets the value at the specified index if the current value is an <see cref="WktValueKind.Object"/>.
    /// </summary>
    /// <param name="index">The item index.</param>
    /// <returns>The value at the specified index.</returns>
    public WktElement this[int index] => this.ValueKind is WktValueKind.Object ? this.nodeValue[index] : throw new InvalidOperationException();

    /// <summary>
    /// Parses text representing a single WKT value into a <see cref="WktElement"/>.
    /// </summary>
    /// <param name="wkt">The WKT to parse.</param>
    /// <returns>A <see cref="WktElement"/> representation of the WKT value.</returns>
    public static WktElement Parse(string wkt) => Parse(wkt.AsSpan());

    /// <summary>
    /// Parses text representing a single WKT value into a <see cref="WktElement"/>.
    /// </summary>
    /// <param name="wkt">The WKT to parse.</param>
    /// <returns>A <see cref="WktElement"/> representation of the WKT value.</returns>
    public static WktElement Parse(ReadOnlySpan<char> wkt)
    {
        var byteCount = System.Text.Encoding.UTF8.GetByteCount(wkt);
        Span<byte> bytes = stackalloc byte[byteCount];
        var actualByteCount = System.Text.Encoding.UTF8.GetBytes(wkt, bytes);
        return Parse(bytes[..actualByteCount]);
    }

    /// <summary>
    /// Parses UTF8-encoded text representing a single WKT value into a <see cref="WktElement"/>.
    /// </summary>
    /// <param name="wkt">The WKT to parse.</param>
    /// <returns>A <see cref="WktElement"/> representation of the WKT value.</returns>
    public static WktElement Parse(ReadOnlySpan<byte> wkt)
    {
        var reader = new Utf8WktReader(wkt);

        if (!reader.Read())
        {
            return default;
        }

        return ParseValue(ref reader);
    }

    /// <summary>
    /// Parses one WKT value (including objects) from the provided reader.
    /// </summary>
    /// <param name="reader">The reader to read.</param>
    /// <returns>A <see cref="WktElement"/> representing the value (and nested values) read from the reader.</returns>
    public static WktElement ParseValue(ref Utf8WktReader reader)
    {
        return reader.TokenType switch
        {
            WktTokenType.Keyword => new WktElement(ReadObject(reader.GetString(), ref reader)),
            WktTokenType.String => new WktElement(reader.GetString()),
            WktTokenType.Number => new WktElement(reader.GetDouble()),
            WktTokenType.Literal => new WktElement(reader.GetLiteral()),
            _ => throw new FormatException($"Unexpected token type: {reader.TokenType}"),
        };

        static WktObject ReadObject(string keyword, ref Utf8WktReader reader)
        {
            if (!reader.Read() || reader.TokenType is not WktTokenType.StartObject)
            {
                throw new FormatException("Expected start of object after keyword.");
            }

            var values = new List<WktElement>();
            while (reader.Read())
            {
                if (reader.TokenType is WktTokenType.EndObject)
                {
                    break;
                }

                values.Add(ParseValue(ref reader));
            }

            return new WktObject(keyword, values);
        }
    }

    /// <summary>
    /// Attempts to parse one WKT value (including objects) from the provided reader.
    /// </summary>
    /// <param name="reader">The reader to read.</param>
    /// <param name="element">Receives the parsed element.</param>
    /// <returns><see langword="true"/> if a value was read and parsed into a <see cref="WktElement"/>; <see langword="false"/> if the reader ran out of data while parsing.</returns>
    public static bool TryParseValue(ref Utf8WktReader reader, out WktElement element)
    {
        switch (reader.TokenType)
        {
            case WktTokenType.Keyword when TryReadObject(reader.GetString(), ref reader, out var obj):
                element = new WktElement(obj);
                return true;
            case WktTokenType.String when reader.TryGetString(out var s):
                element = new WktElement(s);
                return true;
            case WktTokenType.Number when reader.TryGetDouble(out var d):
                element = new WktElement(d);
                return true;
            case WktTokenType.Literal when reader.TryGetLiteral(out var l):
                element = new WktElement(l);
                return true;
        }

        element = default;
        return false;

        static bool TryReadObject(string keyword, ref Utf8WktReader reader, out WktObject element)
        {
            if (!reader.Read() || reader.TokenType is not WktTokenType.StartObject)
            {
                element = default;
                return false;
            }

            var values = new List<WktElement>();
            while (reader.Read())
            {
                if (reader.TokenType is WktTokenType.EndObject)
                {
                    break;
                }

                if (TryParseValue(ref reader, out var sub))
                {
                    values.Add(sub);
                }
                else
                {
                    element = default;
                    return false;
                }
            }

            element = new WktObject(keyword, values);
            return true;
        }
    }

    /// <summary>
    /// Gets the number of elements contained within the current object value.
    /// </summary>
    /// <returns>The number of elements contained within the current object value.</returns>
    /// <exception cref="InvalidOperationException">This value's <see cref="ValueKind"/> is not <see cref="WktValueKind.Object"/>.</exception>
    public int GetElementCount() => this.ValueKind is WktValueKind.Object ? this.nodeValue.Count : throw new InvalidOperationException("Element count is not available for non-object values.");

    /// <summary>
    /// Gets an enumerator to enumerate the properties in the WKT object represented by this <see cref="WktElement"/>.
    /// </summary>
    /// <returns>An enumerator to enumerate the properties in the WKT object represented by this <see cref="WktElement"/>.</returns>
    /// <exception cref="InvalidOperationException">This value's <see cref="ValueKind"/> is not <see cref="WktValueKind.Object"/>.</exception>
    public IEnumerator<WktElement> EnumerateObject() => this.ValueKind is WktValueKind.Object ? this.nodeValue.GetEnumerator() : throw new InvalidOperationException("Element enumeration is not available for non-object values.");

    /// <summary>
    /// Gets the keyword of the WKT object.
    /// </summary>
    /// <returns>The keyword.</returns>
    /// <exception cref="InvalidOperationException">The value's <see cref="ValueKind"/> is not <see cref="WktValueKind.Object"/>.</exception>
    public string GetKeyword() => this.nodeValue is { Id: { } keyword } ? keyword : throw new InvalidOperationException("Keyword is not set.");

    /// <summary>
    /// Attempts to gets the keyword of the WKT object.
    /// </summary>
    /// <param name="keyword">The keyword value.</param>
    /// <returns><see langword="true"/> if this is an object; otherwise <see langword="false"/>.</returns>
    public bool TryGetKeyword([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out string? keyword)
    {
        if (this.nodeValue is { Id: { } id })
        {
            keyword = id;
            return true;
        }

        keyword = default;
        return false;
    }

    /// <summary>
    /// Gets the value of the element as a <see cref="string"/>.
    /// </summary>
    /// <returns>The value of the element as a <see cref="string"/>.</returns>
    /// <exception cref="InvalidOperationException">The value's <see cref="ValueKind"/> is not <see cref="WktValueKind.String"/>.</exception>
    public string GetString() => this.ValueKind is WktValueKind.String ? this.stringValue! : throw new InvalidOperationException();

    /// <summary>
    /// Gets the value of the element as a <see cref="WktLiteral"/>.
    /// </summary>
    /// <returns>The value of the element as a <see cref="WktLiteral"/>.</returns>
    /// <exception cref="InvalidOperationException">The value's <see cref="ValueKind"/> is not <see cref="WktValueKind.Literal"/>.</exception>
    public WktLiteral GetLiteral() => this.ValueKind is WktValueKind.Literal ? new WktLiteral(this.literalValue!) : throw new InvalidOperationException();

    /// <summary>
    /// Attempts to represent the current WKT number as a <see cref="WktLiteral"/>.
    /// </summary>
    /// <param name="value">When this method returns, the literal value.</param>
    /// <returns><see langword="true"/> if the number can be represented as a <see cref="WktLiteral"/>; otherwise <see langword="false"/>.</returns>
    public bool TryGetLiteral(out WktLiteral value)
    {
        if (this.ValueKind is WktValueKind.Literal)
        {
            value = new WktLiteral(this.literalValue!);
            return true;
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Gets the value of the element as a <see cref="double"/>.
    /// </summary>
    /// <returns>The value of the element as a <see cref="double"/>.</returns>
    /// <exception cref="InvalidOperationException">The value's <see cref="ValueKind"/> is not <see cref="WktValueKind.Number"/>.</exception>
    public double GetDouble() => this.ValueKind is WktValueKind.Number ? this.doubleValue : throw new InvalidOperationException();

    /// <summary>
    /// Attempts to represent the current WKT number as a <see cref="double"/>.
    /// </summary>
    /// <param name="value">When this method returns, contains a double-precision floating point value equivalent to the current JSON number if the conversion succeeded, or 0 if the conversion failed.</param>
    /// <returns><see langword="true"/> if the number can be represented as a <see cref="double"/>; otherwise <see langword="false"/>.</returns>
    public bool TryGetDouble(out double value)
    {
        if (this.ValueKind is WktValueKind.Number)
        {
            value = this.doubleValue;
            return true;
        }

        value = default;
        return false;
    }
}