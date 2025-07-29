// -----------------------------------------------------------------------
// <copyright file="EwktRecordTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

public class EwktRecordTests
{
    [Test]
    [Arguments("SRID=-1;POINT (6 10)", -1, new[] { 6D, 10D })]
    [Arguments("SRID=-1;POINT(6 10)", -1, new[] { 6D, 10D })]
    [Arguments("SRID=-1;POINT ZM (1 1 5 60)", -1, new[] { 1D, 1D, 5D, 60D })]
    [Arguments("SRID=-1;POINT ZM(1 1 5 60)", -1, new[] { 1D, 1D, 5D, 60D })]
    [Arguments("SRID=-1;POINTZM (1 1 5 60)", -1, new[] { 1D, 1D, 5D, 60D })]
    [Arguments("SRID=-1;POINTZM(1 1 5 60)", -1, new[] { 1D, 1D, 5D, 60D })]
    [Arguments("SRID=-1;POINT M (1 1 80)", -1, new[] { 1D, 1D, 80D })]
    [Arguments("SRID=-1;POINT M(1 1 80)", -1, new[] { 1D, 1D, 80D })]
    [Arguments("SRID=-1;POINTM (1 1 80)", -1, new[] { 1D, 1D, 80D })]
    [Arguments("SRID=-1;POINTM(1 1 80)", -1, new[] { 1D, 1D, 80D })]
    [Arguments("SRID=-1;POINT Z (1 1 80)", -1, new[] { 1D, 1D, 80D })]
    [Arguments("SRID=-1;POINT Z(1 1 80)", -1, new[] { 1D, 1D, 80D })]
    [Arguments("SRID=-1;POINTZ (1 1 80)", -1, new[] { 1D, 1D, 80D })]
    [Arguments("SRID=-1;POINTZ(1 1 80)", -1, new[] { 1D, 1D, 80D })]
    [Arguments("SRID=-1;POINT EMPTY", -1, new[] { 0D, 0D })]
    [Arguments("SRID=-1;POINT(-1 -2)", -1, new[] { -1D, -2D })]
    [Arguments("SRID=12;POINT(10 20)", 12, new[] { 10D, 20D })]
    [Arguments("SRID   =  112   ;   POINT(10 20)", 112, new[] { 10D, 20D })]
    public async Task EwktToPoint(string wkt, int srid, double[] coordinates)
    {
        var record = new EwktRecord(wkt);
        _ = await Assert.That(record.GetSrid()).IsEqualTo(srid);
        _ = await Assert.That(GetCoordinates(record.GetGeometry())).IsEquivalentTo(coordinates);

        static double[] GetCoordinates(object point)
        {
            return point switch
            {
                PointZM pt => [pt.X, pt.Y, pt.Z, pt.Measurement],
                PointM pt => [pt.X, pt.Y, pt.Measurement],
                PointZ pt => [pt.X, pt.Y, pt.Z],
                Point pt => [pt.X, pt.Y],
                _ => throw new InvalidGeometryTypeException(),
            };
        }
    }

    [Test]
    [Arguments("SRID=-1;MULTIPOINT((3.5 5.6),(4.8 10.5))", -1, 2)]
    [Arguments("SRID=-1;MULTIPOINT EMPTY", -1, 0)]
    [Arguments("SRID=-1;MULTIPOINT(10 40, 40 30, 20 20, 30 10)", -1, 4)]
    public async Task EwktToMultiPoint(string wkt, int srid, int count)
    {
        var record = new EwktRecord(wkt);
        _ = await Assert.That(record.GetSrid()).IsEqualTo(srid);
        var points = await Assert.That(record.GetGeometry()).IsAssignableTo<System.Collections.IEnumerable>();
        _ = await Assert.That(points!.Cast<object>()).HasCount().EqualTo(count);
    }

    [Test]
    [Arguments("SRID=-1;LINESTRING(3 4,10 50,20 25)", -1, 3)]
    [Arguments("SRID=-1;LINESTRING EMPTY", -1, 0)]
    [Arguments("SRID=35234;LINESTRING(10 20, 40 50)", 35234, 2)]
    public async Task EwktToLineString(string wkt, int srid, int count)
    {
        var record = new EwktRecord(wkt);
        _ = await Assert.That(record.GetSrid()).IsEqualTo(srid);
        var lineString = record.GetLineString();
        _ = await Assert.That(lineString).IsNotNull().And.HasCount().EqualTo(count);
    }

    [Test]
    [Arguments("SRID=-1;MULTILINESTRING((3 4,10 50,20 25),(-5 -8,-10 -8,-15 -4))", -1, 2)]
    [Arguments("SRID=-1;MULTILINESTRING EMPTY", -1, 0)]
    public async Task EwktToMultiLineString(string wkt, int srid, int count)
    {
        var record = new EwktRecord(wkt);
        _ = await Assert.That(record.GetSrid()).IsEqualTo(srid);
        var lineStrings = record.GetMultiLineString();
        _ = await Assert.That(lineStrings).IsNotNull().And.HasCount().EqualTo(count);
    }

    [Test]
    [Arguments("SRID=-1;POLYGON((1 1,5 1,5 5,1 5,1 1),(2 2, 3 2, 3 3, 2 3,2 2))", -1, 2)]
    [Arguments("SRID=-1;POLYGON EMPTY", -1, 0)]
    [Arguments("SRID=-1;POLYGON ((35 10, 45 45, 15 40, 10 20, 35 10), (20 30, 35 35, 30 20, 20 30))", -1, 2)]
    public async Task EwktToPolygon(string wkt, int srid, int count)
    {
        var record = new EwktRecord(wkt);
        _ = await Assert.That(record.GetSrid()).IsEqualTo(srid);
        var polygon = record.GetPolygon();
        _ = await Assert.That(polygon).IsNotNull().And.HasCount().EqualTo(count);
    }

    [Test]
    [Arguments("SRID=-1;MULTIPOLYGON(((1 1,5 1,5 5,1 5,1 1),(2 2, 3 2, 3 3, 2 3,2 2)),((3 3,6 2,6 4,3 3)))", -1, 2)]
    [Arguments("SRID=-1;MULTIPOLYGON EMPTY", -1, 0)]
    public async Task EwktToMultiPolygon(string wkt, int srid, int count)
    {
        var record = new EwktRecord(wkt);
        _ = await Assert.That(record.GetSrid()).IsEqualTo(srid);
        var polygons = record.GetMultiPolygon();
        _ = await Assert.That(polygons).IsNotNull().And.HasCount().EqualTo(count);
    }
}