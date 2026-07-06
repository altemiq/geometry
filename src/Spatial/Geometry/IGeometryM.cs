// -----------------------------------------------------------------------
// <copyright file="IGeometryM.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

/// <summary>
/// The interface for geometry instances with x-, y-, and m- coordinates.
/// </summary>
public interface IGeometryM : IGeometry
{
    /// <summary>
    /// Calculates the minimum m-coordinate of the geometry.
    /// </summary>
    /// <returns>The minimum m-coordinate of the geometry.</returns>
    double MinM();

    /// <summary>
    /// Calculates the maximum m-coordinate of the geometry.
    /// </summary>
    /// <returns>The maximum m-coordinate of the geometry.</returns>
    double MaxM();
}