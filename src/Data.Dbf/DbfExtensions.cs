// -----------------------------------------------------------------------
// <copyright file="DbfExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#if !NETSTANDARD2_1_OR_GREATER
namespace Altemiq.Data.Dbf;

#pragma warning disable SA1101

/// <summary>
/// The <see cref="Dbf"/> extensions.
/// </summary>
internal static class DbfExtensions
{
    extension(DateTime)
    {
        /// <summary>Converts the specified span representation of a date and time to its <see cref="System.DateTime" /> equivalent using the specified format, culture-specific format information, and style. The format of the string representation must match the specified format exactly or an exception is thrown.</summary>
        /// <param name="s">A span containing the characters that represent a date and time to convert.</param>
        /// <param name="format">A span containing the characters that represent a format specifier that defines the required format of <paramref name="s" />.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
        /// <param name="style">A bitwise combination of the enumeration values that provides additional information about <paramref name="s" />, about style elements that may be present in <paramref name="s" />, or about the conversion from <paramref name="s" /> to a <see cref="System.DateTime" /> value. A typical value to specify is <see cref="System.Globalization.DateTimeStyles.None" />.</param>
        /// <returns>An object that is equivalent to the date and time contained in <paramref name="s" />, as specified by <paramref name="format" />, <paramref name="provider" />, and <paramref name="style" />.</returns>
        public static DateTime ParseExact(
            ReadOnlySpan<char> s,
            ReadOnlySpan<char> format,
            IFormatProvider? provider,
            System.Globalization.DateTimeStyles style = System.Globalization.DateTimeStyles.None) => DateTime.ParseExact(s.ToString(), format.ToString(), provider, style);
    }
}
#endif