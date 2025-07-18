// -----------------------------------------------------------------------
// <copyright file="GaiaGeometryType.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Spatialite;

/// <summary>
/// The GAIA integer codes.
/// </summary>
internal enum GaiaGeometryType
{
    /// <summary>
    /// 2D geometry.
    /// </summary>
    Geometry = 0,

    /// <summary>
    /// 2D point.
    /// </summary>
    Point = 1,

    /// <summary>
    /// 2D line string.
    /// </summary>
    LineString = 2,

    /// <summary>
    /// 2D polygon.
    /// </summary>
    Polygon = 3,

    /// <summary>
    /// 2D multi-point.
    /// </summary>
    MultiPoint = 4,

    /// <summary>
    /// 2D multi-line string.
    /// </summary>
    MultiLineString = 5,

    /// <summary>
    /// 2D multi-polygon.
    /// </summary>
    MultiPolygon = 6,

    /// <summary>
    /// 2D geometry collection.
    /// </summary>
    GeometryCollection = 7,

    /// <summary>
    /// Point with Z value.
    /// </summary>
    PointZ = 1001,

    /// <summary>
    /// LineString with Z value.
    /// </summary>
    LineStringZ = 1002,

    /// <summary>
    /// Polygon with Z value.
    /// </summary>
    PolygonZ = 1003,

    /// <summary>
    /// Multipoint with Z value.
    /// </summary>
    MultiPointZ = 1004,

    /// <summary>
    /// Multi-line string with Z value.
    /// </summary>
    MultiLineStringZ = 1005,

    /// <summary>
    /// Multi-polygon with Z value.
    /// </summary>
    MultiPolygonZ = 1006,

    /// <summary>
    /// Geometry collection with Z value.
    /// </summary>
    GeometryCollectionZ = 1007,

    /// <summary>
    /// Point with M value.
    /// </summary>
    PointM = 2001,

    /// <summary>
    /// LineString with M value.
    /// </summary>
    LineStringM = 2002,

    /// <summary>
    /// Polygon with M value.
    /// </summary>
    PolygonM = 2003,

    /// <summary>
    /// Multi-point with M value.
    /// </summary>
    MultiPointM = 2004,

    /// <summary>
    /// Multi-line string with M value.
    /// </summary>
    MultiLineStringM = 2005,

    /// <summary>
    /// Multi-polygon with M value.
    /// </summary>
    MultiPolygonM = 2006,

    /// <summary>
    /// Geometry collection with M value.
    /// </summary>
    GeometryCollectionM = 2007,

    /// <summary>
    /// Point with Z and M values.
    /// </summary>
    PointZM = 3001,

    /// <summary>
    /// LineString with Z and M values.
    /// </summary>
    LineStringZM = 3002,

    /// <summary>
    /// Polygon with Z and M values.
    /// </summary>
    PolygonZM = 3003,

    /// <summary>
    /// Multipoint with Z and M values.
    /// </summary>
    MultiPointZM = 3004,

    /// <summary>
    /// Multi-line string with Z and M values.
    /// </summary>
    MultiLineStringZM = 3005,

    /// <summary>
    /// Multi-polygon with Z and M values.
    /// </summary>
    MultiPolygonZM = 3006,

    /// <summary>
    /// Geometry collection with Z and M values.
    /// </summary>
    GeometryCollectionZM = 3007,

    /// <summary>
    /// Compressed 2D line string.
    /// </summary>
    CompressedLineString = 1000002,

    /// <summary>
    /// Compressed 2D polygon.
    /// </summary>
    CompressedPolygon = 1000003,

    /// <summary>
    /// Compressed line string with Z value.
    /// </summary>
    CompressedLineStringZ = 1001002,

    /// <summary>
    /// Compressed polygon with Z value.
    /// </summary>
    CompressedPolygonZ = 1001003,

    /// <summary>
    /// Compressed line string with M value.
    /// </summary>
    CompressedLineStringM = 1002002,

    /// <summary>
    /// Compressed polygon with M value.
    /// </summary>
    CompressedPolygonM = 1002003,

    /// <summary>
    /// Compressed line string with Z and M values.
    /// </summary>
    CompressedLineStringZM = 1003002,

    /// <summary>
    /// Compressed polygon with Z and M values.
    /// </summary>
    CompressedPolygonZM = 1003003,
}