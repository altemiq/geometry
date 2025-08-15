// -----------------------------------------------------------------------
// <copyright file="IGeometryDataRecord.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data;

/// <summary>
/// Provides access to the geometry value within each row for a DataReader.
/// </summary>
public interface IGeometryDataRecord : System.Data.IDataRecord
{
    /// <summary>
    /// Gets the value of the specified column as a <see cref="Point"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>An instance of <see cref="Point"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>POINT</c>.</exception>
    Point GetPoint(int i);

    /// <summary>
    /// Gets the value of the specified column as a <see cref="PointZ"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>An instance of <see cref="PointZ"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>POINTZ</c>.</exception>
    PointZ GetPointZ(int i);

    /// <summary>
    /// Gets the value of the specified column as a <see cref="PointM"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>An instance of <see cref="PointM"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>POINTM</c>.</exception>
    PointM GetPointM(int i);

    /// <summary>
    /// Gets the value of the specified column as a <see cref="PointZM"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>An instance of <see cref="PointZM"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>POINTZM</c>.</exception>
    PointZM GetPointZM(int i);

    /// <summary>
    /// Gets the value of the specified column as a collection of <see cref="Point"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>An <see cref="IMultiGeometry{T}"/> filled with instances of <see cref="Point"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINT</c>.</exception>
    IMultiGeometry<Point> GetMultiPoint(int i);

    /// <summary>
    /// Gets the value of the specified column as a collection of <see cref="PointZ"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>An <see cref="IMultiGeometry{T}"/> filled with instances of <see cref="PointZ"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINTZ</c>.</exception>
    IMultiGeometry<PointZ> GetMultiPointZ(int i);

    /// <summary>
    /// Gets the value of the specified column as a collection of <see cref="PointM"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>An <see cref="IMultiGeometry{T}"/> filled with instances of <see cref="PointM"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINTM</c>.</exception>
    IMultiGeometry<PointM> GetMultiPointM(int i);

    /// <summary>
    /// Gets the value of the specified column as a collection of <see cref="PointZM"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>An <see cref="IMultiGeometry{T}"/> filled with instances of <see cref="PointZM"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINTZM</c>.</exception>
    IMultiGeometry<PointZM> GetMultiPointZM(int i);

    /// <summary>
    /// Gets the value of the specified column as <see cref="Polyline"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>A <see cref="Polyline"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>LINESTRING</c>.</exception>
    Polyline GetLineString(int i);

    /// <summary>
    /// Gets the value of the specified column as <see cref="PolylineZ"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>A <see cref="PolylineZ"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>LINESTRINGZ</c>.</exception>
    PolylineZ GetLineStringZ(int i);

    /// <summary>
    /// Gets the value of the specified column as <see cref="PolylineM"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>A <see cref="PolylineM"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>LINESTRINGM</c>.</exception>
    PolylineM GetLineStringM(int i);

    /// <summary>
    /// Gets the value of the specified column as <see cref="PolylineZM"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>A <see cref="PolylineZM"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>LINESTRINGZM</c>.</exception>
    PolylineZM GetLineStringZM(int i);

    /// <summary>
    /// Gets the value of the specified column as a collection of <see cref="Polyline"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>A <see cref="Polyline"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTILINESTRING</c>.</exception>
    IMultiGeometry<Polyline> GetMultiLineString(int i);

    /// <summary>
    /// Gets the value of the specified column as a collection of <see cref="PolylineZ"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>A <see cref="PolylineZ"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTILINESTRINGZ</c>.</exception>
    IMultiGeometry<PolylineZ> GetMultiLineStringZ(int i);

    /// <summary>
    /// Gets the value of the specified column as a collection of <see cref="PolylineM"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>A <see cref="PolylineM"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTILINESTRINGM</c>.</exception>
    IMultiGeometry<PolylineM> GetMultiLineStringM(int i);

    /// <summary>
    /// Gets the value of the specified column as a collection of <see cref="PolylineZM"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>A <see cref="PolylineZM"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTILINESTRINGZM</c>.</exception>
    IMultiGeometry<PolylineZM> GetMultiLineStringZM(int i);

    /// <summary>
    /// Gets the value of the specified column as <see cref="Polygon"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>A <see cref="Polygon"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>POLYGON</c>.</exception>
    Polygon GetPolygon(int i);

    /// <summary>
    /// Gets the value of the specified column as <see cref="PolygonZ"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>A <see cref="PolygonZ"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>POLYGONZ</c>.</exception>
    PolygonZ GetPolygonZ(int i);

    /// <summary>
    /// Gets the value of the specified column as <see cref="PolygonM"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>A <see cref="PolygonM"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>POLYGONM</c>.</exception>
    PolygonM GetPolygonM(int i);

    /// <summary>
    /// Gets the value of the specified column as <see cref="PolygonZM"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>A <see cref="PolygonZM"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>POLYGONZM</c>.</exception>
    PolygonZM GetPolygonZM(int i);

    /// <summary>
    /// Gets the value of the specified column as a collection of <see cref="Polygon"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>A <see cref="Polygon"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOLYGON</c>.</exception>
    IMultiGeometry<Polygon> GetMultiPolygon(int i);

    /// <summary>
    /// Gets the value of the specified column as a collection of <see cref="PolygonZ"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>A <see cref="PolygonZ"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOLYGONZ</c>.</exception>
    IMultiGeometry<PolygonZ> GetMultiPolygonZ(int i);

    /// <summary>
    /// Gets the value of the specified column as a collection of <see cref="PolygonM"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>A <see cref="PolygonM"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOLYGONM</c>.</exception>
    IMultiGeometry<PolygonM> GetMultiPolygonM(int i);

    /// <summary>
    /// Gets the value of the specified column as a collection of <see cref="PolygonZM"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>A <see cref="PolygonZM"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOLYGONZM</c>.</exception>
    IMultiGeometry<PolygonZM> GetMultiPolygonZM(int i);

    /// <summary>
    /// Gets the value of the specified column as a geometry.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>An instance of <see cref="Point"/>, <see cref="PointZ"/>, <see cref="PointZM"/>, or an <see cref="IEnumerable{T}"/> fill with one of those if successful.</returns>
    IGeometry GetGeometry(int i);
}