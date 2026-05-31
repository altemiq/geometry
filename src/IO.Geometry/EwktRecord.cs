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
        var index = this.Wkt.IndexOf(';', StringComparison.Ordinal);
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

        ReadOnlySpan<char> wktSpan = this.Wkt.AsSpan().Slice(index, count);
        Span<byte> bytes = stackalloc byte[System.Text.Encoding.UTF8.GetByteCount(wktSpan)];
        count = System.Text.Encoding.UTF8.GetBytes(wktSpan, bytes);

        return tryParse(bytes[..count], out var result, out _)
            ? result
            : throw new InvalidGeometryTypeException();
    }

    private static int GetSrid(ReadOnlySpan<char> span)
    {
#pragma warning disable SA1008
        return (span.IndexOf('='), span.IndexOf(';')) switch
        {
            ( >= 0 and var start, >= 0 and var end) => int.Parse(span[(start + 1)..end], provider: System.Globalization.CultureInfo.InvariantCulture),
            _ => 0,
        };
#pragma warning restore SA1008
    }
}