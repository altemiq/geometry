// -----------------------------------------------------------------------
// <copyright file="Authority.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geodesy;

/// <summary>
/// Represents the authority for geodetic transformations, projections, data, and ellipsoids.
/// </summary>
/// <param name="name">The name.</param>
/// <param name="value">The value.</param>
public readonly struct Authority(string name, AuthorityCode value) : IEquatable<Authority>, IFormattable
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
        : this(name, new AuthorityCode(value))
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="Authority"/> struct.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="value">The value.</param>
    public Authority(string name, string value)
        : this(name, new AuthorityCode(value))
    {
    }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Gets the value.
    /// </summary>
    public AuthorityCode Value { get; } = value;

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
    public static Authority FromInt32(int value) => new("EPSG", value);

    /// <inheritdoc/>
    public override bool Equals([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] object? obj) => obj is Authority authority && this.Equals(authority);

    /// <inheritdoc/>
    public bool Equals(Authority other) => string.Equals(this.Name, other.Name, StringComparison.Ordinal) && this.Value.Equals(other.Value);

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(this.Name, this.Value);

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
    public string ToWkt(WktFormat format = FormatHelper.DefaultWktFormat)
    {
        var converter = format switch
        {
            WktFormat.Wkt2 => Serialization.Converters.AuthorityConverter.V2,
            _ => Serialization.Converters.AuthorityConverter.V1,
        };

        using var memoryStream = new MemoryStream();

        using (var writer = new Text.Geodesy.Utf8WktWriter(memoryStream))
        {
            converter.Write(writer, this, Text.Geodesy.WktSerializerOptions.Default);
        }

        return System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
    }
}