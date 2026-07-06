// -----------------------------------------------------------------------
// <copyright file="IGeometryZ.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

/// <summary>
/// The interface for geometry instances with x-, y-, and z- coordinates.
/// </summary>
public interface IGeometryZ : IGeometry
{
    /// <summary>
    /// Calculates the minimum z-coordinate of the geometry.
    /// </summary>
    /// <returns>The minimum z-coordinate of the geometry.</returns>
    double MinZ();

    /// <summary>
    /// Calculates the maximum z-coordinate of the geometry.
    /// </summary>
    /// <returns>The maximum z-coordinate of the geometry.</returns>
    double MaxZ();
}