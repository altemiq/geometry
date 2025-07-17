// -----------------------------------------------------------------------
// <copyright file="SizeM.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

using System.Runtime.CompilerServices;

/// <summary>
/// Stores an ordered tuple of floating-point numbers, typically the width, height and depth of a cube.
/// </summary>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public struct SizeM : IEquatable<SizeM>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="SizeM" /> class.
    /// </summary>
    public static readonly SizeM Empty;

    /// <summary>
    /// Initialises a new instance of the <see cref="SizeM" /> struct from the specified existing <see cref="SizeM" />.
    /// </summary>
    /// <param name="size">The <see cref="SizeM" /> from which to create the new <see cref="SizeM" />.</param>
    public SizeM(SizeM size)
        : this(size.Width, size.Height, size.Measurement)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="SizeM" /> struct from the specified <see cref="PointM" />.
    /// </summary>
    /// <param name="point">The <see cref="PointM" /> from which to initialize this <see cref="SizeM" />.</param>
    public SizeM(PointM point)
    {
        (this.Width, this.Height, this.Measurement) = (point.X, point.Y, point.Measurement);
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="SizeM" /> struct from the specified dimensions.
    /// </summary>
    /// <param name="width">The width component of the new <see cref="SizeM" />.</param>
    /// <param name="height">The height component of the new <see cref="SizeM" />.</param>
    /// <param name="depth">The depth component of the new <see cref="SizeM" />.</param>
    public SizeM(double width, double height, double depth) => (this.Width, this.Height, this.Measurement) = (width, height, depth);

    /// <summary>
    /// Gets a value indicating whether this <see cref="SizeM" /> has zero width and height.
    /// </summary>
    /// <value>This property returns <see langword="true"/> when this <see cref="SizeM" /> has both a width and height of zero; otherwise, <see langword="false"/>.</value>
    public readonly bool IsEmpty => this.Width is default(double) && this.Height is default(double) && this.Measurement is default(double);

    /// <summary>
    /// Gets or sets the horizontal component of this <see cref="SizeM" />.
    /// </summary>
    /// <value>The horizontal component of this <see cref="SizeM" />.</value>
    public double Width { get; set; }

    /// <summary>
    /// Gets or sets the vertical component of this <see cref="SizeM" />.
    /// </summary>
    /// <value>The vertical component of this <see cref="SizeM" />.</value>
    public double Height { get; set; }

    /// <summary>
    /// Gets or sets the measurement component of this <see cref="SizeM" />.
    /// </summary>
    /// <value>The measurement component of this <see cref="SizeM" />.</value>
    public double Measurement { get; set; }

    /// <summary>
    /// Converts the specified <see cref="SizeM" /> to a <see cref="PointM" />.
    /// </summary>
    /// <param name="size">The <see cref="SizeM" /> to convert from.</param>
    public static explicit operator PointM(SizeM size) => new(size.Width, size.Height, size.Measurement);

    /// <summary>
    /// Converts the specified <see cref="SizeM" /> to a <see cref="Size" />.
    /// </summary>
    /// <param name="size">The <see cref="SizeM" /> to convert from.</param>
    public static explicit operator Size(SizeM size) => new(size.Width, size.Height);

    /// <summary>
    /// Adds the width and height of one <see cref="SizeM" /> structure to the width and height of another <see cref="SizeM" /> structure.
    /// </summary>
    /// <param name="size1">The first <see cref="SizeM" /> to add.</param>
    /// <param name="size2">The second <see cref="SizeM" /> to add.</param>
    /// <returns>A <see cref="SizeM" /> structure that is the result of the addition operation.</returns>
    public static SizeM operator +(SizeM size1, SizeM size2) => Add(size1, size2);

    /// <summary>
    /// Subtracts the width and height of one <see cref="SizeM" /> structure from the width and height of another <see cref="SizeM" /> structure.
    /// </summary>
    /// <param name="size1">The <see cref="SizeM" /> on the left side of the subtraction operator.</param>
    /// <param name="size2">The <see cref="SizeM" /> on the right side of the subtraction operator.</param>
    /// <returns>A <see cref="SizeM" /> structure that is the result of the subtraction operation.</returns>
    public static SizeM operator -(SizeM size1, SizeM size2) => Subtract(size1, size2);

    /// <summary>
    /// Tests whether two <see cref="SizeM" /> objects are equal.
    /// </summary>
    /// <param name="size1">The <see cref="SizeM" /> on the left side of the equality operator.</param>
    /// <param name="size2">The <see cref="SizeM" /> on the right side of the equality operator.</param>
    /// <returns>This operator returns <see langword="true"/> if <paramref name="size1" /> and <paramref name="size2" /> have equal width and height; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(SizeM size1, SizeM size2) => size1.Width.Equals(size2.Width) && size1.Height.Equals(size2.Height) && size1.Measurement.Equals(size2.Measurement);

    /// <summary>
    /// Tests whether two <see cref="SizeM" /> structures are different.
    /// </summary>
    /// <param name="size1">The <see cref="SizeM" /> on the left side of the inequality operator.</param>
    /// <param name="size2">The <see cref="SizeM" /> on the right side of the inequality operator.</param>
    /// <returns>This operator returns <see langword="true"/> if <paramref name="size1" /> and <paramref name="size2" /> differ either in width or height; <see langword="false"/> if <paramref name="size1" /> and <paramref name="size2" /> are equal.</returns>
    public static bool operator !=(SizeM size1, SizeM size2) => !(size1 == size2);

    /// <summary>
    /// Returns the negation of the specified size.
    /// </summary>
    /// <param name="size">The size to negate.</param>
    /// <returns>The negated of <paramref name="size"/>.</returns>
    public static SizeM operator -(SizeM size) => size.Negate();

    /// <summary>
    /// Adds the width and height of one <see cref="SizeM" /> structure to the width and height of another <see cred="Size3D" /> structure.
    /// </summary>
    /// <param name="size1">The first <see cref="SizeM" /> to add.</param>
    /// <param name="size2">The second <see cref="SizeM" /> to add.</param>
    /// <returns>A <see cref="SizeM" /> structure that is the result of the addition operation.</returns>
    public static SizeM Add(SizeM size1, SizeM size2) => new(size1.Width + size2.Width, size1.Height + size2.Height, size1.Measurement + size2.Measurement);

    /// <summary>
    /// Subtracts the width and height of one <see cref="SizeM" /> structure from the width and height of another <see cref="SizeM" /> structure.
    /// </summary>
    /// <param name="size1">The <see cref="SizeM" /> on the left side of the subtraction operator.</param>
    /// <param name="size2">The <see cref="SizeM" /> on the right side of the subtraction operator.</param>
    /// <returns>A <see cref="SizeM" /> structure that is the result of the subtraction operation.</returns>
    public static SizeM Subtract(SizeM size1, SizeM size2) => new(size1.Width - size2.Width, size1.Height - size2.Height, size1.Measurement - size2.Measurement);

    /// <summary>
    /// Returns the negation of this instance.
    /// </summary>
    /// <returns>The negated of this instance.</returns>
    public readonly SizeM Negate() => new(-this.Width, -this.Height, -this.Measurement);

    /// <inheritdoc/>
    public override readonly bool Equals([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] object? obj) => obj is SizeM size ? this.Equals(size) : base.Equals(obj);

    /// <inheritdoc/>
    public readonly bool Equals(SizeM other) => other.Width.Equals(this.Width) && other.Height.Equals(this.Height) && other.Measurement.Equals(this.Measurement);

    /// <inheritdoc/>
    public override readonly int GetHashCode() =>
#if NETSTANDARD2_0_OR_GREATER || NET461_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        HashCode.Combine(this.Width, this.Height, this.Measurement);
#else
        (this.Width, this.Height, this.Measurement).GetHashCode();
#endif

    /// <summary>
    /// Converts a <see cref="SizeM" /> to a <see cref="PointM" />.
    /// </summary>
    /// <returns>Returns a <see cref="PointM" /> structure.</returns>
    public readonly PointM ToPoint3D() => (PointM)this;

    /// <summary>
    /// Converts a <see cref="SizeM" /> to a <see cref="Size" />.
    /// </summary>
    /// <returns>Returns a <see cref="Size" /> structure.</returns>
    public readonly Size ToSizeD() => (Size)this;

    /// <summary>
    /// Converts a <see cref="SizeM" /> to a <see cref="double" />.
    /// </summary>
    /// <returns>Returns a <see cref="double" /> value representing the diagonal length of the <see cref="SizeM" />.</returns>
    public readonly double ToDouble() => System.Math.Sqrt((this.Height * this.Height) + (this.Width * this.Width) + (this.Measurement * this.Measurement));

    /// <inheritdoc/>
    public override readonly string ToString() => this.ToString(default);

    /// <summary>
    /// Converts the value of this instance to its equivalent string representation.
    /// </summary>
    /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
    /// <returns>A string representation of value of this instance.</returns>
    public readonly string ToString(IFormatProvider? formatProvider) =>
#if NETSTANDARD1_3_OR_GREATER || NET46_OR_GREATER || NETCOREAPP
        ((FormattableString)$"{{{nameof(this.Width)}={this.Width}, {nameof(this.Height)}={this.Height}, {nameof(this.Measurement)}={this.Measurement}}}").ToString(formatProvider);
#else
        string.Format(formatProvider, "{{Width={0}, Height={1}, Measurement={2}}}", this.Width, this.Height, this.Measurement);
#endif
}