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
        var split = new SpanSplitEnumerator<char>(value);
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
#else
        if (input is null)
        {
            throw new ArgumentNullException(nameof(input));
        }
#endif
        return input.AsSpan();
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    private ref struct SpanSplitEnumerator<T>(ReadOnlySpan<T> span)
        where T : IEquatable<T>
    {
        private readonly ReadOnlySpan<T> buffer = span;

        private readonly bool isInitialized = true;

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
            if (!this.isInitialized || this.startNext > this.buffer.Length)
            {
                return false;
            }

            var slice = this.buffer[this.startNext..];
            this.startCurrent = this.startNext;

            var separatorIndex = -1;
            var open = 0;
            for (var i = 1; i < slice.Length; i++)
            {
                switch (slice[i])
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
                            separatorIndex = i;
                        }

                        break;
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