// -----------------------------------------------------------------------
// <copyright file="SizeZ.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

/// <summary>
/// Stores an ordered tuple of floating-point numbers, typically the width, height and depth of a cube.
/// </summary>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public struct SizeZ : IEquatable<SizeZ>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="SizeZ" /> class.
    /// </summary>
    public static readonly SizeZ Empty;

    /// <summary>
    /// Initialises a new instance of the <see cref="SizeZ" /> struct from the specified existing <see cref="SizeZ" />.
    /// </summary>
    /// <param name="size">The <see cref="SizeZ" /> from which to create the new <see cref="SizeZ" />.</param>
    public SizeZ(SizeZ size)
        : this(size.Width, size.Height, size.Depth)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="SizeZ" /> struct from the specified <see cref="PointZ" />.
    /// </summary>
    /// <param name="point">The <see cref="PointZ" /> from which to initialize this <see cref="SizeZ" />.</param>
    public SizeZ(PointZ point) => (this.Width, this.Height, this.Depth) = (point.X, point.Y, point.Z);

    /// <summary>
    /// Initialises a new instance of the <see cref="SizeZ" /> struct from the specified dimensions.
    /// </summary>
    /// <param name="width">The width component of the new <see cref="SizeZ" />.</param>
    /// <param name="height">The height component of the new <see cref="SizeZ" />.</param>
    /// <param name="depth">The depth component of the new <see cref="SizeZ" />.</param>
    public SizeZ(double width, double height, double depth) => (this.Width, this.Height, this.Depth) = (width, height, depth);

    /// <summary>
    /// Gets a value indicating whether this <see cref="SizeZ" /> has zero width and height.
    /// </summary>
    /// <value>This property returns <see langword="true"/> when this <see cref="SizeZ" /> has both a width and height of zero; otherwise, <see langword="false"/>.</value>
    public readonly bool IsEmpty => this.Width is default(double) && this.Height is default(double) && this.Depth is default(double);

    /// <summary>
    /// Gets or sets the horizontal component of this <see cref="SizeZ" />.
    /// </summary>
    /// <value>The horizontal component of this <see cref="SizeZ" />.</value>
    public double Width { get; set; }

    /// <summary>
    /// Gets or sets the vertical component of this <see cref="SizeZ" />.
    /// </summary>
    /// <value>The vertical component of this <see cref="SizeZ" />.</value>
    public double Height { get; set; }

    /// <summary>
    /// Gets or sets the depth component of this <see cref="SizeZ" />.
    /// </summary>
    /// <value>The depth component of this <see cref="SizeZ" />.</value>
    public double Depth { get; set; }

    /// <summary>
    /// Converts the specified <see cref="SizeZ" /> to a <see cref="PointZ" />.
    /// </summary>
    /// <param name="size">The <see cref="SizeZ" /> to convert from.</param>
    public static explicit operator PointZ(SizeZ size) => new(size.Width, size.Height, size.Depth);

    /// <summary>
    /// Converts the specified <see cref="SizeZ" /> to a <see cref="Size" />.
    /// </summary>
    /// <param name="size">The <see cref="SizeZ" /> to convert from.</param>
    public static explicit operator Size(SizeZ size) => new(size.Width, size.Height);

    /// <summary>
    /// Adds the width and height of one <see cref="SizeZ" /> structure to the width and height of another <see cref="SizeZ" /> structure.
    /// </summary>
    /// <param name="size1">The first <see cref="SizeZ" /> to add.</param>
    /// <param name="size2">The second <see cref="SizeZ" /> to add.</param>
    /// <returns>A <see cref="SizeZ" /> structure that is the result of the addition operation.</returns>
    public static SizeZ operator +(SizeZ size1, SizeZ size2) => Add(size1, size2);

    /// <summary>
    /// Subtracts the width and height of one <see cref="SizeZ" /> structure from the width and height of another <see cref="SizeZ" /> structure.
    /// </summary>
    /// <param name="size1">The <see cref="SizeZ" /> on the left side of the subtraction operator.</param>
    /// <param name="size2">The <see cref="SizeZ" /> on the right side of the subtraction operator.</param>
    /// <returns>A <see cref="SizeZ" /> structure that is the result of the subtraction operation.</returns>
    public static SizeZ operator -(SizeZ size1, SizeZ size2) => Subtract(size1, size2);

    /// <summary>
    /// Tests whether two <see cref="SizeZ" /> objects are equal.
    /// </summary>
    /// <param name="size1">The <see cref="SizeZ" /> on the left side of the equality operator.</param>
    /// <param name="size2">The <see cref="SizeZ" /> on the right side of the equality operator.</param>
    /// <returns>This operator returns <see langword="true"/> if <paramref name="size1" /> and <paramref name="size2" /> have equal width and height; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(SizeZ size1, SizeZ size2) => size1.Width.Equals(size2.Width) && size1.Height.Equals(size2.Height) && size1.Depth.Equals(size2.Depth);

    /// <summary>
    /// Tests whether two <see cref="SizeZ" /> structures are different.
    /// </summary>
    /// <param name="size1">The <see cref="SizeZ" /> on the left side of the inequality operator.</param>
    /// <param name="size2">The <see cref="SizeZ" /> on the right side of the inequality operator.</param>
    /// <returns>This operator returns <see langword="true"/> if <paramref name="size1" /> and <paramref name="size2" /> differ either in width or height; <see langword="false"/> if <paramref name="size1" /> and <paramref name="size2" /> are equal.</returns>
    public static bool operator !=(SizeZ size1, SizeZ size2) => !(size1 == size2);

    /// <summary>
    /// Returns the negation of the specified size.
    /// </summary>
    /// <param name="size">The size to negate.</param>
    /// <returns>The negated of <paramref name="size"/>.</returns>
    public static SizeZ operator -(SizeZ size) => size.Negate();

    /// <summary>
    /// Adds the width and height of one <see cref="SizeZ" /> structure to the width and height of another <see cred="Size3D" /> structure.
    /// </summary>
    /// <param name="size1">The first <see cref="SizeZ" /> to add.</param>
    /// <param name="size2">The second <see cref="SizeZ" /> to add.</param>
    /// <returns>A <see cref="SizeZ" /> structure that is the result of the addition operation.</returns>
    public static SizeZ Add(SizeZ size1, SizeZ size2) => new(size1.Width + size2.Width, size1.Height + size2.Height, size1.Depth + size2.Depth);

    /// <summary>
    /// Subtracts the width and height of one <see cref="SizeZ" /> structure from the width and height of another <see cref="SizeZ" /> structure.
    /// </summary>
    /// <param name="size1">The <see cref="SizeZ" /> on the left side of the subtraction operator.</param>
    /// <param name="size2">The <see cref="SizeZ" /> on the right side of the subtraction operator.</param>
    /// <returns>A <see cref="SizeZ" /> structure that is the result of the subtraction operation.</returns>
    public static SizeZ Subtract(SizeZ size1, SizeZ size2) => new(size1.Width - size2.Width, size1.Height - size2.Height, size1.Depth - size2.Depth);

    /// <summary>
    /// Returns the negation of this instance.
    /// </summary>
    /// <returns>The negated of this instance.</returns>
    public readonly SizeZ Negate() => new(-this.Width, -this.Height, -this.Depth);

    /// <inheritdoc/>
    public override readonly bool Equals([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] object? obj) => obj is SizeZ size ? this.Equals(size) : base.Equals(obj);

    /// <inheritdoc/>
    public readonly bool Equals(SizeZ other) => other.Width.Equals(this.Width) && other.Height.Equals(this.Height) && other.Depth.Equals(this.Depth);

    /// <inheritdoc/>
    public override readonly int GetHashCode() =>
#if NETSTANDARD2_0_OR_GREATER || NET461_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        HashCode.Combine(this.Width, this.Height, this.Depth);
#else
        (this.Width, this.Height, this.Depth).GetHashCode();
#endif

    /// <summary>
    /// Converts a <see cref="SizeZ" /> to a <see cref="PointZ" />.
    /// </summary>
    /// <returns>Returns a <see cref="PointZ" /> structure.</returns>
    public readonly PointZ ToPoint3D() => (PointZ)this;

    /// <summary>
    /// Converts a <see cref="SizeZ" /> to a <see cref="Size" />.
    /// </summary>
    /// <returns>Returns a <see cref="Size" /> structure.</returns>
    public readonly Size ToSizeD() => (Size)this;

    /// <summary>
    /// Converts a <see cref="SizeZ" /> to a <see cref="double" />.
    /// </summary>
    /// <returns>Returns a <see cref="double" /> value representing the diagonal length of the <see cref="SizeZ" />.</returns>
    public readonly double ToDouble() => System.Math.Sqrt((this.Height * this.Height) + (this.Width * this.Width) + (this.Depth * this.Depth));

    /// <inheritdoc/>
    public override readonly string ToString() => this.ToString(default);

    /// <summary>
    /// Converts the value of this instance to its equivalent string representation.
    /// </summary>
    /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
    /// <returns>A string representation of value of this instance.</returns>
    public readonly string ToString(IFormatProvider? formatProvider) =>
#if NETSTANDARD1_3_OR_GREATER || NET46_OR_GREATER || NETCOREAPP
        ((FormattableString)$"{{Width={this.Width}, Height={this.Height}, Depth={this.Depth}}}").ToString(formatProvider);
#else
        string.Format(formatProvider, "{{Width={0}, Height={1}, Depth={2}}}", this.Width, this.Height, this.Depth);
#endif
}