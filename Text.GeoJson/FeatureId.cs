// -----------------------------------------------------------------------
// <copyright file="FeatureId.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The feature ID.
/// </summary>
public sealed class FeatureId : IEquatable<FeatureId>
{
    private readonly string? stringValue;
    private readonly long longValue;

    /// <summary>
    /// Initialises a new instance of the <see cref="FeatureId"/> class.
    /// </summary>
    /// <param name="id">The ID value.</param>
    public FeatureId(string id) => this.stringValue = id;

    /// <summary>
    /// Initialises a new instance of the <see cref="FeatureId"/> class.
    /// </summary>
    /// <param name="id">The ID value.</param>
    public FeatureId(long id) => this.longValue = id;

    /// <summary>
    /// Gets the string representation of the value.
    /// </summary>
    public object Value => this.stringValue is { } s ? s : this.longValue;

    /// <summary>
    /// Implicitly converts a <see cref="long"/> value to a <see cref="FeatureId"/>.
    /// </summary>
    /// <param name="value">The long value.</param>
    public static implicit operator FeatureId(long value) => new(value);

    /// <summary>
    /// Implicitly converts a <see cref="string"/> value to a <see cref="FeatureId"/>.
    /// </summary>
    /// <param name="value">The string value.</param>
    public static implicit operator FeatureId(string value) => new(value);

    /// <summary>
    /// Explicitly converts a <see cref="FeatureId"/> value to a <see cref="long"/>.
    /// </summary>
    /// <param name="value">The id value.</param>
    public static explicit operator long(FeatureId value) => value.stringValue is null ? value.longValue : throw new InvalidCastException();

    /// <summary>
    /// Explicitly converts a <see cref="FeatureId"/> value to a <see cref="string"/>.
    /// </summary>
    /// <param name="value">The id value.</param>
    public static explicit operator string(FeatureId value) => value.stringValue is { } s ? s : throw new InvalidCastException();

    /// <summary>
    /// Implements the equals operator.
    /// </summary>
    /// <param name="left">The left-hand side.</param>
    /// <param name="right">The right-hand side.</param>
    /// <returns>The result of the operator.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2589:Boolean expressions should not be gratuitous", Justification = "Checked")]
    public static bool operator ==(FeatureId left, FeatureId right) => (left, right) switch
    {
        (null, null) => true,
        (null, not null) or (not null, null) => false,
        (var x, var y) => x.Equals(y),
    };

    /// <summary>
    /// Implements the not-equals operator.
    /// </summary>
    /// <param name="left">The left-hand side.</param>
    /// <param name="right">The right-hand side.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator !=(FeatureId left, FeatureId right) => !(left == right);

    /// <inheritdoc/>
    public override bool Equals(object obj) => obj is FeatureId other && this.Equals(other);

    /// <inheritdoc/>
    public bool Equals(FeatureId other)
    {
        return ReferenceEquals(this, other) || (other is not null && CheckTypes(this, other) && CheckValues(this, other));

        static bool CheckTypes(FeatureId first, FeatureId second)
        {
            return (first.stringValue is null) == (second.stringValue is null);
        }

        static bool CheckValues(FeatureId first, FeatureId second)
        {
            return first.stringValue is not null
                ? string.Equals(first.stringValue, second.stringValue, StringComparison.Ordinal)
                : first.longValue == second.longValue;
        }
    }

    /// <inheritdoc/>
    public override int GetHashCode() => this.stringValue is { } s ? StringComparer.Ordinal.GetHashCode(s) : this.longValue.GetHashCode();
}