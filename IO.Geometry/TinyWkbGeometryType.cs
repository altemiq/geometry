// -----------------------------------------------------------------------
// <copyright file="TinyWkbGeometryType.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

/// <summary>
/// The TWKB integer codes.
/// </summary>
public enum TinyWkbGeometryType : byte
{
    /// <summary>
    /// A point geometry.
    /// </summary>
    Point = 1,

    /// <summary>
    /// A line string geometry.
    /// </summary>
    Linestring = 2,

    /// <summary>
    /// A polygon geometry.
    /// </summary>
    Polygon = 3,

    /// <summary>
    /// A multi point geometry.
    /// </summary>
    MultiPoint = 4,

    /// <summary>
    /// A multi line string geometry.
    /// </summary>
    MultiLinestring = 5,

    /// <summary>
    /// A multi polygon geometry.
    /// </summary>
    MultiPolygon = 6,

    /// <summary>
    /// A geometry collection.
    /// </summary>
    GeometryCollection = 7,
}