// -----------------------------------------------------------------------
// <copyright file="WktLiteral.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.Geodesy;

/// <summary>
/// The literal value.
/// </summary>
/// <param name="value">The value.</param>
public readonly ref struct WktLiteral(ReadOnlySpan<byte> value)
{
    /// <summary>
    /// Initialises a new instance of the <see cref="WktLiteral"/> struct.
    /// </summary>
    /// <param name="value">The value.</param>
    public WktLiteral(string value)
        : this(System.Text.Encoding.UTF8.GetBytes(value))
    {
    }

    /// <summary>
    /// Gets the span.
    /// </summary>
    internal ReadOnlySpan<byte> Span { get; } = value;

    /// <summary>
    /// Converts a string to a <see cref="WktLiteral"/> instance.
    /// </summary>
    /// <param name="value">The string value.</param>
    public static implicit operator WktLiteral(string value) => new(System.Text.Encoding.UTF8.GetBytes(value));

    /// <summary>
    /// Creates a <see cref="WktLiteral"/> instance from an enum value.
    /// </summary>
    /// <typeparam name="T">The type of enum.</typeparam>
    /// <param name="value">The enum value.</param>
    /// <returns>The literal from the enum.</returns>
    public static WktLiteral FromEnum<T>(T value)
        where T : System.Enum => new(System.Text.Encoding.UTF8.GetBytes(value.ToString()!));

    /// <summary>
    /// Converts this instance into an enum value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of enum.</typeparam>
    /// <returns>The enum value.</returns>
    /// <exception cref="System.InvalidOperationException">This instance is not valid for <typeparamref name="T"/>.</exception>
    public T ToEnum<T>()
        where T : struct, System.Enum
    {
        Span<char> chars = stackalloc char[this.Span.Length];
        var charCount = System.Text.Encoding.UTF8.GetChars(this.Span, chars);

        if (!System.Enum.TryParse<T>(chars[..charCount], ignoreCase: true, out var enumValue))
        {
            ThrowInvalidOperationException(chars[..charCount]);
        }

        return enumValue;

        [System.Diagnostics.CodeAnalysis.DoesNotReturn]
        static void ThrowInvalidOperationException(ReadOnlySpan<char> chars)
        {
#if NET6_0_OR_GREATER
            throw new System.InvalidOperationException($"The literal '{chars}' cannot be converted to the enum type '{typeof(T).Name}'.");
#else
            throw new System.InvalidOperationException($"The literal '{chars.ToString()}' cannot be converted to the enum type '{typeof(T).Name}'.");
#endif
        }
    }

    /// <inheritdoc/>
    public override string ToString() => System.Text.Encoding.UTF8.GetString(this.Span);
}