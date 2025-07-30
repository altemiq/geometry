// -----------------------------------------------------------------------
// <copyright file="TinyWkbWriterTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

public class TinyWkbWriterTests
{
    [Test]
    public Task WriteEmptyPoint() => RunTest(
        writer => writer.Write(Point.Empty),
        "\\x0110");

    [Test]
    public Task WriteEmptyPointZ() => RunTest(
        writer => writer.Write(PointZ.Empty),
        "\\x011801");

    [Test]
    public Task WriteEmptyPointM() => RunTest(
        writer => writer.Write(PointM.Empty),
        "\\x011802");

    [Test]
    public Task WriteEmptyPointZM() => RunTest(
        writer => writer.Write(PointZM.Empty),
        "\\x011803");

    [Test]
    public Task WritePoint() => RunTest(
        static writer => writer.Write(new Point(30.1, 10.2), 1),
        "\\x2100da04cc01");

    [Test]
    public Task WritePointZ() => RunTest(
        static writer => writer.Write(new PointZ(30.1, 10.2, 20.3), 1, 1),
        "\\x210805da04cc019603");

    [Test]
    public Task WritePointM() => RunTest(
        static writer => writer.Write(new PointM(30.1, 10.2, 20.3), 1, 1),
        "\\x210822da04cc019603");

    [Test]
    public Task WritePointZM() => RunTest(
        static writer => writer.Write(new PointZM(30.1, 10.2, 20.3, 15.4), 1, 1, 1),
        "\\x210827da04cc019603b402");

    [Test]
    public Task WriteEmptyMultiPoint() => RunTest(writer => writer.Write(Enumerable.Empty<Point>()), "\\x0410");

    [Test]
    public Task WriteEmptyMultiPointZ() => RunTest(writer => writer.Write(Enumerable.Empty<PointZ>()), "\\x041801");

    [Test]
    public Task WriteEmptyMultiPointM() => RunTest(writer => writer.Write(Enumerable.Empty<PointM>()), "\\x041802");

    [Test]
    public Task WriteEmptyMultiPointZM() => RunTest(writer => writer.Write(Enumerable.Empty<PointZM>()), "\\x041803");

    [Test]
    public Task WriteMultiPoint() => RunTest(
        static writer => writer.Write(new Point[]
        {
            new(10.1, 40.2),
            new(40.3, 30.4),
            new(20.5, 20.6),
            new(30.7, 10.8),
        }, 1),
        "\\x240004ca01a406dc04c3018b03c301cc01c301");

    [Test]
    public Task WriteEmptyLineString() => RunTest(static writer => writer.Write(Polyline.FromPoints()), "\\x0210");

    [Test]
    public Task WriteLineString() => RunTest(
        static writer => writer.Write(new Polyline(new(30.1, 10.2), new(10.3, 30.4), new(40.5, 40.6)), 1),
        "\\x220003da04cc018b039403dc04cc01");

    [Test]
    public Task WriteLineStringZ() => RunTest(
        static writer => writer.Write(new PolylineZ(new(30.1, 10.2, 20.3), new(10.4, 30.5, 5.6), new(40.7, 40.8, 40.9)), 1, 1),
        "\\x22080503da04cc01960389039603a502de04ce01c205");

    [Test]
    public Task WriteLineStringZM() => RunTest(
        static writer => writer.Write(new PolylineZM(new(30.1, 10.2, 20.3, 15.4), new(10.5, 30.6, 5.7, 20.8), new(40.9, 40.0, 40.1, 40.2)), 1, 1, 1),
        "\\x22082703da04cc019603b40287039803a3026ce004bc01b0058403");

    [Test]
    public Task WriteEmptyMultiLineString() => RunTest(
        writer => writer.Write(Enumerable.Empty<Polyline>()),
        "\\x0510");

    [Test]
    public Task WriteEmptyMultiLineStringZ() => RunTest(
        writer => writer.Write(Enumerable.Empty<PolylineZ>()),
        "\\x051801");

    [Test]
    public Task WriteEmptyMultiLineStringM() => RunTest(
        writer => writer.Write(Enumerable.Empty<PolylineM>()),
        "\\x051802");

    [Test]
    public Task WriteEmptyMultiLineStringZM() => RunTest(
        writer => writer.Write(Enumerable.Empty<PolylineZM>()),
        "\\x051803");

    [Test]
    public Task WriteMultiLineString() => RunTest(
        static writer => writer.Write(
        [
            new Polyline(new(10.1, 10.2), new(20.3, 20.4), new(10.5, 40.6)),
            new Polyline(new(40.7, 40.8), new(30.9, 30.0), new(40.1, 20.2), new(30.3, 10.4)),
        ], 1),
        "\\x25000203ca01cc01cc01cc01c301940304dc0404c301d701b801c301c301c301");

    [Test]
    public Task WritePolygonSimple() => RunTest(
        static writer => writer.Write(new Polygon(
            new Point[]
            {
                new(30.1, 10.2),
                new(10.3, 20.4),
                new(20.5, 40.6),
                new(40.7, 40.8),
                new(30.1, 10.2),
            }), 1),
        "\\x23000105da04cc018b03cc01cc019403940304d301e304");

    [Test]
    public Task WritePolygonZSimple() => RunTest(
        static writer => writer.Write(new PolygonZ(
            new PointZ[]
            {
                new(30.1, 10.2, 20.3),
                new(10.4, 20.5, 30.6),
                new(20.7, 40.8, 30.9),
                new(40.0, 40.1, 40.2),
                new(30.1, 10.2, 20.3),
            }), 1, 1),
        "\\x2308050105da04cc0196038903ce01ce01ce0196030682030dba01c501d5048d03");

    [Test]
    public Task WritePolygonZMSimple() => RunTest(
        static writer => writer.Write(new PolygonZM(
            new PointZM[]
            {
                new(30.1, 10.2, 20.3, 15.4),
                new(10.5, 20.6, 30.7, 15.8),
                new(20.9, 40.0, 30.1, 50.2),
                new(40.3, 40.4, 40.5, 40.6),
                new(30.1, 10.2, 20.3, 15.4),
            }), 1, 1, 1),
        "\\x2308270105da04cc019603b4028703d001d00108d00184030bb005840308d001bf01cb01db049303f703");

    [Test]
    public Task WriteEmptyPolygon() => RunTest(static writer => writer.Write(new Polygon()), "\\x0310");

    [Test]
    public Task WriteEmptyMultiPolygon() => RunTest(writer => writer.Write(Enumerable.Empty<Polygon>()), "\\x0610");

    [Test]
    public Task WriteEmptyMultiPolygonZ() => RunTest(writer => writer.Write(Enumerable.Empty<PolygonZ>()), "\\x061801");

    [Test]
    public Task WriteEmptyMultiPolygonM() => RunTest(writer => writer.Write(Enumerable.Empty<PolygonM>()), "\\x061802");

    [Test]
    public Task WriteEmptyMultiPolygonZM() => RunTest(writer => writer.Write(Enumerable.Empty<PolygonZM>()), "\\x061803");
    [Test]
    public Task WriteMultiPolygonSimple() => RunTest(
        static writer => writer.Write(
        [
            Polygon.FromPoints(new(30.1, 20.2), new(10.3, 40.4), new(45.5, 40.6), new(30.1, 20.2)),
            Polygon.FromPoints(new(15.1, 5.2), new(40.3, 10.4), new(10.5, 20.6), new(5.7 ,10.8), new(15.1, 5.2)),
        ], 1),
        "\\x2600020104da0494038b039403c00504b30297030105ab02ab02f80368d304cc015fc301bc016f");

    [Test]
    public Task GetMultiPolygonComplex() => RunTest(
        static writer => writer.Write(
            Polygon.FromPoints(new(40, 40), new(20, 45), new(45, 30), new(40, 40)),
            Polygon.FromPoints(
                [
                    new(20, 35),
                    new(45, 20),
                    new(30, 5),
                    new(10, 10),
                    new(10, 30),
                    new(20, 35)
                ],
                [
                    [
                        new(30, 20),
                        new(20, 25),
                        new(20, 15),
                        new(30, 20)
                    ]
                ]
            )),
        "\\x06000201045050270a321d091402062709321d1d1d270a0028140a04141d130a0013140a");

    private static async Task RunTest(
        Action<TinyWkbWriter> geometryWriter,
        string expected)
    {
        using var memoryStream = new MemoryStream();
        using (var writer = new TinyWkbWriter(memoryStream))
        {
            geometryWriter(writer);
        }

        _ = await Assert.That(ToPostGis(memoryStream.ToArray()))
            .IsEqualTo(expected);

        static string ToPostGis(byte[] bytes)
        {
            return string.Concat("\\x", string.Concat(bytes.Select(static @byte => @byte.ToString("x2"))));
        }
    }
}