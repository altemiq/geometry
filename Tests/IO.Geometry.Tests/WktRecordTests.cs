namespace Altemiq.IO.Geometry;

public class WktRecordTests
{
    [Test]
    [Arguments("POINT (6 10)", new[] { 6D, 10D })]
    [Arguments("POINT(6 10)", new[] { 6D, 10D })]
    [Arguments("POINT EMPTY", new[] { 0D, 0D })]
    [Arguments("POINT(-1 -2)", new[] { -1D, -2D })]
    public async Task WktToPoint(string wkt, double[] coordinates)
    {
        var record = new WktRecord(wkt);
        _ = await Assert.That(GetCoordinates(record.GetPoint())).IsEquivalentTo(coordinates);

        static double[] GetCoordinates(Point pt)
        {
            return [pt.X, pt.Y];
        }
    }
    [Test]
    [Arguments("POINT Z (1 1 80)", new[] { 1D, 1D, 80D })]
    [Arguments("POINT Z(1 1 80)", new[] { 1D, 1D, 80D })]
    [Arguments("POINTZ (1 1 80)", new[] { 1D, 1D, 80D })]
    [Arguments("POINTZ(1 1 80)", new[] { 1D, 1D, 80D })]
    public async Task WktToPointZ(string wkt, double[] coordinates)
    {
        var record = new WktRecord(wkt);
        _ = await Assert.That(GetCoordinates(record.GetPointZ())).IsEquivalentTo(coordinates);

        static double[] GetCoordinates(PointZ pt)
        {
            return [pt.X, pt.Y, pt.Z];
        }
    }
    
    [Test]
    [Arguments("POINT M (1 1 80)", new[] { 1D, 1D, 80D })]
    [Arguments("POINT M(1 1 80)", new[] { 1D, 1D, 80D })]
    [Arguments("POINTM (1 1 80)", new[] { 1D, 1D, 80D })]
    [Arguments("POINTM(1 1 80)", new[] { 1D, 1D, 80D })]
    public async Task WktToPointM(string wkt, double[] coordinates)
    {
        var record = new WktRecord(wkt);
        _ = await Assert.That(GetCoordinates(record.GetPointM())).IsEquivalentTo(coordinates);

        static double[] GetCoordinates(PointM pt)
        {
            return [pt.X, pt.Y, pt.Measurement];
        }
    }
    
    [Test]
    [Arguments("POINT ZM (1 1 5 60)", new[] { 1D, 1D, 5D, 60D })]
    [Arguments("POINT ZM(1 1 5 60)", new[] { 1D, 1D, 5D, 60D })]
    [Arguments("POINTZM (1 1 5 60)", new[] { 1D, 1D, 5D, 60D })]
    [Arguments("POINTZM(1 1 5 60)", new[] { 1D, 1D, 5D, 60D })]
    public async Task WktToPointZM(string wkt, double[] coordinates)
    {
        var record = new WktRecord(wkt);
        _ = await Assert.That(GetCoordinates(record.GetPointZM())).IsEquivalentTo(coordinates);

        static double[] GetCoordinates(PointZM pt)
        {
            return [pt.X, pt.Y, pt.Z, pt.Measurement];
        }
    }

    [Test]
    [Arguments("MULTIPOINT((3.5 5.6),(4.8 10.5))", 2)]
    [Arguments("MULTIPOINT EMPTY", 0)]
    [Arguments("MULTIPOINT(10 40, 40 30, 20 20, 30 10)", 4)]
    public async Task WktToMultiPoint(string wkt, int count)
    {
        var record = new WktRecord(wkt);
        var points = record.GetMultiPoint();
        _ = await Assert.That(points).IsNotNull().And.HasCount().EqualTo(count);
    }

    [Test]
    [Arguments("LINESTRING(3 4,10 50,20 25)", 3)]
    [Arguments("LINESTRING EMPTY", 0)]
    public async Task WktToLineString(string wkt, int count)
    {
        var record = new WktRecord(wkt);
        var lineString = record.GetLineString();
        _ = await Assert.That(lineString).IsNotNull().And.HasCount().EqualTo(count);
    }

    [Test]
    [Arguments("MULTILINESTRING((3 4,10 50,20 25),(-5 -8,-10 -8,-15 -4))", 2)]
    [Arguments("MULTILINESTRING EMPTY", 0)]
    public async Task WktToMultiLineString(string wkt, int count)
    {
        var record = new WktRecord(wkt);
        var lineStrings = record.GetMultiLineString();
        _ = await Assert.That(lineStrings).IsNotNull().And.HasCount().EqualTo(count);
    }

    [Test]
    [Arguments("POLYGON((1 1,5 1,5 5,1 5,1 1),(2 2, 3 2, 3 3, 2 3,2 2))", 2)]
    [Arguments("POLYGON EMPTY", 0)]
    [Arguments("POLYGON ((35 10, 45 45, 15 40, 10 20, 35 10), (20 30, 35 35, 30 20, 20 30))", 2)]
    public async Task WktToPolygon(string wkt, int count)
    {
        var record = new WktRecord(wkt);
        var polygon = record.GetPolygon();
        _ = await Assert.That(polygon).IsNotNull().And.HasCount().EqualTo(count);
    }

    [Test]
    [Arguments("MULTIPOLYGON(((1 1,5 1,5 5,1 5,1 1),(2 2, 3 2, 3 3, 2 3,2 2)),((3 3,6 2,6 4,3 3)))", 2)]
    [Arguments("MULTIPOLYGON EMPTY", 0)]
    public async Task WktToMultiPolygon(string wkt, int count)
    {
        var record = new WktRecord(wkt);
        var polygons = record.GetMultiPolygon();
        _ = await Assert.That(polygons).IsNotNull().And.HasCount().EqualTo(count);
    }
}