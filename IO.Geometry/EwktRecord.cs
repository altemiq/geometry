// -----------------------------------------------------------------------
// <copyright file="EwktRecord.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

/// <summary>
/// Represents an extended <see cref="WktRecord"/>.
/// </summary>
/// <param name="wkt">The well known text.</param>
public class EwktRecord(string wkt) : WktRecord(wkt), Data.ISridGeometryRecord
{
    /// <inheritdoc/>
    public int GetSrid() => GetSrid(this.Wkt.AsSpan());

    /// <inheritdoc />
    protected override T GetValue<T>(TryParse<T> tryParse)
    {
        var index = this.Wkt.IndexOf(';');
        var count = this.Wkt.Length;
        if (index is -1)
        {
            index = 0;
        }
        else
        {
            // find the first and last indexes
            while (++index < this.Wkt.Length)
            {
                if (!char.IsWhiteSpace(this.Wkt[index]))
                {
                    break;
                }
            }

            while (--count > 0)
            {
                if (!char.IsWhiteSpace(this.Wkt[count]))
                {
                    break;
                }
            }

            count -= index;
            count++;
        }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        ReadOnlySpan<char> wktSpan = this.Wkt.AsSpan().Slice(index, count);
        Span<byte> bytes = stackalloc byte[System.Text.Encoding.UTF8.GetByteCount(wktSpan)];
        count = System.Text.Encoding.UTF8.GetBytes(wktSpan, bytes);
        ReadOnlySpan<byte> span = bytes[..count];
#else
        var bytes = new byte[System.Text.Encoding.UTF8.GetByteCount(this.Wkt.ToCharArray(), index, count)];
        System.Text.Encoding.UTF8.GetBytes(this.Wkt.ToCharArray(), index, count, bytes, 0);
        ReadOnlySpan<byte> span = bytes;
#endif

        if (tryParse(span, out var result, out _))
        {
            return result;
        }

        throw new InvalidGeometryTypeException();
    }

    private static int GetSrid(ReadOnlySpan<char> span)
    {
#pragma warning disable SA1008
        return (span.IndexOf('='), span.IndexOf(';')) switch
        {
            ( >= 0, >= 0) indexes => GetSridCore(span[(indexes.Item1 + 1)..indexes.Item2]),
            _ => 0,
        };
#pragma warning restore SA1008

        static int GetSridCore(ReadOnlySpan<char> span)
        {
#if NETSTANDARD2_1_OR_GREATER
            return int.Parse(span, provider: System.Globalization.CultureInfo.InvariantCulture);
#else
            return int.Parse(span.ToString(), System.Globalization.CultureInfo.InvariantCulture);
#endif
        }
    }
}