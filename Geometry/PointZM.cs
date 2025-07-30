// -----------------------------------------------------------------------
// <copyright file="PointZM.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

/// <summary>
/// Represents an ordered pair of floating-point x-, y- and z-coordinates with a measurement that defines a point in a three-dimensional plane.
/// </summary>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly struct PointZM :
#if USE_GENERIC_MATH
    System.Numerics.IAdditionOperators<PointZM, SizeZM, PointZM>,
    System.Numerics.ISubtractionOperators<PointZM, SizeZM, PointZM>,
    System.Numerics.ISubtractionOperators<PointZM, Size3D, PointZM>,
    System.Numerics.ISubtractionOperators<PointZM, PointZM, SizeZM>,
    System.Numerics.IEqualityOperators<PointZM, PointZM, bool>,
#endif
    IEquatable<PointZM>
{
    /// <summary>
    /// Represents a new instance of the <see cref="PointZM" /> class with member data left uninitialized.
    /// </summary>
    public static readonly PointZM Empty = new(default, default, DefaultZ, DefaultMeasurement);

    private const double DefaultZ = default;

    private const double DefaultMeasurement = default;

    /// <summary>
    /// Initialises a new instance of the <see cref="PointZM" /> struct with the specified coordinates.
    /// </summary>
    /// <param name="x">The horizontal position of the point.</param>
    /// <param name="y">The vertical position of the point.</param>
    /// <param name="z">The depth position of the point.</param>
    /// <param name="measurement">The measurement value of the point.</param>
    public PointZM(double x, double y, double z, double measurement) => (this.X, this.Y, this.Z, this.Measurement) = (x, y, z, measurement);

    /// <summary>
    /// Initialises a new instance of the <see cref="PointZM"/> struct.
    /// </summary>
    /// <param name="point">The point D.</param>
    /// <param name="z">The z-coordinate.</param>
    /// <param name="measurement">The measurement.</param>
    public PointZM(Point point, double z = DefaultZ, double measurement = DefaultMeasurement) => (this.X, this.Y, this.Z, this.Measurement) = (point.X, point.Y, z, measurement);

    /// <summary>
    /// Initialises a new instance of the <see cref="PointZM"/> struct.
    /// </summary>
    /// <param name="pointZ">The point 3D.</param>
    /// <param name="measurement">The measurement.</param>
    public PointZM(PointZ pointZ, double measurement = DefaultMeasurement) => (this.X, this.Y, this.Z, this.Measurement) = (pointZ.X, pointZ.Y, pointZ.Z, measurement);

    /// <summary>
    /// Initialises a new instance of the <see cref="PointZM" /> struct with the coordinates contained in the specified array.
    /// </summary>
    /// <param name="coordinates">An array of <see cref="double" /> containing the coordinates.</param>
    /// <param name="startIndex">The index to read the coordinates from.</param>
    /// <remarks><paramref name="coordinates" /> must not be null, and must be at least <paramref name="startIndex" /> + 3 items long.</remarks>
    public PointZM(double[] coordinates, int startIndex = default)
    {
        if (startIndex < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(startIndex), string.Format(Properties.Resources.Culture, Properties.Resources.MustBeZeroOrGreater, nameof(startIndex)));
        }

        if (startIndex > int.MaxValue - 3)
        {
            throw new ArgumentOutOfRangeException(nameof(startIndex), string.Format(Properties.Resources.Culture, Properties.Resources.MustBeOrLess, nameof(startIndex), $"{int.MaxValue} - 3"));
        }

        if (startIndex > coordinates.Length - 4)
        {
            throw new ArgumentOutOfRangeException(nameof(startIndex), string.Format(Properties.Resources.Culture, Properties.Resources.MustBeOrLess, nameof(startIndex), $"{coordinates.Length} - 4"));
        }

        System.Diagnostics.Contracts.Contract.EndContractBlock();
        (this.X, this.Y, this.Z, this.Measurement) = (coordinates[startIndex], coordinates[startIndex + 1], coordinates[startIndex + 2], coordinates[startIndex + 3]);
    }

    /// <summary>
    /// Gets a value indicating whether this <see cref="PointZM" /> is empty.
    /// </summary>
    public bool IsEmpty => this.X is default(double) && this.Y is default(double) && this.Z is default(double) && this.Measurement is default(double);

    /// <summary>
    /// Gets the x-coordinate of this <see cref="PointZM" />.
    /// </summary>
    public double X { get; }

    /// <summary>
    /// Gets the y-coordinate of this <see cref="PointZM" />.
    /// </summary>
    public double Y { get; }

    /// <summary>
    /// Gets the z-coordinate of this <see cref="PointZM" />.
    /// </summary>
    public double Z { get; }

    /// <summary>
    /// Gets the measurement-component of this <see cref="PointZM" />.
    /// </summary>
    public double Measurement { get; }

    /// <summary>
    /// Converts the specified <see cref='PointZM'/> to a <see cref='Point'/>.
    /// </summary>
    /// <param name="point">The <see cref="PointZM" /> to convert from.</param>
    public static explicit operator Point(PointZM point) => new(point.X, point.Y);

    /// <summary>
    /// Converts the specified <see cref='PointZM'/> to a <see cref='PointZ'/>.
    /// </summary>
    /// <param name="point">The <see cref="PointZM" /> to convert from.</param>
    public static explicit operator PointZ(PointZM point) => new(point.X, point.Y, point.Z);

    /// <summary>
    /// Translates a <see cref="PointZM" /> by a given <see cref="SizeZ" />.
    /// </summary>
    /// <param name="point">The <see cref="PointZM" /> to translate.</param>
    /// <param name="size">A <see cref="SizeZ" /> the specifies the pair of numbers to add to the coordinates of <paramref name="point" />.</param>
    /// <returns>The translated <see cref="PointZM" />.</returns>
    public static PointZM operator +(PointZM point, SizeZ size) => Add(point, size);

    /// <summary>
    /// Translates a <see cref="PointZM" /> by a given <see cref="SizeZM" />.
    /// </summary>
    /// <param name="point">The <see cref="PointZM" /> to translate.</param>
    /// <param name="size">A <see cref="SizeZM" /> the specifies the pair of numbers to add to the coordinates of <paramref name="point" />.</param>
    /// <returns>The translated <see cref="PointZM" />.</returns>
    public static PointZM operator +(PointZM point, SizeZM size) => Add(point, size);

    /// <summary>
    /// Translates a <see cref="PointZM" /> by the negative of a given <see cref="SizeZ" />.
    /// </summary>
    /// <param name="point">The <see cref="PointZM" /> to translate.</param>
    /// <param name="size">A <see cref="SizeZ" /> the specifies the pair of numbers to subtract from the coordinates of <paramref name="point" />.</param>
    /// <returns>The translated <see cref="PointZM" />.</returns>
    public static PointZM operator -(PointZM point, SizeZ size) => Subtract(point, size);

    /// <summary>
    /// Translates a <see cref="PointZM" /> by the negative of a given <see cref="SizeZM" />.
    /// </summary>
    /// <param name="point">The <see cref="PointZM" /> to translate.</param>
    /// <param name="size">A <see cref="SizeZM" /> the specifies the pair of numbers to subtract from the coordinates of <paramref name="point" />.</param>
    /// <returns>The translated <see cref="PointZM" />.</returns>
    public static PointZM operator -(PointZM point, SizeZM size) => Subtract(point, size);

    /// <summary>
    /// Translates a <see cref="PointZM" /> by the negative of a given <see cref="SizeZM" />.
    /// </summary>
    /// <param name="first">The first <see cref="PointZM" />.</param>
    /// <param name="second">The second <see cref="PointZM" />.</param>
    /// <returns>An instance of <see cref="SizeZM" /> representing the difference between the two points.</returns>
    public static SizeZM operator -(PointZM first, PointZM second) => Subtract(first, second);

    /// <summary>
    /// Compares two <see cref="PointZM" /> structures. The result specifies whether the values of the <see cref="X" /> and <see cref="Y" /> properties of the two <see cref="PointZM" /> structures are equal.
    /// </summary>
    /// <param name="left">The first <see cref="PointZM" /> to compare.</param>
    /// <param name="right">The second <see cref="PointZM" /> to compare.</param>
    /// <returns>Returns <see langword="true"/> if the <see cref="X" /> and <see cref="Y" /> values of the left and right <see cref="PointZM" /> structures are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(PointZM left, PointZM right) => left.X.Equals(right.X) && left.Y.Equals(right.Y) && left.Z.Equals(right.Z) && left.Measurement.Equals(right.Measurement);

    /// <summary>
    /// Determines whether the coordinates of the specified points are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="PointZM" /> to compare.</param>
    /// <param name="right">The second <see cref="PointZM" /> to compare.</param>
    /// <returns>Returns <see langword="true"/> to indicate the <see cref="X" /> and <see cref="Y" /> values of the <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(PointZM left, PointZM right) => !(left == right);

    /// <summary>
    /// Translates a <see cref="PointZM" /> by a given <see cref="SizeZ" />.
    /// </summary>
    /// <param name="point">The <see cref="PointZM" /> to translate.</param>
    /// <param name="size">A <see cref="SizeZ" /> the specifies the pair of numbers to add to the coordinates of <paramref name="point" />.</param>
    /// <returns>The translated <see cref="PointZM" />.</returns>
    public static PointZM Add(PointZM point, SizeZ size) => new(point.X + size.Width, point.Y + size.Height, point.Z + size.Depth, point.Measurement);

    /// <summary>
    /// Translates a <see cref="PointZM" /> by a given <see cref="SizeZM" />.
    /// </summary>
    /// <param name="point">The <see cref="PointZM" /> to translate.</param>
    /// <param name="size">A <see cref="SizeZM" /> the specifies the pair of numbers to add to the coordinates of <paramref name="point" />.</param>
    /// <returns>The translated <see cref="PointZM" />.</returns>
    public static PointZM Add(PointZM point, SizeZM size) => new(point.X + size.Width, point.Y + size.Height, point.Z + size.Depth, point.Measurement + size.Length);

    /// <summary>
    /// Translates a <see cref="PointZM" /> by the negative of a given <see cref="SizeZ" />.
    /// </summary>
    /// <param name="point">The <see cref="PointZM" /> to translate.</param>
    /// <param name="size">A <see cref="SizeZ" /> the specifies the pair of numbers to subtract from the coordinates of <paramref name="point" />.</param>
    /// <returns>The translated <see cref="PointZM" />.</returns>
    public static PointZM Subtract(PointZM point, SizeZ size) => new(point.X - size.Width, point.Y - size.Height, point.Z - size.Depth, point.Measurement);

    /// <summary>
    /// Translates a <see cref="PointZM" /> by the negative of a given <see cref="SizeZM" />.
    /// </summary>
    /// <param name="point">The <see cref="PointZM" /> to translate.</param>
    /// <param name="size">A <see cref="SizeZM" /> the specifies the pair of numbers to subtract from the coordinates of <paramref name="point" />.</param>
    /// <returns>The translated <see cref="PointZM" />.</returns>
    public static PointZM Subtract(PointZM point, SizeZM size) => new(point.X - size.Width, point.Y - size.Height, point.Z - size.Depth, point.Measurement - size.Length);

    /// <summary>
    /// Gets the difference between two instance of <see cref="PointZM" />.
    /// </summary>
    /// <param name="first">The first <see cref="PointZM" />.</param>
    /// <param name="second">The second <see cref="PointZM"/>.</param>
    /// <returns>An instance of <see cref="SizeZM" /> representing the difference between the two points.</returns>
    public static SizeZM Subtract(PointZM first, PointZM second) => new(first.X - second.X, first.Y - second.Y, first.Z - second.Z, first.Measurement - second.Measurement);

    /// <summary>
    /// Deconstructs this instance into its coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate.</param>
    /// <param name="y">The y-coordinate.</param>
    /// <param name="z">The z-coordinate.</param>
    /// <param name="m">The m-coordinate.</param>
    public void Deconstruct(out double x, out double y, out double z, out double m) => (x, y, z, m) = (this.X, this.Y, this.Z, this.Measurement);

    /// <inheritdoc/>
    public override bool Equals([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] object? obj) => obj is PointZM pointZM ? this.Equals(pointZM) : base.Equals(obj);

    /// <inheritdoc/>
    public bool Equals(PointZM other) => other.X.Equals(this.X) && other.Y.Equals(this.Y) && other.Z.Equals(this.Z) && other.Measurement.Equals(this.Measurement);

    /// <inheritdoc/>
    public override int GetHashCode() =>
#if NETSTANDARD2_0_OR_GREATER || NET461_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        HashCode.Combine(this.X, this.Y, this.Z, this.Measurement);
#else
        (this.X, this.Y, this.Z, this.Measurement).GetHashCode();
#endif

    /// <summary>
    /// Converts a <see cref="PointZM" /> to a <see cref="Point" />.
    /// </summary>
    /// <returns>Returns a <see cref="Point" /> structure.</returns>
    public Point ToPointD() => (Point)this;

    /// <summary>
    /// Converts a <see cref="PointZM" /> to a <see cref="PointZ" />.
    /// </summary>
    /// <returns>Returns a <see cref="PointZ" /> structure.</returns>
    public PointZ ToPoint3D() => (PointZ)this;

    /// <inheritdoc/>
    public override string ToString() => this.ToString(default);

    /// <summary>
    /// Converts the value of this instance to its equivalent string representation.
    /// </summary>
    /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
    /// <returns>A string representation of value of this instance.</returns>
    public string ToString(IFormatProvider? formatProvider) =>
#if NETSTANDARD1_3_OR_GREATER || NET46_OR_GREATER || NETCOREAPP
        ((FormattableString)$"{{X={this.X}, Y={this.Y}, Z={this.Z}, Measurement={this.Measurement}}}").ToString(formatProvider);
#else
        string.Format(formatProvider, "{{X={0}, Y={1}, Z={2}, Measurement={3}}}", this.X, this.Y, this.Z, this.Measurement);
#endif

    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>A new object that is a copy of this instance.</returns>
    public object Clone() => new PointZM(this.X, this.Y, this.Z, this.Measurement);
}