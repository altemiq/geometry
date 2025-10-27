// -----------------------------------------------------------------------
// <copyright file="NumberOrString.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson.Stac;

/// <summary>
/// Represents a <see cref="double"/> or <see cref="string"/> value.
/// </summary>
public sealed class NumberOrString : IEquatable<NumberOrString>
{
    private readonly string? stringValue;
    private readonly double numberValue;

    /// <summary>
    /// Initialises a new instance of the <see cref="NumberOrString"/> class.
    /// </summary>
    /// <param name="id">The ID value.</param>
    public NumberOrString(string id) => this.stringValue = id;

    /// <summary>
    /// Initialises a new instance of the <see cref="NumberOrString"/> class.
    /// </summary>
    /// <param name="id">The ID value.</param>
    public NumberOrString(double id) => this.numberValue = id;

    /// <summary>
    /// Gets the string representation of the value.
    /// </summary>
    public object Value => this.stringValue is { } s ? s : this.numberValue;

    /// <summary>
    /// Implicitly converts a <see cref="double"/> value to a <see cref="FeatureId"/>.
    /// </summary>
    /// <param name="value">The double value.</param>
    public static implicit operator NumberOrString(double value) => new(value);

    /// <summary>
    /// Implicitly converts a <see cref="string"/> value to a <see cref="FeatureId"/>.
    /// </summary>
    /// <param name="value">The string value.</param>
    public static implicit operator NumberOrString(string value) => new(value);

    /// <summary>
    /// Explicitly converts a <see cref="FeatureId"/> value to a <see cref="double"/>.
    /// </summary>
    /// <param name="value">The id value.</param>
    public static explicit operator double(NumberOrString value) => value.stringValue is null ? value.numberValue : throw new InvalidCastException();

    /// <summary>
    /// Explicitly converts a <see cref="FeatureId"/> value to a <see cref="string"/>.
    /// </summary>
    /// <param name="value">The id value.</param>
    public static explicit operator string(NumberOrString value) => value.stringValue is { } s ? s : throw new InvalidCastException();

    /// <summary>
    /// Implements the equals operator.
    /// </summary>
    /// <param name="left">The left-hand side.</param>
    /// <param name="right">The right-hand side.</param>
    /// <returns>The result of the operator.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2589:Boolean expressions should not be gratuitous", Justification = "Checked")]
    public static bool operator ==(NumberOrString left, NumberOrString right) => (left, right) switch
    {
        (null, null) => true,
        (null, not null) or (not null, null) => false,
        var (x, y) => x.Equals(y),
    };

    /// <summary>
    /// Implements the not-equals operator.
    /// </summary>
    /// <param name="left">The left-hand side.</param>
    /// <param name="right">The right-hand side.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator !=(NumberOrString left, NumberOrString right) => !(left == right);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is NumberOrString other && this.Equals(other);

    /// <inheritdoc/>
    public bool Equals(NumberOrString? other)
    {
        return ReferenceEquals(this, other) || (other is not null && CheckTypes(this, other) && CheckValues(this, other));

        static bool CheckTypes(NumberOrString first, NumberOrString second)
        {
            return (first.stringValue is null) == (second.stringValue is null);
        }

        static bool CheckValues(NumberOrString first, NumberOrString second)
        {
            return first.stringValue is not null
                ? string.Equals(first.stringValue, second.stringValue, StringComparison.Ordinal)
                : first.numberValue.Equals(second.numberValue);
        }
    }

    /// <inheritdoc/>
    public override int GetHashCode() => this.stringValue is not null ? StringComparer.Ordinal.GetHashCode(this.stringValue) : this.numberValue.GetHashCode();
}