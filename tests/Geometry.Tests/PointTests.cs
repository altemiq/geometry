namespace Altemiq.Geometry;

public class PointTests
{
    private const double X = 1;
    private const double Y = 2;
    private const double Z = 3;
    private const double M = 4;

    [Test]
    [MethodDataSource(nameof(GetPointInstances))]
    public async Task GetMinX(IGeometry pt)
    {
        await Assert.That(pt.MinX()).IsEqualTo(X);
    }
    
    [Test]
    [MethodDataSource(nameof(GetPointInstances))]
    public async Task GetMaxX(IGeometry pt)
    {
        await Assert.That(pt.MaxX()).IsEqualTo(X);
    }
    
    
    [Test]
    [MethodDataSource(nameof(GetPointInstances))]
    public async Task GetMinY(IGeometry pt)
    {
        await Assert.That(pt.MinY()).IsEqualTo(Y);
    }
    
    [Test]
    [MethodDataSource(nameof(GetPointInstances))]
    public async Task GetMaxY(IGeometry pt)
    {
        await Assert.That(pt.MaxY()).IsEqualTo(Y);
    }
    
    [Test]
    [MethodDataSource(nameof(GetPointZInstances))]
    public async Task GetMinZ(IGeometryZ pt)
    {
        await Assert.That(pt.MinZ()).IsEqualTo(Z);
    }
    
    [Test]
    [MethodDataSource(nameof(GetPointZInstances))]
    public async Task GetMaxZ(IGeometryZ pt)
    {
        await Assert.That(pt.MaxZ()).IsEqualTo(Z);
    }
    
    [Test]
    [MethodDataSource(nameof(GetPointMInstances))]
    public async Task GetMinM(IGeometryM pt)
    {
        await Assert.That(pt.MinM()).IsEqualTo(M);
    }
    
    [Test]
    [MethodDataSource(nameof(GetPointMInstances))]
    public async Task GetMaxM(IGeometryM pt)
    {
        await Assert.That(pt.MaxM()).IsEqualTo(M);
    }
    
    public IEnumerable<Func<IGeometry>> GetPointInstances()
    {
        yield return () => new Point(X, Y);
        yield return () => new PointZ(X, Y, Z);
        yield return () => new PointM(X, Y, M);
        yield return () => new PointZM(X, Y, X, M);
    }
    
    public IEnumerable<Func<IGeometryZ>> GetPointZInstances()
    {
        yield return () => new PointZ(X, Y, Z);
        yield return () => new PointZM(X, Y, Z, M);
    }
    
    public IEnumerable<Func<IGeometryM>> GetPointMInstances()
    {
        yield return () => new PointM(X, Y, M);
        yield return () => new PointZM(X, Y, Z, M);
    }
}