namespace Altemiq.IO.Geometry;

public class EwkbWriterTests
{
    [Test]
    public async Task WritePoint()
    {
        await TestWrite(HelperFunctions.GetByteArrayFromResource("ExtendedPoint"), writer => writer.Write(new Point(30.0, 10.0), 3112));
    }
    
    [Test]
    public async Task WritePointZ()
    {
        await TestWrite(HelperFunctions.GetByteArrayFromResource("ExtendedPointZ"), writer => writer.Write(new PointZ(30.0, 10.0, 20.0), 3112));
        
    }

    [Test]
    public async Task WritePointZM()
    {
        await TestWrite(HelperFunctions.GetByteArrayFromResource("ExtendedPointZM"), writer => writer.Write( new PointZM(30.0, 10.0, 20.0, 15.0), 3112));
    }
    
    [Test]
    public async Task WriteMultiPoint()
    {
        // MULTIPOINT (10 40.0, 40 30.0, 20 20.0, 30 1.0)
        await TestWrite(
            HelperFunctions.GetByteArrayFromResource("ExtendedMultiPoint"),
            writer => writer.Write(new Point[] {new(10.0, 40.0), new(40.0, 30.0), new(20.0, 20.0), new(30.0, 10.0)}, 3112));
    }
    
    [Test]
    public async Task WriteLineString()
    {
        // LINESTRING (30 10.0, 10 30.0, 40 40.0)
        await TestWrite(
            HelperFunctions.GetByteArrayFromResource("ExtendedLineString"),
            writer => writer.Write(Polyline.FromPoints(new Point(30.0, 10.0), new Point(10.0, 30.0), new Point(40.0, 40.0)), 3112));
    }
    
    [Test]
    public async Task WriteLineStringZ()
    {
         // LINESTRINGZ (30 10 20.0, 10 30 5.0, 40 40 4.0)
         await TestWrite(
             HelperFunctions.GetByteArrayFromResource("ExtendedLineStringZ"),
             writer => writer.Write(PolylineZ.FromPoints(new PointZ(30.0, 10.0, 20.0), new PointZ(10.0, 30.0, 5.0), new PointZ(40.0, 40.0, 40.0)), 3112));
    }
    
    [Test]
    public async Task WriteLineStringZM()
    {
        // LINESTRINGZM (30 10 20 15.0, 10 30 5 20.0, 40 40 40 4.0)
        await TestWrite(
            HelperFunctions.GetByteArrayFromResource("ExtendedLineStringZM"), 
            writer => writer.Write(PolylineZM.FromPoints(new PointZM(30.0, 10.0, 20.0, 15.0), new PointZM(10.0, 30.0, 5.0, 20.0), new PointZM(40.0, 40.0, 40.0, 40.0)), 3112));
    }
    
    [Test]
    public async Task WriteMultiLineString()
    {
        await TestWrite(
            HelperFunctions.GetByteArrayFromResource("ExtendedMultiLineString"),
            writer => writer.Write(
                [
                    Polyline.FromPoints(new Point(10.0, 10.0), new Point(20.0, 20.0), new Point(10.0, 40.0)),
                    Polyline.FromPoints(new Point(40.0, 40.0), new Point(30.0, 30.0), new Point(40.0, 20.0), new Point(30.0, 10.0))
                ],
                3112));
    }
    
    [Test]
    public async Task WriteSimplePolygon()
    {
        await TestWrite(
            HelperFunctions.GetByteArrayFromResource("ExtendedSimplePolygon"),
            writer => writer.Write(
                Polygon.FromPoints(new Point(30.0, 10.0), new Point(10.0, 20.0), new Point(20.0, 40.0), new Point(40.0, 40.0), new Point(30.0, 10.0)),
                3112));
    }

    [Test]
    public async Task WriteSimplePolygonZ()
    {
        // POLYGONZ ((30 10 20.0, 10 20 30.0, 20 40 30.0, 40 40 40.0, 30 10 2.0))
        await TestWrite(
            HelperFunctions.GetByteArrayFromResource("ExtendedSimplePolygonZ"),
            writer => writer.Write(
                PolygonZ.FromPoints(new PointZ(30.0, 10.0, 20.0), new PointZ(10.0, 20.0, 30.0), new PointZ(20.0, 40.0, 30.0), new PointZ(40.0, 40.0, 40.0), new PointZ(30.0, 10.0, 20.0)),
                3112));
    }

    [Test]
    public async Task WriteSimplePolygonZM()
    {
        // POLYGONZM ((30 10 20 15.0, 10 20 30 15.0, 20 40 30 50.0, 40 40 40 40.0, 30 10 20 15.0))
        await TestWrite(HelperFunctions.GetByteArrayFromResource("ExtendedSimplePolygonZM"),
            writer => writer.Write(
                PolygonZM.FromPoints(new PointZM(30.0, 10.0, 20.0, 15.0), new PointZM(10.0, 20.0, 30.0, 15.0), new PointZM(20.0, 40.0, 30.0, 50.0), new PointZM(40.0, 40.0, 40.0, 40.0), new PointZM(30.0, 10.0, 20.0, 15.0)),
                3112));
    }
    
    [Test]
    public async Task WriteComplexPolygon()
    {
        await TestWrite(
            HelperFunctions.GetByteArrayFromResource("ExtendedComplexPolygon"),
            writer => writer.Write(
                Polygon.FromPoints(
                    [new(35.0, 10.0), new(10.0, 20.0), new(15.0, 40.0), new(45.0, 45.0), new(35.0, 10.0)],
                    [[ new(20.0, 30.0), new(35.0, 35.0), new(30.0, 20.0), new(20.0, 30.0)]]),
                3112));
    }
    
    [Test]
    public async Task WriteComplexPolygonZ()
    {
        // POLYGONZ ((35 10 15.0, 10 20 30.0, 15 40 25.0, 45 45 45.0, 35 10 15.0), (20 30 10.0, 35 35 35.0, 30 20 25.0, 20 30 25.0))
        await TestWrite(
            HelperFunctions.GetByteArrayFromResource("ExtendedComplexPolygonZ"),
            writer => writer.Write(
                PolygonZ.FromPoints(
                    [new(35.0, 10.0, 15.0), new(10.0, 20.0, 30.0), new(15.0, 40.0, 25.0), new(45.0, 45.0, 45.0), new(35.0, 10.0, 15.0)],
                    [[new(20.0, 30.0, 10.0), new(35.0, 35.0, 35.0), new(30.0, 20.0, 25.0), new(20.0, 30.0, 25.0)]]),
                3112));
    }
    
    [Test]
    public async Task WriteComplexPolygonZM()
    {
        // POLYGONZM ((35 10 15 25.0, 10 20 30 40.0, 15 40 25 30.0, 45 45 45 45.0, 35 10 15 2.0), (20 30 10 40.0, 35 35 35 35.0, 30 20 25 20.0, 20 30 25 35.0))
        await TestWrite(
            HelperFunctions.GetByteArrayFromResource("ExtendedComplexPolygonZM"),
            writer => writer.Write(
                PolygonZM.FromPoints(
                    [new(35.0, 10.0, 15.0, 25.0), new(10.0, 20.0, 30.0, 40.0), new(15.0, 40.0, 25.0, 30.0), new(45.0, 45.0, 45.0, 45.0), new(35.0, 10.0, 15.0, 20.0)],
                    [[new(20.0, 30.0, 10.0, 40.0), new(35.0, 35.0, 35.0, 35.0), new(30.0, 20.0, 25.0, 20.0), new(20.0, 30.0, 25.0, 35.0)]]),
                3112));
    }
    
    [Test]
    public async Task WriteSimpleMultiPolygon()
    {
        await TestWrite(
            HelperFunctions.GetByteArrayFromResource("ExtendedSimpleMultiPolygon"),
            writer => writer.Write(
                [
                    Polygon.FromPoints(new Point(30.0, 20.0), new Point(10.0, 40.0), new Point(45.0, 40.0), new Point(30.0, 20.0)),
                    Polygon.FromPoints(new Point(15.0, 5.0), new Point(40.0, 10.0), new Point(10.0, 20.0), new Point(5.0, 10.0), new Point(15.0, 5.0)),
                ],
                3112));

    }

    [Test]
    public async Task WriteComplexMultiPolygon()
    {
        await TestWrite(
            HelperFunctions.GetByteArrayFromResource("ExtendedComplexMultiPolygon"),
            writer => writer.Write(
                [
                    Polygon.FromPoints(new Point(40.0, 40.0), new Point(20.0, 45.0), new Point(45.0, 30.0), new Point(40.0, 40.0)),
                    Polygon.FromPoints(
                        [new(20.0, 35.0), new(45.0, 20.0), new(30.0, 5.0), new(10.0, 10.0), new(10.0, 30.0), new(20.0, 35.0)],
                        [[new(30.0, 20.0), new(20.0, 25.0), new(20.0, 15.0), new(30.0, 20.0)]])
                ],
                3112));

    }

    private static async Task TestWrite(byte[] bytes, Action<EwkbWriter> write)
    {
        var stream = new MemoryStream();
        using var writer = new EwkbWriter(stream);

        write(writer);
        
        await Assert.That(stream.Position).IsEqualTo(bytes.Length);
        await Assert.That(stream.ToArray()).IsEquivalentTo(bytes);
    }
}