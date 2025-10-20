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
        var node = nodes.Single();
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(node);
#else
        if (node is null)
        {
            throw new ArgumentNullException(nameof(nodes));
        }
#endif

        return ReadCore(node);

        static Authority ReadCore(WellKnownTextNode node)
        {
            if (!string.Equals(node.Id, Wkt1Keyword, StringComparison.OrdinalIgnoreCase)
                && !string.Equals(node.Id, Wkt2Keyword, StringComparison.Ordinal))
            {
                throw new ArgumentException(string.Format(Properties.Resources.Culture, Properties.Resources.IsNotAValidNode, nameof(node), nameof(Authority).ToUpperInvariant()), nameof(node));
            }

            var enumerator = node.Values.GetEnumerator();
            return new(GetValue(enumerator, nameof(Authority.Name)), GetValue(enumerator, nameof(Authority.Value)));

            static string GetValue(IEnumerator<NodeValue> enumerator, string property)
            {
                return enumerator.MoveNext()
                    ? enumerator.Current.Match(
                        _ => throw new ArgumentException(string.Format(Properties.Resources.Culture, Properties.Resources.DoesNotHaveAValidValue, nameof(node), property), nameof(property)),
                        s => s ?? throw new ArgumentException(string.Format(Properties.Resources.Culture, Properties.Resources.DoesNotHaveAValidValue, nameof(node), property), nameof(property)),
                        v => v.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        l => l.ToString())
                    : throw new ArgumentException(string.Format(Properties.Resources.Culture, Properties.Resources.DoesNotHaveAValue, nameof(node), property), nameof(property));
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

            static WellKnownTextNode ToWellKnownTextNode(string name, string value, WellKnownTextFormat format)
            {
                return format switch
                {
                    WellKnownTextFormat.Wkt1 => new(nameof(Authority).ToUpperInvariant(), name, value),
                    WellKnownTextFormat.Wkt2 when int.TryParse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var @int) => new(Wkt2Keyword, name, @int),
                    WellKnownTextFormat.Wkt2 => new(Wkt2Keyword, name, value),
                    _ => throw new ArgumentOutOfRangeException(nameof(format)),
                };
            }
        }
    }
}