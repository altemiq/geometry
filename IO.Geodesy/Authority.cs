// -----------------------------------------------------------------------
// <copyright file="Authority.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geodesy;

/// <summary>
/// Represents the authority for geodetic transformations, projections, data, and ellipsoids.
/// </summary>
/// <remarks>
/// Initialises a new instance of the <see cref="Authority"/> struct.
/// </remarks>
/// <param name="name">The name.</param>
/// <param name="value">The value.</param>
public readonly struct Authority(string name, string value) : IEquatable<Authority>, IFormattable
{
    /// <summary>
    /// Represents an empty <see cref="Authority"/>.
    /// </summary>
    public static readonly Authority Empty;

    /// <summary>
    /// Initialises a new instance of the <see cref="Authority"/> struct.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="value">The value.</param>
    public Authority(string name, int value)
        : this(name, value.ToString(System.Globalization.CultureInfo.InvariantCulture))
    {
    }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Gets the value.
    /// </summary>
    public string Value { get; } = value;

    /// <summary>
    /// Converts an EPSG value to an <see cref="Authority"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static explicit operator Authority(int value) => FromInt32(value);

    /// <summary>
    /// Compares two <see cref="Authority"/> objects. The result specifies whether all the properties of the two <see cref="Authority"/> objects are equal.
    /// </summary>
    /// <param name="left">The first <see cref="Authority"/> to compare.</param>
    /// <param name="right">The second <see cref="Authority"/> to compare.</param>
    /// <returns>Returns <see langword="true"/> if the properties of <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Authority left, Authority right) => left.Equals(right);

    /// <summary>
    /// Compares two <see cref="Authority"/> objects. The result specifies whether the properties of the two <see cref="Authority"/> objects are unequal.
    /// </summary>
    /// <param name="left">The first <see cref="Authority"/> to compare.</param>
    /// <param name="right">The second <see cref="Authority"/> to compare.</param>
    /// <returns>Returns <see langword="true"/> if the properties of <paramref name="left"/> and <paramref name="right"/> differ; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Authority left, Authority right) => !(left == right);

    /// <summary>
    /// Creates a new <see cref="Authority"/> from the specified <see cref="int"/> value.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The new <see cref="Authority"/>.</returns>
    public static Authority FromInt32(int value) => new("EPSG", value.ToString(System.Globalization.CultureInfo.InvariantCulture));

    /// <inheritdoc/>
    public override bool Equals([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] object? obj) => obj is Authority authority ? this.Equals(authority) : base.Equals(obj);

    /// <inheritdoc/>
    public bool Equals(Authority other) => string.Equals(this.Name, other.Name, StringComparison.Ordinal)
        && string.Equals(this.Value, other.Value, StringComparison.Ordinal);

    /// <inheritdoc/>
    public override int GetHashCode() =>
#if NETSTANDARD2_0_OR_GREATER || NET461_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        HashCode.Combine(this.Name, this.Value);
#else
        (this.Name, this.Value).GetHashCode();
#endif

    /// <inheritdoc/>
    public override string ToString() => this.ToString(formatProvider: default);

    /// <summary>
    /// Converts this <see cref="Authority"/> to a human-readable string.
    /// </summary>
    /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
    /// <returns>A string that represents this <see cref="Authority"/>.</returns>
    public string ToString(IFormatProvider? formatProvider) => this.ToString(format: default, formatProvider);

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider) => format switch
    {
        { } f when FormatHelper.TryGetWktFormat(f, out var version) => this.ToWkt(version),
        null => this.ToWkt(),
        _ => throw new FormatException(Properties.Resources.InvalidFormat),
    };

    /// <summary>
    /// Converts this instance into a WKT string.
    /// </summary>
    /// <param name="format">The WKT format.</param>
    /// <returns>The WKT string representing this instance.</returns>
    public string ToWkt(WellKnownTextFormat format = FormatHelper.DefaultWktFormat) => Serialization.WktSerializer.Serialize(this, new() { Format = format });
    
    /// <summary>
    /// Converts this instance into a <see cref="WellKnownTextNode"/>.
    /// </summary>
    /// <param name="format">The WKT format.</param>
    /// <returns>The <see cref="WellKnownTextNode"/>.</returns>
    public WellKnownTextNode ToWellKnownTextNode(WellKnownTextFormat format = FormatHelper.DefaultWktFormat) => Serialization.WktSerializer.GetNode(this, new() { Format = format });
}