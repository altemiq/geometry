namespace Altemiq.Protobuf.Geometry.Converters;

using Altemiq.Geometry.Protobuf.Converters;

public class GeometryConverterTests
{
    [Test]
    public async Task PointFromWkt()
    {
        await Assert.That(GeometryConverter.Parse<Altemiq.Geometry.Point>(new() { Wkt = "POINT (10.1 11.2)" })).IsEquivalentTo(new { X = 10.1, Y = 11.2, IsEmpty = false });
    }

    [Test]
    public async Task PointFromWkb()
    {
        await Assert.That(GeometryConverter.Parse<Altemiq.Geometry.Point>(new() { Wkb = Google.Protobuf.ByteString.FromBase64("AQEAAAAAAAAAAAA+QAAAAAAAACRA") })).IsEquivalentTo(new { X = 30.0, Y = 10.0, IsEmpty = false });
    }

    [Test]
    public async Task PointFromEwkb()
    {
        await Assert.That(GeometryConverter.Parse<Altemiq.Geometry.Point>(new() { Ewkb = Google.Protobuf.ByteString.FromBase64("AQEAACAoDAAAAAAAAAAAPkAAAAAAAAAkQA==") })).IsEquivalentTo(new { X = 30.0, Y = 10.0, IsEmpty = false });
    }

    [Test]
    public async Task PointFromGeoJson()
    {
        await Assert.That(GeometryConverter.Parse<Altemiq.Geometry.Point>(new() { Geojson = "{ \"type\": \"Point\", \"coordinates\": [-104.99404, 39.75621] }" })).IsEquivalentTo(new { X = -104.99404, Y = 39.75621, IsEmpty = false });
    }

    [Test]
    public async Task GeometryFromGeoJson()
    {
        await Assert.That(GeometryConverter.Parse<Altemiq.Geometry.IGeometry>(new() { Geojson = "{ \"type\": \"Point\", \"coordinates\": [-104.99404, 39.75621] }" })).IsTypeOf<Altemiq.Geometry.Point>().And.IsEquivalentTo(new { X = -104.99404, Y = 39.75621, IsEmpty = false });
    }
}