// -----------------------------------------------------------------------
// <copyright file="WktSerializationOptionsExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable CA1050, MA0047, RCS1110

/// <summary>
/// The <see cref="Altemiq.Text.Geodesy.WktSerializerOptions"/> extension.
/// </summary>
public static class WktSerializationOptionsExtensions
{
    extension(Altemiq.Text.Geodesy.WktSerializerOptions)
    {
        /// <summary>
        /// Gets the options for WKT1.
        /// </summary>
        public static Altemiq.Text.Geodesy.WktSerializerOptions Wkt1 => WktSerializationOptionsExtensions.Wkt1Field;

        /// <summary>
        /// Gets the options for WKT2.
        /// </summary>
        public static Altemiq.Text.Geodesy.WktSerializerOptions Wkt2 => WktSerializationOptionsExtensions.Wkt2Field;

        /// <summary>
        /// Gets the options for the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>The options.</returns>
        public static Altemiq.Text.Geodesy.WktSerializerOptions ForFormat(Altemiq.IO.Geodesy.WktFormat format) => format switch
        {
            Altemiq.IO.Geodesy.WktFormat.Wkt2 => Wkt2Field,
            _ => Wkt1Field,
        };
    }

    private static readonly Altemiq.Text.Geodesy.WktSerializerOptions Wkt1Field = new()
    {
        Converters =
        {
            Altemiq.IO.Geodesy.Serialization.Converters.AuthorityConverter.V1,
        },
    };

    private static Altemiq.Text.Geodesy.WktSerializerOptions Wkt2Field => new()
    {
        Converters =
        {
            Altemiq.IO.Geodesy.Serialization.Converters.AuthorityConverter.V2,
        },
    };
}