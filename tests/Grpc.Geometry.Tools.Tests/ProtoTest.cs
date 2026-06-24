namespace Altemiq.Grpc.Geometry.Tools;

public class ProtoTests
{
    [Test]
    public async Task Data()
    {
        var testMessage = new Protobuf.WellKnownTypes.Tests.Test
        {
            Data = new Protobuf.WellKnownTypes.GeometryData(),
            Uuid = Protobuf.WellKnownTypes.Uuid.ForGuid(Guid.NewGuid()),
        };

        await Assert.That(testMessage.Data).IsNotNull();
    }
}