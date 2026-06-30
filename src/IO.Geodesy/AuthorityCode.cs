// -----------------------------------------------------------------------
// <copyright file="AuthorityCode.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geodesy;

/// <summary>
/// Represents the authority code.
/// </summary>
public readonly struct AuthorityCode : System.Runtime.CompilerServices.IUnion, IEquatable<AuthorityCode>
{
    private readonly int intValue;
    private readonly string? stringValue;
    private readonly int tag;

    /// <summary>
    /// Initialises a new instance of the <see cref="AuthorityCode"/> struct.
    /// </summary>
    /// <param name="value">The value.</param>
    public AuthorityCode(int value)
    {
        this.intValue = value;
        this.tag = 1;
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="AuthorityCode"/> struct.
    /// </summary>
    /// <param name="value">The value.</param>
    public AuthorityCode(string value)
    {
        this.stringValue = value;
        this.tag = this.stringValue is not null ? 2 : default;
    }

    /// <inheritdoc />
    public object? Value => this.tag switch
    {
        1 => this.intValue,
        2 => this.stringValue!,
        _ => null,
    };

    /// <summary>
    /// Gets a value indicating whether this instance has a value.
    /// </summary>
    public bool HasValue => this.tag is not 0;

    /// <summary>
    /// Implements the equals operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator ==(AuthorityCode left, AuthorityCode right) => left.Equals(right);

    /// <summary>
    /// Implements the not-equals operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator !=(AuthorityCode left, AuthorityCode right) => !left.Equals(right);

    /// <inheritdoc/>
    public bool Equals(AuthorityCode other)
    {
        if (this.HasValue && other.HasValue && this.tag == other.tag)
        {
            return this.tag switch
            {
                1 => this.intValue == other.intValue,
                2 => string.Equals(this.stringValue, other.stringValue, StringComparison.Ordinal),
                _ => true,
            };
        }

        return false;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is AuthorityCode authorityCode && this.Equals(authorityCode);

    /// <inheritdoc/>
    public override int GetHashCode() => this.tag switch
    {
        1 => this.intValue,
        2 => StringComparer.Ordinal.GetHashCode(this.stringValue!),
        _ => 0,
    };

    /// <summary>
    /// Tries to get the value.
    /// </summary>
    /// <param name="value">The value to get.</param>
    /// <returns>A value indicating whether <paramref name="value"/> was successfully obtained.</returns>
    public bool TryGetValue(out int value)
    {
        value = this.intValue;
        return this.tag is 1;
    }

    /// <summary>
    /// Tries to get the value.
    /// </summary>
    /// <param name="value">The value to get.</param>
    /// <returns>A value indicating whether <paramref name="value"/> was successfully obtained.</returns>
    public bool TryGetValue([System.Diagnostics.CodeAnalysis.MaybeNullWhen(false)] out string value)
    {
        value = this.stringValue;
        return this.tag is 2;
    }
}