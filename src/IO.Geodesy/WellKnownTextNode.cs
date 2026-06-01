// -----------------------------------------------------------------------
// <copyright file="WellKnownTextNode.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geodesy;

/// <summary>
/// The well known text node.
/// </summary>
public class WellKnownTextNode
{
    /// <summary>
    /// Represents a <see cref="WellKnownTextNode"/> structure that is a <see langword="null"/> reference.
    /// </summary>
    public static readonly WellKnownTextNode Empty = new(string.Empty, System.Linq.Enumerable.Empty<WellKnownTextValue>());

    private const char StartChar = '[';

    private const byte StartByte = (byte)StartChar;

    private const char EndChar = ']';

    private const byte EndByte = (byte)EndChar;

    private const char SeparatorChar = ',';

    private const byte SeparatorByte = (byte)SeparatorChar;

    /// <summary>
    /// Initialises a new instance of the <see cref="WellKnownTextNode"/> class.
    /// </summary>
    /// <param name="value">The WKT value.</param>
    public WellKnownTextNode(string value)
        : this(value.AsSpan())
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="WellKnownTextNode"/> class.
    /// </summary>
    /// <param name="value">The WKT value.</param>
    public WellKnownTextNode(ReadOnlySpan<char> value)
    {
        // get the start and end
        var startValue = value.IndexOf(StartChar);
        var endValue = value.LastIndexOf(EndChar);

        // get the name
        this.Id = value[..startValue].Trim().ToString();

        // get the name
        var list = new List<WellKnownTextValue>();
        startValue++;
        value = value[startValue..endValue];
        var split = new SpanSplitEnumerator<char>(value, StartChar, EndChar, SeparatorChar);
        while (split.MoveNext())
        {
            var item = value[split.Current].Trim();

            if (item.IndexOf(StartChar) >= 0 && item.IndexOf(EndChar) >= 0)
            {
                list.Add(new(new WellKnownTextNode(item)));
            }
            else if (double.TryParse(item, System.Globalization.NumberStyles.Float | System.Globalization.NumberStyles.AllowThousands, System.Globalization.CultureInfo.InvariantCulture, out var doubleValue))
            {
                list.Add(new(doubleValue));
            }
            else if (item[0] == '\"' && item[^1] == '\"')
            {
                list.Add(new(item.Trim('\"').ToString()));
            }
            else
            {
                list.Add(new(new Literal(item.ToString())));
            }
        }

        this.Values = list.AsReadOnly();
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="WellKnownTextNode"/> class.
    /// </summary>
    /// <param name="value">The WKT value.</param>
    public WellKnownTextNode(ReadOnlySpan<byte> value)
    {
        // get the start and end
        var startValue = value.IndexOf(StartByte);
        var endValue = value.LastIndexOf(EndByte);

        // get the name
        this.Id = System.Text.Encoding.UTF8.GetString(TrimWriteSpace(value[..startValue]));

        // get the name
        var list = new List<WellKnownTextValue>();
        startValue++;
        value = value[startValue..endValue];
        var split = new SpanSplitEnumerator<byte>(value, StartByte, EndByte, SeparatorByte);
        while (split.MoveNext())
        {
            var item = value[split.Current];

            if (item.IndexOf(StartByte) >= 0 && item.IndexOf(EndByte) >= 0)
            {
                list.Add(new(new WellKnownTextNode(item)));
            }
            else if (System.Buffers.Text.Utf8Parser.TryParse(item, out double doubleValue, out _))
            {
                list.Add(new(doubleValue));
            }
            else if (item[0] == '\"' && item[^1] == '\"')
            {
                list.Add(new(System.Text.Encoding.UTF8.GetString(TrimValue(item, (byte)'\"'))));
            }
            else
            {
                list.Add(new(new Literal(item.ToString())));
            }
        }

        this.Values = list.AsReadOnly();

        static ReadOnlySpan<byte> TrimWriteSpace(ReadOnlySpan<byte> span)
        {
            return Trim(span, b => char.IsWhiteSpace((char)b));
        }

        static ReadOnlySpan<byte> TrimValue(ReadOnlySpan<byte> span, byte value)
        {
            return Trim(span, b => b == value);
        }

        static ReadOnlySpan<T> Trim<T>(ReadOnlySpan<T> span, Func<T, bool> predicate)
        {
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                {
                    continue;
                }

                for (var j = span.Length - 1; j >= i; j--)
                {
                    if (predicate(span[j]))
                    {
                        continue;
                    }

                    return span[i..(j + 1)];
                }
            }

            return default;
        }
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="WellKnownTextNode"/> class.
    /// </summary>
    /// <param name="id">The ID.</param>
    /// <param name="value">The string value.</param>
    public WellKnownTextNode(string id, string value)
        : this(id, new WellKnownTextValue(value))
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="WellKnownTextNode"/> class.
    /// </summary>
    /// <param name="id">The ID.</param>
    /// <param name="values">The values.</param>
    public WellKnownTextNode(string id, params WellKnownTextValue[] values)
        : this(id, (IEnumerable<WellKnownTextValue>)values)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="WellKnownTextNode"/> class.
    /// </summary>
    /// <param name="id">The ID.</param>
    /// <param name="values">The values.</param>
    public WellKnownTextNode(string id, IEnumerable<WellKnownTextValue> values) => (this.Id, this.Values) = (id, values);

    /// <summary>
    /// Gets the ID.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets the values.
    /// </summary>
    public IEnumerable<WellKnownTextValue> Values { get; }

    /// <summary>
    /// Gets the value with the specified ID.
    /// </summary>
    /// <param name="id">The ID.</param>
    /// <returns>The value if found; otherwise <see langword="null"/>.</returns>
    public WellKnownTextNode? this[string id] => this.GetNode(id);

    /// <inheritdoc/>
    public override string ToString() => Serialization.WktSerializer.Serialize(this, Serialization.WktSerializerOptions.Default);

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <param name="id">The ID of the value.</param>
    /// <returns>The value with the ID.</returns>
    public WellKnownTextNode? GetNode(string id)
    {
        if (string.Equals(this.Id, id, StringComparison.Ordinal))
        {
            return this;
        }

        foreach (var value in this.Values)
        {
            if (value.TryGetValue(out WellKnownTextNode? node)
                && string.Equals(node.Id, id, StringComparison.Ordinal))
            {
                return node;
            }
        }

        return default;
    }

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <param name="ids">The IDs that match the value.</param>
    /// <returns>The value that matches <paramref name="ids"/>.</returns>
    public WellKnownTextNode? GetNode(params string[]? ids)
    {
        if (ids is null)
        {
            return null;
        }

        var value = this;
        foreach (var id in ids)
        {
            if (value is null)
            {
                return null;
            }

            value = value.GetNode(id);
        }

        return value;
    }

    /// <summary>
    /// Get the authority code for a node.
    /// </summary>
    /// <param name="targetKey">The target key.</param>
    /// <returns>The authority code.</returns>
    public AuthorityCode GetAuthorityCode(string? targetKey = default)
    {
        if (this.GetAuthorityNode(targetKey) is { } authorityNode)
        {
            using var enumerator = authorityNode.Values.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                return default;
            }

            if (!enumerator.MoveNext())
            {
                return default;
            }

            var value = enumerator.Current;
            if (!value.HasValue)
            {
                return default;
            }

            if (value.TryGetValue(out string? stringValue))
            {
                return new(stringValue);
            }

            if (value.TryGetValue(out double doubleValue))
            {
                return new((int)doubleValue);
            }

            if (value.TryGetValue(out Literal literal))
            {
                var literalValue = literal.ToString();
                return int.TryParse(literalValue, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var srid)
                    ? new(srid)
                    : new(literalValue);
            }
        }

        return default;
    }

    /// <summary>
    /// Gets the authority name for a node.
    /// </summary>
    /// <param name="targetKey">The target key.</param>
    /// <returns>The authority name.</returns>
    public string? GetAuthorityName(string? targetKey = default)
    {
        if (this.GetAuthorityNode(targetKey) is { } authorityNode)
        {
            using var enumerator = authorityNode.Values.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                return default;
            }

            var value = enumerator.Current;
            if (!value.HasValue)
            {
                return default;
            }

            if (value.TryGetValue(out string? stringValue))
            {
                return stringValue;
            }

            if (value.TryGetValue(out Literal literal))
            {
                return literal.ToString();
            }
        }

        return default;
    }

    private WellKnownTextNode? GetAuthorityNode(string? targetKey)
    {
        return GetNodeCore(targetKey) switch
        {
            { } node => node.GetNode("AUTHORITY", "ID"),
            _ => default,
        };

        WellKnownTextNode? GetNodeCore(string? key)
        {
            return key is null ? this : this.GetNode(key);
        }
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    private ref struct SpanSplitEnumerator<T>(ReadOnlySpan<T> span, T start, T end, T separator)
        where T : IEquatable<T>
    {
        private readonly ReadOnlySpan<T> buffer = span;

        private int startCurrent = 0;
        private int endCurrent = 0;
        private int startNext = 0;

        /// <summary>
        /// Gets the current element of the enumeration.
        /// </summary>
        /// <returns>Returns a <see cref="Range"/> instance that indicates the bounds of the current element withing the source span.</returns>
        public readonly Range Current => new(this.startCurrent, this.endCurrent);

        /// <summary>
        /// Advances the enumerator to the next element of the enumeration.
        /// </summary>
        /// <returns><see langword="true"/> if the enumerator was successfully advanced to the next element; <see langword="false"/> if the enumerator has passed the end of the enumeration.</returns>
        public bool MoveNext()
        {
            if (this.startNext > this.buffer.Length)
            {
                return false;
            }

            var slice = this.buffer[this.startNext..];
            this.startCurrent = this.startNext;

            var separatorIndex = -1;
            var open = 0;
            for (var i = 1; i < slice.Length; i++)
            {
                if (slice[i].Equals(start))
                {
                    open++;
                }
                else if (slice[i].Equals(end))
                {
                    open--;
                }
                else if (slice[i].Equals(separator) && open is 0)
                {
                    separatorIndex = i;
                }

                if (separatorIndex >= 0)
                {
                    break;
                }
            }

            var elementLength = separatorIndex != -1 ? separatorIndex : slice.Length;

            this.endCurrent = this.startCurrent + elementLength;
            this.startNext = this.endCurrent + 1;

            return true;
        }
    }
}