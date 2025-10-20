// -----------------------------------------------------------------------
// <copyright file="ShpReaderTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

public class ShpReaderTests
{
    [Test]
    public async Task ReadMultiPnt()
    {
        using var reader = GetReader("multipnt.shp");
        _ = await Assert.That(reader.Header.ShpType).IsEqualTo(ShpType.MultiPoint);
        var record = await Assert.That(reader.Read()).IsNotNull();

        var multipnt = await Assert.That(record.GetGeometry()).IsAssignableTo<IEnumerable<Point>>().And.IsNotNull();

        using var enumerator = multipnt.GetEnumerator();
        _ = await Assert.That(enumerator.MoveNext()).IsTrue();

        var current = enumerator.Current;
        _ = await Assert.That(current).IsEquivalentTo(new Point(483575.5, 4753046));

        _ = await Assert.That(enumerator.MoveNext()).IsFalse();
        _ = await Assert.That(reader.Read()).IsNull();
    }

    [Test]
    public async Task Read3dpoints()
    {
        using var reader = GetReader("3dpoints.shp");
        _ = await Assert.That(reader.Header.ShpType).IsEqualTo(ShpType.PointZ);
        var record = await Assert.That(reader.Read()).IsNotNull();

        var point = await Assert.That(record!.GetGeometry()).IsTypeOf<PointZ>();

        _ = await Assert.That(point).IsEquivalentTo(new PointZ(0.40639999999999965, 7.484799999999999, 0.0));
    }

    private static ShpReader GetReader(string name) => new(Resources.GetStream(name));
}