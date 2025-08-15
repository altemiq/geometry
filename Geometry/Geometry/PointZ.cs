// -----------------------------------------------------------------------
// <copyright file="PointZ.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

/// <summary>
/// Represents an ordered pair of floating-point x-, y- and z-coordinates that defines a point in a three-dimensional plane.
/// </summary>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly struct PointZ :
    IGeometry,
#if USE_GENERIC_MATH
    System.Numerics.IAdditionOperators<PointZ, SizeZ, PointZ>,
    System.Numerics.ISubtractionOperators<PointZ, SizeZ, PointZ>,
    System.Numerics.ISubtractionOperators<PointZ, PointZ, SizeZ>,
    System.Numerics.IEqualityOperators<PointZ, PointZ, bool>,
#endif
    IEquatable<PointZ>
{
    /// <summary>
    /// Represents a new instance of the <see cref="PointZ" /> class with member data left uninitialized.
    /// </summary>
    public static readonly PointZ Empty = new(default, default, DefaultZ);

    private const double DefaultZ = default;

    /// <summary>
    /// Initialises a new instance of the <see cref="PointZ" /> struct with the specified coordinates.
    /// </summary>
    /// <param name="x">The horizontal position of the point.</param>
    /// <param name="y">The vertical position of the point.</param>
    /// <param name="z">The depth position of the point.</param>
    public PointZ(double x, double y, double z) => (this.X, this.Y, this.Z) = (x, y, z);

    /// <summary>
    /// Initialises a new instance of the <see cref="PointZ"/> struct.
    /// </summary>
    /// <param name="point">An instance of <see cref="Point"/> to use to initialise.</param>
    /// <param name="z">The depth position of the point.</param>
    public PointZ(Point point, double z = DefaultZ) => (this.X, this.Y, this.Z) = (point.X, point.Y, z);

    /// <summary>
    /// Initialises a new instance of the <see cref="PointZ" /> struct with the coordinates contained in the specified array.
    /// </summary>
    /// <param name="coordinates">An array of <see cref="double" /> containing the coordinates.</param>
    /// <param name="startIndex">The index to read the coordinates from.</param>
    /// <remarks><paramref name="coordinates" /> must not be null, and must be at least <paramref name="startIndex" /> + 3 items long.</remarks>
    public PointZ(double[] coordinates, int startIndex = default)
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
        (this.X, this.Y, this.Z) = (coordinates[startIndex], coordinates[startIndex + 1], coordinates[startIndex + 2]);
    }

    /// <summary>
    /// Gets a value indicating whether this <see cref="PointZ" /> is empty.
    /// </summary>
    public bool IsEmpty => this.X is default(double) && this.Y is default(double) && this.Z is default(double);

    /// <summary>
    /// Gets the x-coordinate of this <see cref="PointZ" />.
    /// </summary>
    public double X { get; }

    /// <summary>
    /// Gets the y-coordinate of this <see cref="PointZ" />.
    /// </summary>
    public double Y { get; }

    /// <summary>
    /// Gets the z-coordinate of this <see cref="PointZ" />.
    /// </summary>
    public double Z { get; }

    /// <summary>
    /// Converts the specified <see cref="PointZ"/> to a <see cref="Point"/>.
    /// </summary>
    /// <param name="point">The <see cref="PointZ"/> to convert from.</param>
    public static explicit operator Point(PointZ point) => new(point.X, point.Y);

    /// <summary>
    /// Translates a <see cref="PointZ" /> by a given <see cref="SizeZ" />.
    /// </summary>
    /// <param name="point">The <see cref="PointZ" /> to translate.</param>
    /// <param name="size">A <see cref="SizeZ" /> the specifies the pair of numbers to add to the coordinates of <paramref name="point" />.</param>
    /// <returns>The translated <see cref="PointZ" />.</returns>
    public static PointZ operator +(PointZ point, SizeZ size) => Add(point, size);

    /// <summary>
    /// Translates a <see cref="PointZ" /> by the negative of a given <see cref="SizeZ" />.
    /// </summary>
    /// <param name="point">The <see cref="PointZ" /> to translate.</param>
    /// <param name="size">A <see cref="SizeZ" /> the specifies the pair of numbers to subtract from the coordinates of <paramref name="point" />.</param>
    /// <returns>The translated <see cref="PointZ" />.</returns>
    public static PointZ operator -(PointZ point, SizeZ size) => Subtract(point, size);

    /// <summary>
    /// Gets the difference between two instance of <see cref="PointZ" />.
    /// </summary>
    /// <param name="first">The first <see cref="PointZ" />.</param>
    /// <param name="second">The second <see cref="PointZ" />.</param>
    /// <returns>An instance of <see cref="SizeZ" /> representing the difference between the two points.</returns>
    public static SizeZ operator -(PointZ first, PointZ second) => Subtract(first, second);

    /// <summary>
    /// Compares two <see cref="PointZ" /> structures. The result specifies whether the values of the <see cref="X" /> and <see cref="Y" /> properties of the two <see cref="PointZ" /> structures are equal.
    /// </summary>
    /// <param name="left">The first <see cref="PointZ" /> to compare.</param>
    /// <param name="right">The second <see cref="PointZ" /> to compare.</param>
    /// <returns>Returns <see langword="true"/> if the <see cref="X" /> and <see cref="Y" /> values of the left and right <see cref="PointZ" /> structures are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(PointZ left, PointZ right) => left.X.Equals(right.X) && left.Y.Equals(right.Y) && left.Z.Equals(right.Z);

    /// <summary>
    /// Determines whether the coordinates of the specified points are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="PointZ" /> to compare.</param>
    /// <param name="right">The second <see cref="PointZ" /> to compare.</param>
    /// <returns>Returns <see langword="true"/> to indicate the <see cref="X" /> and <see cref="Y" /> values of the <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(PointZ left, PointZ right) => !(left == right);

    /// <summary>
    /// Translates a <see cref="PointZ" /> by a given <see cref="SizeZ" />.
    /// </summary>
    /// <param name="point">The <see cref="PointZ" /> to translate.</param>
    /// <param name="size">A <see cref="SizeZ" /> the specifies the pair of numbers to add to the coordinates of <paramref name="point" />.</param>
    /// <returns>The translated <see cref="PointZ" />.</returns>
    public static PointZ Add(PointZ point, SizeZ size) => new(point.X + size.Width, point.Y + size.Height, point.Z + size.Depth);

    /// <summary>
    /// Translates a <see cref="PointZ" /> by the negative of a given <see cref="SizeZ" />.
    /// </summary>
    /// <param name="point">The <see cref="PointZ" /> to translate.</param>
    /// <param name="size">A <see cref="SizeZ" /> the specifies the pair of numbers to subtract from the coordinates of <paramref name="point" />.</param>
    /// <returns>The translated <see cref="PointZ" />.</returns>
    public static PointZ Subtract(PointZ point, SizeZ size) => new(point.X - size.Width, point.Y - size.Height, point.Z - size.Depth);

    /// <summary>
    /// Gets the difference between two instance of <see cref="PointZ" />.
    /// </summary>
    /// <param name="first">The first <see cref="PointZ" />.</param>
    /// <param name="second">The second <see cref="PointZ"/>.</param>
    /// <returns>An instance of <see cref="SizeZ" /> representing the difference between the two points.</returns>
    public static SizeZ Subtract(PointZ first, PointZ second) => new(first.X - second.X, first.Y - second.Y, first.Z - second.Z);

    /// <summary>
    /// Deconstructs this instance into its coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate.</param>
    /// <param name="y">The y-coordinate.</param>
    /// <param name="z">The z-coordinate.</param>
    public void Deconstruct(out double x, out double y, out double z) => (x, y, z) = (this.X, this.Y, this.Z);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => base.Equals(obj);

    /// <inheritdoc/>
    public bool Equals(PointZ other) => other.X.Equals(this.X) && other.Y.Equals(this.Y) && other.Z.Equals(this.Z);

    /// <inheritdoc/>
    public override int GetHashCode() =>
#if NETSTANDARD2_0_OR_GREATER || NET461_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        HashCode.Combine(this.X, this.Y, this.Z);
#else
        (this.X, this.Y, this.Z).GetHashCode();
#endif

    /// <summary>
    /// Converts a <see cref="PointZ" /> to a <see cref="Point" />.
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
        ((FormattableString)$"{{X={this.X}, Y={this.Y}, Z={this.Z}}}").ToString(formatProvider);
#else
        string.Format(formatProvider, "{{X={0}, Y={1}, Z={2}}}", this.X, this.Y, this.Z);
#endif

    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>A new object that is a copy of this instance.</returns>
    public object Clone() => new PointZ(this.X, this.Y, this.Z);
}