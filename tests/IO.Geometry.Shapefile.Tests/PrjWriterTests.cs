// -----------------------------------------------------------------------
// <copyright file="PrjWriterTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

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

        var wkt = new Geodesy.WellKnownTextNode(System.Text.Encoding.UTF8.GetString(memoryStream.ToArray()));
        _ = await Assert.That(wkt["PROJCS"]).IsTypeOf<Geodesy.WellKnownTextNode>();
        //.And.Satisfies(n => n.Values, n => n.IsAssignableTo<IEnumerable<object>>().And.Contains(o => o.Equals("GDA_1994_MGA_Zone_55")).And.IsAssignableTo<System.Collections.IEnumerable?>());
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

        var wkt = new Geodesy.WellKnownTextNode(System.Text.Encoding.UTF8.GetString(memoryStream.ToArray()));
        _ = await Assert.That(wkt["GEOGCS"]).IsTypeOf<Geodesy.WellKnownTextNode>();
        //           .And.Satisfies(n => n.Values, n => n.IsAssignableTo<IEnumerable<object>>().And.Contains(o => o.Equals("GCS_WGS_1984")).And.IsAssignableTo<System.Collections.IEnumerable?>());
    }
}