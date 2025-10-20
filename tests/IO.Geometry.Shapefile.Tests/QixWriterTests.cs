// -----------------------------------------------------------------------
// <copyright file="QixWriterTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

public class QixWriterTests
{
    [Test]
    public async Task WriteMultiPnt()
    {
        var stream = new MemoryStream();
        using (var writer = new QixWriter(stream, leaveOpen: true))
        {
            var node = new QixNode
            {
                Extents = new(483575.50, 4753046.00, 483575.50, 4753046.00),
                Shapes = [0],
            };

            writer.Write(node);
        }

        _ = await Assert.That(stream.ToArray()).IsEquivalentTo(Resources.GetBytes("multipnt.qix"));
    }
}