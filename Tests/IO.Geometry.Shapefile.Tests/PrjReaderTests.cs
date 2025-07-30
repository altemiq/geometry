// -----------------------------------------------------------------------
// <copyright file="PrjReaderTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

using TUnit.Assertions.AssertConditions.Throws;

public class PrjReaderTests
{
    private const string Gda94 = "GEOGCS[\"GCS_GDA_1994\",DATUM[\"D_GDA_1994\",SPHEROID[\"GRS_1980\",6378137.0,298.257222101]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]]";

    [Test]
    public async Task ReadGda94()
    {
        var reader = new PrjReader(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(Gda94)));
        var wkt = reader.Read();
        _ = await Assert.That(wkt).IsNotNull();
    }

    [Test]
    public async Task GetPcsID()
    {
        var wkid = PrjReader.GetWellKnownId(new Geodesy.WellKnownTextNode("PROJCS[\"GDA_1994_MGA_Zone_55\",GEOGCS[\"GCS_GDA_1994\",DATUM[\"D_GDA_1994\",SPHEROID[\"GRS_1980\",6378137.0,298.257222101]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"False_Easting\",500000.0],PARAMETER[\"False_Northing\",10000000.0],PARAMETER[\"Central_Meridian\",147.0],PARAMETER[\"Scale_Factor\",0.9996],PARAMETER[\"Latitude_Of_Origin\",0.0],UNIT[\"Meter\",1.0]]"));
        _ = await Assert.That(wkid).IsEqualTo(28355);
    }

    [Test]
    public async Task GetBadPcsID() => await Assert.That(static () => PrjReader.GetWellKnownId("PROJCS[\"NOT\",GEOGCS[\"GCS_GDA_1994\",DATUM[\"D_GDA_1994\",SPHEROID[\"GRS_1980\",6378137.0,298.257222101]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"False_Easting\",500000.0],PARAMETER[\"False_Northing\",10000000.0],PARAMETER[\"Central_Meridian\",147.0],PARAMETER[\"Scale_Factor\",0.9996],PARAMETER[\"Latitude_Of_Origin\",0.0],UNIT[\"Meter\",1.0]]")).Throws<KeyNotFoundException>();

    [Test]
    public async Task TryGetPcsID()
    {
        _ = await Assert.That(PrjReader.TryGetWellKnownId("PROJCS[\"GDA_1994_MGA_Zone_55\",GEOGCS[\"GCS_GDA_1994\",DATUM[\"D_GDA_1994\",SPHEROID[\"GRS_1980\",6378137.0,298.257222101]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"False_Easting\",500000.0],PARAMETER[\"False_Northing\",10000000.0],PARAMETER[\"Central_Meridian\",147.0],PARAMETER[\"Scale_Factor\",0.9996],PARAMETER[\"Latitude_Of_Origin\",0.0],UNIT[\"Meter\",1.0]]", out var wkid)).IsTrue();
        _ = await Assert.That(wkid).IsEqualTo(28355);
    }

    [Test]
    public async Task TryGetBadPcsID()
    {
        _ = await Assert.That(PrjReader.TryGetWellKnownId("PROJCS[\"NOT\",GEOGCS[\"GCS_GDA_1994\",DATUM[\"D_GDA_1994\",SPHEROID[\"GRS_1980\",6378137.0,298.257222101]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"False_Easting\",500000.0],PARAMETER[\"False_Northing\",10000000.0],PARAMETER[\"Central_Meridian\",147.0],PARAMETER[\"Scale_Factor\",0.9996],PARAMETER[\"Latitude_Of_Origin\",0.0],UNIT[\"Meter\",1.0]]", out var wkid)).IsFalse();
        _ = await Assert.That(wkid).IsEqualTo(default);
    }

    [Test]
    public async Task GetGcsID() => await Assert.That(PrjReader.GetWellKnownId(new Geodesy.WellKnownTextNode("GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]]"))).IsEqualTo(4326);

    [Test]
    public async Task GetBadGcsID() => await Assert.That(static () => PrjReader.GetWellKnownId("GEOGCS[\"NOT\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]]")).Throws<KeyNotFoundException>();

    [Test]
    public async Task TryGetGcsID()
    {
        _ = await Assert.That(PrjReader.TryGetWellKnownId("GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]]", out var wkid)).IsTrue();
        _ = await Assert.That(wkid).IsEqualTo(4326);
    }

    [Test]
    public async Task TryGetBadGcsID()
    {
        _ = await Assert.That(PrjReader.TryGetWellKnownId("GEOGCS[\"NOT\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]]", out var wkid)).IsFalse();
        _ = await Assert.That(wkid).IsEqualTo(default);
    }
}