// -----------------------------------------------------------------------
// <copyright file="TinyWktReaderHeaderExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

/// <summary>
/// Extension methods for <see cref="TinyWkbRecordHeader"/> instances.
/// </summary>
public static class TinyWktReaderHeaderExtensions
{
    /// <summary>
    /// Computes the scale value.
    /// </summary>
    /// <param name="precision">The precision.</param>
    /// <returns>The scale value.</returns>
    public static double Scale(this int precision) => System.Math.Pow(10, precision);

    /// <summary>
    /// Computes the scale value for x-ordinate values.
    /// </summary>
    /// <param name="self">A TWKB-header.</param>
    /// <returns>The scale value for x-ordinate values.</returns>
    public static double ScaleX(this TinyWkbRecordHeader self) => Scale(self.PrecisionXY);

    /// <summary>
    /// Computes the scale value for y-coordinate values.
    /// </summary>
    /// <param name="self">A TWKB-header.</param>
    /// <returns>The scale value for y-ordinate values.</returns>
    public static double ScaleY(this TinyWkbRecordHeader self) => Scale(self.PrecisionXY);

    /// <summary>
    /// Computes the scale value for z-ordinate values.
    /// </summary>
    /// <param name="self">A TWKB-header.</param>
    /// <returns>The scale value for z-ordinate values.</returns>
    public static double ScaleZ(this TinyWkbRecordHeader self) => Scale(self.PrecisionZ);

    /// <summary>
    /// Computes the scale value for m-ordinate values.
    /// </summary>
    /// <param name="self">A TWKB-header.</param>
    /// <returns>The scale value for m-ordinate values.</returns>
    public static double ScaleM(this TinyWkbRecordHeader self) => Scale(self.PrecisionM);

    /// <summary>
    /// Computes the de-scale value.
    /// </summary>
    /// <param name="precision">The precision.</param>
    /// <returns>The de-scale value.</returns>
    public static double Descale(this int precision) => System.Math.Pow(10, -precision);

    /// <summary>
    /// Computes the de-scale value for x-ordinate values.
    /// </summary>
    /// <param name="self">A TWKB-header.</param>
    /// <returns>The de-scale value for x-ordinate values.</returns>
    public static double DescaleX(this TinyWkbRecordHeader self) => Descale(self.PrecisionXY);

    /// <summary>
    /// Computes the de-scale value for y-ordinate values.
    /// </summary>
    /// <param name="self">A TWKB-header.</param>
    /// <returns>The de-scale value for y-ordinate values.</returns>
    public static double DescaleY(this TinyWkbRecordHeader self) => Descale(self.PrecisionXY);

    /// <summary>
    /// Computes the de-scale value for z-ordinate values.
    /// </summary>
    /// <param name="self">A TWKB-header.</param>
    /// <returns>The de-scale value for z-ordinate values.</returns>
    public static double DescaleZ(this TinyWkbRecordHeader self) => Descale(self.PrecisionZ);

    /// <summary>
    /// Computes the de-scale value for m-ordinate values.
    /// </summary>
    /// <param name="self">A TWKB-header.</param>
    /// <returns>The de-scale value for m-ordinate values.</returns>
    public static double DescaleM(this TinyWkbRecordHeader self) => Descale(self.PrecisionM);
}