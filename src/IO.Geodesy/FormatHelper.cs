// -----------------------------------------------------------------------
// <copyright file="FormatHelper.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geodesy;

/// <summary>
/// The format helper.
/// </summary>
internal static partial class FormatHelper
{
    /// <summary>
    /// The default WKT format.
    /// </summary>
    public const WellKnownTextFormat DefaultWktFormat = WellKnownTextFormat.Wkt1;

    /// <summary>
    /// Gets the well known text format from the format string.
    /// </summary>
    /// <param name="format">The format.</param>
    /// <returns>The WTK format.</returns>
    public static WellKnownTextFormat GetWktFormat(string? format) => format switch
    {
        "wkt" => DefaultWktFormat,
        "wkt:1" => WellKnownTextFormat.Wkt1,
        "wkt:2" => WellKnownTextFormat.Wkt2,
        ['w', 'k', 't', ..] => throw new FormatException("Invalid WKT format string"),
        _ => WellKnownTextFormat.None,
    };

    /// <summary>
    /// Converts the string representation of a WTK format to its <see cref="WellKnownTextFormat"/> equivalent.
    /// A return value indicates whether the operation succeeded.
    /// </summary>
    /// <param name="format">The format.</param>
    /// <param name="version">When this method returns, contains the <see cref="WellKnownTextFormat"/> contained in <paramref name="format"/>, if the conversion succeeded, or an undefined value if the conversion failed.</param>
    /// <returns><see langword="true"/> if <paramref name="format"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetWktFormat(string? format, out WellKnownTextFormat version)
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
                    1 => (true, WellKnownTextFormat.Wkt1),
                    2 => (true, WellKnownTextFormat.Wkt2),
                    _ => (false, default),
                };

                return result;
            }
        }

        version = default;
        return false;
    }
}