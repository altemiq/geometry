namespace Altemiq.Text.Geodesy;

public class Utf8WktReaderTests
{
    [Test]
    public async Task Read()
    {
        const string Text = "GEOGCS[\"WGS 84\",DATUM[\"World Geodetic System 1984\",SPHEROID[\"WGS 84\",6378137.0,298.257223563,AUTHORITY[\"EPSG\",7030]],AUTHORITY[\"EPSG\",6326]],PRIMEM[\"Greenwich\",0.0,AUTHORITY[\"EPSG\",8901]],UNIT[\"degree\",0.017453292519943295,AUTHORITY[\"EPSG\",9122]],AXIS[\"Latitude\",NORTH],AXIS[\"Longitude\",EAST]]";
        var bytes = System.Text.Encoding.UTF8.GetBytes(Text);

        var reader = new Utf8WktReader(bytes.AsSpan());
        var read = reader.Read();
        var tokenType = reader.TokenType;
        var readLiteral = reader.TryGetLiteral(out var literal);
        var literalString = literal.ToString();
        await Assert.That(read).IsTrue();
        await Assert.That(tokenType).IsNotEqualTo(WktTokenType.None);
        await Assert.That(readLiteral).IsTrue();
        await Assert.That(literalString).IsEqualTo("GEOGCS");
    }

    [Test]
    public async Task ReadAll()
    {
        const string Text = "GEOGCS[\"WGS 84\",DATUM[\"World Geodetic System 1984\",SPHEROID[\"WGS 84\",6378137.0,298.257223563,AUTHORITY[\"EPSG\",7030]],AUTHORITY[\"EPSG\",6326]],PRIMEM[\"Greenwich\",0.0,AUTHORITY[\"EPSG\",8901]],UNIT[\"degree\",0.017453292519943295,AUTHORITY[\"EPSG\",9122]],AXIS[\"Latitude\",NORTH],AXIS[\"Longitude\",EAST]]";
        var bytes = System.Text.Encoding.UTF8.GetBytes(Text);

        await Assert.That(() =>
        {
            var reader = new Utf8WktReader(bytes.AsSpan());
            while (reader.Read())
            {
            }
        }).ThrowsNothing();
    }
}
