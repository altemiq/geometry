// -----------------------------------------------------------------------
// <copyright file="PrjWriterTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Shapefile;

public class PrjWriterTests
{
    [Test]
    public async Task WritePcs()
    {
        using var memoryStream = new MemoryStream();
        using (var prjWriter = new PrjWriter(memoryStream, true))
        {
            prjWriter.Write(28355);
        }

        memoryStream.Flush();

        _ = await Assert.That(Text.Geodesy.WktElement.Parse(memoryStream.ToArray()))
            .Member(static x => x.ValueKind, valueKind => valueKind.IsEqualTo(Text.Geodesy.WktValueKind.Object)).And
            .Member(static x => x.GetKeyword(), static keyword => keyword.IsEqualTo("PROJCS")).And
            .Member(static x => x[0].GetString(), s => s.IsEqualTo("GDA_1994_MGA_Zone_55"));
    }

    [Test]
    public async Task WriteGcs()
    {
        using var memoryStream = new MemoryStream();
        using (var prjWriter = new PrjWriter(memoryStream, true))
        {
            prjWriter.Write(4326);
        }

        memoryStream.Flush();

        _ = await Assert.That(Text.Geodesy.WktElement.Parse(memoryStream.ToArray()))
            .Member(static x => x.ValueKind, valueKind => valueKind.IsEqualTo(Text.Geodesy.WktValueKind.Object)).And
            .Member(static x => x.GetKeyword(), static keyword => keyword.IsEqualTo("GEOGCS")).And
            .Member(static x => x[0].GetString(), s => s.IsEqualTo("GCS_WGS_1984"));
    }
}