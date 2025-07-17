// -----------------------------------------------------------------------
// <copyright file="SizeZM.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

/// <summary>
/// Stores an ordered tuple of floating-point numbers, typically the width, height, depth, and length of a 4D cube.
/// </summary>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public struct SizeZM : IEquatable<SizeZM>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="SizeZM" /> class.
    /// </summary>
    public static readonly SizeZM Empty;

    /// <summary>
    /// Initialises a new instance of the <see cref="SizeZM" /> struct from the specified existing <see cref="SizeZM" />.
    /// </summary>
    /// <param name="size">The <see cref="SizeZM" /> from which to create the new <see cref="SizeZM" />.</param>
    public SizeZM(SizeZM size)
        : this(size.Width, size.Height, size.Depth, size.Length)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="SizeZM" /> struct from the specified <see cref="PointZ" />.
    /// </summary>
    /// <param name="point">The <see cref="PointZM" /> from which to initialize this <see cref="SizeZM" />.</param>
    public SizeZM(PointZM point) => (this.Width, this.Height, this.Depth, this.Length) = (point.X, point.Y, point.Z, point.Measurement);

    /// <summary>
    /// Initialises a new instance of the <see cref="SizeZM" /> struct from the specified dimensions.
    /// </summary>
    /// <param name="width">The width component of the new <see cref="SizeZM" />.</param>
    /// <param name="height">The height component of the new <see cref="SizeZM" />.</param>
    /// <param name="depth">The depth component of the new <see cref="SizeZM" />.</param>
    /// <param name="length">The length component of the new <see cref="SizeZM" />.</param>
    public SizeZM(double width, double height, double depth, double length) => (this.Width, this.Height, this.Depth, this.Length) = (width, height, depth, length);

    /// <summary>
    /// Gets a value indicating whether this <see cref="SizeZM" /> has zero width and height.
    /// </summary>
    /// <value>This property returns <see langword="true"/> when this <see cref="SizeZM" /> has both a width and height of zero; otherwise, <see langword="false"/>.</value>
    public readonly bool IsEmpty => this.Width is default(double) && this.Height is default(double) && this.Depth is default(double) && this.Length is default(double);

    /// <summary>
    /// Gets or sets the horizontal component of this <see cref="SizeZM" />.
    /// </summary>
    /// <value>The horizontal component of this <see cref="SizeZM" />.</value>
    public double Width { get; set; }

    /// <summary>
    /// Gets or sets the vertical component of this <see cref="SizeZM" />.
    /// </summary>
    /// <value>The vertical component of this <see cref="SizeZM" />.</value>
    public double Height { get; set; }

    /// <summary>
    /// Gets or sets the depth component of this <see cref="SizeZM" />.
    /// </summary>
    /// <value>The depth component of this <see cref="SizeZM" />.</value>
    public double Depth { get; set; }

    /// <summary>
    /// Gets or sets the length component of this <see cref="SizeZM" />.
    /// </summary>
    /// <value>The length component of this <see cref="SizeZM" />.</value>
    public double Length { get; set; }

    /// <summary>
    /// Converts the specified <see cref="SizeZM" /> to a <see cref="PointZM" />.
    /// </summary>
    /// <param name="size">The <see cref="SizeZM" /> to convert from.</param>
    public static explicit operator PointZM(SizeZM size) => new(size.Width, size.Height, size.Depth, size.Length);

    /// <summary>
    /// Converts the specified <see cref="SizeZM" /> to a <see cref="SizeZ" />.
    /// </summary>
    /// <param name="size">The <see cref="SizeZM" /> to convert from.</param>
    public static explicit operator SizeZ(SizeZM size) => new(size.Width, size.Height, size.Depth);

    /// <summary>
    /// Converts the specified <see cref="SizeZM" /> to a <see cref="Size" />.
    /// </summary>
    /// <param name="size">The <see cref="SizeZM" /> to convert from.</param>
    public static explicit operator Size(SizeZM size) => new(size.Width, size.Height);

    /// <summary>
    /// Adds the width and height of one <see cref="SizeZM" /> structure to the width and height of another <see cref="SizeZM" /> structure.
    /// </summary>
    /// <param name="size1">The first <see cref="SizeZM" /> to add.</param>
    /// <param name="size2">The second <see cref="SizeZM" /> to add.</param>
    /// <returns>A <see cref="SizeZM" /> structure that is the result of the addition operation.</returns>
    public static SizeZM operator +(SizeZM size1, SizeZM size2) => Add(size1, size2);

    /// <summary>
    /// Subtracts the width and height of one <see cref="SizeZM" /> structure from the width and height of another <see cref="SizeZM" /> structure.
    /// </summary>
    /// <param name="size1">The <see cref="SizeZM" /> on the left side of the subtraction operator.</param>
    /// <param name="size2">The <see cref="SizeZM" /> on the right side of the subtraction operator.</param>
    /// <returns>A <see cref="SizeZM" /> structure that is the result of the subtraction operation.</returns>
    public static SizeZM operator -(SizeZM size1, SizeZM size2) => Subtract(size1, size2);

    /// <summary>
    /// Tests whether two <see cref="SizeZM" /> objects are equal.
    /// </summary>
    /// <param name="size1">The <see cref="SizeZM" /> on the left side of the equality operator.</param>
    /// <param name="size2">The <see cref="SizeZM" /> on the right side of the equality operator.</param>
    /// <returns>This operator returns <see langword="true"/> if <paramref name="size1" /> and <paramref name="size2" /> have equal width and height; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(SizeZM size1, SizeZM size2) => size1.Width.Equals(size2.Width) && size1.Height.Equals(size2.Height) && size1.Depth.Equals(size2.Depth) && size1.Length.Equals(size2.Length);

    /// <summary>
    /// Tests whether two <see cref="SizeZM" /> structures are different.
    /// </summary>
    /// <param name="size1">The <see cref="SizeZM" /> on the left side of the inequality operator.</param>
    /// <param name="size2">The <see cref="SizeZM" /> on the right side of the inequality operator.</param>
    /// <returns>This operator returns <see langword="true"/> if <paramref name="size1" /> and <paramref name="size2" /> differ either in width or height; <see langword="false"/> if <paramref name="size1" /> and <paramref name="size2" /> are equal.</returns>
    public static bool operator !=(SizeZM size1, SizeZM size2) => !(size1 == size2);

    /// <summary>
    /// Returns the negation of the specified size.
    /// </summary>
    /// <param name="size">The size to negate.</param>
    /// <returns>The negated of <paramref name="size"/>.</returns>
    public static SizeZM operator -(SizeZM size) => size.Negate();

    /// <summary>
    /// Adds the width and height of one <see cref="SizeZM" /> structure to the width and height of another <see cred="Size4D" /> structure.
    /// </summary>
    /// <param name="size1">The first <see cref="SizeZM" /> to add.</param>
    /// <param name="size2">The second <see cref="SizeZM" /> to add.</param>
    /// <returns>A <see cref="SizeZM" /> structure that is the result of the addition operation.</returns>
    public static SizeZM Add(SizeZM size1, SizeZM size2) => new(size1.Width + size2.Width, size1.Height + size2.Height, size1.Depth + size2.Depth, size1.Length + size2.Length);

    /// <summary>
    /// Subtracts the width and height of one <see cref="SizeZM" /> structure from the width and height of another <see cref="SizeZM" /> structure.
    /// </summary>
    /// <param name="size1">The <see cref="SizeZM" /> on the left side of the subtraction operator.</param>
    /// <param name="size2">The <see cref="SizeZM" /> on the right side of the subtraction operator.</param>
    /// <returns>A <see cref="SizeZM" /> structure that is the result of the subtraction operation.</returns>
    public static SizeZM Subtract(SizeZM size1, SizeZM size2) => new(size1.Width - size2.Width, size1.Height - size2.Height, size1.Depth - size2.Depth, size1.Length - size2.Length);

    /// <summary>
    /// Returns the negation of this instance.
    /// </summary>
    /// <returns>The negated of this instance.</returns>
    public readonly SizeZM Negate() => new(-this.Width, -this.Height, -this.Depth, -this.Length);

    /// <inheritdoc/>
    public override readonly bool Equals([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] object? obj) => obj is SizeZM size ? this.Equals(size) : base.Equals(obj);

    /// <inheritdoc/>
    public readonly bool Equals(SizeZM other) => other.Width.Equals(this.Width) && other.Height.Equals(this.Height) && other.Depth.Equals(this.Depth) && other.Length.Equals(this.Length);

    /// <inheritdoc/>
    public override readonly int GetHashCode() =>
#if NETSTANDARD2_0_OR_GREATER || NET461_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        HashCode.Combine(this.Width, this.Height, this.Depth, this.Length);
#else
        (this.Width, this.Height, this.Depth, this.Length).GetHashCode();
#endif

    /// <summary>
    /// Converts a <see cref="SizeZM" /> to a <see cref="PointZM" />.
    /// </summary>
    /// <returns>Returns a <see cref="PointZM" /> structure.</returns>
    public readonly PointZM ToPoint4D() => (PointZM)this;

    /// <summary>
    /// Converts a <see cref="SizeZM" /> to a <see cref="SizeZ" />.
    /// </summary>
    /// <returns>Returns a <see cref="SizeZ" /> structure.</returns>
    public readonly SizeZ ToSize3D() => (SizeZ)this;

    /// <summary>
    /// Converts a <see cref="SizeZM" /> to a <see cref="Size" />.
    /// </summary>
    /// <returns>Returns a <see cref="Size" /> structure.</returns>
    public readonly Size ToSizeD() => (Size)this;

    /// <summary>
    /// Converts a <see cref="SizeZM" /> to a <see cref="double" />.
    /// </summary>
    /// <returns>Returns a <see cref="double" /> value representing the diagonal length of the <see cref="SizeZM" />.</returns>
    public readonly double ToDouble() => System.Math.Sqrt((this.Height * this.Height) + (this.Width * this.Width) + (this.Depth * this.Depth) + (this.Length * this.Length));

    /// <inheritdoc/>
    public override readonly string ToString() => this.ToString(default);

    /// <summary>
    /// Converts the value of this instance to its equivalent string representation.
    /// </summary>
    /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
    /// <returns>A string representation of value of this instance.</returns>
    public readonly string ToString(IFormatProvider? formatProvider) =>
#if NETSTANDARD1_3_OR_GREATER || NET46_OR_GREATER || NETCOREAPP
        ((FormattableString)$"{{Width={this.Width}, Height={this.Height}, Depth={this.Depth}, Length={this.Length}}}").ToString(formatProvider);
#else
        string.Format(formatProvider, "{{Width={0}, Height={1}, Depth={2}, Length={3}}}", this.Width, this.Height, this.Depth, this.Length);
#endif
}