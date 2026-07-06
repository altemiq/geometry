// -----------------------------------------------------------------------
// <copyright file="IGeometryWriter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data;

/// <summary>
/// Represents an interface that writes geometries.
/// </summary>
public interface IGeometryWriter
{
    /// <summary>
    /// Writes the specified point.
    /// </summary>
    /// <param name="point">The point to write.</param>
    void Write(Point point);

    /// <summary>
    /// Writes the specified point.
    /// </summary>
    /// <param name="point">The point to write.</param>
    void Write(PointZ point);

    /// <summary>
    /// Writes the specified point.
    /// </summary>
    /// <param name="point">The point to write.</param>
    void Write(PointM point);

    /// <summary>
    /// Writes the specified point.
    /// </summary>
    /// <param name="point">The point to write.</param>
    void Write(PointZM point);

    /// <summary>
    /// Writes the specified points as a MultiPoint.
    /// </summary>
    /// <param name="points">The points to write.</param>
    void Write(IEnumerable<Point> points);

    /// <summary>
    /// Writes the specified points as a MultiPoint.
    /// </summary>
    /// <param name="points">The points to write.</param>
    void Write(IEnumerable<PointZ> points);

    /// <summary>
    /// Writes the specified points as a MultiPoint.
    /// </summary>
    /// <param name="points">The points to write.</param>
    void Write(IEnumerable<PointM> points);

    /// <summary>
    /// Writes the specified points as a MultiPoint.
    /// </summary>
    /// <param name="points">The points to write.</param>
    void Write(IEnumerable<PointZM> points);

    /// <summary>
    /// Writes the specified points as a LineString.
    /// </summary>
    /// <param name="polyline">The points to write.</param>
    void Write(Polyline<Point> polyline);

    /// <summary>
    /// Writes the specified points as a LineString.
    /// </summary>
    /// <param name="polyline">The points to write.</param>
    void Write(Polyline<PointZ> polyline);

    /// <summary>
    /// Writes the specified points as a LineString.
    /// </summary>
    /// <param name="polyline">The points to write.</param>
    void Write(Polyline<PointM> polyline);

    /// <summary>
    /// Writes the specified points as a LineString.
    /// </summary>
    /// <param name="polyline">The points to write.</param>
    void Write(Polyline<PointZM> polyline);

    /// <summary>
    /// Writes the specified lines as a MultiLineString.
    /// </summary>
    /// <param name="polylines">The lines to write.</param>
    void Write(params IEnumerable<Polyline<Point>> polylines);

    /// <summary>
    /// Writes the specified lines as a MultiLineString.
    /// </summary>
    /// <param name="polylines">The lines to write.</param>
    void Write(params IEnumerable<Polyline<PointZ>> polylines);

    /// <summary>
    /// Writes the specified lines as a MultiLineString.
    /// </summary>
    /// <param name="polylines">The lines to write.</param>
    void Write(params IEnumerable<Polyline<PointM>> polylines);

    /// <summary>
    /// Writes the specified lines as a MultiLineString.
    /// </summary>
    /// <param name="polylines">The lines to write.</param>
    void Write(params IEnumerable<Polyline<PointZM>> polylines);

    /// <summary>
    /// Writes the specified points as a Polygon.
    /// </summary>
    /// <param name="polygon">The points to write.</param>
    void Write(Polygon<Point> polygon);

    /// <summary>
    /// Writes the specified points as a Polygon.
    /// </summary>
    /// <param name="polygon">The points to write.</param>
    void Write(Polygon<PointZ> polygon);

    /// <summary>
    /// Writes the specified points as a Polygon.
    /// </summary>
    /// <param name="polygon">The points to write.</param>
    void Write(Polygon<PointM> polygon);

    /// <summary>
    /// Writes the specified points as a Polygon.
    /// </summary>
    /// <param name="polygon">The points to write.</param>
    void Write(Polygon<PointZM> polygon);

    /// <summary>
    /// Writes the specified polygons as a MultiPolygon.
    /// </summary>
    /// <param name="polygons">The polygons to write.</param>
    void Write(params IEnumerable<Polygon<Point>> polygons);

    /// <summary>
    /// Writes the specified polygons as a MultiPolygon.
    /// </summary>
    /// <param name="polygons">The polygons to write.</param>
    void Write(params IEnumerable<Polygon<PointZ>> polygons);

    /// <summary>
    /// Writes the specified polygons as a MultiPolygon.
    /// </summary>
    /// <param name="polygons">The polygons to write.</param>
    void Write(params IEnumerable<Polygon<PointM>> polygons);

    /// <summary>
    /// Writes the specified polygons as a MultiPolygon.
    /// </summary>
    /// <param name="polygons">The polygons to write.</param>
    void Write(params IEnumerable<Polygon<PointZM>> polygons);
}