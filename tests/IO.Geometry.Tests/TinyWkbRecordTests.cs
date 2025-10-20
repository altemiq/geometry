// -----------------------------------------------------------------------
// <copyright file="TinyWkbRecordTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using TUnit.Assertions.AssertConditions.Throws;

namespace Altemiq.IO.Geometry;

public class TinyWkbRecordTests
{
    [Test]
    public async Task GetEmptyPoint()
    {
        // POINT EMPTY
        using var record = new TinyWkbRecord(FromPostGis("\\x0110"));
        var point = record.GetPoint();
        _ = await Assert.That(point).IsEquivalentTo(Point.Empty);
    }

    [Test]
    public async Task GetEmptyPointWithSize()
    {
        using var record = new TinyWkbRecord(FromPostGis("\\x011200"));
        var point = record.GetPoint();
        _ = await Assert.That(point).IsEquivalentTo(Point.Empty);
    }

    [Test]
    public async Task GetPoint()
    {
        using var record = new TinyWkbRecord(FromPostGis("\\x01003c14"));
        var point = record.GetPoint();
        _ = await Assert.That(point.X).IsEqualTo(30);
        _ = await Assert.That(point.Y).IsEqualTo(10);
    }

    [Test]
    public async Task GetEmptyPointZ()
    {
        using var record = new TinyWkbRecord(FromPostGis("\\x011801"));
        var point = record.GetPointZ();
        _ = await Assert.That(point).IsEquivalentTo(PointZ.Empty);
    }

    [Test]
    public async Task GetEmptyPointM()
    {
        using var record = new TinyWkbRecord(FromPostGis("\\x011802"));
        var point = record.GetPointM();
        _ = await Assert.That(point).IsEquivalentTo(PointM.Empty);
    }

    [Test]
    public async Task GetPointZ()
    {
        using var record = new TinyWkbRecord(FromPostGis("\\x0108013c1428"));
        var point = record.GetPointZ();
        _ = await Assert.That(point.X).IsEqualTo(30);
        _ = await Assert.That(point.Y).IsEqualTo(10);
        _ = await Assert.That(point.Z).IsEqualTo(20);
    }

    [Test]
    public async Task GetPointM()
    {
        using var record = new TinyWkbRecord(FromPostGis("\\x0108023c1428"));
        var point = record.GetPointM();
        _ = await Assert.That(point.X).IsEqualTo(30);
        _ = await Assert.That(point.Y).IsEqualTo(10);
        _ = await Assert.That(point.Measurement).IsEqualTo(20);
    }

    [Test]
    public async Task GetEmptyPointZM()
    {
        // POINTZM EMPTY
        using var record = new TinyWkbRecord(FromPostGis("\\x011803"));
        var point = record.GetPointZM();
        _ = await Assert.That(point).IsEquivalentTo(PointZM.Empty);
    }

    [Test]
    public async Task GetPointZM()
    {
        // POINTZM (30 10 20 15)
        using var reader = new TinyWkbRecord(FromPostGis("\\x0108033c14281e"));
        var point = reader.GetPointZM();
        _ = await Assert.That(point.X).IsEqualTo(30);
        _ = await Assert.That(point.Y).IsEqualTo(10);
        _ = await Assert.That(point.Z).IsEqualTo(20);
        _ = await Assert.That(point.Measurement).IsEqualTo(15);
    }

    [Test]
    public async Task GetEmptyLineString()
    {
        // LINESTRING EMPTY
        using var record = new TinyWkbRecord(FromPostGis("\\x0210"));
        var lineString = record.GetLineString();
        _ = await Assert.That(lineString).IsNotNull().And.HasCount().EqualTo(0);
    }

    [Test]
    public async Task GetLineString()
    {
        // LINESTRING (30.1 10.2, 10.3 30.4, 40.5 40.6)
        using var reader = new TinyWkbRecord(FromPostGis("\\x220003da04cc018b039403dc04cc01"));
        var lineString = reader.GetLineString();

        _ = await Assert.That(lineString[0].X).IsEqualTo(30.1);
        _ = await Assert.That(lineString[0].Y).IsEqualTo(10.2);

        _ = await Assert.That(lineString[1].X).IsEqualTo(10.3);
        _ = await Assert.That(lineString[1].Y).IsEqualTo(30.4);

        _ = await Assert.That(lineString[2].X).IsEqualTo(40.5);
        _ = await Assert.That(lineString[2].Y).IsEqualTo(40.6);
    }

    [Test]
    public async Task GetLineStringZ()
    {
        // LINESTRINGZ (30.1 10.2 20.3, 10.4 30.5 5.6, 40.7 40.8 40.9)
        using var reader = new TinyWkbRecord(FromPostGis("\\x22080503da04cc01960389039603a502de04ce01c205"));
        var lineString = reader.GetLineStringZ();

        _ = await Assert.That(lineString[0].X).IsEqualTo(30.1);
        _ = await Assert.That(lineString[0].Y).IsEqualTo(10.2);
        _ = await Assert.That(lineString[0].Z).IsEqualTo(20.3);

        _ = await Assert.That(lineString[1].X).IsEqualTo(10.4);
        _ = await Assert.That(lineString[1].Y).IsEqualTo(30.5);
        _ = await Assert.That(lineString[1].Z).IsEqualTo(5.6);

        _ = await Assert.That(lineString[2].X).IsEqualTo(40.7);
        _ = await Assert.That(lineString[2].Y).IsEqualTo(40.8);
        _ = await Assert.That(lineString[2].Z).IsEqualTo(40.9);
    }
    [Test]
    public async Task GetLineStringZM()
    {
        // LINESTRINGZM (30.1 10.2 20.3 15.4, 10.5 30.6 5.7 20.8, 40.9 40.0 40.1 40.2)
        using var reader = new TinyWkbRecord(FromPostGis("\\x22082703da04cc019603b40287039803a3026ce004bc01b0058403"));
        var lineString = reader.GetLineStringZM();

        _ = await Assert.That(lineString[0].X).IsEqualTo(30.1);
        _ = await Assert.That(lineString[0].Y).IsEqualTo(10.2);
        _ = await Assert.That(lineString[0].Z).IsEqualTo(20.3);
        _ = await Assert.That(lineString[0].Measurement).IsEqualTo(15.4);

        _ = await Assert.That(lineString[1].X).IsEqualTo(10.5);
        _ = await Assert.That(lineString[1].Y).IsEqualTo(30.6);
        _ = await Assert.That(lineString[1].Z).IsEqualTo(5.7);
        _ = await Assert.That(lineString[1].Measurement).IsEqualTo(20.8);

        _ = await Assert.That(lineString[2].X).IsEqualTo(40.9);
        _ = await Assert.That(lineString[2].Y).IsEqualTo(40.0);
        _ = await Assert.That(lineString[2].Z).IsEqualTo(40.1);
        _ = await Assert.That(lineString[2].Measurement).IsEqualTo(40.2);
    }

    [Test]
    public async Task GetEmptyPolygon()
    {
        // POLYGON EMPTY
        using var record = new TinyWkbRecord(FromPostGis("\\x0310"));
        var polygon = record.GetPolygon();
        _ = await Assert.That(polygon).HasCount().EqualTo(0);
    }

    [Test]
    public async Task GetPolygonWithSizeAndBBox()
    {
        using var reader = new TinyWkbRecord(FromPostGis("\\x23031dce01e004cc01e4040105da04cc018b03cc01cc019403940304d301e304"));
        await Assert.That(reader.GetPolygon).ThrowsNothing().And.IsNotEmpty();
    }

    [Test]
    public async Task GetPolygonZWithSizeAndBBox()
    {
        using var reader = new TinyWkbRecord(FromPostGis("\\x230b252ad001d004cc01e40496038e030105da04cc0196038903ce01ce01ce0196030682030dba01c501d5048d03"));
        await Assert.That(reader.GetPolygonZ).ThrowsNothing().And.IsNotEmpty();
    }

    [Test]
    public async Task GetPolygonMWithSizeAndBBox()
    {
        using var reader = new TinyWkbRecord(FromPostGis("\\x230b262ad001d004cc01e40496038e030105da04cc0196038903ce01ce01ce0196030682030dba01c501d5048d03"));
        await Assert.That(reader.GetPolygonM).ThrowsNothing().And.IsNotEmpty();
    }

    [Test]
    public async Task GetPolygonZMWithSizeAndBBox()
    {
        using var reader = new TinyWkbRecord(FromPostGis("\\x230b2737d201d404cc01dc0496039403b402b8050105da04cc019603b4028703d001d00108d00184030bb005840308d001bf01cb01db049303f703"));
        await Assert.That(reader.GetPolygonZM).ThrowsNothing().And.IsNotEmpty();
    }

    [Test]
    public async Task GetPolygonSimple()
    {
        // POLYGON ((30.1 10.2, 10.3 20.4, 20.5 40.6, 40.7 40.8, 30.1 10.2))
        using var reader = new TinyWkbRecord(FromPostGis("\\x23000105da04cc018b03cc01cc019403940304d301e304"));
        var polygon = reader.GetPolygon();
        _ = await Assert.That(polygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(1);

        var linearRing = polygon[0];
        _ = await Assert.That(linearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(5);

        _ = await Assert.That(linearRing[0].X).IsEqualTo(30.1);
        _ = await Assert.That(linearRing[0].Y).IsEqualTo(10.2);

        _ = await Assert.That(linearRing[1].X).IsEqualTo(10.3);
        _ = await Assert.That(linearRing[1].Y).IsEqualTo(20.4);

        _ = await Assert.That(linearRing[2].X).IsEqualTo(20.5);
        _ = await Assert.That(linearRing[2].Y).IsEqualTo(40.6);

        _ = await Assert.That(linearRing[3].X).IsEqualTo(40.7);
        _ = await Assert.That(linearRing[3].Y).IsEqualTo(40.8);

        _ = await Assert.That(linearRing[4].X).IsEqualTo(30.1);
        _ = await Assert.That(linearRing[4].Y).IsEqualTo(10.2);
    }

    [Test]
    public async Task GetPolygonZSimple()
    {
        // POLYGONZ ((30.1 10.2 20.3, 10.4 20.5 30.6, 20.7 40.8 30.9, 40.0 40.1 40.2, 30.1 10.2 20.3))
        using var reader = new TinyWkbRecord(FromPostGis("\\x2308050105da04cc0196038903ce01ce01ce0196030682030dba01c501d5048d03"));
        var polygon = reader.GetPolygonZ();
        _ = await Assert.That(polygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(1);

        var linearRing = polygon[0];
        _ = await Assert.That(linearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(5);

        _ = await Assert.That(linearRing[0].X).IsEqualTo(30.1);
        _ = await Assert.That(linearRing[0].Y).IsEqualTo(10.2);
        _ = await Assert.That(linearRing[0].Z).IsEqualTo(20.3);

        _ = await Assert.That(linearRing[1].X).IsEqualTo(10.4);
        _ = await Assert.That(linearRing[1].Y).IsEqualTo(20.5);
        _ = await Assert.That(linearRing[1].Z).IsEqualTo(30.6);

        _ = await Assert.That(linearRing[2].X).IsEqualTo(20.7);
        _ = await Assert.That(linearRing[2].Y).IsEqualTo(40.8);
        _ = await Assert.That(linearRing[2].Z).IsEqualTo(30.9);

        _ = await Assert.That(linearRing[3].X).IsEqualTo(40.0);
        _ = await Assert.That(linearRing[3].Y).IsEqualTo(40.1);
        _ = await Assert.That(linearRing[3].Z).IsEqualTo(40.2);

        _ = await Assert.That(linearRing[4].X).IsEqualTo(30.1);
        _ = await Assert.That(linearRing[4].Y).IsEqualTo(10.2);
        _ = await Assert.That(linearRing[4].Z).IsEqualTo(20.3);
    }

    [Test]
    public async Task GetPolygonZMSimple()
    {
        // POLYGONZM ((30.1 10.2 20.3 15.4, 10.5 20.6 30.7 15.8, 20.9 40.0 30.1 50.2, 40.3 40.4 40.5 40.6, 30.1 10.2 20.3 15.4))
        using var reader = new TinyWkbRecord(FromPostGis("\\x2308270105da04cc019603b4028703d001d00108d00184030bb005840308d001bf01cb01db049303f703"));
        var polygon = reader.GetPolygonZM();
        _ = await Assert.That(polygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(1);

        var linearRing = polygon[0];
        _ = await Assert.That(linearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(5);

        _ = await Assert.That(linearRing[0].X).IsEqualTo(30.1);
        _ = await Assert.That(linearRing[0].Y).IsEqualTo(10.2);
        _ = await Assert.That(linearRing[0].Z).IsEqualTo(20.3);
        _ = await Assert.That(linearRing[0].Measurement).IsEqualTo(15.4);

        _ = await Assert.That(linearRing[1].X).IsEqualTo(10.5);
        _ = await Assert.That(linearRing[1].Y).IsEqualTo(20.6);
        _ = await Assert.That(linearRing[1].Z).IsEqualTo(30.7);
        _ = await Assert.That(linearRing[1].Measurement).IsEqualTo(15.8);

        _ = await Assert.That(linearRing[2].X).IsEqualTo(20.9);
        _ = await Assert.That(linearRing[2].Y).IsEqualTo(40.0);
        _ = await Assert.That(linearRing[2].Z).IsEqualTo(30.1);
        _ = await Assert.That(linearRing[2].Measurement).IsEqualTo(50.2);

        _ = await Assert.That(linearRing[3].X).IsEqualTo(40.3);
        _ = await Assert.That(linearRing[3].Y).IsEqualTo(40.4);
        _ = await Assert.That(linearRing[3].Z).IsEqualTo(40.5);
        _ = await Assert.That(linearRing[3].Measurement).IsEqualTo(40.6);

        _ = await Assert.That(linearRing[4].X).IsEqualTo(30.1);
        _ = await Assert.That(linearRing[4].Y).IsEqualTo(10.2);
        _ = await Assert.That(linearRing[4].Z).IsEqualTo(20.3);
        _ = await Assert.That(linearRing[4].Measurement).IsEqualTo(15.4);
    }

    [Test]
    public async Task GetPolygonComplex()
    {
        // POLYGON ((35.1 10.2, 10.3 20.4, 15.5 40.6, 45.7 45.8, 35.1 10.2), (20.1 30.2, 35.3 35.4, 30.5 20.6, 20.1 30.2))
        using var reader = new TinyWkbRecord(FromPostGis("\\x23000205be05cc01ef03cc01689403dc0468d301c70504ab029003b002685fa702cf01c001"));
        var polygon = reader.GetPolygon();
        _ = await Assert.That(polygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(2);

        var firstLinearRing = polygon[0];
        _ = await Assert.That(firstLinearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(5);

        _ = await Assert.That(firstLinearRing[0].X).IsEqualTo(35.1);
        _ = await Assert.That(firstLinearRing[0].Y).IsEqualTo(10.2);

        _ = await Assert.That(firstLinearRing[1].X).IsEqualTo(10.3);
        _ = await Assert.That(firstLinearRing[1].Y).IsEqualTo(20.4);

        _ = await Assert.That(firstLinearRing[2].X).IsEqualTo(15.5);
        _ = await Assert.That(firstLinearRing[2].Y).IsEqualTo(40.6);

        _ = await Assert.That(firstLinearRing[3].X).IsEqualTo(45.7);
        _ = await Assert.That(firstLinearRing[3].Y).IsEqualTo(45.8);

        _ = await Assert.That(firstLinearRing[4].X).IsEqualTo(35.1);
        _ = await Assert.That(firstLinearRing[4].Y).IsEqualTo(10.2);

        var secondLinearRing = polygon[1];
        _ = await Assert.That(secondLinearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(4);

        _ = await Assert.That(secondLinearRing[0].X).IsEqualTo(20.1);
        _ = await Assert.That(secondLinearRing[0].Y).IsEqualTo(30.2);

        _ = await Assert.That(secondLinearRing[1].X).IsEqualTo(35.3);
        _ = await Assert.That(secondLinearRing[1].Y).IsEqualTo(35.4);

        _ = await Assert.That(secondLinearRing[2].X).IsEqualTo(30.5);
        _ = await Assert.That(secondLinearRing[2].Y).IsEqualTo(20.6);

        _ = await Assert.That(secondLinearRing[3].X).IsEqualTo(20.1);
        _ = await Assert.That(secondLinearRing[3].Y).IsEqualTo(30.2);
    }

    [Test]
    public async Task GetPolygonZComplex()
    {
        // POLYGONZ ((35.1 10.2 15.3, 10.4 20.5 30.6, 15.7 40.8 25.9, 45.0 45.1 45.2, 35.1 10.2 15.3), (20.1 30.2 10.3, 35.4 35.5 35.6, 30.7 20.8 25.9, 20.1 30.2 25.3))
        using var reader = new TinyWkbRecord(FromPostGis("\\x2308250205be05cc01b202ed03ce01b2026a96035dca04568203c501b905d50404ab02900363b2026afa035da502c101d301bc010b"));
        var polygon = reader.GetPolygonZ();
        _ = await Assert.That(polygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(2);

        var linearRing = polygon[0];
        _ = await Assert.That(linearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(5);
        var firstLinearRing = polygon[0];
        _ = await Assert.That(firstLinearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(5);

        _ = await Assert.That(firstLinearRing[0].X).IsEqualTo(35.1);
        _ = await Assert.That(firstLinearRing[0].Y).IsEqualTo(10.2);
        _ = await Assert.That(firstLinearRing[0].Z).IsEqualTo(15.3);

        _ = await Assert.That(firstLinearRing[1].X).IsEqualTo(10.4);
        _ = await Assert.That(firstLinearRing[1].Y).IsEqualTo(20.5);
        _ = await Assert.That(firstLinearRing[1].Z).IsEqualTo(30.6);

        _ = await Assert.That(firstLinearRing[2].X).IsEqualTo(15.7);
        _ = await Assert.That(firstLinearRing[2].Y).IsEqualTo(40.8);
        _ = await Assert.That(firstLinearRing[2].Z).IsEqualTo(25.9);

        _ = await Assert.That(firstLinearRing[3].X).IsEqualTo(45.0);
        _ = await Assert.That(firstLinearRing[3].Y).IsEqualTo(45.1);
        _ = await Assert.That(firstLinearRing[3].Z).IsEqualTo(45.2);

        _ = await Assert.That(firstLinearRing[4].X).IsEqualTo(35.1);
        _ = await Assert.That(firstLinearRing[4].Y).IsEqualTo(10.2);
        _ = await Assert.That(firstLinearRing[4].Z).IsEqualTo(15.3);

        var secondLinearRing = polygon[1];
        _ = await Assert.That(secondLinearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(4);

        _ = await Assert.That(secondLinearRing[0].X).IsEqualTo(20.1);
        _ = await Assert.That(secondLinearRing[0].Y).IsEqualTo(30.2);
        _ = await Assert.That(secondLinearRing[0].Z).IsEqualTo(10.3);

        _ = await Assert.That(secondLinearRing[1].X).IsEqualTo(35.4);
        _ = await Assert.That(secondLinearRing[1].Y).IsEqualTo(35.5);
        _ = await Assert.That(secondLinearRing[1].Z).IsEqualTo(35.6);

        _ = await Assert.That(secondLinearRing[2].X).IsEqualTo(30.7);
        _ = await Assert.That(secondLinearRing[2].Y).IsEqualTo(20.8);
        _ = await Assert.That(secondLinearRing[2].Z).IsEqualTo(25.9);

        _ = await Assert.That(secondLinearRing[3].X).IsEqualTo(20.1);
        _ = await Assert.That(secondLinearRing[3].Y).IsEqualTo(30.2);
        _ = await Assert.That(secondLinearRing[3].Z).IsEqualTo(25.3);
    }

    /// <summary>
    /// Reads the complex polygon with Z and M values.
    /// </summary>
    [Test]
    public async Task GetPolygonZMComplex()
    {
        // POLYGONZM ((35.1 10.2 15.3 25.4, 10.5 20.6 30.7 40.8, 15.9 40.0 25.1 30.2, 45.3 45.4 45.5 45.6, 35.1 10.2 15.3 20.4), (20.1 30.2 10.3 40.4, 35.5 35.6 35.7 35.8, 30.9 20.0 25.1 20.2, 20.1 30.2 25.3 35.4))
        using var reader = new TinyWkbRecord(FromPostGis("\\x2308270205be05cc01b202fc03eb03d001b402b4026c84036fd301cc046c9803b402cb01bf05db04f70304ab029003639003b4026cfc035b5bb702d301b702d701cc0104b002"));
        var polygon = reader.GetPolygonZM();
        _ = await Assert.That(polygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(2);

        var linearRing = polygon[0];
        _ = await Assert.That(linearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(5);
        var firstLinearRing = polygon[0];
        _ = await Assert.That(firstLinearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(5);

        _ = await Assert.That(firstLinearRing[0].X).IsEqualTo(35.1);
        _ = await Assert.That(firstLinearRing[0].Y).IsEqualTo(10.2);
        _ = await Assert.That(firstLinearRing[0].Z).IsEqualTo(15.3);
        _ = await Assert.That(firstLinearRing[0].Measurement).IsEqualTo(25.4);

        _ = await Assert.That(firstLinearRing[1].X).IsEqualTo(10.5);
        _ = await Assert.That(firstLinearRing[1].Y).IsEqualTo(20.6);
        _ = await Assert.That(firstLinearRing[1].Z).IsEqualTo(30.7);
        _ = await Assert.That(firstLinearRing[1].Measurement).IsEqualTo(40.8);

        _ = await Assert.That(firstLinearRing[2].X).IsEqualTo(15.9);
        _ = await Assert.That(firstLinearRing[2].Y).IsEqualTo(40.0);
        _ = await Assert.That(firstLinearRing[2].Z).IsEqualTo(25.1);
        _ = await Assert.That(firstLinearRing[2].Measurement).IsEqualTo(30.2);

        _ = await Assert.That(firstLinearRing[3].X).IsEqualTo(45.3);
        _ = await Assert.That(firstLinearRing[3].Y).IsEqualTo(45.4);
        _ = await Assert.That(firstLinearRing[3].Z).IsEqualTo(45.5);
        _ = await Assert.That(firstLinearRing[3].Measurement).IsEqualTo(45.6);

        _ = await Assert.That(firstLinearRing[4].X).IsEqualTo(35.1);
        _ = await Assert.That(firstLinearRing[4].Y).IsEqualTo(10.2);
        _ = await Assert.That(firstLinearRing[4].Z).IsEqualTo(15.3);
        _ = await Assert.That(firstLinearRing[4].Measurement).IsEqualTo(20.4);

        var secondLinearRing = polygon[1];
        _ = await Assert.That(secondLinearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(4);

        _ = await Assert.That(secondLinearRing[0].X).IsEqualTo(20.1);
        _ = await Assert.That(secondLinearRing[0].Y).IsEqualTo(30.2);
        _ = await Assert.That(secondLinearRing[0].Z).IsEqualTo(10.3);
        _ = await Assert.That(secondLinearRing[0].Measurement).IsEqualTo(40.4);

        _ = await Assert.That(secondLinearRing[1].X).IsEqualTo(35.5);
        _ = await Assert.That(secondLinearRing[1].Y).IsEqualTo(35.6);
        _ = await Assert.That(secondLinearRing[1].Z).IsEqualTo(35.7);
        _ = await Assert.That(secondLinearRing[1].Measurement).IsEqualTo(35.8);

        _ = await Assert.That(secondLinearRing[2].X).IsEqualTo(30.9);
        _ = await Assert.That(secondLinearRing[2].Y).IsEqualTo(20.0);
        _ = await Assert.That(secondLinearRing[2].Z).IsEqualTo(25.1);
        _ = await Assert.That(secondLinearRing[2].Measurement).IsEqualTo(20.2);

        _ = await Assert.That(secondLinearRing[3].X).IsEqualTo(20.1);
        _ = await Assert.That(secondLinearRing[3].Y).IsEqualTo(30.2);
        _ = await Assert.That(secondLinearRing[3].Z).IsEqualTo(25.3);
        _ = await Assert.That(secondLinearRing[3].Measurement).IsEqualTo(35.4);
    }

    [Test]
    public async Task GetEmptyMultiPoint()
    {
        // MULTIPOINT EMPTY
        using var record = new TinyWkbRecord(FromPostGis("\\x0410"));
        _ = await Assert.That(record.GetMultiPoint()).IsEmpty();
    }

    [Test]
    public async Task GetEmptyMultiPointZ()
    {
        using var record = new TinyWkbRecord(FromPostGis("\\x041801"));
        _ = await Assert.That(record.GetMultiPointZ()).IsEmpty();
    }

    [Test]
    public async Task GetEmptyMultiPointM()
    {
        using var record = new TinyWkbRecord(FromPostGis("\\x041802"));
        _ = await Assert.That(record.GetMultiPointM()).IsEmpty();
    }

    [Test]
    public async Task GetEmptyMultiPointZM()
    {
        // MULTIPOINTZM EMPTY
        using var record = new TinyWkbRecord(FromPostGis("\\x041803"));
        _ = await Assert.That(record.GetMultiPointZM()).IsEmpty();
    }

    [Test]
    public async Task GetMultiPoint()
    {
        // MULTIPOINT (10.1 40.2, 40.3 30.4, 20.5 20.6, 30.7 10.8)
        using var reader = new TinyWkbRecord(FromPostGis("\\x240004ca01a406dc04c3018b03c301cc01c301"));
        var multiPoint = reader.GetMultiPoint().ToArray();

        var firstPoint = multiPoint[0];
        _ = await Assert.That(firstPoint.X).IsEqualTo(10.1);
        _ = await Assert.That(firstPoint.Y).IsEqualTo(40.2);

        var secondPoint = multiPoint[1];
        _ = await Assert.That(secondPoint.X).IsEqualTo(40.3);
        _ = await Assert.That(secondPoint.Y).IsEqualTo(30.4);

        var thirdPoint = multiPoint[2];
        _ = await Assert.That(thirdPoint.X).IsEqualTo(20.5);
        _ = await Assert.That(thirdPoint.Y).IsEqualTo(20.6);

        var forthPoint = multiPoint[3];
        _ = await Assert.That(forthPoint.X).IsEqualTo(30.7);
        _ = await Assert.That(forthPoint.Y).IsEqualTo(10.8);
    }

    [Test]
    public async Task GetMultiPointZMWithIdList()
    {
        using var reader = new TinyWkbRecord(FromPostGis("\\x840f4b50e4f304b8d10fa8960498be06e6a001a68301e6cf02ac7f0514283c5064e4f304acda08eaa102c2ec02aee40683c404b113d062a488038e17d415a53698d2038aa706877b8549ce9202fdb7019d08901f"));
        _ = await Assert.That(reader.GetMultiPointZM())
            .IsNotEmpty();
    }

    [Test]
    public async Task GetEmptyMultiLineString()
    {
        using var reader = new TinyWkbRecord(FromPostGis("\\x0510"));
        _ = await Assert.That(reader.GetMultiLineString())
            .IsEmpty();
    }

    [Test]
    public async Task GetMultiLineString()
    {
        // MULTILINESTRING ((10.1 10.2, 20.3 20.4, 10.5 40.6), (40.7 40.8, 30.9 30.0, 40.1 20.2, 30.3 10.4))
        using var reader = new TinyWkbRecord(FromPostGis("\\x25000203ca01cc01cc01cc01c301940304dc0404c301d701b801c301c301c301"));
        var lines = reader.GetMultiLineString().ToArray();
        _ = await Assert.That(lines).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(2);

        var firstLine = lines[0];
        _ = await Assert.That(firstLine).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(3);

        _ = await Assert.That(firstLine[0].X).IsEqualTo(10.1);
        _ = await Assert.That(firstLine[0].Y).IsEqualTo(10.2);

        _ = await Assert.That(firstLine[1].X).IsEqualTo(20.3);
        _ = await Assert.That(firstLine[1].Y).IsEqualTo(20.4);

        _ = await Assert.That(firstLine[2].X).IsEqualTo(10.5);
        _ = await Assert.That(firstLine[2].Y).IsEqualTo(40.6);

        // second line
        var secondLine = lines[1];
        _ = await Assert.That(secondLine).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(4);

        _ = await Assert.That(secondLine[0].X).IsEqualTo(40.7);
        _ = await Assert.That(secondLine[0].Y).IsEqualTo(40.8);

        _ = await Assert.That(secondLine[1].X).IsEqualTo(30.9);
        _ = await Assert.That(secondLine[1].Y).IsEqualTo(30.0);

        _ = await Assert.That(secondLine[2].X).IsEqualTo(40.1);
        _ = await Assert.That(secondLine[2].Y).IsEqualTo(20.2);

        _ = await Assert.That(secondLine[3].X).IsEqualTo(30.3);
        _ = await Assert.That(secondLine[3].Y).IsEqualTo(10.4);
    }

    [Test]
    public async Task GetEmptyMultiPolygon()
    {
        using var reader = new TinyWkbRecord(FromPostGis("\\x0610"));
        _ = await Assert.That(reader.GetMultiPolygon())
            .IsEmpty();
    }

    [Test]
    public async Task GetMultiPolygonSimple()
    {
        // MULTIPOLYGON (((30.1 20.2, 10.3 40.4, 45.5 40.6, 30.1 20.2)), ((15.1 5.2, 40.3 10.4, 10.5 20.6, 5.7 10.8, 15.1 5.2)))
        using var reader = new TinyWkbRecord(FromPostGis("\\x2600020104da0494038b039403c00504b30297030105ab02ab02f80368d304cc015fc301bc016f"));
        var polygons = reader.GetMultiPolygon().ToArray();
        _ = await Assert.That(polygons).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(2);

        var firstPolygon = polygons[0];
        _ = await Assert.That(firstPolygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(1);

        // first ring
        var firstPolygonLinearRing = firstPolygon[0];
        _ = await Assert.That(firstPolygonLinearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(4);

        _ = await Assert.That(firstPolygonLinearRing[0].X).IsEqualTo(30.1);
        _ = await Assert.That(firstPolygonLinearRing[0].Y).IsEqualTo(20.2);

        _ = await Assert.That(firstPolygonLinearRing[1].X).IsEqualTo(10.3);
        _ = await Assert.That(firstPolygonLinearRing[1].Y).IsEqualTo(40.4);

        _ = await Assert.That(firstPolygonLinearRing[2].X).IsEqualTo(45.5);
        _ = await Assert.That(firstPolygonLinearRing[2].Y).IsEqualTo(40.6);

        _ = await Assert.That(firstPolygonLinearRing[3].X).IsEqualTo(30.1);
        _ = await Assert.That(firstPolygonLinearRing[3].Y).IsEqualTo(20.2);

        // second polygon
        var secondPolygon = polygons[1];
        _ = await Assert.That(secondPolygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(1);

        // first ring
        var secondPolygonLinearRing = secondPolygon[0];
        _ = await Assert.That(secondPolygonLinearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(5);

        _ = await Assert.That(secondPolygonLinearRing[0].X).IsEqualTo(15.1);
        _ = await Assert.That(secondPolygonLinearRing[0].Y).IsEqualTo(5.2);

        _ = await Assert.That(secondPolygonLinearRing[1].X).IsEqualTo(40.3);
        _ = await Assert.That(secondPolygonLinearRing[1].Y).IsEqualTo(10.4);

        _ = await Assert.That(secondPolygonLinearRing[2].X).IsEqualTo(10.5);
        _ = await Assert.That(secondPolygonLinearRing[2].Y).IsEqualTo(20.6);

        _ = await Assert.That(secondPolygonLinearRing[3].X).IsEqualTo(5.7);
        _ = await Assert.That(secondPolygonLinearRing[3].Y).IsEqualTo(10.8);

        _ = await Assert.That(secondPolygonLinearRing[4].X).IsEqualTo(15.1);
        _ = await Assert.That(secondPolygonLinearRing[4].Y).IsEqualTo(5.2);
    }

    [Test]
    public async Task GetMultiPolygonComplex()
    {
        // MULTIPOLYGON (((40 40, 20 45, 45 30, 40 40)), ((20 35, 45 20, 30 5, 10 10, 10 30, 20 35), (30 20, 20 25, 20 15, 30 20)))
        using var reader = new TinyWkbRecord(FromPostGis("\\x2600020104a006a0068f0364f403ab0263c80102068f0363f403ab02ab02ab028f0364009003c8016404c801ab02c7016400c701c80164"));
        var polygons = reader.GetMultiPolygon().ToArray();
        _ = await Assert.That(polygons).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(2);

        var firstPolygon = polygons[0];
        _ = await Assert.That(firstPolygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(1);

        // first ring
        var firstPolygonLinearRing = firstPolygon[0];
        _ = await Assert.That(firstPolygonLinearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(4);

        _ = await Assert.That(firstPolygonLinearRing[0].X).IsEqualTo(40.0);
        _ = await Assert.That(firstPolygonLinearRing[0].Y).IsEqualTo(40.0);

        _ = await Assert.That(firstPolygonLinearRing[1].X).IsEqualTo(20.0);
        _ = await Assert.That(firstPolygonLinearRing[1].Y).IsEqualTo(45.0);

        _ = await Assert.That(firstPolygonLinearRing[2].X).IsEqualTo(45.0);
        _ = await Assert.That(firstPolygonLinearRing[2].Y).IsEqualTo(30.0);

        _ = await Assert.That(firstPolygonLinearRing[3].X).IsEqualTo(40.0);
        _ = await Assert.That(firstPolygonLinearRing[3].Y).IsEqualTo(40.0);

        // second polygon
        var secondPolygon = polygons[1];
        _ = await Assert.That(secondPolygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(2);

        // first ring
        var secondPolygonFirstLinearRing = secondPolygon[0];
        _ = await Assert.That(secondPolygonFirstLinearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(6);

        _ = await Assert.That(secondPolygonFirstLinearRing[0].X).IsEqualTo(20.0);
        _ = await Assert.That(secondPolygonFirstLinearRing[0].Y).IsEqualTo(35.0);

        _ = await Assert.That(secondPolygonFirstLinearRing[1].X).IsEqualTo(45.0);
        _ = await Assert.That(secondPolygonFirstLinearRing[1].Y).IsEqualTo(20.0);

        _ = await Assert.That(secondPolygonFirstLinearRing[2].X).IsEqualTo(30.0);
        _ = await Assert.That(secondPolygonFirstLinearRing[2].Y).IsEqualTo(5.0);

        _ = await Assert.That(secondPolygonFirstLinearRing[3].X).IsEqualTo(10.0);
        _ = await Assert.That(secondPolygonFirstLinearRing[3].Y).IsEqualTo(10.0);

        _ = await Assert.That(secondPolygonFirstLinearRing[4].X).IsEqualTo(10.0);
        _ = await Assert.That(secondPolygonFirstLinearRing[4].Y).IsEqualTo(30.0);

        _ = await Assert.That(secondPolygonFirstLinearRing[5].X).IsEqualTo(20.0);
        _ = await Assert.That(secondPolygonFirstLinearRing[5].Y).IsEqualTo(35.0);

        // second ring
        var secondPolygonSecondLinearRing = secondPolygon[1];
        _ = await Assert.That(secondPolygonSecondLinearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(4);

        _ = await Assert.That(secondPolygonSecondLinearRing[0].X).IsEqualTo(30.0);
        _ = await Assert.That(secondPolygonSecondLinearRing[0].Y).IsEqualTo(20.0);

        _ = await Assert.That(secondPolygonSecondLinearRing[1].X).IsEqualTo(20.0);
        _ = await Assert.That(secondPolygonSecondLinearRing[1].Y).IsEqualTo(25.0);

        _ = await Assert.That(secondPolygonSecondLinearRing[2].X).IsEqualTo(20.0);
        _ = await Assert.That(secondPolygonSecondLinearRing[2].Y).IsEqualTo(15.0);

        _ = await Assert.That(secondPolygonSecondLinearRing[3].X).IsEqualTo(30.0);
        _ = await Assert.That(secondPolygonSecondLinearRing[3].Y).IsEqualTo(20.0);
    }

    private static byte[] FromPostGis(string value)
    {
        return value switch
        {
            ['\\', 'x', ..] => [.. GetBytes(value)],
            { Length: >= 2 } => throw new ArgumentException("value is invalid", nameof(value)),
            _ => throw new ArgumentException("value must be more than 2 bytes long", nameof(value)),
        };

        static IEnumerable<byte> GetBytes(string value)
        {
            for (var i = 2; i < value.Length; i += 2)
            {
                var byteString = value.Substring(i, 2);
                yield return byte.Parse(byteString, System.Globalization.NumberStyles.HexNumber);
            }
        }
    }
}