// -----------------------------------------------------------------------
// <copyright file="EwktRecordTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

public class EwktWriterTests
{
    [Test]
    public async Task PointToEwkt()
    {
        _ = await Assert.That(Write(writer => writer.Write(new Point(30.0, 10.0), 4326))).IsEqualTo("SRID=4326;POINT (30 10)");
    }

    [Test]
    public async Task Point3DToWkt()
    {
        _ = await Assert.That(Write(writer => writer.Write(new PointZ(1.0, 1.0, 80.0), 4326))).IsEqualTo("SRID=4326;POINT Z (1 1 80)");
    }

    [Test]
    public async Task Point4DToWkt()
    {
        _ = await Assert.That(Write(writer => writer.Write(new PointZM(1.0, 1.0, 5.0, 60.0), 4326))).IsEqualTo("SRID=4326;POINT ZM (1 1 5 60)");
    }

    [Test]
    public async Task NullPointToWkt()
    {
        _ = await Assert.That(Write(writer => writer.Write(Point.Empty, 4326))).IsEqualTo("SRID=4326;POINT EMPTY");
    }

    [Test]
    public async Task MultiPointToWkt()
    {
        _ = await Assert.That(Write(writer => writer.Write([new(10.0, 40.0), new(40.0, 30.0), new(20.0, 20.0), new Point(30.0, 10.0)], 4326))).IsEqualTo("SRID=4326;MULTIPOINT (10 40, 40 30, 20 20, 30 10)");
    }

    [Test]
    public async Task LineStringToWkt()
    {
        _ = await Assert.That(Write(writer => writer.Write(new Polyline(new Point(30.0, 10.0), new Point(10.0, 30.0), new Point(40.0, 40.0)), 4326))).IsEqualTo("SRID=4326;LINESTRING (30 10, 10 30, 40 40)");
    }

    [Test]
    public async Task MultiLineStringToWkt()
    {
        _ = await Assert.That(Write(writer => writer.Write([new Polyline(new Point(10.0, 10.0), new Point(20.0, 20.0), new Point(10.0, 40.0)), new Polyline(new Point(40.0, 40.0), new Point(30.0, 30.0), new Point(40.0, 20.0), new Point(30.0, 10.0))], 4326))).IsEqualTo("SRID=4326;MULTILINESTRING ((10 10, 20 20, 10 40), (40 40, 30 30, 40 20, 30 10))");
    }

    [Test]
    public async Task PolygonToWkt()
    {
        _ = await Assert.That(Write(writer => writer.Write(Polygon.FromPoints(new Point(30.0, 10.0), new Point(40.0, 40.0), new Point(20.0, 40.0), new Point(10.0, 20.0), new Point(30.0, 10.0)), 4326))).IsEqualTo("SRID=4326;POLYGON ((30 10, 40 40, 20 40, 10 20, 30 10))");
    }

    [Test]
    public async Task PolygonWithHoleToWkt()
    {
        _ = await Assert.That(Write(writer => writer.Write(
            new Polygon(
                [new(35.0, 10.0), new(45.0, 45.0), new(15.0, 40.0), new(10.0, 20.0), new(35.0, 10.0)],
                [new(20.0, 30.0), new(35.0, 35.0), new(30.0, 20.0), new(20.0, 30.0)]),
            srid: 4326)))
            .IsEqualTo("SRID=4326;POLYGON ((35 10, 45 45, 15 40, 10 20, 35 10), (20 30, 35 35, 30 20, 20 30))");
    }

    [Test]
    public async Task MultiPolygonToWkt()
    {
        _ = await Assert.That(Write(writer => writer.Write(
                polygons: [new Polygon(new Point[] { new(30.0, 20.0), new(45.0, 40.0), new(10.0, 40.0), new(30.0, 20.0) }), new Polygon(new Point[] { new(15.0, 5.0), new(40.0, 10.0), new(10.0, 20.0), new(5.0, 10.0), new(15.0, 5.0) })], 
                srid: 4326)))
            .IsEqualTo("SRID=4326;MULTIPOLYGON (((30 20, 45 40, 10 40, 30 20)), ((15 5, 40 10, 10 20, 5 10, 15 5)))");
    }

    [Test]
    public async Task MultiPolygonWithHoleToWkt()
    {
        _ = await Assert.That(Write(writer => writer.Write(
            polygons:
            [
                new Polygon(new Point[] { new(40.0, 40.0), new(20.0, 45.0), new(45.0, 30.0), new(40.0, 40.0) }),
                new Polygon(
                    new Point[] { new(20.0, 35.0), new(10.0, 30.0), new(10.0, 10.0), new(30.0, 5.0), new(45.0, 20.0), new(20.0, 35.0) },
                    [new(30.0, 20.0), new(20.0, 15.0), new(20.0, 25.0), new(30.0, 20.0)])
            ],
            srid: 4326)))
            .IsEqualTo("SRID=4326;MULTIPOLYGON (((40 40, 20 45, 45 30, 40 40)), ((20 35, 10 30, 10 10, 30 5, 45 20, 20 35), (30 20, 20 15, 20 25, 30 20)))");
    }

    private static string Write(Action<EwktWriter> action)
    {
        var builder = new System.Text.StringBuilder();
        var writer = new EwktWriter(builder);
        action(writer);
        return builder.ToString();
    }
}