// -----------------------------------------------------------------------
// <copyright file="MapSymbol.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.MapInfo;

/// <summary>
/// The <see cref="MapInfo"/> <see cref="Altemiq.Geometry.Point"/> with a symbol.
/// </summary>
/// <param name="X">The x-coordinate.</param>
/// <param name="Y">The y-coordinate.</param>
/// <param name="SymbolId">The symbol ID.</param>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
public readonly record struct MapSymbol(double X, double Y, byte SymbolId) : Altemiq.Geometry.IGeometry
{
    /// <summary>
    /// Gets the point.
    /// </summary>
    public Altemiq.Geometry.Point Point => new(this.X, this.Y);

    /// <summary>
    /// Gets a value indicating whether this instance is empty.
    /// </summary>
    public bool IsEmpty => this.X.Equals(default) || this.Y.Equals(default);

    /// <summary>
    /// Deconstructs this instance.
    /// </summary>
    /// <param name="x">The x-coordinate.</param>
    /// <param name="y">The y-coordinate.</param>
    public void Deconstruct(out double x, out double y) => (x, y) = (this.X, this.Y);

    /// <inheritdoc/>
    double Altemiq.Geometry.IGeometry.MinX() => this.X;

    /// <inheritdoc/>
    double Altemiq.Geometry.IGeometry.MaxX() => this.X;

    /// <inheritdoc/>
    double Altemiq.Geometry.IGeometry.MinY() => this.Y;

    /// <inheritdoc/>
    double Altemiq.Geometry.IGeometry.MaxY() => this.Y;
}