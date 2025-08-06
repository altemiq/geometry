// -----------------------------------------------------------------------
// <copyright file="Envelope.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

using static System.Math;

/// <summary>
/// Stores a set of four doubles that represent the location and size of an envelope.
/// </summary>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly struct Envelope : IEquatable<Envelope>
{
    /// <summary>
    /// Represents an instance of the <see cref="Envelope"/> class with its members uninitialized.
    /// </summary>
    public static readonly Envelope Empty = new(0, 0, 0, 0);

    /// <summary>
    /// Initialises a new instance of the <see cref="Envelope"/> struct with the specified edge locations.
    /// </summary>
    /// <param name="left">The x-coordinate of the lower-left corner of the envelope.</param>
    /// <param name="bottom">The y-coordinate of the lower-right corner of the envelope.</param>
    /// <param name="right">The x-coordinate of the upper-right corner of the envelope.</param>
    /// <param name="top">The y-coordinate of the lower-left corner of the envelope.</param>
    public Envelope(double left, double bottom, double right, double top) => (this.Left, this.Bottom, this.Right, this.Top) = (left, bottom, right, top);

    /// <summary>
    /// Initialises a new instance of the <see cref="Envelope"/> struct with the specified location and size.
    /// </summary>
    /// <param name="location">A <see cref="Point"/> that represents the lower-left corner of the rectangular region.</param>
    /// <param name="size">A <see cref="Geometry.Size"/> that represents the width and height of the rectangular region.</param>
    public Envelope(Point location, Size size) => (this.Left, this.Bottom, this.Right, this.Top) = (location.X, location.Y, location.X + size.Width, location.Y + size.Height);

    /// <summary>
    /// Gets the coordinates of the lower-left corner of this <see cref="Envelope"/> structure.
    /// </summary>
    /// <value>A Point that represents the lower-left corner of this <see cref="Envelope"/> structure.</value>
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
    [System.ComponentModel.Browsable(false)]
#endif
    public Point Location => new(this.Left, this.Bottom);

    /// <summary>
    /// Gets the size of this <see cref="Envelope"/>.
    /// </summary>
    /// <value>A Size that represents the width and height of this <see cref="Envelope"/> structure.</value>
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
    [System.ComponentModel.Browsable(false)]
#endif
    public Size Size => new(this.Width, this.Height);

    /// <summary>
    /// Gets the x-coordinate of the lower-left corner of this <see cref="Envelope"/> structure.
    /// </summary>
    /// <value>The x-coordinate of the lower-left corner of this <see cref="Envelope"/> structure.</value>
    public double X => this.Left;

    /// <summary>
    /// Gets the y-coordinate of the lower-left corner of this <see cref="Envelope"/> structure.
    /// </summary>
    /// <value>The y-coordinate of the lower-left corner of this <see cref="Envelope"/> structure.</value>
    public double Y => this.Bottom;

    /// <summary>
    /// Gets the width of this <see cref="Envelope"/> structure.
    /// </summary>
    /// <value>The width of this <see cref="Envelope"/> structure.</value>
    public double Width => this.Right - this.Left;

    /// <summary>
    /// Gets the height of this <see cref="Envelope"/> structure.
    /// </summary>
    /// <value>The height of this <see cref="Envelope"/> structure.</value>
    public double Height => this.Top - this.Bottom;

    /// <summary>
    /// Gets the y-coordinate that is the sum of <see cref="Y"/> and <see cref="Height"/> property values of this <see cref="Envelope"/> structure.
    /// </summary>
    /// <value>The y-coordinate that is the sum of <see cref="Y"/> and <see cref="Height"/> of this <see cref="Envelope"/>.</value>
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
    [System.ComponentModel.Browsable(false)]
#endif
    public double Bottom { get; }

    /// <summary>
    /// Gets the x-coordinate of the left edge of this <see cref="Envelope"/> structure.
    /// </summary>
    /// <value>The x-coordinate of the left edge of this <see cref="Envelope"/> structure.</value>
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
    [System.ComponentModel.Browsable(false)]
#endif
    public double Left { get; }

    /// <summary>
    /// Gets the y-coordinate of the top edge of this <see cref="Envelope"/> structure.
    /// </summary>
    /// <value>The y-coordinate of the top edge of this <see cref="Envelope"/> structure.</value>
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
    [System.ComponentModel.Browsable(false)]
#endif
    public double Top { get; }

    /// <summary>
    /// Gets the x-coordinate that is the sum of <see cref="X"/> and <see cref="Width"/> property values of this <see cref="Envelope"/> structure.
    /// </summary>
    /// <value>The x-coordinate that is the sum of <see cref="X"/> and <see cref="Width"/> of this <see cref="Envelope"/>.</value>
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
    [System.ComponentModel.Browsable(false)]
#endif
    public double Right { get; }

    /// <summary>
    /// Gets a value indicating whether the <see cref="Width" /> or <see cref="Height" /> property of this <see cref="Envelope" /> has a value of zero.
    /// </summary>
    /// <value>This property returns <see langword="true"/> if the <see cref="Width" /> or <see cref="Height" /> property of this <see cref="Envelope" /> has a value of zero; otherwise, <see langword="false"/>.</value>
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
    [System.ComponentModel.Browsable(false)]
#endif
    public bool IsEmpty => (this.Width <= 0D) || (this.Height <= 0D);

    /// <summary>
    /// Tests whether two <see cref="Envelope"/> structures have equal location and size.
    /// </summary>
    /// <param name="left">The <see cref="Envelope"/> structure that is to the left of the equality operator.</param>
    /// <param name="right">The <see cref="Envelope"/> structure that is to the right of the equality operator.</param>
    /// <returns>This operator returns <see langword="true"/> if the two <see cref="Envelope"/> structures have equal <see cref="X"/>, <see cref="Y"/>, <see cref="Width"/> and <see cref="Height"/> properties.</returns>
    public static bool operator ==(Envelope left, Envelope right) => left.Left.Equals(right.Left) && left.Right.Equals(right.Right) && left.Bottom.Equals(right.Bottom) && left.Top.Equals(right.Top);

    /// <summary>
    /// Tests whether two <see cref="Envelope"/> structures differ in location or size.
    /// </summary>
    /// <param name="left">The <see cref="Envelope"/> structure that is to the left of the inequality operator.</param>
    /// <param name="right">The <see cref="Envelope"/> structure that is to the right of the inequality operator.</param>
    /// <returns>This operator returns <see langword="true"/> if any of the <see cref="X"/>, <see cref="Y"/>, <see cref="Width"/> or <see cref="Height"/> properties of the two Rectangles are unequal; otherwise <see langword="false"/>.</returns>
    public static bool operator !=(Envelope left, Envelope right) => !(left == right);

    /// <summary>
    /// Creates a <see cref="Envelope"/> structure with the specified location and size.
    /// </summary>
    /// <param name="x">The x-coordinate of the lower-left corner of this <see cref="Envelope"/> structure.</param>
    /// <param name="y">The y-coordinate of the lower-left corner of this <see cref="Envelope"/> structure.</param>
    /// <param name="width">The width of this <see cref="Envelope"/> structure.</param>
    /// <param name="height">The height of this <see cref="Envelope"/> structure.</param>
    /// <returns>The new <see cref="Envelope"/> that this method creates.</returns>
    public static Envelope FromXYWH(double x, double y, double width, double height) => new(x, y, x + width, y + height);

    /// <summary>
    /// Creates and returns an inflated copy of the specified <see cref="Envelope"/> structure. The copy is inflated by the specified amount. The original <see cref="Envelope"/> structure remains unmodified.
    /// </summary>
    /// <param name="rect">The <see cref="Envelope"/> with which to start. This envelope is not modified.</param>
    /// <param name="x">The amount to inflate this <see cref="Envelope"/> horizontally.</param>
    /// <param name="y">The amount to inflate this <see cref="Envelope"/> vertically.</param>
    /// <returns>The inflated <see cref="Envelope"/>.</returns>
    public static Envelope Inflate(Envelope rect, double x, double y) => rect.Inflate(x, y);

    /// <summary>
    /// Returns a third <see cref="Envelope"/> structure that represents the intersection of two other <see cref="Envelope"/> structures. If there is no intersection, an empty <see cref="Envelope"/> is returned.
    /// </summary>
    /// <param name="a">The first envelope to intersect.</param>
    /// <param name="b">The second envelope to intersect.</param>
    /// <returns>A <see cref="Envelope"/> that represents the intersection of <paramref name="a"/> and <paramref name="b"/>.</returns>
    public static Envelope Intersect(Envelope a, Envelope b)
    {
        var x1 = Max(a.Left, b.Left);
        var x2 = Min(a.Right, b.Right);
        var y1 = Max(a.Bottom, b.Bottom);
        var y2 = Min(a.Top, b.Top);

        return x2 >= x1 && y2 >= y1
            ? new(x1, y1, x2, y2)
            : Empty;
    }

    /// <summary>
    /// Creates the smallest possible third envelope that can contain both of two rectangles that form a union.
    /// </summary>
    /// <param name="a">The first envelope to union.</param>
    /// <param name="b">The second envelope to union.</param>
    /// <returns>A <see cref="Envelope"/> structure that bounds the union of the two <see cref="Envelope"/> structures.</returns>
    public static Envelope Union(Envelope a, Envelope b)
    {
        var x1 = Min(a.Left, b.Left);
        var x2 = Max(a.Right, b.Right);
        var y1 = Min(a.Bottom, b.Bottom);
        var y2 = Max(a.Top, b.Top);

        return new(x1, y1, x2, y2);
    }

    /// <inheritdoc/>
    public override bool Equals([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] object? obj) => obj is Envelope envelope && this.Equals(envelope);

    /// <inheritdoc/>
    public bool Equals(Envelope other) => other.Left.Equals(this.Left) && other.Right.Equals(this.Right) && other.Bottom.Equals(this.Bottom) && other.Top.Equals(this.Top);

    /// <summary>
    /// Determines if the specified point is contained within this <see cref="Envelope"/> structure.
    /// </summary>
    /// <param name="x">The x-coordinate of the point to test.</param>
    /// <param name="y">The y-coordinate of the point to test.</param>
    /// <returns>This method returns <see langword="true"/> if the point defined by <paramref name="x"/> and <paramref name="y"/> is contained within this <see cref="Envelope"/> structure; otherwise, <see langword="false"/>.</returns>
    public bool Contains(double x, double y) =>
        (this.Height > 0 ? this.Bottom <= y && y < this.Top : this.Bottom >= y && y > this.Top)
        && (this.Width > 0 ? this.Left <= x && x < this.Right : this.Left >= x && x > this.Right);

    /// <summary>
    /// Determines if the specified point is contained within this <see cref="Envelope"/> structure.
    /// </summary>
    /// <param name="point">The <see cref="Point"/> to test.</param>
    /// <returns>This method returns <see langword="true"/> if <paramref name="point"/> is contained within this <see cref="Envelope"/> structure; otherwise, <see langword="false"/>.</returns>
    public bool Contains(Point point) => this.Contains(point.X, point.Y);

    /// <summary>
    /// Determines if the rectangular region represented by <paramref name="envelope"/> is entirely contained within this <see cref="Envelope"/> structure.
    /// </summary>
    /// <param name="envelope">The <see cref="Envelope"/> to test.</param>
    /// <returns>This method returns <see langword="true"/> if the rectangular region represented by <paramref name="envelope"/> is entirely contained within this <see cref="Envelope"/> structure; otherwise, <see langword="false"/>.</returns>
    public bool Contains(Envelope envelope) => this.Left <= envelope.Left && envelope.Right <= this.Right && this.Bottom >= envelope.Bottom && envelope.Top >= this.Top;

    /// <inheritdoc/>
    public override int GetHashCode() =>
#if NETSTANDARD2_0_OR_GREATER || NET461_OR_GREATER
        HashCode.Combine(this.Left, this.Bottom, this.Right, this.Top);
#else
        (this.Left, this.Bottom, this.Right, this.Top).GetHashCode();
#endif

    /// <summary>
    /// Inflates this <see cref="Envelope"/> by the specified amount.
    /// </summary>
    /// <param name="width">The amount to inflate this <see cref="Envelope"/> horizontally.</param>
    /// <param name="height">The amount to inflate this <see cref="Envelope"/> vertically.</param>
    /// <returns>The inflated envelope.</returns>
    public Envelope Inflate(double width, double height) => new(this.Left - width, this.Bottom - height, this.Right + width, this.Top + height);

    /// <summary>
    /// Inflates this <see cref="Envelope"/> by the specified amount.
    /// </summary>
    /// <param name="size">The amount to inflate this envelope.</param>
    /// <returns>The inflated envelope.</returns>
    public Envelope Inflate(Size size) => this.Inflate(size.Width, size.Height);

    /// <summary>
    /// Returns a <see cref="Envelope"/> with the intersection of this instance and the specified <see cref="Envelope"/>.
    /// </summary>
    /// <param name="envelope">The <see cref="Envelope"/> to intersect.</param>
    /// <returns>The inflated envelope.</returns>
    public Envelope Intersect(Envelope envelope) => Intersect(envelope, this);

    /// <summary>
    /// Determines if this envelope intersects with <paramref name="rect"/>.
    /// </summary>
    /// <param name="rect">The envelope to test.</param>
    /// <returns>This method returns <see langword="true"/> if there is any intersection, otherwise <see langword="false"/>.</returns>
    public bool IntersectsWith(Envelope rect) => (rect.Left < this.Right) && (this.Left < rect.Right) && (rect.Bottom < this.Top) && (this.Bottom < rect.Top);

    /// <summary>
    /// Adjusts the location of this envelope by the specified amount.
    /// </summary>
    /// <param name="position">Amount to offset the location.</param>
    /// <returns>The offset envelope.</returns>
    public Envelope Offset(Point position) => this.Offset(position.X, position.Y);

    /// <summary>
    /// Adjusts the location of this envelope by the specified amount.
    /// </summary>
    /// <param name="x">The horizontal offset.</param>
    /// <param name="y">The vertical offset.</param>
    /// <returns>The offset envelope.</returns>
    public Envelope Offset(double x, double y) => new(this.Left + x, this.Bottom + y, this.Right + x, this.Top + y);

    /// <inheritdoc/>
    public override string ToString() => ((FormattableString)$"{{{nameof(this.Left)}={this.Left}, {nameof(this.Bottom)}={this.Bottom}, {nameof(this.Right)}={this.Right}, {nameof(this.Top)}={this.Top}}}").ToString(System.Globalization.CultureInfo.CurrentCulture);
}