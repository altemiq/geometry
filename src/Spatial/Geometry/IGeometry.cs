// -----------------------------------------------------------------------
// <copyright file="IGeometry.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

/// <summary>
/// The interface for geometry instances with x-, and y- coordinates.
/// </summary>
public interface IGeometry
{
    /// <summary>
    /// Calculates the minimum x-coordinate of the geometry.
    /// </summary>
    /// <returns>The minimum x-coordinate of the geometry.</returns>
    double MinX();

    /// <summary>
    /// Calculates the maximum x-coordinate of the geometry.
    /// </summary>
    /// <returns>The maximum x-coordinate of the geometry.</returns>
    double MaxX();

    /// <summary>
    /// Calculates the minimum y-coordinate of the geometry.
    /// </summary>
    /// <returns>The minimum y-coordinate of the geometry.</returns>
    double MinY();

    /// <summary>
    /// Calculates the minimum y-coordinate of the geometry.
    /// </summary>
    /// <returns>The minimum y-coordinate of the geometry.</returns>
    double MaxY();
}