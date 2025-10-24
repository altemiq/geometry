// -----------------------------------------------------------------------
// <copyright file="EwkbRecordTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

public class EwkbRecordTests
{
    /// Reads the zero dimensions.
    [Test]
    public async Task GetZeroDimensions()
    {
        _ = await Assert.That(new EwkbRecord(CreateBytes()).IsNull()).IsTrue();

        static byte[] CreateBytes()
        {
            return [1, .. BitConverter.GetBytes(1), .. BitConverter.GetBytes(3112)];
        }
    }

    // Point tests

    /// Reads a POINT object.
    [Test]
    public async Task GetPoint()
    {
        var reader = new EwkbRecord(HelperFunctions.GetByteArrayFromResource("ExtendedPoint"));
        _ = await Assert.That(reader.GetSrid()).IsEqualTo(3112);
        _ = await Assert.That(reader.GetPoint()).IsEquivalentTo(new Point(30.0, 10.0));
    }

    /// Reads a POINTZ object.
    [Test]
    public async Task GetPointZ()
    {
        var reader = new EwkbRecord(HelperFunctions.GetByteArrayFromResource("ExtendedPointZ"));
        _ = await Assert.That(reader.GetSrid()).IsEqualTo(3112);
        _ = await Assert.That(reader.GetPointZ()).IsEquivalentTo(new PointZ(30.0, 10.0, 20.0));
    }

    /// Reads a POINTZM object
    [Test]
    public async Task GetPointZM()
    {
        var reader = new EwkbRecord(HelperFunctions.GetByteArrayFromResource("ExtendedPointZM"));
        _ = await Assert.That(reader.GetSrid()).IsEqualTo(3112);
        _ = await Assert.That(reader.GetPointZM()).IsEquivalentTo(new PointZM(30.0, 10.0, 20.0, 15.0));
    }
    [Test]
    public async Task GetPointFromLineString()
    {
        var wkbReader = new EwkbRecord(HelperFunctions.GetByteArrayFromResource("ExtendedLineString"));
        _ = await Assert.That(() => wkbReader.GetPoint()).Throws<InvalidGeometryTypeException>();
    }

    // MULTIPOINT tests
    /// <summary>
    /// Reads the multi point.
    /// </summary>
    [Test]
    public async Task GetMultiPoint()
    {
        // MULTIPOINT (10 40.0, 40 30.0, 20 20.0, 30 1.0)
        var reader = new EwkbRecord(HelperFunctions.GetByteArrayFromResource("ExtendedMultiPoint"));
        _ = await Assert.That(reader.GetSrid()).IsEqualTo(3112);

        await Assert.That(reader.GetMultiPoint())
            .HasCount().EqualTo(4)
            .And.IsEquivalentTo([new Point(10.0, 40.0), new(40.0, 30.0), new(20.0, 20.0), new(30.0, 10.0)]);
    }

    [Test]
    public async Task GetMultiPointFromLineString()
    {
        var wkbReader = new EwkbRecord(HelperFunctions.GetByteArrayFromResource("ExtendedLineString"));
        _ = await Assert.That(() => wkbReader.GetMultiPoint()).Throws<InvalidGeometryTypeException>();
    }

    [Test]
    public async Task GetLineString()
    {
        // LINESTRING (30 10.0, 10 30.0, 40 40.0)
        var reader = new EwkbRecord(HelperFunctions.GetByteArrayFromResource("ExtendedLineString"));
        _ = await Assert.That(reader.GetSrid()).IsEqualTo(3112);
        var lineString = reader.GetLineString();
        _ = await Assert.That(lineString).IsNotNull()
            .And.IsEquivalentTo([new Point(30.0, 10.0), new(10.0, 30.0), new(40.0, 40.0)]);
    }

    [Test]
    public async Task GetLineStringZ()
    {
        // LINESTRINGZ (30 10 20.0, 10 30 5.0, 40 40 4.0)
        var reader = new EwkbRecord(HelperFunctions.GetByteArrayFromResource("ExtendedLineStringZ"));
        _ = await Assert.That(reader.GetSrid()).IsEqualTo(3112);
        _ = await Assert.That(reader.GetLineStringZ()).IsNotNull()
            .And.IsEquivalentTo([new PointZ(30.0, 10.0, 20.0), new(10.0, 30.0, 5.0), new(40.0, 40.0, 40.0)]);
    }

    [Test]
    public async Task GetLineStringZM()
    {
        // LINESTRINGZM (30 10 20 15.0, 10 30 5 20.0, 40 40 40 4.0)
        var reader = new EwkbRecord(HelperFunctions.GetByteArrayFromResource("ExtendedLineStringZM"));
        _ = await Assert.That(reader.GetSrid()).IsEqualTo(3112);
        _ = await Assert.That(reader.GetLineStringZM()).IsNotNull()
            .And.IsEquivalentTo([new PointZM(30.0, 10.0, 20.0, 15.0), new(10.0, 30.0, 5.0, 20.0), new(40.0, 40.0, 40.0, 40.0)]);
    }

    [Test]
    public async Task GetLineStringFromPoint()
    {
        var wkbReader = new EwkbRecord(HelperFunctions.GetByteArrayFromResource("ExtendedPoint"));
        await Assert.That(() => wkbReader.GetLineString()).Throws<InvalidGeometryTypeException>();
    }

    // MULTILINESTRING
    /// <summary>
    /// Reads the multi line string from point.
    /// </summary>
    [Test]
    public async Task GetMultiLineStringFromPoint()
    {
        var wkbReader = new EwkbRecord(HelperFunctions.GetByteArrayFromResource("ExtendedPoint"));
        _ = await Assert.That(() => wkbReader.GetMultiLineString()).Throws<InvalidGeometryTypeException>();
    }

    /// <summary>
    /// Reads the multi line string.
    /// </summary>
    [Test]
    public async Task GetMultiLineString()
    {
        var reader = new EwkbRecord(HelperFunctions.GetByteArrayFromResource("ExtendedMultiLineString"));
        _ = await Assert.That(reader.GetSrid()).IsEqualTo(3112);

        // MULTILINESTRING ((10 10.0, 20 20.0, 10 40.0), (40 40.0, 30 30.0, 40 20.0, 30 10.0))
        var lines = reader.GetMultiLineString().ToArray();
        _ = await Assert.That(lines).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(2);

        _ = await Assert.That(lines[0]).IsNotNull()
            .And.IsNotEmpty()
            .And.HasCount().EqualTo(3)
            .And.IsEquivalentTo([new Point(10.0, 10.0), new(20.0, 20.0), new(10.0, 40.0)]);

        // second line
        _ = await Assert.That(lines[1]).IsNotNull()
            .And.IsNotEmpty()
            .And.HasCount().EqualTo(4)
            .And.IsEquivalentTo([new Point(40.0, 40.0), new(30.0, 30.0), new(40.0, 20.0), new(30.0, 10.0),]);
    }

    [Test]
    public async Task GetPolygonFromPoint()
    {
        var wkbReader = new EwkbRecord(HelperFunctions.GetByteArrayFromResource("ExtendedPoint"));
        _ = await Assert.That(() => wkbReader.GetPolygon()).Throws<InvalidGeometryTypeException>();
    }

    /// <summary>
    /// Reads the simple polygon.
    /// </summary>
    [Test]
    public async Task GetSimplePolygon()
    {
        var reader = new EwkbRecord(HelperFunctions.GetByteArrayFromResource("ExtendedSimplePolygon"));
        _ = await Assert.That(reader.GetSrid()).IsEqualTo(3112);

        var polygon = reader.GetPolygon();
        _ = await Assert.That(polygon).HasCount().EqualTo(1);

        _ = await Assert.That(polygon[0]).IsNotNull()
            .And.IsNotEmpty()
            .And.HasCount().EqualTo(5)
            .And.IsEquivalentTo([new Point(30.0, 10.0), new(10.0, 20.0), new(20.0, 40.0), new(40.0, 40.0), new(30.0, 10.0)]);
    }

    /// <summary>
    /// Reads the simple polygon Z.
    /// </summary>
    [Test]
    public async Task GetSimplePolygonZ()
    {
        // POLYGONZ ((30 10 20.0, 10 20 30.0, 20 40 30.0, 40 40 40.0, 30 10 2.0))
        var reader = new EwkbRecord(HelperFunctions.GetByteArrayFromResource("ExtendedSimplePolygonZ"));
        _ = await Assert.That(reader.GetSrid()).IsEqualTo(3112);

        var polygon = reader.GetPolygonZ();
        _ = await Assert.That(polygon).HasCount().EqualTo(1);

        _ = await Assert.That(polygon[0]).IsNotNull()
            .And.IsNotEmpty()
            .And.HasCount().EqualTo(5)
            .And.IsEquivalentTo([new PointZ(30.0, 10.0, 20.0), new(10.0, 20.0, 30.0), new(20.0, 40.0, 30.0), new(40.0, 40.0, 40.0), new(30.0, 10.0, 20.0)]);
    }

    [Test]
    public async Task GetSimplePolygonZM()
    {
        // POLYGONZM ((30 10 20 15.0, 10 20 30 15.0, 20 40 30 50.0, 40 40 40 40.0, 30 10 20 15.0))
        var reader = new EwkbRecord(HelperFunctions.GetByteArrayFromResource("ExtendedSimplePolygonZM"));
        _ = await Assert.That(reader.GetSrid()).IsEqualTo(3112);

        var polygon = reader.GetPolygonZM();
        _ = await Assert.That(polygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(1);

        _ = await Assert.That(polygon[0]).IsNotNull()
            .And.IsNotEmpty()
            .And.HasCount().EqualTo(5)
            .And.IsEquivalentTo([new PointZM(30.0, 10.0, 20.0, 15.0), new(10.0, 20.0, 30.0, 15.0), new(20.0, 40.0, 30.0, 50.0), new(40.0, 40.0, 40.0, 40.0), new(30.0, 10.0, 20.0, 15.0)]);
    }

    [Test]
    public async Task GetComplexPolygon()
    {
        var reader = new EwkbRecord(HelperFunctions.GetByteArrayFromResource("ExtendedComplexPolygon"));
        _ = await Assert.That(reader.GetSrid()).IsEqualTo(3112);

        var polygon = reader.GetPolygon();
        _ = await Assert.That(polygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(2);

        _ = await Assert.That(polygon[0]).IsNotNull()
            .And.IsNotEmpty()
            .And.HasCount().EqualTo(5)
            .And.IsEquivalentTo([new Point(35.0, 10.0), new(10.0, 20.0), new(15.0, 40.0), new(45.0, 45.0), new(35.0, 10.0)]);

        _ = await Assert.That(polygon[1]).IsNotNull()
            .And.IsNotEmpty()
            .And.HasCount().EqualTo(4)
            .And.IsEquivalentTo([new Point(20.0, 30.0), new(35.0, 35.0), new(30.0, 20.0), new(20.0, 30.0)]);
    }

    [Test]
    public async Task GetComplexPolygonZ()
    {
        // POLYGONZ ((35 10 15.0, 10 20 30.0, 15 40 25.0, 45 45 45.0, 35 10 15.0), (20 30 10.0, 35 35 35.0, 30 20 25.0, 20 30 25.0))
        var reader = new EwkbRecord(HelperFunctions.GetByteArrayFromResource("ExtendedComplexPolygonZ"));
        _ = await Assert.That(reader.GetSrid()).IsEqualTo(3112);

        var polygon = reader.GetPolygonZ();
        _ = await Assert.That(polygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(2);

        _ = await Assert.That(polygon[0]).IsNotNull()
            .And.IsNotEmpty()
            .And.HasCount().EqualTo(5)
            .And.IsEquivalentTo([new PointZ(35.0, 10.0, 15.0), new(10.0, 20.0, 30.0), new(15.0, 40.0, 25.0), new(45.0, 45.0, 45.0), new(35.0, 10.0, 15.0)
            ]);

        _ = await Assert.That(polygon[1]).IsNotNull()
            .And.IsNotEmpty()
            .And.HasCount().EqualTo(4)
            .And.IsEquivalentTo([new PointZ(20.0, 30.0, 10.0), new(35.0, 35.0, 35.0), new(30.0, 20.0, 25.0), new(20.0, 30.0, 25.0)]);
    }

    [Test]
    public async Task GetComplexPolygonZM()
    {
        // POLYGONZM ((35 10 15 25.0, 10 20 30 40.0, 15 40 25 30.0, 45 45 45 45.0, 35 10 15 2.0), (20 30 10 40.0, 35 35 35 35.0, 30 20 25 20.0, 20 30 25 35.0))
        var reader = new EwkbRecord(HelperFunctions.GetByteArrayFromResource("ExtendedComplexPolygonZM"));
        _ = await Assert.That(reader.GetSrid()).IsEqualTo(3112);

        var polygon = reader.GetPolygonZM();
        _ = await Assert.That(polygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(2);

        _ = await Assert.That(polygon[0]).IsNotNull()
            .And.IsNotEmpty()
            .And.HasCount().EqualTo(5)
            .IsEquivalentTo([new PointZM(35.0, 10.0, 15.0, 25.0), new (10.0, 20.0, 30.0, 40.0), new (15.0, 40.0, 25.0, 30.0), new (45.0, 45.0, 45.0, 45.0), new (35.0, 10.0, 15.0, 20.0)]);

        _ = await Assert.That(polygon[1]).IsNotNull()
            .And.IsNotEmpty()
            .And.HasCount().EqualTo(4)
            .And.IsEquivalentTo([new PointZM(20.0, 30.0, 10.0, 40.0), new(35.0, 35.0, 35.0, 35.0), new(30.0, 20.0, 25.0, 20.0), new(20.0, 30.0, 25.0, 35.0)]);
    }

    [Test]
    public async Task GetMultiPolygonFromPoint()
    {
        var wkbReader = new EwkbRecord(HelperFunctions.GetByteArrayFromResource("ExtendedPoint"));
        _ = await Assert.That(() => wkbReader.GetMultiPolygon()).Throws<InvalidGeometryTypeException>();
    }

    [Test]
    public async Task GetSimpleMultiPolygon()
    {
        var reader = new EwkbRecord(HelperFunctions.GetByteArrayFromResource("ExtendedSimpleMultiPolygon"));
        _ = await Assert.That(reader.GetSrid()).IsEqualTo(3112);

        // MULTIPOLYGON (((30 20.0, 10 40.0, 45 40.0, 30 2.0)), ((15 5.0, 40 10.0, 10 20.0, 5 10.0, 15 5.0)))
        var polygons = reader.GetMultiPolygon().ToArray();
        _ = await Assert.That(polygons).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(2);

        var polygon = polygons[0];
        _ = await Assert.That(polygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(1);

        // first ring
        _ = await Assert.That(polygon[0]).IsNotNull()
            .And.IsNotEmpty()
            .And.HasCount().EqualTo(4)
            .And.IsEquivalentTo([new Point(30.0, 20.0), new(10.0, 40.0), new(45.0, 40.0), new(30.0, 20.0)]);

        // second polygon
        polygon = polygons[1];
        _ = await Assert.That(polygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(1);

        // first ring
        _ = await Assert.That(polygon[0]).IsNotNull()
            .And.IsNotEmpty()
            .And.HasCount().EqualTo(5)
            .And.IsEquivalentTo([new Point(15.0, 5.0), new(40.0, 10.0), new(10.0, 20.0), new(5.0, 10.0), new(15.0, 5.0)]);
    }

    [Test]
    public async Task GetComplexMultiPolygon()
    {
        var reader = new EwkbRecord(HelperFunctions.GetByteArrayFromResource("ExtendedComplexMultiPolygon"));
        _ = await Assert.That(reader.GetSrid()).IsEqualTo(3112);

        var polygons = reader.GetMultiPolygon().ToArray();
        _ = await Assert.That(polygons).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(2);

        var polygon = polygons.ElementAt(0);
        _ = await Assert.That(polygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(1);

        // first ring
        _ = await Assert.That(polygon[0]).IsNotNull()
            .And.IsNotEmpty()
            .And.HasCount().EqualTo(4)
            .And.IsEquivalentTo([new Point(40.0, 40.0), new(20.0, 45.0), new(45.0, 30.0), new(40.0, 40.0)]);

        // second polygon
        polygon = polygons.ElementAt(1);
        _ = await Assert.That(polygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(2);

        // first ring
        _ = await Assert.That(polygon[0]).IsNotNull()
            .And.IsNotEmpty()
            .And.HasCount().EqualTo(6)
            .And.IsEquivalentTo([new Point(20.0, 35.0), new(45.0, 20.0), new(30.0, 5.0), new(10.0, 10.0), new(10.0, 30.0), new(20.0, 35.0)]);

        // second ring
        _ = await Assert.That(polygon[1]).IsNotNull()
            .And.IsNotEmpty()
            .And.HasCount().EqualTo(4)
            .And.IsEquivalentTo([new Point(30.0, 20.0), new(20.0, 25.0), new(20.0, 15.0), new(30.0, 20.0)]);
    }
}