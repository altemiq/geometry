// -----------------------------------------------------------------------
// <copyright file="WktConverter{T}.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geodesy.Serialization;

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
    /// <param name="nodes">The nodes.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The converted value.</returns>
    public abstract T Read(IEnumerable<WellKnownTextNode> nodes, Type typeToConvert, WktSerializerOptions options);

    /// <summary>
    /// Writes a specified value as WKT.
    /// </summary>
    /// <param name="value">The value to convert to WKT.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The <see cref="WellKnownTextNode"/> instances that represents <paramref name="value"/>.</returns>
    public abstract IEnumerable<WellKnownTextNode> Write(T value, WktSerializerOptions options);

    /// <summary>
    /// Read the value as an object.
    /// </summary>
    /// <param name="nodes">The nodes.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The converted value.</returns>
    internal sealed override object? ReadAsObject(IEnumerable<WellKnownTextNode> nodes, Type typeToConvert, WktSerializerOptions options) => this.Read(nodes, typeToConvert, options);

    /// <summary>
    /// Writes the value from an object.
    /// </summary>
    /// <param name="value">The value to write.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The <see cref="WellKnownTextNode"/> instances that represents <paramref name="value"/>.</returns>
    internal sealed override IEnumerable<WellKnownTextNode> WriteAsObject(object value, WktSerializerOptions options) => this.Write(WktSerializer.UnboxOnWrite<T>(value), options);
}