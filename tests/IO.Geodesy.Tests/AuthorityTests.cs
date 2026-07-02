namespace Altemiq.IO.Geodesy;

public class AuthorityTests
{
    [Test]
    [MethodDataSource(nameof(GetAuthorities))]
    public async Task ParseAuthority(Authority authority, string text, WktFormat format)
    {
        var deserialized = Text.Geodesy.WktSerializer.Deserialize<Authority>(text, Text.Geodesy.WktSerializerOptions.ForFormat(format));
        await Assert.That(deserialized).IsEqualTo(authority);
    }

    public static IEnumerable<Func<(Authority, string, WktFormat)>> GetAuthorities()
    {
        yield return () => (new Authority("EPSG", "4326"), "AUTHORITY[\"EPSG\",\"4326\"]", WktFormat.Wkt1);
        yield return () => (new Authority("EPSG", 4326), "ID[\"EPSG\",4326]", WktFormat.Wkt2);
    }
}
