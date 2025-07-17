using TUnit.Assertions.AssertConditions.Throws;

namespace Altemiq.IO.Geometry;

public class WkbReaderTests
{
    [Test]
    [MatrixDataSource]
    public async Task GetZeroDimensions([Matrix(0, 1)] byte byteOrder)
    {
        byte[] data = [byteOrder, .. BitConverter.GetBytes(1)];
        var reader = new WkbRecord(data);
        _ = await Assert.That(reader.IsNull()).IsTrue();
    }

    [Test]
    public async Task GetPoint()
    {
        var reader = new WkbRecord(HelperFunctions.GetByteArrayFromResource(nameof(Point)));
        _ = await Assert.That(reader.GetPoint())
            .Satisfies(point => point.X, x => x.IsEqualTo(30D)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(10D));
    }

    [Test]
    public async Task GetPointZ()
    {
        var reader = new WkbRecord(HelperFunctions.GetByteArrayFromResource(nameof(PointZ)));
        _ = await Assert.That(reader.GetPointZ())
            .Satisfies(point => point.X, x => x.IsEqualTo(30D)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(10D)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(20D));
    }

    [Test]
    public async Task GetPointZM()
    {
        var reader = new WkbRecord(HelperFunctions.GetByteArrayFromResource(nameof(PointZM)));
        _ = await Assert.That(reader.GetPointZM())
            .Satisfies(point => point.X, x => x.IsEqualTo(30D)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(10D)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(20D)).And
            .Satisfies(point => point.Measurement, m => m.IsEqualTo(15D));
    }

    [Test]
    public async Task GetPointFromLineString()
    {
        var wkbReader = new WkbRecord(HelperFunctions.GetByteArrayFromResource("LineString"));
        _ = await Assert.That(() => wkbReader.GetPoint()).Throws<InvalidGeometryTypeException>();
    }

    [Test]
    public async Task GetMultiPoint()
    {
        // MULTIPOINT (10 40.0, 40 30.0, 20 20.0, 30 1.0)
        var reader = new WkbRecord(HelperFunctions.GetByteArrayFromResource("MultiPoint"));

        var multiPoint = reader.GetMultiPoint();

        _ = await Assert.That(multiPoint.ElementAt(0))
            .Satisfies(point => point.X, x => x.IsEqualTo(10.0))
            .Satisfies(point => point.Y, x => x.IsEqualTo(40.0));

        _ = await Assert.That(multiPoint.ElementAt(1))
            .Satisfies(point => point.X, x => x.IsEqualTo(40.0))
            .Satisfies(point => point.Y, x => x.IsEqualTo(30.0));

        _ = await Assert.That(multiPoint.ElementAt(2))
            .Satisfies(point => point.X, x => x.IsEqualTo(20.0))
            .Satisfies(point => point.Y, x => x.IsEqualTo(20.0));

        _ = await Assert.That(multiPoint.ElementAt(3))
            .Satisfies(point => point.X, x => x.IsEqualTo(30.0))
            .Satisfies(point => point.Y, x => x.IsEqualTo(10.0));
    }

    [Test]
    public async Task GetMultiPointFromLineString()
    {
        var reader = new WkbRecord(HelperFunctions.GetByteArrayFromResource("LineString"));
        _ = await Assert.That(() => reader.GetMultiPoint()).Throws<InvalidGeometryTypeException>();
    }

    [Test]
    public async Task GetLineString()
    {
        // LINESTRING (30 10.0, 10 30.0, 40 40.0)
        var reader = new WkbRecord(HelperFunctions.GetByteArrayFromResource("LineString"));
        var lineString = reader.GetLineString();

        _ = await Assert.That(lineString[0])
            .Satisfies(point => point.X, x => x.IsEqualTo(30.0))
            .Satisfies(point => point.Y, y => y.IsEqualTo(10.0));

        _ = await Assert.That(lineString[1])
            .Satisfies(point => point.X, x => x.IsEqualTo(10.0))
            .Satisfies(point => point.Y, y => y.IsEqualTo(30.0));

        _ = await Assert.That(lineString[2])
            .Satisfies(point => point.X, x => x.IsEqualTo(40.0))
            .Satisfies(point => point.Y, y => y.IsEqualTo(40.0));
    }

    [Test]
    public async Task GetLineStringZ()
    {
        // LINESTRINGZ (30 10 20.0, 10 30 5.0, 40 40 40)
        var reader = new WkbRecord(HelperFunctions.GetByteArrayFromResource("LineStringZ"));
        var lineString = reader.GetLineStringZ();

        _ = await Assert.That(lineString[0])
            .Satisfies(point => point.X, x => x.IsEqualTo(30.0))
            .Satisfies(point => point.Y, y => y.IsEqualTo(10.0))
            .Satisfies(point => point.Z, z => z.IsEqualTo(20.0));

        _ = await Assert.That(lineString[1])
            .Satisfies(point => point.X, x => x.IsEqualTo(10.0))
            .Satisfies(point => point.Y, y => y.IsEqualTo(30.0))
            .Satisfies(point => point.Z, z => z.IsEqualTo(5.0));

        _ = await Assert.That(lineString[2])
            .Satisfies(point => point.X, x => x.IsEqualTo(40.0))
            .Satisfies(point => point.Y, y => y.IsEqualTo(40.0))
            .Satisfies(point => point.Z, z => z.IsEqualTo(40.0));
    }

    [Test]
    public async Task GetLineStringZM()
    {
        // LINESTRINGZM (30 10 20 15.0, 10 30 5 20.0, 40 40 40 40)
        var reader = new WkbRecord(HelperFunctions.GetByteArrayFromResource("LineStringZM"));
        var lineString = reader.GetLineStringZM();

        _ = await Assert.That(lineString[0])
            .Satisfies(point => point.X, x => x.IsEqualTo(30.0))
            .Satisfies(point => point.Y, y => y.IsEqualTo(10.0))
            .Satisfies(point => point.Z, z => z.IsEqualTo(20.0))
            .Satisfies(point => point.Measurement, m => m.IsEqualTo(15.0));

        _ = await Assert.That(lineString[1])
            .Satisfies(point => point.X, x => x.IsEqualTo(10.0))
            .Satisfies(point => point.Y, y => y.IsEqualTo(30.0))
            .Satisfies(point => point.Z, z => z.IsEqualTo(5.0))
            .Satisfies(point => point.Measurement, m => m.IsEqualTo(20.0));

        _ = await Assert.That(lineString[2])
            .Satisfies(point => point.X, x => x.IsEqualTo(40.0))
            .Satisfies(point => point.Y, y => y.IsEqualTo(40.0))
            .Satisfies(point => point.Z, z => z.IsEqualTo(40.0))
            .Satisfies(point => point.Measurement, m => m.IsEqualTo(40.0));
    }

    [Test]
    public async Task GetLineStringFromPoint()
    {
        var reader = new WkbRecord(HelperFunctions.GetByteArrayFromResource("Point"));

        _ = await Assert.That(() => reader.GetLineString()).Throws<InvalidGeometryTypeException>();
    }

    // MULTILINESTRING
    /// <summary>
    /// Reads the multi line string from point.
    /// </summary>
    [Test]
    public async Task GetMultiLineStringFromPoint()
    {
        var reader = new WkbRecord(HelperFunctions.GetByteArrayFromResource("Point"));

        _ = await Assert.That(() => reader.GetMultiLineString()).Throws<InvalidGeometryTypeException>();
    }

    /// <summary>
    /// Reads the multi line string.
    /// </summary>
    [Test]
    public async Task GetMultiLineString()
    {
        var reader = new WkbRecord(HelperFunctions.GetByteArrayFromResource("MultiLineString"));

        // MULTILINESTRING ((10 10.0, 20 20.0, 10 40.0), (40 40.0, 30 30.0, 40 20.0, 30 10.0))
        var lines = reader.GetMultiLineString().ToArray();
        _ = await Assert.That(lines).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(2);

        var line = lines.ElementAt(0);
        _ = await Assert.That(line).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(3);

        _ = await Assert.That(line[0])
            .Satisfies(point => point.X, x => x.IsEqualTo(10.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(10.0));

        _ = await Assert.That(line[1])
            .Satisfies(point => point.X, x => x.IsEqualTo(20.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(20.0));

        _ = await Assert.That(line[2])
            .Satisfies(point => point.X, x => x.IsEqualTo(10.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(40.0));

        // second line
        line = lines.ElementAt(1);
        _ = await Assert.That(line).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(4);

        _ = await Assert.That(line[0])
            .Satisfies(point => point.X, x => x.IsEqualTo(40.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(40.0));

        _ = await Assert.That(line[1])
            .Satisfies(point => point.X, x => x.IsEqualTo(30.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(30.0));

        _ = await Assert.That(line[2])
            .Satisfies(point => point.X, x => x.IsEqualTo(40.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(20.0));

        _ = await Assert.That(line[3])
            .Satisfies(point => point.X, x => x.IsEqualTo(30.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(10.0));
    }

    [Test]
    public async Task GetPolygonFromPoint()
    {
        var reader = new WkbRecord(HelperFunctions.GetByteArrayFromResource("Point"));
        _ = await Assert.That(() => reader.GetPolygon()).Throws<InvalidGeometryTypeException>();
    }

    /// <summary>
    /// Reads the simple polygon.
    /// </summary>
    [Test]
    public async Task GetSimplePolygon()
    {
        var reader = new WkbRecord(HelperFunctions.GetByteArrayFromResource("SimplePolygon"));

        var polygon = reader.GetPolygon();
        _ = await Assert.That(polygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(1);

        var linearRing = polygon[0];
        _ = await Assert.That(linearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(5);

        _ = await Assert.That(linearRing[0])
            .Satisfies(point => point.X, x => x.IsEqualTo(30.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(10.0));

        _ = await Assert.That(linearRing[1])
            .Satisfies(point => point.X, x => x.IsEqualTo(10.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(20.0));

        _ = await Assert.That(linearRing[2])
            .Satisfies(point => point.X, x => x.IsEqualTo(20.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(40.0));

        _ = await Assert.That(linearRing[3])
            .Satisfies(point => point.X, x => x.IsEqualTo(40.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(40.0));

        _ = await Assert.That(linearRing[4])
            .Satisfies(point => point.X, x => x.IsEqualTo(30.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(10.0));
    }

    /// <summary>
    /// Reads the simple polygon Z.
    /// </summary>
    [Test]
    public async Task GetSimplePolygonZ()
    {
        // POLYGONZ ((30 10 20.0, 10 20 30.0, 20 40 30.0, 40 40 40.0, 30 10 2.0))
        var reader = new WkbRecord(HelperFunctions.GetByteArrayFromResource("SimplePolygonZ"));
        var polygon = reader.GetPolygonZ();
        _ = await Assert.That(polygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(1);

        var linearRing = polygon[0];
        _ = await Assert.That(linearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(5);

        _ = await Assert.That(linearRing[0])
            .Satisfies(point => point.X, x => x.IsEqualTo(30.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(10.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(20.0));

        _ = await Assert.That(linearRing[1])
            .Satisfies(point => point.X, x => x.IsEqualTo(10.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(20.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(30.0));

        _ = await Assert.That(linearRing[2])
            .Satisfies(point => point.X, x => x.IsEqualTo(20.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(40.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(30.0));

        _ = await Assert.That(linearRing[3])
            .Satisfies(point => point.X, x => x.IsEqualTo(40.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(40.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(40.0));

        _ = await Assert.That(linearRing[4])
            .Satisfies(point => point.X, x => x.IsEqualTo(30.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(10.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(20.0));
    }

    [Test]
    public async Task GetSimplePolygonZM()
    {
        // POLYGONZM ((30 10 20 15.0, 10 20 30 15.0, 20 40 30 50.0, 40 40 40 40.0, 30 10 20 15.0))
        var reader = new WkbRecord(HelperFunctions.GetByteArrayFromResource("SimplePolygonZM"));

        var polygon = reader.GetPolygonZM();
        _ = await Assert.That(polygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(1);

        var linearRing = polygon[0];
        _ = await Assert.That(linearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(5);

        _ = await Assert.That(linearRing[0])
            .Satisfies(point => point.X, x => x.IsEqualTo(30.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(10.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(20.0)).And
            .Satisfies(point => point.Measurement, m => m.IsEqualTo(15.0));

        _ = await Assert.That(linearRing[1])
            .Satisfies(point => point.X, x => x.IsEqualTo(10.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(20.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(30.0)).And
            .Satisfies(point => point.Measurement, m => m.IsEqualTo(15.0));

        _ = await Assert.That(linearRing[2])
            .Satisfies(point => point.X, x => x.IsEqualTo(20.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(40.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(30.0)).And
            .Satisfies(point => point.Measurement, m => m.IsEqualTo(50.0));

        _ = await Assert.That(linearRing[3])
            .Satisfies(point => point.X, x => x.IsEqualTo(40.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(40.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(40.0)).And
            .Satisfies(point => point.Measurement, m => m.IsEqualTo(40.0));

        _ = await Assert.That(linearRing[4])
            .Satisfies(point => point.X, x => x.IsEqualTo(30.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(10.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(20.0)).And
            .Satisfies(point => point.Measurement, m => m.IsEqualTo(15.0));
    }

    [Test]
    public async Task GetComplexPolygon()
    {
        var reader = new WkbRecord(HelperFunctions.GetByteArrayFromResource("ComplexPolygon"));

        var polygon = reader.GetPolygon();
        _ = await Assert.That(polygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(2);

        var linearRing = polygon[0];
        _ = await Assert.That(linearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(5);

        _ = await Assert.That(linearRing[0])
            .Satisfies(point => point.X, x => x.IsEqualTo(35.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(10.0));

        _ = await Assert.That(linearRing[1])
            .Satisfies(point => point.X, x => x.IsEqualTo(10.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(20.0));

        _ = await Assert.That(linearRing[2])
            .Satisfies(point => point.X, x => x.IsEqualTo(15.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(40.0));

        _ = await Assert.That(linearRing[3])
            .Satisfies(point => point.X, x => x.IsEqualTo(45.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(45.0));

        _ = await Assert.That(linearRing[4])
            .Satisfies(point => point.X, x => x.IsEqualTo(35.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(10.0));

        linearRing = polygon[1];
        _ = await Assert.That(linearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(4);

        _ = await Assert.That(linearRing[0])
            .Satisfies(point => point.X, x => x.IsEqualTo(20.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(30.0));

        _ = await Assert.That(linearRing[1])
            .Satisfies(point => point.X, x => x.IsEqualTo(35.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(35.0));

        _ = await Assert.That(linearRing[2])
            .Satisfies(point => point.X, x => x.IsEqualTo(30.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(20.0));

        _ = await Assert.That(linearRing[3])
            .Satisfies(point => point.X, x => x.IsEqualTo(20.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(30.0));
    }

    [Test]
    public async Task GetComplexPolygonZ()
    {
        // POLYGONZ ((35 10 15.0, 10 20 30.0, 15 40 25.0, 45 45 45.0, 35 10 15.0), (20 30 10.0, 35 35 35.0, 30 20 25.0, 20 30 25.0))
        var reader = new WkbRecord(HelperFunctions.GetByteArrayFromResource("ComplexPolygonZ"));

        var polygon = reader.GetPolygonZ();
        _ = await Assert.That(polygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(2);

        var linearRing = polygon[0];
        _ = await Assert.That(linearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(5);

        _ = await Assert.That(linearRing[0])
            .Satisfies(point => point.X, x => x.IsEqualTo(35.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(10.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(15.0));

        _ = await Assert.That(linearRing[1])
            .Satisfies(point => point.X, x => x.IsEqualTo(10.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(20.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(30.0));

        _ = await Assert.That(linearRing[2])
            .Satisfies(point => point.X, x => x.IsEqualTo(15.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(40.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(25.0));

        _ = await Assert.That(linearRing[3])
            .Satisfies(point => point.X, x => x.IsEqualTo(45.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(45.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(45.0));

        _ = await Assert.That(linearRing[4])
            .Satisfies(point => point.X, x => x.IsEqualTo(35.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(10.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(15.0));

        linearRing = polygon[1];
        _ = await Assert.That(linearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(4);

        _ = await Assert.That(linearRing[0])
            .Satisfies(point => point.X, x => x.IsEqualTo(20.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(30.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(10.0));

        _ = await Assert.That(linearRing[1])
            .Satisfies(point => point.X, x => x.IsEqualTo(35.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(35.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(35.0));

        _ = await Assert.That(linearRing[2])
            .Satisfies(point => point.X, x => x.IsEqualTo(30.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(20.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(25.0));

        _ = await Assert.That(linearRing[3])
            .Satisfies(point => point.X, x => x.IsEqualTo(20.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(30.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(25.0));
    }

    [Test]
    public async Task GetComplexPolygonZM()
    {
        // POLYGONZM ((35 10 15 25.0, 10 20 30 40.0, 15 40 25 30.0, 45 45 45 45.0, 35 10 15 2.0), (20 30 10 40.0, 35 35 35 35.0, 30 20 25 20.0, 20 30 25 35.0))
        var reader = new WkbRecord(HelperFunctions.GetByteArrayFromResource("ComplexPolygonZM"));

        var polygon = reader.GetPolygonZM();
        _ = await Assert.That(polygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(2);

        var linearRing = polygon[0];
        _ = await Assert.That(linearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(5);

        _ = await Assert.That(linearRing[0])
            .Satisfies(point => point.X, x => x.IsEqualTo(35.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(10.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(15.0)).And
            .Satisfies(point => point.Measurement, m => m.IsEqualTo(25.0));

        _ = await Assert.That(linearRing[1])
            .Satisfies(point => point.X, x => x.IsEqualTo(10.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(20.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(30.0)).And
            .Satisfies(point => point.Measurement, m => m.IsEqualTo(40.0));

        _ = await Assert.That(linearRing[2])
            .Satisfies(point => point.X, x => x.IsEqualTo(15.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(40.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(25.0)).And
            .Satisfies(point => point.Measurement, m => m.IsEqualTo(30.0));

        _ = await Assert.That(linearRing[3])
            .Satisfies(point => point.X, x => x.IsEqualTo(45.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(45.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(45.0)).And
            .Satisfies(point => point.Measurement, m => m.IsEqualTo(45.0));

        _ = await Assert.That(linearRing[4])
            .Satisfies(point => point.X, x => x.IsEqualTo(35.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(10.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(15.0)).And
            .Satisfies(point => point.Measurement, m => m.IsEqualTo(20.0));

        linearRing = polygon[1];
        _ = await Assert.That(linearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(4);

        _ = await Assert.That(linearRing[0])
            .Satisfies(point => point.X, x => x.IsEqualTo(20.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(30.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(10.0)).And
            .Satisfies(point => point.Measurement, m => m.IsEqualTo(40.0));

        _ = await Assert.That(linearRing[1])
            .Satisfies(point => point.X, x => x.IsEqualTo(35.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(35.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(35.0)).And
            .Satisfies(point => point.Measurement, m => m.IsEqualTo(35.0));

        _ = await Assert.That(linearRing[2])
            .Satisfies(point => point.X, x => x.IsEqualTo(30.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(20.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(25.0)).And
            .Satisfies(point => point.Measurement, m => m.IsEqualTo(20.0));

        _ = await Assert.That(linearRing[3])
            .Satisfies(point => point.X, x => x.IsEqualTo(20.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(30.0)).And
            .Satisfies(point => point.Z, z => z.IsEqualTo(25.0)).And
            .Satisfies(point => point.Measurement, m => m.IsEqualTo(35.0));
    }

    [Test]
    public async Task GetMultiPolygonFromPoint()
    {
        var reader = new WkbRecord(HelperFunctions.GetByteArrayFromResource("Point"));
        _ = await Assert.That(() => reader.GetMultiPolygon()).Throws<InvalidGeometryTypeException>();
    }

    [Test]
    public async Task GetSimpleMultiPolygon()
    {
        var reader = new WkbRecord(HelperFunctions.GetByteArrayFromResource("SimpleMultiPolygon"));

        // MULTIPOLYGON (((30 20.0, 10 40.0, 45 40.0, 30 2.0)), ((15 5.0, 40 10.0, 10 20.0, 5 10.0, 15 5.0)))
        var polygons = reader.GetMultiPolygon().ToArray();
        _ = await Assert.That(polygons).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(2);

        var polygon = polygons[0];
        _ = await Assert.That(polygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(1);

        // first ring
        var linearRing = polygon[0];
        _ = await Assert.That(linearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(4);

        _ = await Assert.That(linearRing[0])
            .Satisfies(point => point.X, x => x.IsEqualTo(30.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(20.0));

        _ = await Assert.That(linearRing[1])
            .Satisfies(point => point.X, x => x.IsEqualTo(10.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(40.0));

        _ = await Assert.That(linearRing[2])
            .Satisfies(point => point.X, x => x.IsEqualTo(45.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(40.0));

        _ = await Assert.That(linearRing[3])
            .Satisfies(point => point.X, x => x.IsEqualTo(30.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(20.0));

        // second polygon
        polygon = polygons[1];
        _ = await Assert.That(polygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(1);

        // first ring
        linearRing = polygon[0];
        _ = await Assert.That(linearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(5);

        _ = await Assert.That(linearRing[0])
            .Satisfies(point => point.X, x => x.IsEqualTo(15.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(5.0));

        _ = await Assert.That(linearRing[1])
            .Satisfies(point => point.X, x => x.IsEqualTo(40.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(10.0));

        _ = await Assert.That(linearRing[2])
            .Satisfies(point => point.X, x => x.IsEqualTo(10.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(20.0));

        _ = await Assert.That(linearRing[3])
            .Satisfies(point => point.X, x => x.IsEqualTo(5.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(10.0));

        _ = await Assert.That(linearRing[4])
            .Satisfies(point => point.X, x => x.IsEqualTo(15.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(5.0));
    }

    [Test]
    public async Task GetComplexMultiPolygon()
    {
        var reader = new WkbRecord(HelperFunctions.GetByteArrayFromResource("ComplexMultiPolygon"));

        var polygons = reader.GetMultiPolygon().ToArray();
        _ = await Assert.That(polygons).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(2);

        var polygon = polygons.ElementAt(0);
        _ = await Assert.That(polygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(1);

        // first ring
        var linearRing = polygon[0];
        _ = await Assert.That(linearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(4);

        _ = await Assert.That(linearRing[0])
            .Satisfies(point => point.X, x => x.IsEqualTo(40.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(40.0));

        _ = await Assert.That(linearRing[1])
            .Satisfies(point => point.X, x => x.IsEqualTo(20.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(45.0));

        _ = await Assert.That(linearRing[2])
            .Satisfies(point => point.X, x => x.IsEqualTo(45.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(30.0));

        _ = await Assert.That(linearRing[3])
            .Satisfies(point => point.X, x => x.IsEqualTo(40.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(40.0));

        // second polygon
        polygon = polygons.ElementAt(1);
        _ = await Assert.That(polygon).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(2);

        // first ring
        linearRing = polygon[0];
        _ = await Assert.That(linearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(6);

        _ = await Assert.That(linearRing[0])
            .Satisfies(point => point.X, x => x.IsEqualTo(20.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(35.0));

        _ = await Assert.That(linearRing[1])
            .Satisfies(point => point.X, x => x.IsEqualTo(45.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(20.0));

        _ = await Assert.That(linearRing[2])
            .Satisfies(point => point.X, x => x.IsEqualTo(30.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(5.0));

        _ = await Assert.That(linearRing[3])
            .Satisfies(point => point.X, x => x.IsEqualTo(10.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(10.0));

        _ = await Assert.That(linearRing[4])
            .Satisfies(point => point.X, x => x.IsEqualTo(10.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(30.0));

        _ = await Assert.That(linearRing[5])
            .Satisfies(point => point.X, x => x.IsEqualTo(20.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(35.0));

        // second ring
        linearRing = polygon[1];
        _ = await Assert.That(linearRing).IsNotNull().And.IsNotEmpty().And.HasCount().EqualTo(4);

        _ = await Assert.That(linearRing[0])
            .Satisfies(point => point.X, x => x.IsEqualTo(30.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(20.0));

        _ = await Assert.That(linearRing[1])
            .Satisfies(point => point.X, x => x.IsEqualTo(20.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(25.0));

        _ = await Assert.That(linearRing[2])
            .Satisfies(point => point.X, x => x.IsEqualTo(20.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(15.0));

        _ = await Assert.That(linearRing[3])
            .Satisfies(point => point.X, x => x.IsEqualTo(30.0)).And
            .Satisfies(point => point.Y, y => y.IsEqualTo(20.0));
    }
}