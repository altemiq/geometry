// -----------------------------------------------------------------------
// <copyright file="IGeometryRecord.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data;

/// <summary>
/// Provides access to the geometry value within each row for a DataReader.
/// </summary>
public interface IGeometryRecord
{
    /// <summary>
    /// Gets the value as a <see cref="Point"/>.
    /// </summary>
    /// <returns>An instance of <see cref="Point"/> if successful; otherwise <see langword="null"/>.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>POINT</c>.</exception>
    Point GetPoint();

    /// <summary>
    /// Gets the value as a <see cref="PointZ"/>.
    /// </summary>
    /// <returns>An instance of <see cref="PointZ"/> if successful; otherwise <see langword="null"/>.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>POINTZ</c>.</exception>
    PointZ GetPointZ();

    /// <summary>
    /// Gets the value as a <see cref="PointM"/>.
    /// </summary>
    /// <returns>An instance of <see cref="PointM"/> if successful; otherwise <see langword="null"/>.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>POINTM</c>.</exception>
    PointM GetPointM();

    /// <summary>
    /// Gets the value as a <see cref="PointZM"/>.
    /// </summary>
    /// <returns>An instance of <see cref="PointZM"/> if successful; otherwise <see langword="null"/>.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>POINTZM</c>.</exception>
    PointZM GetPointZM();

    /// <summary>
    /// Gets the value as a collection of <see cref="Point"/>.
    /// </summary>
    /// <returns>An <see cref="IReadOnlyCollection{T}"/> filled with instances of <see cref="Point"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINT</c>.</exception>
    IReadOnlyCollection<Point> GetMultiPoint();

    /// <summary>
    /// Gets the value as a collection of <see cref="PointZ"/>.
    /// </summary>
    /// <returns>An <see cref="IReadOnlyCollection{T}"/> filled with instances of <see cref="PointZ"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINTZ</c>.</exception>
    IReadOnlyCollection<PointZ> GetMultiPointZ();

    /// <summary>
    /// Gets the value as a collection of <see cref="PointM"/>.
    /// </summary>
    /// <returns>An <see cref="IReadOnlyCollection{T}"/> filled with instances of <see cref="PointM"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINTM</c>.</exception>
    IReadOnlyCollection<PointM> GetMultiPointM();

    /// <summary>
    /// Gets the value as a collection of <see cref="Point"/>.
    /// </summary>
    /// <returns>An <see cref="IReadOnlyCollection{T}"/> filled with instances of <see cref="Point"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINTZM</c>.</exception>
    IReadOnlyCollection<PointZM> GetMultiPointZM();

    /// <summary>
    /// Gets the value as <see cref="Polyline"/>.
    /// </summary>
    /// <returns>A <see cref="Polyline"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>LINESTRING</c>.</exception>
    Polyline GetLineString();

    /// <summary>
    /// Gets the value as <see cref="PolylineZ"/>.
    /// </summary>
    /// <returns>A <see cref="PolylineZ"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>LINESTRINGZ</c>.</exception>
    PolylineZ GetLineStringZ();

    /// <summary>
    /// Gets the value as <see cref="PolylineM"/>.
    /// </summary>
    /// <returns>A <see cref="PolylineM"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>LINESTRINGM</c>.</exception>
    PolylineM GetLineStringM();

    /// <summary>
    /// Gets the value as <see cref="PolylineZM"/>.
    /// </summary>
    /// <returns>A <see cref="PolylineZM"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>LINESTRINGZM</c>.</exception>
    PolylineZM GetLineStringZM();

    /// <summary>
    /// Gets the value as a collection of <see cref="Polyline"/>.
    /// </summary>
    /// <returns>A <see cref="Polyline"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTILINESTRING</c>.</exception>
    IReadOnlyCollection<Polyline> GetMultiLineString();

    /// <summary>
    /// Gets the value as a collection of <see cref="PolylineZ"/>.
    /// </summary>
    /// <returns>A <see cref="PolylineZ"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTILINESTRINGZ</c>.</exception>
    IReadOnlyCollection<PolylineZ> GetMultiLineStringZ();

    /// <summary>
    /// Gets the value as a collection of <see cref="PolylineM"/>.
    /// </summary>
    /// <returns>A <see cref="PolylineM"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTILINESTRINGM</c>.</exception>
    IReadOnlyCollection<PolylineM> GetMultiLineStringM();

    /// <summary>
    /// Gets the value as a collection of <see cref="PolylineZM"/>.
    /// </summary>
    /// <returns>A <see cref="PolylineZM"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTILINESTRINGZM</c>.</exception>
    IReadOnlyCollection<PolylineZM> GetMultiLineStringZM();

    /// <summary>
    /// Gets the value as <see cref="Polygon"/>.
    /// </summary>
    /// <returns>A <see cref="Polygon"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>POLYGON</c>.</exception>
    Polygon GetPolygon();

    /// <summary>
    /// Gets the value as <see cref="PolygonZ"/>.
    /// </summary>
    /// <returns>A <see cref="PolygonZ"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>POLYGONZ</c>.</exception>
    PolygonZ GetPolygonZ();

    /// <summary>
    /// Gets the value as <see cref="PolygonM"/>.
    /// </summary>
    /// <returns>A <see cref="PolygonM"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>POLYGONM</c>.</exception>
    PolygonM GetPolygonM();

    /// <summary>
    /// Gets the value as <see cref="PolygonZM"/>.
    /// </summary>
    /// <returns>A <see cref="PolygonZM"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>POLYGONZM</c>.</exception>
    PolygonZM GetPolygonZM();

    /// <summary>
    /// Gets the value as a collection of <see cref="Polygon"/>.
    /// </summary>
    /// <returns>A <see cref="Polygon"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOLYGON</c>.</exception>
    IReadOnlyCollection<Polygon> GetMultiPolygon();

    /// <summary>
    /// Gets the value as a collection of <see cref="PolygonZ"/>.
    /// </summary>
    /// <returns>A <see cref="PolygonZ"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOLYGONZ</c>.</exception>
    IReadOnlyCollection<PolygonZ> GetMultiPolygonZ();

    /// <summary>
    /// Gets the value as a collection of <see cref="PolygonM"/>.
    /// </summary>
    /// <returns>A <see cref="PolygonM"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOLYGONM</c>.</exception>
    IReadOnlyCollection<PolygonM> GetMultiPolygonM();

    /// <summary>
    /// Gets the value as a collection of <see cref="PolygonZM"/>.
    /// </summary>
    /// <returns>A <see cref="PolygonZM"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOLYGONZM</c>.</exception>
    IReadOnlyCollection<PolygonZM> GetMultiPolygonZM();

    /// <summary>
    /// Gets the value as a geometry.
    /// </summary>
    /// <returns>An instance of <see cref="Point"/>, <see cref="PointZ"/> <see cref="PointZM"/>, <see cref="Polyline"/>, <see cref="PolylineZ"/> <see cref="PolylineZM"/>, or an <see cref="IEnumerable{T}"/> fill with one of those if successful; otherwise <see langword="null"/>.</returns>
    object? GetGeometry();

    /// <summary>
    /// Return whether this record is set to null.
    /// </summary>
    /// <returns><see langword="true"/> if this record is set to null; otherwise, <see langword="false"/>.</returns>
    bool IsNull();
}