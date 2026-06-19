namespace Altemiq.Grpc.Geometry.Tools;

public class ProtoTests
{
    [Test]
    public async Task Data()
    {
         var testMessage = new Protobuf.WellKnownTypes.Tests.Test { Data = new Protobuf.WellKnownTypes.GeometryData() };
         await Assert.That(testMessage.Data).IsNotNull();
    }

    public static IEnumerable<Func<Guid>> GuidData()
    {
        yield return Guid.NewGuid;
#if NET9_0_OR_GREATER
        yield return Guid.CreateVersion7;
#endif
    }
}