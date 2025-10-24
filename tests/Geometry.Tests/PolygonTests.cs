namespace Altemiq.Geometry;

public class PolygonTests
{
    [Test]
    public async Task Area()
    {
        ISurfaceGeometry polygon = Polygon.FromPoints(new(1, 6), new(3,1), new(7,2), new(4, 4), new(8,5 ));
        await Assert.That(polygon.Area()).IsEqualTo(16.5);
    }
}