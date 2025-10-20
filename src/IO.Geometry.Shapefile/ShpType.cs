// -----------------------------------------------------------------------
// <copyright file="ShpType.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

/// <summary>
/// Shape type enumeration.
/// </summary>
public enum ShpType
{
    /// <summary>
    /// Shape with no geometric data.
    /// </summary>
    NullShape = 0,

    // 2D Shape Types (pre ArcView 3.x):

    /// <summary>
    /// 2D point.
    /// </summary>
    Point = 1,

    /// <summary>
    /// 2D polyline.
    /// </summary>
    PolyLine = 3,

    /// <summary>
    /// 2D polygon.
    /// </summary>
    Polygon = 5,

    /// <summary>
    /// Set of 2D points.
    /// </summary>
    MultiPoint = 8,

    // 3D Shape Types (may include "measure" values for vertices)

    /// <summary>
    /// 3D point.
    /// </summary>
    PointZ = 11,

    /// <summary>
    /// 3D polyline.
    /// </summary>
    PolyLineZ = 13,

    /// <summary>
    /// 3D polygon.
    /// </summary>
    PolygonZ = 15,

    /// <summary>
    /// Set of 3D points.
    /// </summary>
    MultiPointZ = 18,

    // 2D + Measure Types

    /// <summary>
    /// 2D point with measure.
    /// </summary>
    PointM = 21,

    /// <summary>
    /// 2D polyline with measure.
    /// </summary>
    PolyLineM = 23,

    /// <summary>
    /// 2D polygon with measure.
    /// </summary>
    PolygonM = 25,

    /// <summary>
    /// Set of 2D points with measures.
    /// </summary>
    MultiPointM = 28,

    // Complex (TIN-like) with Z, and Measure

    /// <summary>
    /// Collection of surface patches.
    /// </summary>
    MultiPatch = 31,
}