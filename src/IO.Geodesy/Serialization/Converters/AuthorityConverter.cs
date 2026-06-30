// -----------------------------------------------------------------------
// <copyright file="AuthorityConverter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geodesy.Serialization.Converters;

/// <summary>
/// The <see cref="Authority"/> <see cref="WktConverter{T}"/>.
/// </summary>
internal sealed class AuthorityConverter : WktConverter<Authority>
{
    private const string Wkt1Keyword = "AUTHORITY";

    private const string Wkt2Keyword = "ID";

    /// <summary>
    /// Tests to see if the specified node is an <see cref="Authority"/> node.
    /// </summary>
    /// <param name="node">The node to test.</param>
    /// <returns><see langword="true"/> if <paramref name="node"/> is a valid <see cref="Authority"/> node; otherwise <see langword="false"/>.</returns>
    public static bool IsValidNode([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] WellKnownTextNode? node) => node is not null && (IsValidNode(node, WellKnownTextFormat.Wkt1) || IsValidNode(node, WellKnownTextFormat.Wkt2));

    /// <summary>
    /// Tests to see if the specified node is an <see cref="Authority"/> node.
    /// </summary>
    /// <param name="node">The node to test.</param>
    /// <param name="format">The format.</param>
    /// <returns><see langword="true"/> if <paramref name="node"/> is a valid <see cref="Authority"/> node; otherwise <see langword="false"/>.</returns>
    public static bool IsValidNode([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] WellKnownTextNode? node, WellKnownTextFormat format) => node is not null && (format, node.Id) is (WellKnownTextFormat.Wkt1, Wkt1Keyword) or (WellKnownTextFormat.Wkt2, Wkt2Keyword);

    /// <inheritdoc/>
    public override Authority Read(IEnumerable<WellKnownTextNode> nodes, Type typeToConvert, WktSerializerOptions options)
    {
        var node = nodes.SingleOrDefault();
#pragma warning disable S3236
        ArgumentNullException.ThrowIfNull(node, nameof(nodes));
#pragma warning restore S3236
        return ReadCore(node);

        static Authority ReadCore(WellKnownTextNode node)
        {
            if (!string.Equals(node.Id, Wkt1Keyword, StringComparison.OrdinalIgnoreCase)
                && !string.Equals(node.Id, Wkt2Keyword, StringComparison.Ordinal))
            {
                throw new ArgumentException(string.Format(Properties.Resources.Culture, Properties.Resources.IsNotAValidNode, nameof(node), nameof(Authority).ToUpperInvariant()), nameof(node));
            }

            var enumerator = node.Values.GetEnumerator();
            return new(GetName(enumerator, nameof(Authority.Name)), GetCode(enumerator, nameof(Authority.Value)));

            static string GetName(IEnumerator<WellKnownTextValue> enumerator, string property)
            {
                if (!enumerator.MoveNext())
                {
                    throw new ArgumentException(string.Format(Properties.Resources.Culture, Properties.Resources.DoesNotHaveAValue, nameof(node), property), nameof(property));
                }

                if (enumerator.Current.TryGetValue(out string? s))
                {
                    return s;
                }

                if (enumerator.Current.TryGetValue(out Literal l))
                {
                    return l.ToString();
                }

                throw new ArgumentException(string.Format(Properties.Resources.Culture, Properties.Resources.DoesNotHaveAValue, nameof(node), property), nameof(property));
            }

            static AuthorityCode GetCode(IEnumerator<WellKnownTextValue> enumerator, string property)
            {
                if (!enumerator.MoveNext())
                {
                    throw new ArgumentException(string.Format(Properties.Resources.Culture, Properties.Resources.DoesNotHaveAValue, nameof(node), property), nameof(property));
                }

                if (enumerator.Current.TryGetValue(out string? s))
                {
                    return new(s);
                }

                if (enumerator.Current.TryGetValue(out double v))
                {
                    var truncated = Math.Truncate(v);
                    if (v.Equals(truncated))
                    {
                        return new((int)truncated);
                    }
                }

                if (enumerator.Current.TryGetValue(out Literal l))
                {
                    // see if this is an integer
                    var literalValue = l.ToString();
                    if (int.TryParse(literalValue, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var i))
                    {
                        return new(i);
                    }

                    return new(literalValue);
                }

                throw new ArgumentException(string.Format(Properties.Resources.Culture, Properties.Resources.DoesNotHaveAValue, nameof(node), property), nameof(property));
            }
        }
    }

    /// <inheritdoc/>
    public override IEnumerable<WellKnownTextNode> Write(Authority value, WktSerializerOptions options)
    {
        yield return ToWellKnownTextNode(value, options.Format);

        static WellKnownTextNode ToWellKnownTextNode(Authority value, WellKnownTextFormat format = FormatHelper.DefaultWktFormat)
        {
            return value == Authority.Empty
                ? WellKnownTextNode.Empty
                : ToWellKnownTextNode(value.Name, value.Value, format);

            static WellKnownTextNode ToWellKnownTextNode(string name, AuthorityCode value, WellKnownTextFormat format)
            {
                return format switch
                {
                    WellKnownTextFormat.Wkt1 when value.TryGetValue(out int @int) => new(nameof(Authority).ToUpperInvariant(), new(name), new(@int)),
                    WellKnownTextFormat.Wkt1 when value.TryGetValue(out string? @string) => new(nameof(Authority).ToUpperInvariant(), new(name), new(@string)),
                    WellKnownTextFormat.Wkt2 when value.TryGetValue(out int @int) => new(Wkt2Keyword, new(name), new(@int)),
                    WellKnownTextFormat.Wkt2 when value.TryGetValue(out string? @string) => new(Wkt2Keyword, new(name), new(@string)),
                    _ => throw new ArgumentOutOfRangeException(nameof(format)),
                };
            }
        }
    }
}