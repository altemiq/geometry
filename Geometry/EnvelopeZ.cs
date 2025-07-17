// -----------------------------------------------------------------------
// <copyright file="EnvelopeZ.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

using static System.Math;

/// <summary>
/// Stores a set of four doubles that represent the location and size of an envelope.
/// </summary>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly struct EnvelopeZ : IEquatable<EnvelopeZ>
{
    /// <summary>
    /// Represents an instance of the <see cref="EnvelopeZ"/> class with its members uninitialized.
    /// </summary>
    public static readonly EnvelopeZ Empty = new(0, 0, 0, 0, 0, 0);

    /// <summary>
    /// Initialises a new instance of the <see cref="EnvelopeZ"/> struct with the specified edge locations.
    /// </summary>
    /// <param name="left">The x-coordinate of the lower-left-front corner of the envelope.</param>
    /// <param name="bottom">The y-coordinate of the lower-right-front corner of the envelope.</param>
    /// <param name="front">The z-coordinate of the lower-left-front corner of the envelope.</param>
    /// <param name="right">The x-coordinate of the upper-right-back corner of the envelope.</param>
    /// <param name="top">The y-coordinate of the upper-right-back corner of the envelope.</param>
    /// <param name="back">The z-coordinate of the upper-right-back corner of the envelope.</param>
    public EnvelopeZ(double left, double bottom, double front, double right, double top, double back) => (this.Left, this.Bottom, this.Front, this.Right, this.Top, this.Back) = (left, bottom, front, right, top, back);

    /// <summary>
    /// Initialises a new instance of the <see cref="EnvelopeZ"/> struct with the specified location and size.
    /// </summary>
    /// <param name="location">A <see cref="PointZ"/> that represents the lower-left corner of the rectangular region.</param>
    /// <param name="size">A <see cref="SizeZ"/> that represents the width, height, and depth of the rectangular region.</param>
    public EnvelopeZ(PointZ location, SizeZ size)
    {
        (this.Left, this.Bottom, this.Front, this.Right, this.Top, this.Back) = (location.X, location.Y, location.Z, location.X + size.Width, location.Y + size.Height, location.Z + size.Depth);
    }

    /// <summary>
    /// Gets the coordinates of the lower-left corner of this <see cref="EnvelopeZ"/> structure.
    /// </summary>
    /// <value>A Point that represents the lower-left, front corner of this <see cref="EnvelopeZ"/> structure.</value>
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
    [System.ComponentModel.Browsable(false)]
#endif
    public PointZ Location => new(this.Left, this.Bottom, this.Z);

    /// <summary>
    /// Gets the size of this <see cref="EnvelopeZ"/>.
    /// </summary>
    /// <value>A Size that represents the width, height, and depth of this <see cref="EnvelopeZ"/> structure.</value>
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
    [System.ComponentModel.Browsable(false)]
#endif
    public SizeZ Size => new(this.Width, this.Height, this.Depth);

    /// <summary>
    /// Gets the x-coordinate of the lower-left corner of this <see cref="EnvelopeZ"/> structure.
    /// </summary>
    /// <value>The x-coordinate of the lower-left corner of this <see cref="EnvelopeZ"/> structure.</value>
    public double X => this.Left;

    /// <summary>
    /// Gets the y-coordinate of the lower-left corner of this <see cref="EnvelopeZ"/> structure.
    /// </summary>
    /// <value>The y-coordinate of the lower-left corner of this <see cref="EnvelopeZ"/> structure.</value>
    public double Y => this.Bottom;

    /// <summary>
    /// Gets the z-coordinate of the lower-left corner of this <see cref="EnvelopeZ"/> structure.
    /// </summary>
    /// <value>The z-coordinate of the lower-left corner of this <see cref="EnvelopeZ"/> structure.</value>
    public double Z => this.Front;

    /// <summary>
    /// Gets the width of this <see cref="EnvelopeZ"/> structure.
    /// </summary>
    /// <value>The width of this <see cref="EnvelopeZ"/> structure.</value>
    public double Width => this.Right - this.Left;

    /// <summary>
    /// Gets the height of this <see cref="EnvelopeZ"/> structure.
    /// </summary>
    /// <value>The height of this <see cref="EnvelopeZ"/> structure.</value>
    public double Height => this.Top - this.Bottom;

    /// <summary>
    /// Gets the depth of this <see cref="EnvelopeZ"/> structure.
    /// </summary>
    /// <value>The depth of this <see cref="EnvelopeZ"/> structure.</value>
    public double Depth => this.Bottom - this.Front;

    /// <summary>
    /// Gets the y-coordinate that is the sum of <see cref="Y"/> and <see cref="Height"/> property values of this <see cref="EnvelopeZ"/> structure.
    /// </summary>
    /// <value>The y-coordinate that is the sum of <see cref="Y"/> and <see cref="Height"/> of this <see cref="EnvelopeZ"/>.</value>
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
    [System.ComponentModel.Browsable(false)]
#endif
    public double Bottom { get; }

    /// <summary>
    /// Gets the x-coordinate of the left edge of this <see cref="EnvelopeZ"/> structure.
    /// </summary>
    /// <value>The x-coordinate of the left edge of this <see cref="EnvelopeZ"/> structure.</value>
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
    [System.ComponentModel.Browsable(false)]
#endif
    public double Left { get; }

    /// <summary>
    /// Gets the z-coordinate of the front edge of this <see cref="EnvelopeZ"/> structure.
    /// </summary>
    /// <value>The z-coordinate of the front edge of this <see cref="EnvelopeZ"/> structure.</value>
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
    [System.ComponentModel.Browsable(false)]
#endif
    public double Front { get; }

    /// <summary>
    /// Gets the y-coordinate of the top edge of this <see cref="EnvelopeZ"/> structure.
    /// </summary>
    /// <value>The y-coordinate of the top edge of this <see cref="EnvelopeZ"/> structure.</value>
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
    [System.ComponentModel.Browsable(false)]
#endif
    public double Top { get; }

    /// <summary>
    /// Gets the x-coordinate that is the sum of <see cref="X"/> and <see cref="Width"/> property values of this <see cref="EnvelopeZ"/> structure.
    /// </summary>
    /// <value>The x-coordinate that is the sum of <see cref="X"/> and <see cref="Width"/> of this <see cref="EnvelopeZ"/>.</value>
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
    [System.ComponentModel.Browsable(false)]
#endif
    public double Right { get; }

    /// <summary>
    /// Gets the z-coordinate that is the sum of <see cref="Z"/> and <see cref="Depth"/> property values of this <see cref="EnvelopeZ"/> structure.
    /// </summary>
    /// <value>The z-coordinate that is the sum of <see cref="Z"/> and <see cref="Depth"/> of this <see cref="EnvelopeZ"/>.</value>
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
    [System.ComponentModel.Browsable(false)]
#endif
    public double Back { get; }

    /// <summary>
    /// Gets a value indicating whether the <see cref="Width" />, <see cref="Height" />, or <see cref="Depth"/> property of this <see cref="EnvelopeZ" /> has a value of zero.
    /// </summary>
    /// <value>This property returns <see langword="true"/> if the <see cref="Width" />, <see cref="Height" />, or <see cref="Depth"/> property of this <see cref="EnvelopeZ" /> has a value of zero; otherwise, <see langword="false"/>.</value>
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
    [System.ComponentModel.Browsable(false)]
#endif
    public bool IsEmpty => (this.Width <= 0D) || (this.Height <= 0D) || (this.Depth <= 0D);

    /// <summary>
    /// Tests whether two <see cref="EnvelopeZ"/> structures have equal location and size.
    /// </summary>
    /// <param name="left">The <see cref="EnvelopeZ"/> structure that is to the left of the equality operator.</param>
    /// <param name="right">The <see cref="EnvelopeZ"/> structure that is to the right of the equality operator.</param>
    /// <returns>This operator returns <see langword="true"/> if the two <see cref="EnvelopeZ"/> structures have equal <see cref="X"/>, <see cref="Y"/> <see cref="Z"/>, <see cref="Width"/>, <see cref="Height"/>, and <see cref="Depth"/> properties.</returns>
    public static bool operator ==(EnvelopeZ left, EnvelopeZ right) => left.Left.Equals(right.Left) && left.Right.Equals(right.Right) && left.Front.Equals(right.Front) && left.Right.Equals(right.Right) && left.Bottom.Equals(right.Bottom) && left.Back.Equals(right.Back);

    /// <summary>
    /// Tests whether two <see cref="EnvelopeZ"/> structures differ in location or size.
    /// </summary>
    /// <param name="left">The <see cref="EnvelopeZ"/> structure that is to the left of the inequality operator.</param>
    /// <param name="right">The <see cref="EnvelopeZ"/> structure that is to the right of the inequality operator.</param>
    /// <returns>This operator returns <see langword="true"/> if any of the <see cref="X"/>, <see cref="Y"/>, <see cref="Z"/>, <see cref="Width"/>,<see cref="Height"/>, or <see cref="Depth"/> properties of the two Rectangles are unequal; otherwise <see langword="false"/>.</returns>
    public static bool operator !=(EnvelopeZ left, EnvelopeZ right) => !(left == right);

    /// <summary>
    /// Initialises a new instance of the <see cref="EnvelopeZ"/> struct with the specified location and size.
    /// </summary>
    /// <param name="x">The x-coordinate of the lower-left-front corner of this <see cref="EnvelopeZ"/> structure.</param>
    /// <param name="y">The y-coordinate of the lower-left-front corner of this <see cref="EnvelopeZ"/> structure.</param>
    /// <param name="z">The z-coordinate of the lower-left-front corner of this <see cref="EnvelopeZ"/> structure.</param>
    /// <param name="width">The width of this <see cref="EnvelopeZ"/> structure.</param>
    /// <param name="height">The height of this <see cref="EnvelopeZ"/> structure.</param>
    /// <param name="depth">The depth of this <see cref="EnvelopeZ"/> structure.</param>
    /// <returns>The new <see cref="EnvelopeZ"/> that this method creates.</returns>
    public static EnvelopeZ FromXYZWHD(double x, double y, double z, double width, double height, double depth) => new(x, y, z, x + width, y + height, z + depth);

    /// <summary>
    /// Creates and returns an inflated copy of the specified <see cref="EnvelopeZ"/> structure. The copy is inflated by the specified amount. The original <see cref="EnvelopeZ"/> structure remains unmodified.
    /// </summary>
    /// <param name="envelope">The <see cref="EnvelopeZ"/> with which to start. This envelope is not modified.</param>
    /// <param name="x">The amount to inflate this <see cref="EnvelopeZ"/> horizontally.</param>
    /// <param name="y">The amount to inflate this <see cref="EnvelopeZ"/> vertically.</param>
    /// <param name="z">The amount to inflate this <see cref="EnvelopeZ"/> int depth.</param>
    /// <returns>The inflated <see cref="EnvelopeZ"/>.</returns>
    public static EnvelopeZ Inflate(EnvelopeZ envelope, double x, double y, double z) => envelope.Inflate(x, y, z);

    /// <summary>
    /// Returns a third <see cref="EnvelopeZ"/> structure that represents the intersection of two other <see cref="EnvelopeZ"/> structures. If there is no intersection, an empty <see cref="EnvelopeZ"/> is returned.
    /// </summary>
    /// <param name="a">The first envelope to intersect.</param>
    /// <param name="b">The second envelope to intersect.</param>
    /// <returns>A <see cref="EnvelopeZ"/> that represents the intersection of <paramref name="a"/> and <paramref name="b"/>.</returns>
    public static EnvelopeZ Intersect(EnvelopeZ a, EnvelopeZ b)
    {
        var x1 = Max(a.Left, b.Left);
        var x2 = Min(a.Right, b.Right);
        var y1 = Max(a.Bottom, b.Bottom);
        var y2 = Min(a.Top, b.Top);
        var z1 = Max(a.Front, b.Front);
        var z2 = Min(a.Back, b.Back);

        return x2 >= x1 && y2 >= y1 && z2 >= z1
            ? new(x1, y1, z1, x2, y2, z2)
            : Empty;
    }

    /// <summary>
    /// Creates the smallest possible third envelope that can contain both of two rectangles that form a union.
    /// </summary>
    /// <param name="a">The first envelope to union.</param>
    /// <param name="b">The second envelope to union.</param>
    /// <returns>A <see cref="EnvelopeZ"/> structure that bounds the union of the two <see cref="EnvelopeZ"/> structures.</returns>
    public static EnvelopeZ Union(EnvelopeZ a, EnvelopeZ b)
    {
        var x1 = Min(a.Left, b.Left);
        var x2 = Max(a.Right, b.Right);
        var y1 = Min(a.Bottom, b.Bottom);
        var y2 = Max(a.Top, b.Top);
        var z1 = Min(a.Front, b.Front);
        var z2 = Max(a.Back, b.Back);

        return new(x1, y1, z1, x2, y2, z2);
    }

    /// <inheritdoc/>
    public override bool Equals([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] object? obj) => obj is EnvelopeZ envelope && this.Equals(envelope);

    /// <inheritdoc/>
    public bool Equals(EnvelopeZ other) => other.Left.Equals(this.Left) && other.Bottom.Equals(this.Bottom) && other.Front.Equals(this.Front) && other.Right.Equals(this.Right) && other.Top.Equals(this.Top) && other.Back.Equals(this.Back);

    /// <summary>
    /// Determines if the specified point is contained within this <see cref="EnvelopeZ"/> structure.
    /// </summary>
    /// <param name="x">The x-coordinate of the point to test.</param>
    /// <param name="y">The y-coordinate of the point to test.</param>
    /// <param name="z">The z-coordinate of the point to test.</param>
    /// <returns>This method returns <see langword="true"/> if the point defined by <paramref name="x"/>, <paramref name="y"/>, and <paramref name="z"/> is contained within this <see cref="EnvelopeZ"/> structure; otherwise, <see langword="false"/>.</returns>
    public bool Contains(double x, double y, double z) =>
        (this.Height > 0 ? this.Bottom <= y && y < this.Top : this.Bottom >= y && y > this.Top)
        && (this.Width > 0 ? this.Left <= x && x < this.Right : this.Left >= x && x > this.Right)
        && (this.Depth > 0 ? this.Front <= z && z < this.Back : this.Front >= z && z > this.Back);

    /// <summary>
    /// Determines if the specified point is contained within this <see cref="EnvelopeZ"/> structure.
    /// </summary>
    /// <param name="point">The <see cref="Point"/> to test.</param>
    /// <returns>This method returns <see langword="true"/> if <paramref name="point"/> is contained within this <see cref="EnvelopeZ"/> structure; otherwise, <see langword="false"/>.</returns>
    public bool Contains(PointZ point) => this.Contains(point.X, point.Y, point.Z);

    /// <summary>
    /// Determines if the rectangular region represented by <paramref name="envelope"/> is entirely contained within this <see cref="EnvelopeZ"/> structure.
    /// </summary>
    /// <param name="envelope">The <see cref="EnvelopeZ"/> to test.</param>
    /// <returns>This method returns <see langword="true"/> if the rectangular region represented by <paramref name="envelope"/> is entirely contained within this <see cref="EnvelopeZ"/> structure; otherwise, <see langword="false"/>.</returns>
    public bool Contains(EnvelopeZ envelope) => this.Left <= envelope.Left && envelope.Right <= this.Right
                                                                           && this.Bottom <= envelope.Bottom && envelope.Top <= this.Top
                                                                           && this.Front <= envelope.Front && envelope.Back <= this.Back;

    /// <inheritdoc/>
    public override int GetHashCode() =>
#if NETSTANDARD2_0_OR_GREATER || NET461_OR_GREATER
        HashCode.Combine(this.Left, this.Bottom, this.Front, this.Right, this.Top, this.Back);
#else
        (this.Left, this.Bottom, this.Front, this.Right, this.Top, this.Back).GetHashCode();
#endif

    /// <summary>
    /// Inflates this <see cref="EnvelopeZ"/> by the specified amount.
    /// </summary>
    /// <param name="width">The amount to inflate this <see cref="EnvelopeZ"/> horizontally.</param>
    /// <param name="height">The amount to inflate this <see cref="EnvelopeZ"/> vertically.</param>
    /// <param name="depth">The amount to inflate this <see cref="EnvelopeZ"/> in depth.</param>
    /// <returns>The inflated envelope.</returns>
    public EnvelopeZ Inflate(double width, double height, double depth) => new(this.Left - width, this.Bottom - height, this.Front - depth, this.Right + width, this.Top + height, this.Back + depth);

    /// <summary>
    /// Inflates this <see cref="EnvelopeZ"/> by the specified amount.
    /// </summary>
    /// <param name="size">The amount to inflate this envelope.</param>
    /// <returns>The inflated envelope.</returns>
    public EnvelopeZ Inflate(SizeZ size) => this.Inflate(size.Width, size.Height, size.Depth);

    /// <summary>
    /// Returns a <see cref="EnvelopeZ"/> with the intersection of this instance and the specified <see cref="EnvelopeZ"/>.
    /// </summary>
    /// <param name="envelope">The <see cref="EnvelopeZ"/> to intersect.</param>
    /// <returns>The inflated envelope.</returns>
    public EnvelopeZ Intersect(EnvelopeZ envelope) => Intersect(envelope, this);

    /// <summary>
    /// Determines if this envelope intersects with <paramref name="rect"/>.
    /// </summary>
    /// <param name="rect">The envelope to test.</param>
    /// <returns>This method returns <see langword="true"/> if there is any intersection, otherwise <see langword="false"/>.</returns>
    public bool IntersectsWith(EnvelopeZ rect) =>
        (rect.Left < this.Right)
        && (this.Left < rect.Right)
        && (rect.Bottom < this.Top)
        && (this.Bottom < rect.Top)
        && (rect.Front < this.Back)
        && (this.Front < rect.Back);

    /// <summary>
    /// Adjusts the location of this envelope by the specified amount.
    /// </summary>
    /// <param name="position">Amount to offset the location.</param>
    /// <returns>The offset envelope.</returns>
    public EnvelopeZ Offset(PointZ position) => this.Offset(position.X, position.Y, position.Z);

    /// <summary>
    /// Adjusts the location of this envelope by the specified amount.
    /// </summary>
    /// <param name="x">The horizontal offset.</param>
    /// <param name="y">The vertical offset.</param>
    /// <param name="z">The depth offset.</param>
    /// <returns>The offset envelope.</returns>
    public EnvelopeZ Offset(double x, double y, double z) => new(this.Left + x, this.Bottom + y, this.Z + z, this.Right + x, this.Top + x, this.Back + z);

    /// <inheritdoc/>
    public override string ToString() => ((FormattableString)$"{{{nameof(this.Left)}={this.Left}, {nameof(this.Bottom)}={this.Bottom}, {nameof(this.Depth)}={this.Depth}, {nameof(this.Right)}={this.Right}, {nameof(this.Top)}={this.Top}, {nameof(this.Back)}={this.Back}}}").ToString(System.Globalization.CultureInfo.CurrentCulture);
}