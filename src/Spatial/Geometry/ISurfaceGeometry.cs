// -----------------------------------------------------------------------
// <copyright file="ISurfaceGeometry.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

/// <summary>
/// Represents a surface <see cref="IGeometry"/>.
/// </summary>
public interface ISurfaceGeometry : IGeometry
{
    /// <summary>
    /// Gets the area of the surface.
    /// </summary>
    /// <returns>The surface.</returns>
    double Area();
}