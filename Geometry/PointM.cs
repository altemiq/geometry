// -----------------------------------------------------------------------
// <copyright file="PointM.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

/// <summary>
/// Represents an ordered pair of floating-point x-, y-coordinates with a measurement that defines a point in a two-dimensional plane.
/// </summary>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly struct PointM :
#if NET7_0_OR_GREATER
    System.Numerics.IAdditionOperators<PointM, SizeM, PointM>,
    System.Numerics.ISubtractionOperators<PointM, SizeM, PointM>,
    System.Numerics.ISubtractionOperators<PointM, PointM, SizeM>,
    System.Numerics.IEqualityOperators<PointM, PointM, bool>,
#endif
    IEquatable<PointM>
{
    /// <summary>
    /// Represents a new instance of the <see cref="PointM" /> class with member data left uninitialized.
    /// </summary>
    public static readonly PointM Empty = new(default, default, DefaultMeasurement);

    private const double DefaultMeasurement = default;

    /// <summary>
    /// Initialises a new instance of the <see cref="PointM" /> struct with the specified coordinates.
    /// </summary>
    /// <param name="x">The horizontal position of the point.</param>
    /// <param name="y">The vertical position of the point.</param>
    /// <param name="measurement">The measurement value of the point.</param>
    public PointM(double x, double y, double measurement) => (this.X, this.Y, this.Measurement) = (x, y, measurement);

    /// <summary>
    /// Initialises a new instance of the <see cref="PointM"/> struct.
    /// </summary>
    /// <param name="point">An instance of <see cref="Point"/> to use to initialise.</param>
    /// <param name="measurement">The measurement value of the point.</param>
    public PointM(Point point, double measurement = DefaultMeasurement) => (this.X, this.Y, this.Measurement) = (point.X, point.Y, measurement);

    /// <summary>
    /// Initialises a new instance of the <see cref="PointM" /> struct with the coordinates contained in the specified array.
    /// </summary>
    /// <param name="coordinates">An array of <see cref="double" /> containing the coordinates.</param>
    /// <param name="startIndex">The index to read the coordinates from.</param>
    /// <remarks><paramref name="coordinates" /> must not be null, and must be at least <paramref name="startIndex" /> + 3 items long.</remarks>
    public PointM(double[] coordinates, int startIndex = default)
    {
        if (startIndex < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(startIndex), string.Format(Properties.Resources.Culture, Properties.Resources.MustBeZeroOrGreater, nameof(startIndex)));
        }

        if (startIndex > int.MaxValue - 2)
        {
            throw new ArgumentOutOfRangeException(nameof(startIndex), string.Format(Properties.Resources.Culture, Properties.Resources.MustBeOrLess, nameof(startIndex), $"{int.MaxValue} - 2"));
        }

        if (startIndex > coordinates.Length - 3)
        {
            throw new ArgumentOutOfRangeException(nameof(startIndex), string.Format(Properties.Resources.Culture, Properties.Resources.MustBeOrLess, nameof(startIndex), $"{coordinates.Length} - 3"));
        }

        System.Diagnostics.Contracts.Contract.EndContractBlock();
        (this.X, this.Y, this.Measurement) = (coordinates[startIndex], coordinates[startIndex + 1], coordinates[startIndex + 2]);
    }

    /// <summary>
    /// Gets a value indicating whether this <see cref="PointM" /> is empty.
    /// </summary>
    public bool IsEmpty => this.X is default(double) && this.Y is default(double) && this.Measurement is default(double);

    /// <summary>
    /// Gets the x-coordinate of this <see cref="PointM" />.
    /// </summary>
    public double X { get; }

    /// <summary>
    /// Gets the y-coordinate of this <see cref="PointM" />.
    /// </summary>
    public double Y { get; }

    /// <summary>
    /// Gets the z-coordinate of this <see cref="PointM" />.
    /// </summary>
    public double Measurement { get; }

    /// <summary>
    /// Converts the specified <see cref="PointM"/> to a <see cref="Point"/>.
    /// </summary>
    /// <param name="point">The <see cref="PointM"/> to convert from.</param>
    public static explicit operator Point(PointM point) => new(point.X, point.Y);

    /// <summary>
    /// Translates a <see cref="PointM" /> by a given <see cref="SizeM" />.
    /// </summary>
    /// <param name="point">The <see cref="PointM" /> to translate.</param>
    /// <param name="size">A <see cref="SizeM" /> the specifies the pair of numbers to add to the coordinates of <paramref name="point" />.</param>
    /// <returns>The translated <see cref="PointM" />.</returns>
    public static PointM operator +(PointM point, SizeM size) => Add(point, size);

    /// <summary>
    /// Translates a <see cref="PointM" /> by the negative of a given <see cref="SizeM" />.
    /// </summary>
    /// <param name="point">The <see cref="PointM" /> to translate.</param>
    /// <param name="size">A <see cref="SizeM" /> the specifies the pair of numbers to subtract from the coordinates of <paramref name="point" />.</param>
    /// <returns>The translated <see cref="PointM" />.</returns>
    public static PointM operator -(PointM point, SizeM size) => Subtract(point, size);

    /// <summary>
    /// Gets the difference between two instance of <see cref="PointM" />.
    /// </summary>
    /// <param name="first">The first <see cref="PointM" />.</param>
    /// <param name="second">The second <see cref="PointM" />.</param>
    /// <returns>An instance of <see cref="SizeM" /> representing the difference between the two points.</returns>
    public static SizeM operator -(PointM first, PointM second) => Subtract(first, second);

    /// <summary>
    /// Compares two <see cref="PointM" /> structures. The result specifies whether the values of the <see cref="X" /> and <see cref="Y" /> properties of the two <see cref="PointM" /> structures are equal.
    /// </summary>
    /// <param name="left">The first <see cref="PointM" /> to compare.</param>
    /// <param name="right">The second <see cref="PointM" /> to compare.</param>
    /// <returns>Returns <see langword="true"/> if the <see cref="X" /> and <see cref="Y" /> values of the left and right <see cref="PointM" /> structures are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(PointM left, PointM right) => left.X.Equals(right.X) && left.Y.Equals(right.Y) && left.Measurement.Equals(right.Measurement);

    /// <summary>
    /// Determines whether the coordinates of the specified points are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="PointM" /> to compare.</param>
    /// <param name="right">The second <see cref="PointM" /> to compare.</param>
    /// <returns>Returns <see langword="true"/> to indicate the <see cref="X" /> and <see cref="Y" /> values of the <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(PointM left, PointM right) => !(left == right);

    /// <summary>
    /// Translates a <see cref="PointM" /> by a given <see cref="SizeM" />.
    /// </summary>
    /// <param name="point">The <see cref="PointM" /> to translate.</param>
    /// <param name="size">A <see cref="SizeM" /> the specifies the pair of numbers to add to the coordinates of <paramref name="point" />.</param>
    /// <returns>The translated <see cref="PointM" />.</returns>
    public static PointM Add(PointM point, SizeM size) => new(point.X + size.Width, point.Y + size.Height, point.Measurement + size.Measurement);

    /// <summary>
    /// Translates a <see cref="PointM" /> by the negative of a given <see cref="SizeM" />.
    /// </summary>
    /// <param name="point">The <see cref="PointM" /> to translate.</param>
    /// <param name="size">A <see cref="SizeM" /> the specifies the pair of numbers to subtract from the coordinates of <paramref name="point" />.</param>
    /// <returns>The translated <see cref="PointM" />.</returns>
    public static PointM Subtract(PointM point, SizeM size) => new(point.X - size.Width, point.Y - size.Height, point.Measurement - size.Measurement);

    /// <summary>
    /// Gets the difference between two instance of <see cref="PointM" />.
    /// </summary>
    /// <param name="first">The first <see cref="PointM" />.</param>
    /// <param name="second">The second <see cref="PointM"/>.</param>
    /// <returns>An instance of <see cref="SizeM" /> representing the difference between the two points.</returns>
    public static SizeM Subtract(PointM first, PointM second) => new(first.X - second.X, first.Y - second.Y, first.Measurement - second.Measurement);

    /// <summary>
    /// Deconstructs this instance into its coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate.</param>
    /// <param name="y">The y-coordinate.</param>
    /// <param name="measurement">The measurement value.</param>
    public void Deconstruct(out double x, out double y, out double measurement) => (x, y, measurement) = (this.X, this.Y, this.Measurement);

    /// <inheritdoc/>
    public override bool Equals([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] object? obj) => obj is PointM point && this.Equals(point);

    /// <inheritdoc/>
    public bool Equals(PointM other) => other.X.Equals(this.X) && other.Y.Equals(this.Y) && other.Measurement.Equals(this.Measurement);

    /// <inheritdoc/>
    public override int GetHashCode() =>
#if NETSTANDARD2_0_OR_GREATER || NET461_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        HashCode.Combine(this.X, this.Y, this.Measurement);
#else
        (this.X, this.Y, this.Z).GetHashCode();
#endif

    /// <summary>
    /// Converts a <see cref="PointM" /> to a <see cref="Point" />.
    /// </summary>
    /// <returns>Returns a <see cref="Point" /> structure.</returns>
    public Point ToPointD() => (Point)this;

    /// <inheritdoc/>
    public override string ToString() => this.ToString(default);

    /// <summary>
    /// Converts the value of this instance to its equivalent string representation.
    /// </summary>
    /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
    /// <returns>A string representation of value of this instance.</returns>
    public string ToString(IFormatProvider? formatProvider) =>
#if NETSTANDARD1_3_OR_GREATER || NET46_OR_GREATER || NETCOREAPP
        ((FormattableString)$"{{X={this.X}, Y={this.Y}, Z={this.Measurement}}}").ToString(formatProvider);
#else
        string.Format(formatProvider, "{{X={0}, Y={1}, Z={2}}}", this.X, this.Y, this.Z);
#endif

    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>A new object that is a copy of this instance.</returns>
    public object Clone() => new PointM(this.X, this.Y, this.Measurement);
}