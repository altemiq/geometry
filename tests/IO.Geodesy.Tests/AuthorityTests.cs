namespace Altemiq.IO.Geodesy;

public class AuthorityTests
{
    [Test]
    [MethodDataSource(nameof(GetAuthorities))]
    public async Task ParseAuthority(Authority authority, string text, WellKnownTextFormat format)
    {
        var deserialized = Serialization.WktSerializer.Deserialize<Authority>(text, new() { Format = format });
        await Assert.That(deserialized).IsEqualTo(authority);
    }

    public static IEnumerable<Func<(Authority, string, WellKnownTextFormat)>> GetAuthorities()
    {
        yield return () => (new Authority("EPSG", "4326"), "AUTHORITY[\"EPSG\",\"4326\"]", WellKnownTextFormat.Wkt1);
        yield return () => (new Authority("EPSG", 4326), "ID[\"EPSG\",4326]", WellKnownTextFormat.Wkt2);
    }
}
