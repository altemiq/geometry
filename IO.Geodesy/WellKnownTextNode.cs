// -----------------------------------------------------------------------
// <copyright file="WellKnownTextNode.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geodesy;

/// <summary>
/// The well known text node.
/// </summary>
public partial class WellKnownTextNode
{
    /// <summary>
    /// Represents a <see cref="WellKnownTextNode"/> structure that is a <see langword="null"/> reference.
    /// </summary>
    public static readonly WellKnownTextNode Empty = new(string.Empty, Enumerable.Empty<NodeValue>());

    private const char StartChar = '[';

    private const char EndChar = ']';

    private const char SeparatorChar = ',';

    /// <summary>
    /// Initialises a new instance of the <see cref="WellKnownTextNode"/> class.
    /// </summary>
    /// <param name="value">The WKT value.</param>
    public WellKnownTextNode(string value)
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        : this(GetSpan(value))
    {
    }
#else
    {
        var startString = StartChar.ToString();
        var endString = EndChar.ToString();

        // get the start and end
        var startValue = value.IndexOf(StartChar);
        var endValue = value.LastIndexOf(EndChar);

        // get the name
        this.Id = value[..startValue];

        // get the name
        this.Values = [.. Split(value[(startValue + 1)..endValue])
            .Select(item => item.Trim())
            .Select(
            item =>
            {
                if (item.Contains(startString) && item.Contains(endString))
                {
                    // this is another value
                    return OneOf.From<WellKnownTextNode, string, double, Literal>(new WellKnownTextNode(item));
                }

                if (double.TryParse(item, System.Globalization.NumberStyles.Float | System.Globalization.NumberStyles.AllowThousands, System.Globalization.CultureInfo.InvariantCulture, out var doubleValue))
                {
                    return OneOf.From<WellKnownTextNode, string, double, Literal>(doubleValue);
                }

                // if this is quoted
                if (item.StartsWith("\"", StringComparison.Ordinal) && item.EndsWith("\"", StringComparison.Ordinal))
                {
                    // strip the quotes
                    return OneOf.From<WellKnownTextNode, string, double, Literal>(item.Trim('\"'));
                }

                // this is a literal value
                return OneOf.From<WellKnownTextNode, string, double, Literal>(new Literal(item));
            }),];

        static IEnumerable<string> Split(string value)
        {
            var start = 0;
            var open = 0;
            for (var i = 1; i < value.Length; i++)
            {
                switch (value[i])
                {
                    case StartChar:
                        open++;
                        break;
                    case EndChar:
                        open--;
                        break;
                    case SeparatorChar:
                        if (open is 0)
                        {
                            yield return value[start..i];
                            start = i + 1;
                        }

                        break;
                }
            }

            yield return value[start..];
        }
    }
#endif

    /// <summary>
    /// Initialises a new instance of the <see cref="WellKnownTextNode"/> class.
    /// </summary>
    /// <param name="id">The ID.</param>
    /// <param name="value">The string value.</param>
    public WellKnownTextNode(string id, string value)
        : this(id, OneOf.From<WellKnownTextNode, string, double, Literal>(value))
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="WellKnownTextNode"/> class.
    /// </summary>
    /// <param name="id">The ID.</param>
    /// <param name="values">The values.</param>
    public WellKnownTextNode(string id, params NodeValue[] values)
        : this(id, (IEnumerable<NodeValue>)values)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="WellKnownTextNode"/> class.
    /// </summary>
    /// <param name="id">The ID.</param>
    /// <param name="values">The values.</param>
    public WellKnownTextNode(string id, IEnumerable<NodeValue> values) => (this.Id, this.Values) = (id, values);

    /// <summary>
    /// Gets the ID.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets the values.
    /// </summary>
    public IEnumerable<NodeValue> Values { get; }

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
    public WellKnownTextNode? GetNode(string id) => string.Equals(this.Id, id, StringComparison.Ordinal) ? this : this.Values
        .Where(v => v.IsT0)
        .Select(v => v.AsT0!)
        .FirstOrDefault(value => string.Equals(value!.Id, id, StringComparison.Ordinal));

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
    public AuthorityCode GetAuthorityCode(string? targetKey = default) =>
        this.GetAuthorityNode(targetKey)?.Values
            .ElementAt(1)
            .Match<AuthorityCode>(
                static _ => default,
                static stringValue => stringValue,
                static doubleValue => (int)doubleValue,
                static literal =>
                {
                    var value = literal.ToString();
                    return int.TryParse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var srid)
                        ? srid
                        : value;
                }) ?? default;

    /// <summary>
    /// Gets the authority name for a node.
    /// </summary>
    /// <param name="targetKey">The target key.</param>
    /// <returns>The authority name.</returns>
    public string? GetAuthorityName(string? targetKey = default) => this.GetAuthorityNode(targetKey)?.Values.ElementAt(0).Match(
        static _ => default,
        static stringValue => stringValue,
        static _ => default,
        static literal => literal.ToString());

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
}