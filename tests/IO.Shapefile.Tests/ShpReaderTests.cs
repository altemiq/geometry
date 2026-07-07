// -----------------------------------------------------------------------
// <copyright file="ShpReaderTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Shapefile;

public class ShpReaderTests
{
    [Test]
    public async Task ReadMultiPnt()
    {
        using var reader = GetReader("multipnt.shp");
        _ = await Assert.That(reader.Header.ShpType).IsEqualTo(ShpType.MultiPoint);
        _ = await Assert.That(reader.Read())
            .IsNotNull().And
            .Satisfies(
                static record => record.GetGeometry(),
                static geometry => geometry
                    .IsTypeOf<IEnumerable<Point>>().And
                    .IsEquivalentTo([new Point(483575.5, 4753046)]));
        _ = await Assert.That(reader.Read()).IsNull();
    }

    [Test]
    public async Task Read3dPoints()
    {
        using var reader = GetReader("3dpoints.shp");
        _ = await Assert.That(reader.Header.ShpType).IsEqualTo(ShpType.PointZ);
        _ = await Assert.That(reader.Read())
            .IsNotNull().And
            .Satisfies(
                static record => record.GetGeometry(),
                static geometry => geometry
                    .IsTypeOf<PointZ>().And
                    .IsEquivalentTo(new PointZ(0.40639999999999965, 7.484799999999999, 0.0)));
    }

    private static ShpReader GetReader(string name) => new(Resources.GetStream(name));
}