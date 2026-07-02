// -----------------------------------------------------------------------
// <copyright file="WktConverter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.Geodesy.Serialization;

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
}