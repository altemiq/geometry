// -----------------------------------------------------------------------
// <copyright file="Point.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

/// <summary>
/// Represents an ordered pair of floating-point x- and y-coordinates that defines a point in a two-dimensional plane.
/// </summary>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly struct Point :
#if NET7_0_OR_GREATER
    System.Numerics.IAdditionOperators<Point, Size, Point>,
    System.Numerics.ISubtractionOperators<Point, Size, Point>,
    System.Numerics.ISubtractionOperators<Point, Point, Size>,
    System.Numerics.IEqualityOperators<Point, Point, bool>,
#endif
    IEquatable<Point>
{
    /// <summary>
    /// Represents a new instance of the <see cref="Point" /> class with member data left uninitialized.
    /// </summary>
    public static readonly Point Empty;

    /// <summary>
    /// Initialises a new instance of the <see cref="Point" /> struct with the specified coordinates.
    /// </summary>
    /// <param name="x">The horizontal position of the point.</param>
    /// <param name="y">The vertical position of the point.</param>
    public Point(double x, double y) => (this.X, this.Y) = (x, y);

    /// <summary>
    /// Initialises a new instance of the <see cref="Point" /> struct with the coordinates contained in the specified array.
    /// </summary>
    /// <param name="coordinates">An array of <see cref="double" /> containing the coordinates.</param>
    /// <param name="startIndex">The index to read the coordinates from.</param>
    /// <remarks><paramref name="coordinates" /> must not be null, and must be at least <paramref name="startIndex" /> + 2 items long.</remarks>
    public Point(double[] coordinates, int startIndex = default)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(coordinates);
#else
        if (coordinates is null)
        {
            throw new ArgumentNullException(nameof(coordinates));
        }
#endif

        if (startIndex < 0 || startIndex > int.MaxValue - 1 || startIndex > coordinates.Length - 2)
        {
            throw new ArgumentOutOfRangeException(nameof(startIndex));
        }

        System.Diagnostics.Contracts.Contract.EndContractBlock();
        (this.X, this.Y) = (coordinates[startIndex], coordinates[startIndex + 1]);
    }

    /// <summary>
    /// Gets a value indicating whether this <see cref="Point" /> is empty.
    /// </summary>
    public bool IsEmpty => this.X is default(double) && this.Y is default(double);

    /// <summary>
    /// Gets the x-coordinate of this <see cref="Point" />.
    /// </summary>
    public double X { get; }

    /// <summary>
    /// Gets the y-coordinate of this <see cref="Point" />.
    /// </summary>
    public double Y { get; }

    /// <summary>
    /// Translates a <see cref="Point" /> by a given <see cref="Size" />.
    /// </summary>
    /// <param name="point">The <see cref="Point" /> to translate.</param>
    /// <param name="size">A <see cref="Size" /> the specifies the pair of numbers to add to the coordinates of <paramref name="point" />.</param>
    /// <returns>The translated <see cref="Point" />.</returns>
    public static Point operator +(Point point, Size size) => Add(point, size);

    /// <summary>
    /// Translates a <see cref="Point" /> by the negative of a given <see cref="Size" />.
    /// </summary>
    /// <param name="point">The <see cref="Point" /> to translate.</param>
    /// <param name="size">A <see cref="Size" /> the specifies the pair of numbers to subtract from the coordinates of <paramref name="point" />.</param>
    /// <returns>The translated <see cref="Point" />.</returns>
    public static Point operator -(Point point, Size size) => Subtract(point, size);

    /// <summary>
    /// Gets the difference between two instance of <see cref="Point" />.
    /// </summary>
    /// <param name="first">The first <see cref="Point" />.</param>
    /// <param name="second">The second <see cref="Point" />.</param>
    /// <returns>An instance of <see cref="Size" /> representing the difference between the two points.</returns>
    public static Size operator -(Point first, Point second) => Subtract(first, second);

    /// <summary>
    /// Compares two <see cref="Point" /> structures. The result specifies whether the values of the <see cref="X" /> and <see cref="Y" /> properties of the two <see cref="Point" /> structures are equal.
    /// </summary>
    /// <param name="left">The first <see cref="Point" /> to compare.</param>
    /// <param name="right">The second <see cref="Point" /> to compare.</param>
    /// <returns>Returns <see langword="true"/> if the <see cref="X" /> and <see cref="Y" /> values of the left and right <see cref="Point" /> structures are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Point left, Point right) => left.X.Equals(right.X) && left.Y.Equals(right.Y);

    /// <summary>
    /// Determines whether the coordinates of the specified points are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="Point" /> to compare.</param>
    /// <param name="right">The second <see cref="Point" /> to compare.</param>
    /// <returns>Returns <see langword="true"/> to indicate the <see cref="X" /> and <see cref="Y" /> values of the <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Point left, Point right) => !(left == right);

    /// <summary>
    /// Translates a <see cref="Point" /> by a given <see cref="Size" />.
    /// </summary>
    /// <param name="point">The <see cref="Point" /> to translate.</param>
    /// <param name="size">A <see cref="Size" /> the specifies the pair of numbers to add to the coordinates of <paramref name="point" />.</param>
    /// <returns>The translated <see cref="Point" />.</returns>
    public static Point Add(Point point, Size size) => new(point.X + size.Width, point.Y + size.Height);

    /// <summary>
    /// Translates a <see cref="Point" /> by the negative of a given <see cref="Size" />.
    /// </summary>
    /// <param name="point">The <see cref="Point" /> to translate.</param>
    /// <param name="size">A <see cref="Size" /> the specifies the pair of numbers to subtract from the coordinates of <paramref name="point" />.</param>
    /// <returns>The translated <see cref="Point" />.</returns>
    public static Point Subtract(Point point, Size size) => new(point.X - size.Width, point.Y - size.Height);

    /// <summary>
    /// Gets the difference between two instance of <see cref="Point" />.
    /// </summary>
    /// <param name="first">The first <see cref="Point" />.</param>
    /// <param name="second">The second <see cref="Point" />.</param>
    /// <returns>An instance of <see cref="Size" /> representing the difference between the two points.</returns>
    public static Size Subtract(Point first, Point second) => new(first.X - second.X, first.Y - second.Y);

    /// <summary>
    /// Deconstructs this instance into its coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate.</param>
    /// <param name="y">The y-coordinate.</param>
    public void Deconstruct(out double x, out double y) => (x, y) = (this.X, this.Y);

    /// <inheritdoc/>
    public override bool Equals([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] object? obj) => obj is Point point && this.Equals(point);

    /// <inheritdoc/>
    public bool Equals(Point other) => other.X.Equals(this.X) && other.Y.Equals(this.Y);

    /// <inheritdoc/>
    public override int GetHashCode() =>
#if NETSTANDARD2_0_OR_GREATER || NET461_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        HashCode.Combine(this.X, this.Y);
#else
        (this.X, this.Y).GetHashCode();
#endif

    /// <inheritdoc/>
    public override string ToString() => this.ToString(default);

    /// <summary>
    /// Converts the value of this instance to its equivalent string representation.
    /// </summary>
    /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
    /// <returns>A string representation of value of this instance.</returns>
    public string ToString(IFormatProvider? formatProvider) =>
#if NETSTANDARD1_3_OR_GREATER || NET46_OR_GREATER || NETCOREAPP
        ((FormattableString)$"{{X={this.X}, Y={this.Y}}}").ToString(formatProvider);
#else
        string.Format(formatProvider, "{{X={0}, Y={1}}}", this.X, this.Y);
#endif

    /// <summary>
    /// Returns the distance from this instance to the specified point.
    /// </summary>
    /// <param name="point">The <see cref="Point" /> to get the distance to.</param>
    /// <returns>The distance between the two points.</returns>
    public double To(Point point)
    {
        var deltaX = this.X - point.X;
        var deltaY = this.Y - point.Y;
        return Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY));
    }

    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>A new object that is a copy of this instance.</returns>
    public object Clone() => new Point(this.X, this.Y);
}