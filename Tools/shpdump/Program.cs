// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.CommandLine;
using Altemiq.Geometry;
using Altemiq.IO.Geometry.Shapefile;

var fileArgument = new Argument<FileInfo>("shp-file").AcceptExistingOnly();
var validateOption = new Option<bool>("-validate");
var headerOnlyOption = new Option<bool>("-ho");
var precisionOption = new Option<bool>("-precision");

var root = new RootCommand
{
    fileArgument,
    validateOption,
    headerOnlyOption,
    precisionOption,
};

root.SetAction(parseResult => Process(
        parseResult.InvocationConfiguration,
        parseResult.GetValue(fileArgument)!,
        parseResult.GetValue(validateOption),
        parseResult.GetValue(headerOnlyOption),
        parseResult.GetValue(precisionOption)));

return await root.Parse(args).InvokeAsync().ConfigureAwait(false);

#pragma warning disable RCS1163, S1172
static void Process(InvocationConfiguration configuration, FileInfo shpFile, bool validate, bool headerOnly, bool precision)
#pragma warning restore RCS1163, S1172
{
    using var reader = new ShapefileReader(shpFile.FullName);
    configuration.Output.WriteLine($"Shapefile Type: {reader.ShpHeader.ShpType}   # of Shapes: {reader.Count}");
    configuration.Output.WriteLine(string.Empty);
    configuration.Output.WriteLine(string.Create(System.Globalization.CultureInfo.InvariantCulture, $"File Bounds: ( {reader.ShpHeader.Extents.Left,11:0.000}, {reader.ShpHeader.Extents.Bottom,11:0.000},{reader.ShpHeader.Extents.Front:0.###},{reader.ShpHeader.Extents.Measurement:g})"));
    configuration.Output.WriteLine(string.Create(System.Globalization.CultureInfo.InvariantCulture, $"         to  ( {reader.ShpHeader.Extents.Right,11:0.000}, {reader.ShpHeader.Extents.Top,11:0.000},{reader.ShpHeader.Extents.Back:0.###},{reader.ShpHeader.Extents.Measurement + reader.ShpHeader.Extents.Length:g})"));

    for (var i = 0; i < reader.Count && !headerOnly; i++)
    {
        var record = reader.Read(i);

        var geometry = record.IsNull() ? null : record.GetGeometry();
        var name = GeometryToName(geometry);
        var vertices = GetVertices(geometry);
        var parts = GetParts(geometry);

        configuration.Output.WriteLine(string.Empty);
        configuration.Output.Write(string.Create(System.Globalization.CultureInfo.InvariantCulture, $"Shape:{i} ({name}) "));
        configuration.Output.Write(string.Create(System.Globalization.CultureInfo.InvariantCulture, $" nVertices={vertices},"));
        configuration.Output.WriteLine(string.Create(System.Globalization.CultureInfo.InvariantCulture, $" nParts={parts}"));
        var bounds = GetBounds(geometry);
        configuration.Output.WriteLine(string.Create(System.Globalization.CultureInfo.InvariantCulture, $"  Bounds:( {bounds.X,11:0.000}, {bounds.Y,11:0.000}, {bounds.Z:0.###}, {bounds.Measurement:0.###})"));
        configuration.Output.WriteLine(string.Create(System.Globalization.CultureInfo.InvariantCulture, $"      to ( {bounds.Right,11:0.000}, {bounds.Top,11:0.000}, {bounds.Z + bounds.Depth:0.###}, {bounds.Measurement + bounds.Length:0.###})"));

        Print(configuration.Output, geometry);

        if (validate)
        {
            // rewind the objects
        }

        configuration.Output.WriteLine(string.Empty);
    }

    static void Print(TextWriter output, IGeometry? geometry)
    {
        switch (geometry)
        {
            case IEnumerable<Polyline> lineStrings:
                PrintPolylines(output, lineStrings);
                break;
            case IEnumerable<PolylineZ> lineStrings:
                PrintPolylines(output, lineStrings);
                break;
            case IEnumerable<PolylineM> lineStrings:
                PrintPolylines(output, lineStrings);
                break;
            case IEnumerable<PolylineZM> lineStrings:
                PrintPolylines(output, lineStrings);
                break;
            case Polyline lineString:
                PrintPolyline(output, lineString);
                break;
            case PolylineZ lineString:
                PrintPolyline(output, lineString);
                break;
            case PolylineM lineString:
                PrintPolyline(output, lineString);
                break;
            case PolylineZM lineString:
                PrintPolyline(output, lineString);
                break;
            case IEnumerable<Polygon> polygons:
                PrintPolygons(output, polygons);
                break;
            case IEnumerable<PolygonZ> polygons:
                PrintPolygons(output, polygons);
                break;
            case IEnumerable<PolygonM> polygons:
                PrintPolygons(output, polygons);
                break;
            case IEnumerable<PolygonZM> polygons:
                PrintPolygons(output, polygons);
                break;
            case Polygon polygon:
                PrintPolygon(output, polygon);
                break;
            case PolygonZ polygon:
                PrintPolygon(output, polygon);
                break;
            case PolygonM polygon:
                PrintPolygon(output, polygon);
                break;
            case PolygonZM polygon:
                PrintPolygon(output, polygon);
                break;
            case IEnumerable<Point> points:
                PrintPoints(output, points);
                break;
            case IEnumerable<PointZ> points:
                PrintPoints(output, points);
                break;
            case IEnumerable<PointM> points:
                PrintPoints(output, points);
                break;
            case IEnumerable<PointZM> points:
                PrintPoints(output, points);
                break;
            case not null:
                output.WriteLine(PrintPoint(geometry));
                break;
        }

        static void PrintPolylines<T>(TextWriter output, IEnumerable<Polyline<T>> lineStrings)
            where T : IGeometry
        {
            var plus = ' ';
            foreach (var lineString in lineStrings)
            {
                PrintPolyline(output, lineString, plus);
                plus = '+';
            }
        }

        static void PrintPolyline<T>(TextWriter output, Polyline<T> lineString, char plus = ' ')
            where T : IGeometry
        {
            foreach (var pt in lineString)
            {
                output.WriteLine(PrintPoint(pt, plus));
                plus = ' ';
            }
        }

        static void PrintPolygons<T>(TextWriter output, IEnumerable<Polygon<T>> polygons)
            where T : IGeometry
        {
            var plus = ' ';
            foreach (var polygon in polygons)
            {
                PrintPolygon(output, polygon, plus);
                plus = '+';
            }
        }

        static void PrintPolygon<T>(TextWriter output, Polygon<T> polygon, char plus = ' ')
            where T : IGeometry
        {
            foreach (var linearRing in polygon)
            {
                var partType = linearRing is ShpLinearRing<T> shpLinearRing
                    ? $"{shpLinearRing.PartType}"
                    : nameof(ShpPartType.Ring);

                foreach (var pt in linearRing)
                {
                    output.WriteLine(PrintPoint(pt, plus, partType));
                    partType = string.Empty;
                    plus = ' ';
                }

                plus = '+';
            }
        }

        static void PrintPoints<T>(TextWriter output, IEnumerable<T> points)
            where T : IGeometry
        {
            foreach (var point in points)
            {
                output.WriteLine(PrintPoint(point));
            }
        }

        static string PrintPoint(IGeometry point, char plus = ' ', string partType = "")
        {
            return point switch
            {
                PointZM point4D => string.Create(System.Globalization.CultureInfo.InvariantCulture, $"   {plus} ( {point4D.X,11:0.000}, {point4D.Y,11:0.000}, {point4D.Z:0.###}, {point4D.Measurement:0.###}) {partType} "),
                PointZ point3D => string.Create(System.Globalization.CultureInfo.InvariantCulture, $"   {plus} ( {point3D.X,11:0.000}, {point3D.Y,11:0.000}, {point3D.Z:0.###}) {partType} "),
                Point point2D => string.Create(System.Globalization.CultureInfo.InvariantCulture, $"   {plus} ( {point2D.X,11:0.000}, {point2D.Y,11:0.000}, 0, 0) {partType} "),
                _ => "null",
            };
        }
    }

    static EnvelopeZM GetBounds<T>(T? geometry)
        where T : IGeometry
    {
        return geometry switch
        {
            Point point => EnvelopeZM.FromXYZMWHDL(point.X, point.Y, 0D, 0D, 0D, 0D, 0D, 0D),
            PointZ point => EnvelopeZM.FromXYZMWHDL(point.X, point.Y, point.Z, 0D, 0D, 0D, 0D, 0D),
            PointM point => EnvelopeZM.FromXYZMWHDL(point.X, point.Y, 0D, point.Measurement, 0D, 0D, 0D, 0D),
            PointZM point => EnvelopeZM.FromXYZMWHDL(point.X, point.Y, point.Z, point.Measurement, 0D, 0D, 0D, 0D),
            Polyline<Point> polyline => GetFullBounds(polyline),
            Polyline<PointZ> polyline => GetFullBounds(polyline),
            Polyline<PointM> polyline => GetFullBounds(polyline),
            Polyline<PointZM> polyline => GetFullBounds(polyline),
            IEnumerable<Polyline> polylines => GetFullBounds(polylines),
            IEnumerable<PolylineZ> polylines => GetFullBounds(polylines),
            IEnumerable<PolylineM> polylines => GetFullBounds(polylines),
            IEnumerable<PolylineZM> polylines => GetFullBounds(polylines),
            Polygon<Point> polygon => GetFullBounds(polygon.SelectMany(p => p)),
            Polygon<PointZ> polygon => GetFullBounds(polygon.SelectMany(p => p)),
            Polygon<PointM> polygon => GetFullBounds(polygon.SelectMany(p => p)),
            Polygon<PointZM> polygon => GetFullBounds(polygon.SelectMany(p => p)),
            IEnumerable<Polygon> polygons => GetFullBounds(polygons),
            IEnumerable<PolygonZ> polygons => GetFullBounds(polygons),
            IEnumerable<PolygonM> polygons => GetFullBounds(polygons),
            IEnumerable<PolygonZM> polygons => GetFullBounds(polygons),
            IEnumerable<Point> points => GetFullBounds(points),
            IEnumerable<PointZ> points => GetFullBounds(points),
            IEnumerable<PointM> points => GetFullBounds(points),
            IEnumerable<PointZM> points => GetFullBounds(points),
            _ => EnvelopeZM.Empty,
        };

        static EnvelopeZM GetFullBounds<TGeometry>(IEnumerable<TGeometry> geometries)
            where TGeometry : IGeometry
        {
            return geometries.Select(GetBounds).Aggregate(EnvelopeZM.Union);
        }
    }

    static int GetParts(IGeometry? geometry)
    {
        return geometry switch
        {
            Point or PointZ or PointM or PointZM => 0,
            ICollection<Point> or ICollection<PointZ> or ICollection<PointM> or ICollection<PointZM> => 0,
            ICollection<Polyline> polylines => polylines.Count,
            ICollection<PolylineZ> polylines => polylines.Count,
            ICollection<PolylineM> polylines => polylines.Count,
            ICollection<PolylineZM> polylines => polylines.Count,
            ICollection<Polygon> polygons => polygons.Sum(polygon => polygon.Count),
            ICollection<PolygonZ> polygons => polygons.Sum(polygon => polygon.Count),
            ICollection<PolygonM> polygons => polygons.Sum(polygon => polygon.Count),
            ICollection<PolygonZM> polygons => polygons.Sum(polygon => polygon.Count),
            not null => throw new InvalidGeometryTypeException(),
            null => 0,
        };
    }

    static int GetVertices(IGeometry? geometry)
    {
        return geometry switch
        {
            Point or PointZ or PointM or PointZM => 0,
            IEnumerable<Polyline> lineStrings => GetLineStringVertices(lineStrings),
            IEnumerable<PolylineZ> lineStrings => GetLineStringVertices(lineStrings),
            IEnumerable<PolylineM> lineStrings => GetLineStringVertices(lineStrings),
            IEnumerable<PolylineZM> lineStrings => GetLineStringVertices(lineStrings),
            IEnumerable<Polygon> lineStrings => GetPolygonVertices(lineStrings),
            IEnumerable<PolygonZ> lineStrings => GetPolygonVertices(lineStrings),
            IEnumerable<PolygonM> lineStrings => GetPolygonVertices(lineStrings),
            IEnumerable<PolygonZM> lineStrings => GetPolygonVertices(lineStrings),
            IEnumerable<Point> or IEnumerable<PointZ> or IEnumerable<PointM> or IEnumerable<PointZM> => 1,
            not null => throw new InvalidGeometryTypeException(),
            null => 0,
        };

        static int GetLineStringVertices<T>(IEnumerable<Polyline<T>> lineStrings)
        {
            return lineStrings.Sum(lineString => lineString.Count);
        }

        static int GetPolygonVertices<T>(IEnumerable<Polygon<T>> polygons)
        {
            return polygons.Sum(polygon => polygon.Sum(ring => ring.Count));
        }
    }

    static string GeometryToName(IGeometry? geometry)
    {
        return geometry switch
        {
            Polygon or IMultiGeometry<Polygon> => nameof(ShpType.Polygon),
            PolygonZ or PolygonZM or IMultiGeometry<PolygonZ> or IMultiGeometry<PolygonZM> => nameof(ShpType.PolygonZ),
            PolygonM or IMultiGeometry<PolygonM> => nameof(ShpType.PolygonM),
            Polyline or IMultiGeometry<Polyline> => nameof(ShpType.PolyLine),
            PolylineZ or PolylineZM or IMultiGeometry<PolylineZ> or IMultiGeometry<PolylineZM> => nameof(ShpType.PolyLineZ),
            PolylineM or IMultiGeometry<PolylineM> => nameof(ShpType.PolyLineM),
            Point or IMultiGeometry<Point> => nameof(ShpType.Point),
            PointZ or PointZM or IMultiGeometry<PointZ> or IMultiGeometry<PointZM> => nameof(ShpType.PointZ),
            PointM or IMultiGeometry<PointM> => nameof(ShpType.PointM),
            not null => throw new InvalidGeometryTypeException(),
            null => nameof(ShpType.NullShape),
        };
    }
}