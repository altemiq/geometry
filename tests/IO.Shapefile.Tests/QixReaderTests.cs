// -----------------------------------------------------------------------
// <copyright file="QixReaderTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Shapefile;

public class QixReaderTests
{
    [Test]
    public async Task ReadPLine()
    {
        var reader = new QixReader(Resources.GetStream("pline.qix"));
        _ = await Assert.That(reader.Count).IsEqualTo(7);
        _ = await Assert.That(reader.Read())
            .IsNotEqualTo(QixNode.Empty).And
            .Member(static node => node.Extents, static extents => extents.IsEqualTo(new Envelope(1296367.50, 228199.390625, 1302699.00, 237185.03125))).And
            .Member(static node => node.Shapes, static shapes => shapes.IsEmpty());
    }
}