// -----------------------------------------------------------------------
// <copyright file="WktConverter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geodesy.Serialization;

/// <summary>
/// Converts an object or value to or from Well-known text.
/// </summary>
public abstract class WktConverter
{
    /// <summary>
    /// Gets the type to convert.
    /// </summary>
    internal virtual Type? TypeToConvert => null;

    /// <summary>
    /// When overridden in a derived class, determines whether the converter instance can convert the specified object type.
    /// </summary>
    /// <param name="typeToConvert">The type of the object to check whether it can be converted by this converter instance.</param>
    /// <returns><see langword="true"/> if the instance can convert the specified object type; otherwise, <see langword="false"/>.</returns>
    public abstract bool CanConvert(Type typeToConvert);

    /// <summary>
    /// Reads and converts the WKT.
    /// </summary>
    /// <param name="nodes">The nodes.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The converted value.</returns>
    internal abstract object? ReadAsObject(IEnumerable<WellKnownTextNode> nodes, Type typeToConvert, WktSerializerOptions options);

    /// <summary>
    /// Writes a specified value as WKT.
    /// </summary>
    /// <param name="value">The value to convert to WKT.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The <see cref="WellKnownTextNode"/> values that represents <paramref name="value"/>.</returns>
    internal abstract IEnumerable<WellKnownTextNode> WriteAsObject(object value, WktSerializerOptions options);
}