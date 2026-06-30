// -----------------------------------------------------------------------
// <copyright file="Literal.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.Geodesy;

/// <summary>
/// The literal value.
/// </summary>
/// <param name="value">The value.</param>
public readonly ref struct Literal(ReadOnlySpan<byte> value)
{
    private readonly ReadOnlySpan<byte> value = value;

    /// <summary>
    /// Initialises a new instance of the <see cref="Literal"/> struct.
    /// </summary>
    /// <param name="value">The value.</param>
    public Literal(string value)
        : this(System.Text.Encoding.UTF8.GetBytes(value))
    {
    }

    /// <summary>
    /// Converts a string to a <see cref="Literal"/> instance.
    /// </summary>
    /// <param name="value">The string value.</param>
    public static implicit operator Literal(string value) => new(System.Text.Encoding.UTF8.GetBytes(value));

    /// <inheritdoc/>
    public override string ToString() => System.Text.Encoding.UTF8.GetString(this.value);
}