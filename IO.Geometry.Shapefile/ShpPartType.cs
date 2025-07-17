// -----------------------------------------------------------------------
// <copyright file="ShpPartType.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

/// <summary>
/// The SHP part type.
/// </summary>
public enum ShpPartType
{
    /// <summary>
    /// A linked strip of triangles, where every vertex (after the first two) completes a new triangle.
    /// A new triangle is always formed by connecting the new vertext with its two immediate predecessors.
    /// </summary>
    TriangleStrip = 0,

    /// <summary>
    /// A linked fan of triangles, where every vertex (after the first two) completes a new triangle.
    /// A new triangle is always formed by connecting the new vertext with its immediate predecessor and the first vertex of the part.
    /// </summary>
    TriangleFan = 1,

    /// <summary>
    /// The outer ring of a polygon.
    /// </summary>
    OuterRing = 2,

    /// <summary>
    /// A hole of a polygon.
    /// </summary>
    InnerRing = 3,

    /// <summary>
    /// The first ring of a polygon of an unspecified type.
    /// </summary>
    FirstRing = 4,

    /// <summary>
    /// The ring of a polygon of an unspecified type.
    /// </summary>
    Ring = 5,
}