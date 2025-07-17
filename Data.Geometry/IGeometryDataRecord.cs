// -----------------------------------------------------------------------
// <copyright file="IGeometryDataRecord.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data;

/// <summary>
/// Provides access to the geometry value within each row for a DataReader.
/// </summary>
public interface IGeometryDataRecord
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
    /// <returns>An <see cref="IReadOnlyCollection{T}"/> filled with instances of <see cref="Point"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINT</c>.</exception>
    IReadOnlyCollection<Point> GetMultiPoint(int i);

    /// <summary>
    /// Gets the value of the specified column as a collection of <see cref="PointZ"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>An <see cref="IReadOnlyCollection{T}"/> filled with instances of <see cref="PointZ"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINTZ</c>.</exception>
    IReadOnlyCollection<PointZ> GetMultiPointZ(int i);

    /// <summary>
    /// Gets the value of the specified column as a collection of <see cref="Point"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>An <see cref="IReadOnlyCollection{T}"/> filled with instances of <see cref="Point"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOINTZM</c>.</exception>
    IReadOnlyCollection<PointZM> GetMultiPointZM(int i);

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
    IReadOnlyCollection<Polyline> GetMultiLineString(int i);

    /// <summary>
    /// Gets the value of the specified column as a collection of <see cref="PolylineZ"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>A <see cref="PolylineZ"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTILINESTRINGZ</c>.</exception>
    IReadOnlyCollection<PolylineZ> GetMultiLineStringZ(int i);

    /// <summary>
    /// Gets the value of the specified column as a collection of <see cref="PolylineZM"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>A <see cref="PolylineZM"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTILINESTRINGZM</c>.</exception>
    IReadOnlyCollection<PolylineZM> GetMultiLineStringZM(int i);

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
    IReadOnlyCollection<Polygon> GetMultiPolygon(int i);

    /// <summary>
    /// Gets the value of the specified column as a collection of <see cref="PolygonZ"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>A <see cref="PolygonZ"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOLYGONZ</c>.</exception>
    IReadOnlyCollection<PolygonZ> GetMultiPolygonZ(int i);

    /// <summary>
    /// Gets the value of the specified column as a collection of <see cref="PolygonZM"/>.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>A <see cref="PolygonZM"/> if successful.</returns>
    /// <exception cref="InvalidGeometryTypeException">The type of geometry is not <c>MULTIPOLYGONZM</c>.</exception>
    IReadOnlyCollection<PolygonZM> GetMultiPolygonZM(int i);

    /// <summary>
    /// Gets the value of the specified column as a geometry.
    /// </summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>An instance of <see cref="Point"/>, <see cref="PointZ"/>, <see cref="PointZM"/>, or an <see cref="IEnumerable{T}"/> fill with one of those if successful.</returns>
    object? GetGeometry(int i);

    /// <summary>
    /// Return whether the specified field is set to null.
    /// </summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns><see langword="true"/> if the specified field is set to null; otherwise, <see langword="false"/>.</returns>
    bool IsDBNull(int i);
}