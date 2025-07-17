// -----------------------------------------------------------------------
// <copyright file="QixReaderTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

public class QixReaderTests
{
    [Test]
    public async Task ReadPLine()
    {
        var reader = new QixReader(Resources.GetStream("pline.qix"));
        _ = await Assert.That(reader.Count).IsEqualTo(7);

        var node = reader.Read();
        _ = await Assert.That(node).IsNotEqualTo(QixNode.Empty);

        _ = await Assert.That(node.Extents).IsEqualTo(new(1296367.50, 228199.390625, 1302699.00, 237185.03125));
        _ = await Assert.That(node.Shapes).IsEmpty();
    }
}