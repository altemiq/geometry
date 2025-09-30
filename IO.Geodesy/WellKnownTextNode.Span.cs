// -----------------------------------------------------------------------
// <copyright file="WellKnownTextNode.Span.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geodesy;

/// <content>
/// <see cref="Span{T}"/> based methods.
/// </content>
public partial class WellKnownTextNode
{
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
        var list = new List<NodeValue>();
        startValue++;
        value = value[startValue..endValue];
        var split = new SpanSplitEnumerator<char>(value, StartChar, EndChar, SeparatorChar);
        while (split.MoveNext())
        {
            var item = value[split.Current].Trim();

            if (item.IndexOf(StartChar) >= 0 && item.IndexOf(EndChar) >= 0)
            {
                list.Add(OneOf.From<WellKnownTextNode, string, double, Literal>(new WellKnownTextNode(item)));
            }
            else if (double.TryParse(item, System.Globalization.NumberStyles.Float | System.Globalization.NumberStyles.AllowThousands, System.Globalization.CultureInfo.InvariantCulture, out var doubleValue))
            {
                list.Add(OneOf.From<WellKnownTextNode, string, double, Literal>(doubleValue));
            }
            else if (item[0] == '\"' && item[^1] == '\"')
            {
                list.Add(OneOf.From<WellKnownTextNode, string, double, Literal>(item.Trim('\"').ToString()));
            }
            else
            {
                list.Add(OneOf.From<WellKnownTextNode, string, double, Literal>(new Literal(item.ToString())));
            }
        }

        this.Values = list.AsReadOnly();
    }

    private static ReadOnlySpan<char> GetSpan(string input)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(input);
        return input.AsSpan();
#else
        return input is not null ? input.AsSpan() : throw new ArgumentNullException(nameof(input));
#endif
    }
}