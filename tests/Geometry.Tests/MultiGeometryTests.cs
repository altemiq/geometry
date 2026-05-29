namespace Altemiq.Geometry;

using System.Reflection;

public class MultiGeometryTests
{
    [Test]
    [MethodDataSource(nameof(GetMultiGeometries))]
    public async Task CreateMultiGeometry(IGeometry[] geometries, Type expectedType)
    {
        // get the type of values
        var type = geometries.First().GetType();
    
        // cast the geometries to the correct type
        var castMethod = typeof(MultiGeometryTests).GetMethod(nameof(CastArray), BindingFlags.NonPublic | BindingFlags.Static)!.MakeGenericMethod(typeof(IGeometry), type);
        var createMethod = typeof(MultiGeometryTests).GetMethod(nameof(MultiGeometryCreate), BindingFlags.NonPublic | BindingFlags.Static)!.MakeGenericMethod(type);
    
        await Assert.That(createMethod.Invoke(null, [castMethod.Invoke(null, [geometries])])).IsNotNull().And.IsAssignableTo(expectedType, nameof(expectedType));
    }
    
    public IEnumerable<Func<(IGeometry[] geometry, Type expectedType)>> GetMultiGeometries()
    {
        yield return () => ([new Point(1, 2), new Point(3, 4)], typeof(IGeometry));
        yield return () => ([new PointZ(1, 2, 3), new PointZ(4, 5, 6)], typeof(IGeometryZ));
        yield return () => ([new PointM(1, 2, 3), new PointM(4, 5, 6)], typeof(IGeometryM));
        yield return () => ([new PointZM(1, 2, 3, 4), new PointZM(5, 6, 7, 8)], typeof(IGeometryM));
    
        yield return () => ([new Polyline(new Point(1, 2), new Point(3, 4))], typeof(IGeometry));
        yield return () => ([new PolylineZ(new PointZ(1, 2, 3), new PointZ(4, 5, 6))], typeof(IGeometryZ));
        yield return () => ([new PolylineM(new PointM(1, 2, 3), new PointM(4, 5, 6))], typeof(IGeometryM));
        yield return () => ([new PolylineZM(new PointZM(1, 2, 3, 4), new PointZM(5, 6, 7, 8))], typeof(IGeometryM));
    
        yield return () => ([new Polygon(new LinearRing<Point>(new Point(1, 2), new Point(3, 4)))], typeof(IGeometry));
        yield return () => ([new PolygonZ(new LinearRing<PointZ>(new PointZ(1, 2, 3), new PointZ(4, 5, 6)))], typeof(IGeometryZ));
        yield return () => ([new PolygonM(new LinearRing<PointM>(new PointM(1, 2, 3), new PointM(4, 5, 6)))], typeof(IGeometryM));
        yield return () => ([new PolygonZM(new LinearRing<PointZM>(new PointZM(1, 2, 3, 4), new PointZM(5, 6, 7, 8)))], typeof(IGeometryM));
    }
    
    private static MultiGeometry<T> MultiGeometryCreate<T>(T[] geometries)
        where T : IGeometry
    {
        return MultiGeometry.Create(geometries.AsSpan());
    }
    
    private static TTarget[] CastArray<TSource, TTarget>(TSource[] source)
        where TTarget : TSource
    {
        var destination = new TTarget[source.Length];
        for (var i = 0; i < source.Length; i++)
        {
            destination[i] = (TTarget)source[i];
        }
    
        return destination;
    }
}