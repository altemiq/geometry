// -----------------------------------------------------------------------
// <copyright file="WktConverter{T}.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.Geodesy.Serialization;

/// <summary>
/// Converts an object or value to or from well-known text.
/// </summary>
/// <typeparam name="T">The type of object or value handled by the converter.</typeparam>
public abstract class WktConverter<T> : WktConverter
{
    /// <inheritdoc/>
    internal override Type TypeToConvert => typeof(T);

    /// <inheritdoc/>
    public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(T);

    /// <summary>
    /// Reads and converts the WKT to type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The converted value.</returns>
    public abstract T Read(ref Utf8WktReader reader, Type typeToConvert, WktSerializerOptions options);

    /// <summary>
    /// Writes a specified value as WKT.
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to convert to WKT.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    public abstract void Write(Utf8WktWriter writer, T value, WktSerializerOptions options);
}