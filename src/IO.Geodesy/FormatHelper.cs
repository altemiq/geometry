// -----------------------------------------------------------------------
// <copyright file="FormatHelper.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geodesy;

/// <summary>
/// The format helper.
/// </summary>
internal static class FormatHelper
{
    /// <summary>
    /// The default WKT format.
    /// </summary>
    public const WktFormat DefaultWktFormat = WktFormat.Wkt1;

    /// <summary>
    /// Gets the well known text format from the format string.
    /// </summary>
    /// <param name="format">The format.</param>
    /// <returns>The WTK format.</returns>
    public static WktFormat GetWktFormat(string? format) => format switch
    {
        "wkt" => DefaultWktFormat,
        "wkt:1" => WktFormat.Wkt1,
        "wkt:2" => WktFormat.Wkt2,
        ['w', 'k', 't', ..] => throw new FormatException("Invalid WKT format string"),
        _ => WktFormat.None,
    };

    /// <summary>
    /// Gets the well known text version from the format string.
    /// </summary>
    /// <param name="format">The format.</param>
    /// <returns>The WTK format.</returns>
    public static WktFormat GetWktFormat(ReadOnlySpan<char> format) => format switch
    {
#pragma warning disable format
        ['w', 'k', 't'] => DefaultWktFormat,
        ['w', 'k', 't', ':', '1'] => WktFormat.Wkt1,
        ['w', 'k', 't', ':', '2'] => WktFormat.Wkt2,
        ['w', 'k', 't', ..] => throw new FormatException("Invalid WKT format string"),
#pragma warning restore format
        _ => WktFormat.None,
    };

    /// <summary>
    /// Converts the string representation of a WTK format to its <see cref="WktFormat"/> equivalent.
    /// A return value indicates whether the operation succeeded.
    /// </summary>
    /// <param name="format">The format.</param>
    /// <param name="version">When this method returns, contains the <see cref="WktFormat"/> contained in <paramref name="format"/>, if the conversion succeeded, or an undefined value if the conversion failed.</param>
    /// <returns><see langword="true"/> if <paramref name="format"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetWktFormat(string? format, out WktFormat version)
    {
        if (format?.Length >= 3 && format.StartsWith("wkt", StringComparison.OrdinalIgnoreCase))
        {
            if (format.Length is 3)
            {
                version = DefaultWktFormat;
                return true;
            }

            if (format[3] is not ':')
            {
                version = default;
                return false;
            }

            if (int.TryParse(format[4..], System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var intVersion))
            {
                (var result, version) = intVersion switch
                {
                    1 => (true, WktFormat.Wkt1),
                    2 => (true, WktFormat.Wkt2),
                    _ => (false, default),
                };

                return result;
            }
        }

        version = default;
        return false;
    }

    /// <summary>
    /// Converts the string representation of a WTK format to its <see cref="WktFormat"/> equivalent.
    /// A return value indicates whether the operation succeeded.
    /// </summary>
    /// <param name="format">The format.</param>
    /// <param name="version">When this method returns, contains the <see cref="WktFormat"/> contained in <paramref name="format"/>, if the conversion succeeded, or an undefined value if the conversion failed.</param>
    /// <returns><see langword="true"/> if <paramref name="format"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetWktFormat(ReadOnlySpan<char> format, out WktFormat version)
    {
        if (format.Length >= 3 && format.StartsWith("wkt", StringComparison.OrdinalIgnoreCase))
        {
            if (format.Length is 3)
            {
                version = DefaultWktFormat;
                return true;
            }

            if (format[3] is not ':')
            {
                version = default;
                return false;
            }

            if (int.TryParse(format[4..], System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var intVersion))
            {
                (var result, version) = intVersion switch
                {
                    1 => (true, WktFormat.Wkt1),
                    2 => (true, WktFormat.Wkt2),
                    _ => (false, default),
                };

                return result;
            }
        }

        version = default;
        return false;
    }
}