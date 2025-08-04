// -----------------------------------------------------------------------
// <copyright file="Size.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

/// <summary>
/// Stores an ordered pair of floating-point numbers, typically the width and height of a rectangle.
/// </summary>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly struct Size : IEquatable<Size>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="Size" /> class.
    /// </summary>
    public static readonly Size Empty;

    /// <summary>
    /// Initialises a new instance of the <see cref="Size" /> struct from the specified existing <see cref="Size" />.
    /// </summary>
    /// <param name="size">The <see cref="Size" /> from which to create the new <see cref="Size" />.</param>
    public Size(Size size)
        : this(size.Width, size.Height)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="Size" /> struct from the specified <see cref="Point" />.
    /// </summary>
    /// <param name="point">The <see cref="Point" /> from which to initialize this <see cref="Size" />.</param>
    public Size(Point point) => (this.Width, this.Height) = (point.X, point.Y);

    /// <summary>
    /// Initialises a new instance of the <see cref="Size" /> struct from the specified dimensions.
    /// </summary>
    /// <param name="width">The width component of the new <see cref="Size" />.</param>
    /// <param name="height">The height component of the new <see cref="Size" />.</param>
    public Size(double width, double height) => (this.Width, this.Height) = (width, height);

    /// <summary>
    /// Gets a value indicating whether this <see cref="Size" /> has zero width and height.
    /// </summary>
    /// <value>This property returns <see langword="true"/> when this <see cref="Size" /> has both a width and height of zero; otherwise, <see langword="false"/>.</value>
    public bool IsEmpty => this.Width is default(double) && this.Height is default(double);

    /// <summary>
    /// Gets the horizontal component of this <see cref="Size" />.
    /// </summary>
    public double Width { get; }

    /// <summary>
    /// Gets the vertical component of this <see cref="Size" />.
    /// </summary>
    public double Height { get; }

    /// <summary>
    /// Converts the specified <see cref="Size" /> to an <see cref="Point" />.
    /// </summary>
    /// <param name="size">The <see cref="Size" /> to convert from.</param>
    public static explicit operator Point(Size size) => new(size.Width, size.Height);

    /// <summary>
    /// Adds the width and height of one <see cref="Size" /> structure to the width and height of another <see cref="Size" /> structure.
    /// </summary>
    /// <param name="size1">The first <see cref="Size" /> to add.</param>
    /// <param name="size2">The second <see cref="Size" /> to add.</param>
    /// <returns>A <see cref="Size" /> structure that is the result of the addition operation.</returns>
    public static Size operator +(Size size1, Size size2) => Add(size1, size2);

    /// <summary>
    /// Subtracts the width and height of one <see cref="Size" /> structure from the width and height of another <see cref="Size" /> structure.
    /// </summary>
    /// <param name="size1">The <see cref="Size" /> on the left side of the subtraction operator.</param>
    /// <param name="size2">The <see cref="Size" /> on the right side of the subtraction operator.</param>
    /// <returns>A <see cref="Size" /> structure that is the result of the subtraction operation.</returns>
    public static Size operator -(Size size1, Size size2) => Subtract(size1, size2);

    /// <summary>
    /// Tests whether two <see cref="Size" /> objects are equal.
    /// </summary>
    /// <param name="size1">The <see cref="Size" /> on the left side of the equality operator.</param>
    /// <param name="size2">The <see cref="Size" /> on the right side of the equality operator.</param>
    /// <returns>This operator returns <see langword="true"/> if <paramref name="size1" /> and <paramref name="size2" /> have equal width and height; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Size size1, Size size2) => size1.Width.Equals(size2.Width) && size1.Height.Equals(size2.Height);

    /// <summary>
    /// Tests whether two <see cref="Size" /> structures are different.
    /// </summary>
    /// <param name="size1">The <see cref="Size" /> on the left side of the inequality operator.</param>
    /// <param name="size2">The <see cref="Size" /> on the right side of the inequality operator.</param>
    /// <returns>This operator returns <see langword="true"/> if <paramref name="size1" /> and <paramref name="size2" /> differ either in width or height; <see langword="false"/> if <paramref name="size1" /> and <paramref name="size2" /> are equal.</returns>
    public static bool operator !=(Size size1, Size size2) => !(size1 == size2);

    /// <summary>
    /// Returns the negation of the specified size.
    /// </summary>
    /// <param name="size">The size to negate.</param>
    /// <returns>The negated of <paramref name="size"/>.</returns>
    public static Size operator -(Size size) => size.Negate();

    /// <summary>
    /// Adds the width and height of one <see cref="Size" /> structure to the width and height of another <see cred="SizeD" /> structure.
    /// </summary>
    /// <param name="size1">The first <see cref="Size" /> to add.</param>
    /// <param name="size2">The second <see cref="Size" /> to add.</param>
    /// <returns>A <see cref="Size" /> structure that is the result of the addition operation.</returns>
    public static Size Add(Size size1, Size size2) => new(size1.Width + size2.Width, size1.Height + size2.Height);

    /// <summary>
    /// Subtracts the width and height of one <see cref="Size" /> structure from the width and height of another <see cref="Size" /> structure.
    /// </summary>
    /// <param name="size1">The <see cref="Size" /> on the left side of the subtraction operator.</param>
    /// <param name="size2">The <see cref="Size" /> on the right side of the subtraction operator.</param>
    /// <returns>A <see cref="Size" /> structure that is the result of the subtraction operation.</returns>
    public static Size Subtract(Size size1, Size size2) => new(size1.Width - size2.Width, size1.Height - size2.Height);

    /// <summary>
    /// Returns the negation of this instance.
    /// </summary>
    /// <returns>The negated of this instance.</returns>
    public Size Negate() => new(-this.Width, -this.Height);

    /// <inheritdoc/>
    public override bool Equals([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] object? obj) => obj is Size size ? this.Equals(size) : base.Equals(obj);

    /// <inheritdoc/>
    public bool Equals(Size other) => other.Width.Equals(this.Width) && other.Height.Equals(this.Height);

    /// <inheritdoc/>
    public override int GetHashCode() =>
#if NETSTANDARD2_0_OR_GREATER || NET461_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        HashCode.Combine(this.Width, this.Height);
#else
        (this.Width, this.Height).GetHashCode();
#endif

    /// <summary>
    /// Converts a <see cref="Size" /> to an <see cref="Point" />.
    /// </summary>
    /// <returns>Returns an <see cref="Point" />.</returns>
    public Point ToPointD() => (Point)this;

    /// <summary>
    /// Converts a <see cref="Size" /> to a <see cref="double" />.
    /// </summary>
    /// <returns>Returns a <see cref="double" /> value representing the diagonal length of the <see cref="Size" />.</returns>
    public double ToDouble() => Math.Sqrt((this.Height * this.Height) + (this.Width * this.Width));

    /// <inheritdoc/>
    public override string ToString() => this.ToString(default);

    /// <summary>
    /// Converts the value of this instance to its equivalent string representation.
    /// </summary>
    /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
    /// <returns>A string representation of value of this instance.</returns>
    public string ToString(IFormatProvider? formatProvider) =>
#if NETSTANDARD1_3_OR_GREATER || NET46_OR_GREATER || NETCOREAPP
        ((FormattableString)$"{{Width={this.Width}, Height={this.Height}}}").ToString(formatProvider);
#else
        string.Format(formatProvider, "{{Width={0}, Height={1}}}", this.Width, this.Height);
#endif
}