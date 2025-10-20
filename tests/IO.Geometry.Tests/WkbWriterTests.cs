// -----------------------------------------------------------------------
// <copyright file="WkbWriterTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

public class WkbWriterTests
{
    [Test]
    public async Task CreateWkbWriterUsingStream()
    {
        using var stream = new MemoryStream();
        using var writer = new WkbWriter(stream);
        _ = await Assert.That(stream).IsNotNull();
    }

    [Test]
    public async Task WritePoint()
    {
        var point = new Point(30.0, 10.0);
        var bytes = new byte[21];

        using (var writer = new WkbWriter(bytes)) { writer.Write(point); }

        _ = await Assert.That(bytes).IsEquivalentTo(HelperFunctions.GetByteArrayFromResource("Point"));
    }

    [Test]
    public async Task WritePointZ()
    {
        var point = new PointZ(30.0, 10.0, 20.0);
        var bytes = new byte[29];

        using (var writer = new WkbWriter(bytes)) { writer.Write(point); }

        _ = await Assert.That(bytes).IsEquivalentTo(HelperFunctions.GetByteArrayFromResource("PointZ"));
    }

    [Test]
    public async Task WritePointZM()
    {
        var point = new PointZM(30.0, 10.0, 20.0, 15.0);
        var bytes = new byte[37];

        using (var writer = new WkbWriter(bytes)) { writer.Write(point); }

        _ = await Assert.That(bytes).IsEquivalentTo(HelperFunctions.GetByteArrayFromResource("PointZM"));
    }

    [Test]
    public async Task WriteMultiPoint()
    {
        ICollection<Point> points = [new(10.0, 40.0), new(40.0, 30.0), new(20.0, 20.0), new(30.0, 10.0)];
        var bytes = new byte[93];

        using (var writer = new WkbWriter(bytes)) { writer.Write(points); }

        _ = await Assert.That(bytes).IsEquivalentTo(HelperFunctions.GetByteArrayFromResource("MultiPoint"));
    }

    [Test]
    public async Task WriteLineString()
    {
        // LINESTRING (30 10, 10 30, 40 4)
        var lineString = new Polyline(new(30.0, 10.0), new(10.0, 30.0), new(40.0, 40.0));
        var bytes = new byte[57];

        using (var writer = new WkbWriter(bytes)) { writer.Write(lineString); }

        _ = await Assert.That(bytes).IsEquivalentTo(HelperFunctions.GetByteArrayFromResource("LineString"));
    }

    [Test]
    public async Task WriteLineStringZ()
    {
        // LINESTRINGZ (30 10 20.0, 10 30 5.0, 40 40 4.0)
        var lineString = new PolylineZ(new(30.0, 10.0, 20.0), new(10.0, 30.0, 5.0), new(40.0, 40.0, 40.0));
        var bytes = new byte[81];

        using (var writer = new WkbWriter(bytes)) { writer.Write(lineString); }

        _ = await Assert.That(bytes).IsEquivalentTo(HelperFunctions.GetByteArrayFromResource("LineStringZ"));
    }

    [Test]
    public async Task WriteLineStringZM()
    {
        // LINESTRINGZM (30 10 20 15.0, 10 30 5 20.0, 40 40 40 4.0)
        var lineString = new PolylineZM(new(30.0, 10.0, 20.0, 15.0), new(10.0, 30.0, 5.0, 20.0), new(40.0, 40.0, 40.0, 40.0));
        var bytes = new byte[105];

        using (var writer = new WkbWriter(bytes)) { writer.Write(lineString); }

        _ = await Assert.That(bytes).IsEquivalentTo(HelperFunctions.GetByteArrayFromResource("LineStringZM"));
    }

    [Test]
    public async Task WriteMultiLineString()
    {
        // MULTILINESTRING ((10 10.0, 20 20.0, 10 4.0), (40 40.0, 30 30.0, 40 20.0, 30 1.0))
        var lines = new[] { new Polyline(new(10.0, 10.0), new(20.0, 20.0), new(10.0, 40.0)), new Polyline(new(40.0, 40.0), new(30.0, 30.0), new(40.0, 20.0), new(30.0, 10.0)) };
        var bytes = new byte[139];

        using (var writer = new WkbWriter(bytes)) { writer.Write(lines); }

        _ = await Assert.That(bytes).IsEquivalentTo(HelperFunctions.GetByteArrayFromResource("MultiLineString"));
    }

    [Test]
    public async Task WriteSimplePolygon()
    {
        var polygon = new Polygon([new(30.0, 10.0), new(10.0, 20.0), new(20.0, 40.0), new(40.0, 40.0), new Point(30.0, 10.0)]);
        var bytes = new byte[93];

        using (var writer = new WkbWriter(bytes)) { writer.Write(polygon); }

        _ = await Assert.That(bytes).IsEquivalentTo(HelperFunctions.GetByteArrayFromResource("SimplePolygon"));
    }

    [Test]
    public async Task WriteComplexPolygon()
    {
        var polygon = new Polygon
        {
            new([new(35.0, 10.0), new(10.0, 20.0), new(15.0, 40.0), new(45.0, 45.0), new(35.0, 10.0)]),
            new([new(20.0, 30.0), new(35.0, 35.0), new(30.0, 20.0), new(20.0, 30.0)])
        };

        var bytes = new byte[161];

        using (var writer = new WkbWriter(bytes)) { writer.Write(polygon); }

        _ = await Assert.That(bytes).IsEquivalentTo(HelperFunctions.GetByteArrayFromResource("ComplexPolygon"));
    }

    [Test]
    public async Task WriteSimpleMultiPolygon()
    {
        // MULTIPOLYGON (((30 20, 10 40, 45 40, 30 20)), ((15 5, 40 10, 10 20, 5 10, 15 5)))
        var polygons = new[] { new Polygon(new[] { new Point(30.0, 20.0), new Point(10.0, 40.0), new Point(45.0, 40.0), new Point(30.0, 20.0) }), new Polygon(new[] { new Point(15.0, 5.0), new Point(40.0, 10.0), new Point(10.0, 20.0), new Point(5.0, 10.0), new Point(15.0, 5.0) }) };
        var bytes = new byte[179];

        using (var writer = new WkbWriter(bytes)) { writer.Write(polygons); }

        _ = await Assert.That(bytes).IsEquivalentTo(HelperFunctions.GetByteArrayFromResource("SimpleMultiPolygon"));
    }

    [Test]
    public async Task WriteComplexMultiPolygon()
    {
        // MULTIPOLYGON (((40 40, 20 45, 45 30, 40 40)), ((20 35, 45 20, 30 5, 10 10, 10 30, 20 35), (30 20, 20 25, 20 15, 30 20)))
        var polygon = new Polygon
        {
            new([new(20.0, 35.0), new(45.0, 20.0), new(30.0, 5.0), new(10.0, 10.0), new(10.0, 30.0), new(20.0, 35.0)]),
            new([new(30.0, 20.0), new(20.0, 25.0), new(20.0, 15.0), new(30.0, 20.0)])
        };

        var polygons = new[] { new Polygon(new[] { new Point(40.0, 40.0), new Point(20.0, 45.0), new Point(45.0, 30.0), new Point(40.0, 40.0) }), polygon };

        var bytes = new byte[263];

        using (var writer = new WkbWriter(bytes)) { writer.Write(polygons); }

        _ = await Assert.That(bytes).IsEquivalentTo(HelperFunctions.GetByteArrayFromResource("ComplexMultiPolygon"));
    }
}