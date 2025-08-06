// -----------------------------------------------------------------------
// <copyright file="EnvelopeZM.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

using static System.Math;

/// <summary>
/// Stores a set of four doubles that represent the location and size of an envelope.
/// </summary>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly struct EnvelopeZM : IEquatable<EnvelopeZM>
{
    /// <summary>
    /// Represents an instance of the <see cref="EnvelopeZM"/> class with its members uninitialized.
    /// </summary>
    public static readonly EnvelopeZM Empty = new(0, 0, 0, 0, 0, 0, 0, 0);

    /// <summary>
    /// Initialises a new instance of the <see cref="EnvelopeZM"/> struct with the specified edge locations.
    /// </summary>
    /// <param name="left">The x-coordinate of the lower-left-front-start corner of the envelope.</param>
    /// <param name="bottom">The y-coordinate of the lower-right-front-start corner of the envelope.</param>
    /// <param name="front">The z-coordinate of the lower-left-front-start corner of the envelope.</param>
    /// <param name="start">The measurement-coordinate of the lower-left-front-start corner of the envelope.</param>
    /// <param name="right">The x-coordinate of the upper-right-back-end corner of the envelope.</param>
    /// <param name="top">The y-coordinate of the upper-right-back-end corner of the envelope.</param>
    /// <param name="back">The z-coordinate of the upper-right-back-end corner of the envelope.</param>
    /// <param name="end">The measurement-coordinate of the upper-right-back-end corner of the envelope.</param>
    public EnvelopeZM(double left, double bottom, double front, double start, double right, double top, double back, double end) => (this.Left, this.Bottom, this.Front, this.Start, this.Right, this.Top, this.Back, this.End) = (left, bottom, front, start, right, top, back, end);

    /// <summary>
    /// Initialises a new instance of the <see cref="EnvelopeZM"/> struct with the specified location and size.
    /// </summary>
    /// <param name="location">A <see cref="PointZM"/> that represents the lower-left corner of the rectangular region.</param>
    /// <param name="size">A <see cref="SizeZ"/> that represents the width, height, and depth of the rectangular region.</param>
    /// <param name="length">The length of the rectangular region.</param>
    public EnvelopeZM(PointZM location, SizeZ size, double length) => (this.Left, this.Bottom, this.Front, this.Start, this.Right, this.Top, this.Back, this.End) = (location.X, location.Y, location.Z, location.Measurement, location.X + size.Width, location.Y + size.Height, location.Z + size.Depth, location.Measurement + length);

    /// <summary>
    /// Initialises a new instance of the <see cref="EnvelopeZM"/> struct with the specified location and size.
    /// </summary>
    /// <param name="location">A <see cref="PointZM"/> that represents the lower-left corner of the rectangular region.</param>
    /// <param name="size">A <see cref="SizeZM"/> that represents the width, height, depth, and length of the rectangular region.</param>
    public EnvelopeZM(PointZM location, SizeZM size) => (this.Left, this.Bottom, this.Front, this.Start, this.Right, this.Top, this.Back, this.End) = (location.X, location.Y, location.Z, location.Measurement, location.X + size.Width, location.Y + size.Height, location.Z + size.Depth, location.Measurement + size.Length);

    /// <summary>
    /// Gets the coordinates of the lower-left corner of this <see cref="EnvelopeZM"/> structure.
    /// </summary>
    /// <value>A Point that represents the lower-left, front corner of this <see cref="EnvelopeZM"/> structure.</value>
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
    [System.ComponentModel.Browsable(false)]
#endif
    public PointZM Location => new(this.X, this.Y, this.Z, this.Measurement);

    /// <summary>
    /// Gets the size of this <see cref="EnvelopeZM"/>.
    /// </summary>
    /// <value>A Size that represents the width, height, depth, and length. of this <see cref="EnvelopeZM"/> structure.</value>
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
    [System.ComponentModel.Browsable(false)]
#endif
    public SizeZM Size => new(this.Width, this.Height, this.Depth, this.Length);

    /// <summary>
    /// Gets the x-coordinate of the lower-left-front-start corner of this <see cref="EnvelopeZM"/> structure.
    /// </summary>
    /// <value>The x-coordinate of the lower-left-front-start corner of this <see cref="EnvelopeZM"/> structure.</value>
    public double X => this.Left;

    /// <summary>
    /// Gets the y-coordinate of the lower-left-front-start corner of this <see cref="EnvelopeZM"/> structure.
    /// </summary>
    /// <value>The y-coordinate of the lower-left-front-start corner of this <see cref="EnvelopeZM"/> structure.</value>
    public double Y => this.Bottom;

    /// <summary>
    /// Gets the z-coordinate of the lower-left-front-start corner of this <see cref="EnvelopeZM"/> structure.
    /// </summary>
    /// <value>The z-coordinate of the lower-left-front-start corner of this <see cref="EnvelopeZM"/> structure.</value>
    public double Z => this.Front;

    /// <summary>
    /// Gets the measurement-coordinate of the lower-left-front-start corner of this <see cref="EnvelopeZM"/> structure.
    /// </summary>
    /// <value>The measurement-coordinate of the lower-left-front-start corner of this <see cref="EnvelopeZM"/> structure.</value>
    public double Measurement => this.Start;

    /// <summary>
    /// Gets the width of this <see cref="EnvelopeZM"/> structure.
    /// </summary>
    /// <value>The width of this <see cref="EnvelopeZM"/> structure.</value>
    public double Width => this.Right - this.Left;

    /// <summary>
    /// Gets the height of this <see cref="EnvelopeZM"/> structure.
    /// </summary>
    /// <value>The height of this <see cref="EnvelopeZM"/> structure.</value>
    public double Height => this.Top - this.Bottom;

    /// <summary>
    /// Gets the depth of this <see cref="EnvelopeZM"/> structure.
    /// </summary>
    /// <value>The depth of this <see cref="EnvelopeZM"/> structure.</value>
    public double Depth => this.Back - this.Front;

    /// <summary>
    /// Gets the length of this <see cref="EnvelopeZM"/> structure.
    /// </summary>
    /// <value>The length of this <see cref="EnvelopeZM"/> structure.</value>
    public double Length => this.End - this.Start;

    /// <summary>
    /// Gets the y-coordinate that is the sum of <see cref="Y"/> and <see cref="Height"/> property values of this <see cref="EnvelopeZM"/> structure.
    /// </summary>
    /// <value>The y-coordinate that is the sum of <see cref="Y"/> and <see cref="Height"/> of this <see cref="EnvelopeZM"/>.</value>
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
    [System.ComponentModel.Browsable(false)]
#endif
    public double Bottom { get; }

    /// <summary>
    /// Gets the x-coordinate of the left edge of this <see cref="EnvelopeZM"/> structure.
    /// </summary>
    /// <value>The x-coordinate of the left edge of this <see cref="EnvelopeZM"/> structure.</value>
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
    [System.ComponentModel.Browsable(false)]
#endif
    public double Left { get; }

    /// <summary>
    /// Gets the z-coordinate of the front edge of this <see cref="EnvelopeZM"/> structure.
    /// </summary>
    /// <value>The z-coordinate of the front edge of this <see cref="EnvelopeZM"/> structure.</value>
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
    [System.ComponentModel.Browsable(false)]
#endif
    public double Front { get; }

    /// <summary>
    /// Gets the start-coordinate of the starting edge of this <see cref="EnvelopeZM"/> structure.
    /// </summary>
    /// <value>The start-coordinate of the starting edge of this <see cref="EnvelopeZM"/> structure.</value>
    public double Start { get; }

    /// <summary>
    /// Gets the y-coordinate of the top edge of this <see cref="EnvelopeZM"/> structure.
    /// </summary>
    /// <value>The y-coordinate of the top edge of this <see cref="EnvelopeZM"/> structure.</value>
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
    [System.ComponentModel.Browsable(false)]
#endif
    public double Top { get; }

    /// <summary>
    /// Gets the x-coordinate that is the sum of <see cref="X"/> and <see cref="Width"/> property values of this <see cref="EnvelopeZM"/> structure.
    /// </summary>
    /// <value>The x-coordinate that is the sum of <see cref="X"/> and <see cref="Width"/> of this <see cref="EnvelopeZM"/>.</value>
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
    [System.ComponentModel.Browsable(false)]
#endif
    public double Right { get; }

    /// <summary>
    /// Gets the z-coordinate that is the sum of <see cref="Z"/> and <see cref="Depth"/> property values of this <see cref="EnvelopeZM"/> structure.
    /// </summary>
    /// <value>The z-coordinate that is the sum of <see cref="Z"/> and <see cref="Depth"/> of this <see cref="EnvelopeZM"/>.</value>
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
    [System.ComponentModel.Browsable(false)]
#endif
    public double Back { get; }

    /// <summary>
    /// Gets the measurement-coordinate that is the sum of <see cref="Measurement"/> and <see cref="Length"/> property values of this <see cref="EnvelopeZM"/> structure.
    /// </summary>
    /// <value>The measurement-coordinate that is the sum of <see cref="Measurement"/> and <see cref="Length"/> of this <see cref="EnvelopeZM"/>.</value>
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
    [System.ComponentModel.Browsable(false)]
#endif
    public double End { get; }

    /// <summary>
    /// Gets a value indicating whether the <see cref="Width" />, <see cref="Height" />, <see cref="Depth"/>, or <see cref="Length"/> property of this <see cref="EnvelopeZM" /> has a value of zero.
    /// </summary>
    /// <value>This property returns <see langword="true"/> if the <see cref="Width" />, <see cref="Height" />, <see cref="Depth"/>, or <see cref="Length"/> property of this <see cref="EnvelopeZ" /> has a value of zero; otherwise, <see langword="false"/>.</value>
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
    [System.ComponentModel.Browsable(false)]
#endif
    public bool IsEmpty => (this.Width <= 0D) || (this.Height <= 0D) || (this.Depth <= 0D) || (this.Length <= 0D);

    /// <summary>
    /// Tests whether two <see cref="EnvelopeZM"/> structures have equal location and size.
    /// </summary>
    /// <param name="left">The <see cref="EnvelopeZM"/> structure that is to the left of the equality operator.</param>
    /// <param name="right">The <see cref="EnvelopeZM"/> structure that is to the right of the equality operator.</param>
    /// <returns>This operator returns <see langword="true"/> if the two <see cref="EnvelopeZM"/> structures have equal <see cref="X"/>, <see cref="Y"/>, <see cref="Z"/>, <see cref="Measurement"/> <see cref="Width"/>, <see cref="Height"/>, <see cref="Depth"/>, and <see cref="Length"/> properties.</returns>
    public static bool operator ==(EnvelopeZM left, EnvelopeZM right) => left.X.Equals(right.X) && left.Y.Equals(right.Y) && left.Z.Equals(right.Z) && left.Measurement.Equals(right.Measurement) && left.Width.Equals(right.Width) && left.Height.Equals(right.Height) && left.Depth.Equals(right.Depth) && left.Length.Equals(right.Length);

    /// <summary>
    /// Tests whether two <see cref="EnvelopeZM"/> structures differ in location or size.
    /// </summary>
    /// <param name="left">The <see cref="EnvelopeZM"/> structure that is to the left of the inequality operator.</param>
    /// <param name="right">The <see cref="EnvelopeZM"/> structure that is to the right of the inequality operator.</param>
    /// <returns>This operator returns <see langword="true"/> if any of the <see cref="X"/>, <see cref="Y"/>, <see cref="Z"/>, <see cref="Measurement"/> <see cref="Width"/>, <see cref="Height"/>, <see cref="Depth"/>, or <see cref="Length"/> properties of the two Rectangles are unequal; otherwise <see langword="false"/>.</returns>
    public static bool operator !=(EnvelopeZM left, EnvelopeZM right) => !(left == right);

    /// <summary>
    /// Initialises a new instance of the <see cref="EnvelopeZM"/> struct with the specified location and size.
    /// </summary>
    /// <param name="x">The x-coordinate of the lower-left-front-start corner of this <see cref="EnvelopeZM"/> structure.</param>
    /// <param name="y">The y-coordinate of the lower-left-front-start corner of this <see cref="EnvelopeZM"/> structure.</param>
    /// <param name="z">The z-coordinate of the lower-left-front-start corner of this <see cref="EnvelopeZM"/> structure.</param>
    /// <param name="m">The m-coordinate of the lower-left-front-start corner of this <see cref="EnvelopeZM"/> structure.</param>
    /// <param name="width">The width of this <see cref="EnvelopeZM"/> structure.</param>
    /// <param name="height">The height of this <see cref="EnvelopeZM"/> structure.</param>
    /// <param name="depth">The depth of this <see cref="EnvelopeZM"/> structure.</param>
    /// <param name="length">The length of this <see cref="EnvelopeZM"/> structure.</param>
    /// <returns>The new <see cref="EnvelopeZM"/> that this method creates.</returns>
    public static EnvelopeZM FromXYZMWHDL(double x, double y, double z, double m, double width, double height, double depth, double length) => new(x, y, z, m, x + width, y + height, z + depth, m + length);

    /// <summary>
    /// Creates and returns an inflated copy of the specified <see cref="EnvelopeZM"/> structure. The copy is inflated by the specified amount. The original <see cref="EnvelopeZM"/> structure remains unmodified.
    /// </summary>
    /// <param name="envelope">The <see cref="EnvelopeZM"/> with which to start. This envelope is not modified.</param>
    /// <param name="x">The amount to inflate this <see cref="EnvelopeZM"/> horizontally.</param>
    /// <param name="y">The amount to inflate this <see cref="EnvelopeZM"/> vertically.</param>
    /// <param name="z">The amount to inflate this <see cref="EnvelopeZM"/> in depth.</param>
    /// <param name="m">The amount to inflate this <see cref="EnvelopeZM"/> in length.</param>
    /// <returns>The inflated <see cref="EnvelopeZM"/>.</returns>
    public static EnvelopeZM Inflate(EnvelopeZM envelope, double x, double y, double z, double m) => envelope.Inflate(x, y, z, m);

    /// <summary>
    /// Returns a third <see cref="EnvelopeZM"/> structure that represents the intersection of two other <see cref="EnvelopeZM"/> structures. If there is no intersection, an empty <see cref="EnvelopeZM"/> is returned.
    /// </summary>
    /// <param name="a">The first envelope to intersect.</param>
    /// <param name="b">The second envelope to intersect.</param>
    /// <returns>A <see cref="EnvelopeZM"/> that represents the intersection of <paramref name="a"/> and <paramref name="b"/>.</returns>
    public static EnvelopeZM Intersect(EnvelopeZM a, EnvelopeZM b)
    {
        var x1 = Max(a.Left, b.Left);
        var x2 = Min(a.Right, b.Right);
        var y1 = Max(a.Bottom, b.Bottom);
        var y2 = Min(a.Top, b.Top);
        var z1 = Max(a.Front, b.Front);
        var z2 = Min(a.Back, b.Back);
        var m1 = Max(a.Start, b.Start);
        var m2 = Min(a.End, b.End);

        return x2 >= x1 && y2 >= y1 && z2 >= z1 && m2 >= m1
            ? new(x1, y1, z1, m1, x2, y2, z2, m2)
            : Empty;
    }

    /// <summary>
    /// Creates the smallest possible third envelope that can contain both of two rectangles that form a union.
    /// </summary>
    /// <param name="a">The first envelope to union.</param>
    /// <param name="b">The second envelope to union.</param>
    /// <returns>A <see cref="EnvelopeZM"/> structure that bounds the union of the two <see cref="EnvelopeZM"/> structures.</returns>
    public static EnvelopeZM Union(EnvelopeZM a, EnvelopeZM b)
    {
        var x1 = Min(a.Left, b.Left);
        var x2 = Max(a.Right, b.Right);
        var y1 = Min(a.Bottom, b.Bottom);
        var y2 = Max(a.Top, b.Top);
        var z1 = Min(a.Front, b.Front);
        var z2 = Max(a.Back, b.Back);
        var m1 = Min(a.Start, b.Start);
        var m2 = Max(a.End, b.End);

        return new(x1, y1, z1, m1, x2, y2, z2, m2);
    }

    /// <inheritdoc/>
    public override bool Equals([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] object? obj) => obj is EnvelopeZM envelope && this.Equals(envelope);

    /// <inheritdoc/>
    public bool Equals(EnvelopeZM other) => other.Left.Equals(this.Left) && other.Bottom.Equals(this.Bottom) && other.Front.Equals(this.Front) && other.Start.Equals(this.Start) && other.Right.Equals(this.Right) && other.Top.Equals(this.Top) && other.Back.Equals(this.Back) && other.End.Equals(this.End);

    /// <summary>
    /// Determines if the specified point is contained within this <see cref="EnvelopeZM"/> structure.
    /// </summary>
    /// <param name="x">The x-coordinate of the point to test.</param>
    /// <param name="y">The y-coordinate of the point to test.</param>
    /// <param name="z">The z-coordinate of the point to test.</param>
    /// <param name="m">The m-coordinate of the point to test.</param>
    /// <returns>This method returns <see langword="true"/> if the point defined by <paramref name="x"/>, <paramref name="y"/>, and <paramref name="z"/> is contained within this <see cref="EnvelopeZM"/> structure; otherwise, <see langword="false"/>.</returns>
    public bool Contains(double x, double y, double z, double m) =>
        (this.Height > 0 ? this.Bottom <= y && y < this.Top : this.Bottom >= y && y > this.Top)
        && (this.Width > 0 ? this.Left <= x && x < this.Right : this.Left >= x && x > this.Right)
        && (this.Depth > 0 ? this.Front <= z && z < this.Back : this.Front >= z && z > this.Back)
        && (this.Length > 0 ? this.Start <= m && m < this.End : this.Start >= m && m > this.End);

    /// <summary>
    /// Determines if the specified point is contained within this <see cref="EnvelopeZM"/> structure.
    /// </summary>
    /// <param name="point">The <see cref="Point"/> to test.</param>
    /// <returns>This method returns <see langword="true"/> if <paramref name="point"/> is contained within this <see cref="EnvelopeZM"/> structure; otherwise, <see langword="false"/>.</returns>
    public bool Contains(PointZM point) => this.Contains(point.X, point.Y, point.Z, point.Measurement);

    /// <summary>
    /// Determines if the rectangular region represented by <paramref name="envelope"/> is entirely contained within this <see cref="EnvelopeZM"/> structure.
    /// </summary>
    /// <param name="envelope">The <see cref="EnvelopeZM"/> to test.</param>
    /// <returns>This method returns <see langword="true"/> if the rectangular region represented by <paramref name="envelope"/> is entirely contained within this <see cref="EnvelopeZM"/> structure; otherwise, <see langword="false"/>.</returns>
    public bool Contains(EnvelopeZM envelope) => this.Left <= envelope.Left && envelope.Right <= this.Right
                                                                           && this.Bottom <= envelope.Bottom && envelope.Top <= this.Top
                                                                           && this.Front <= envelope.Z && envelope.Back <= this.Back
                                                                           && this.Start <= envelope.Start && envelope.End <= this.End;

    /// <inheritdoc/>
    public override int GetHashCode() =>
#if NETSTANDARD2_0_OR_GREATER || NET461_OR_GREATER
        HashCode.Combine(this.Left, this.Bottom, this.Front, this.Start, this.Right, this.Top, this.Back, this.End);
#else
        (this.Left, this.Bottom, this.Front, this.Start, this.Right, this.Top, this.Back, this.End).GetHashCode();
#endif

    /// <summary>
    /// Inflates this <see cref="EnvelopeZM"/> by the specified amount.
    /// </summary>
    /// <param name="width">The amount to inflate this <see cref="EnvelopeZM"/> horizontally.</param>
    /// <param name="height">The amount to inflate this <see cref="EnvelopeZM"/> vertically.</param>
    /// <param name="depth">The amount to inflate this <see cref="EnvelopeZM"/> in depth.</param>
    /// <param name="length">The amount to inflate this <see cref="EnvelopeZM"/> in length.</param>
    /// <returns>The inflated envelope.</returns>
    public EnvelopeZM Inflate(double width, double height, double depth, double length) => new(this.Left - width, this.Bottom - height, this.Front - depth, this.Start - length, this.Right + width, this.Top + height, this.Back + depth, this.End + length);

    /// <summary>
    /// Inflates this <see cref="EnvelopeZM"/> by the specified amount.
    /// </summary>
    /// <param name="size">The amount to inflate this envelope.</param>
    /// <param name="length">The amount to inflate this <see cref="EnvelopeZM"/> in length.</param>
    /// <returns>The inflated envelope.</returns>
    public EnvelopeZM Inflate(SizeZ size, double length) => this.Inflate(size.Width, size.Height, size.Depth, length);

    /// <summary>
    /// Inflates this <see cref="EnvelopeZM"/> by the specified amount.
    /// </summary>
    /// <param name="size">The amount to inflate this envelope.</param>
    /// <returns>The inflated envelope.</returns>
    public EnvelopeZM Inflate(SizeZM size) => this.Inflate(size.Width, size.Height, size.Depth, size.Length);

    /// <summary>
    /// Returns a <see cref="EnvelopeZM"/> with the intersection of this instance and the specified <see cref="EnvelopeZM"/>.
    /// </summary>
    /// <param name="envelope">The <see cref="EnvelopeZM"/> to intersect.</param>
    /// <returns>The inflated envelope.</returns>
    public EnvelopeZM Intersect(EnvelopeZM envelope) => Intersect(envelope, this);

    /// <summary>
    /// Determines if this envelope intersects with <paramref name="rect"/>.
    /// </summary>
    /// <param name="rect">The envelope to test.</param>
    /// <returns>This method returns <see langword="true"/> if there is any intersection, otherwise <see langword="false"/>.</returns>
    public bool IntersectsWith(EnvelopeZM rect) =>
        (rect.Left < this.Right)
        && (this.Left < rect.Right)
        && (rect.Bottom < this.Top)
        && (this.Bottom < rect.Top)
        && (rect.Front < this.Back)
        && (this.Front < rect.Back)
        && (rect.Start < this.End)
        && (this.Start < rect.End);

    /// <summary>
    /// Adjusts the location of this envelope by the specified amount.
    /// </summary>
    /// <param name="position">Amount to offset the location.</param>
    /// <returns>The offset envelope.</returns>
    public EnvelopeZM Offset(PointZM position) => this.Offset(position.X, position.Y, position.Z, position.Measurement);

    /// <summary>
    /// Adjusts the location of this envelope by the specified amount.
    /// </summary>
    /// <param name="x">The horizontal offset.</param>
    /// <param name="y">The vertical offset.</param>
    /// <param name="z">The depth offset.</param>
    /// <param name="m">The length offset.</param>
    /// <returns>The offset envelope.</returns>
    public EnvelopeZM Offset(double x, double y, double z, double m) => new(this.Left + x, this.Bottom + y, this.Front + z, this.Start + m, this.Right + x, this.Top + z, this.Back + z, this.End + m);

    /// <inheritdoc/>
    public override string ToString() => ((FormattableString)$"{{{nameof(this.Left)}={this.Left}, {nameof(this.Bottom)}={this.Bottom}, {nameof(this.Front)}={this.Front}, {nameof(this.Start)}={this.Start}, {nameof(this.Right)}={this.Right}, {nameof(this.Top)}={this.Top}, {nameof(this.Back)}={this.Back}, {nameof(this.End)}={this.End}}}").ToString(System.Globalization.CultureInfo.CurrentCulture);
}