// -----------------------------------------------------------------------
// <copyright file="ExtensionMethods.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The extension methods.
/// </summary>
internal static class ExtensionMethods
{
    /// <summary>
    /// Moves to the specified token type.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="type">The token type to move to.</param>
    /// <returns><see langword="true"/> if <paramref name="reader"/> has moved forward to <paramref name="type"/>.</returns>
    public static bool ReadTo(this ref Utf8JsonReader reader, JsonTokenType type)
    {
        if (reader.TokenType == type)
        {
            return true;
        }

        while (reader.Read())
        {
            if (reader.TokenType == type)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Moves to the specified token type.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="types">The token types to move to.</param>
    /// <returns><see langword="true"/> if <paramref name="reader"/> has moved forward to the first valid type from <paramref name="types"/>.</returns>
    public static bool ReadTo(this ref Utf8JsonReader reader, params JsonTokenType[] types)
    {
        if (types.Contains(reader.TokenType))
        {
            return true;
        }

        while (reader.Read())
        {
            if (types.Contains(reader.TokenType))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Gets the type.
    /// </summary>
    /// <typeparam name="T">The type of enum.</typeparam>
    /// <param name="reader">The reader.</param>
    /// <returns>The type from the reader.</returns>
    /// <exception cref="InvalidOperationException">Cannot read type.</exception>
    public static T GetType<T>(this ref Utf8JsonReader reader)
        where T : struct, Enum
    {
        T type;
        string propertyName;
        do
        {
            if (!reader.ReadTo(JsonTokenType.PropertyName))
            {
                throw new InvalidOperationException();
            }

            propertyName = reader.GetString() ?? throw new InvalidOperationException();
            _ = reader.Read();
        }
        while (!string.Equals(propertyName, nameof(type), StringComparison.Ordinal));

        return Enum.TryParse(reader.GetString(), ignoreCase: false, out type)
            ? type
            : throw new InvalidOperationException();
    }
}