// -----------------------------------------------------------------------
// <copyright file="GeoJsonExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#if !NETSTANDARD2_1_OR_GREATER && !NETCOREAPP2_0_OR_GREATER
namespace Altemiq.Text.GeoJson;

/// <summary>
/// The <see cref="GeoJson"/> extensions.
/// </summary>
internal static class GeoJsonExtensions
{
    extension(Enum)
    {
        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants specified by <typeparamref name="TEnum"/> to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="TEnum">An enumeration type.</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <returns>An object of type <typeparamref name="TEnum"/> whose value is represented by <paramref name="value"/>.</returns>
        public static TEnum Parse<TEnum>(string value)
            where TEnum : Enum => (TEnum)Enum.Parse(typeof(TEnum), value);
    }
}
#endif