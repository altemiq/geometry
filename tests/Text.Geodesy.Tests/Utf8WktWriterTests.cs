namespace Altemiq.Text.Geodesy;

public class Utf8WktWriterTests
{
    [Test]
    [MethodDataSource(nameof(GetWktValues))]
    public async Task Format(string expected, bool indented)
    {
        using var memoryStream = new MemoryStream();
        var options = default(WktWriterOptions);
        options.Indented = indented;
        using (var writer = new Utf8WktWriter(memoryStream, options))
        {
            writer.WriteStartObject("GEOGCS");
            writer.WriteStringValue("WGS 84");

            // Datum
            writer.WriteStartObject("DATUM");
            writer.WriteStringValue("World Geodetic System 1984");

            // Spheroid
            writer.WriteStartObject("SPHEROID");
            writer.WriteStringValue("WGS 84");
            writer.WriteNumberValue(6378137.0);
            writer.WriteNumberValue(298.257223563);

            // Spheroid Authority
            writer.WriteStartObject("AUTHORITY");
            writer.WriteStringValue("EPSG");
            writer.WriteNumberValue(7030);
            writer.WriteEndObject();

            writer.WriteEndObject();

            // Datum Authority
            writer.WriteStartObject("AUTHORITY");
            writer.WriteStringValue("EPSG");
            writer.WriteNumberValue(6326);
            writer.WriteEndObject();

            writer.WriteEndObject();

            // Prime Meridian
            writer.WriteStartObject("PRIMEM");
            writer.WriteStringValue("Greenwich");
            writer.WriteNumberValue(0.0);

            // Prime Meridian Authority
            writer.WriteStartObject("AUTHORITY");
            writer.WriteStringValue("EPSG");
            writer.WriteNumberValue(8901);
            writer.WriteEndObject();

            writer.WriteEndObject();

            // Unit
            writer.WriteStartObject("UNIT");
            writer.WriteStringValue("degree");
            writer.WriteNumberValue(0.017453292519943295);

            // Unit Authority
            writer.WriteStartObject("AUTHORITY");
            writer.WriteStringValue("EPSG");
            writer.WriteNumberValue(9122);
            writer.WriteEndObject();

            writer.WriteEndObject();

            // Axis Latitude
            writer.WriteStartObject("AXIS");
            writer.WriteStringValue("Latitude");
            writer.WriteLiteralValue("NORTH");
            writer.WriteEndObject();

            // Axis Longitude
            writer.WriteStartObject("AXIS");
            writer.WriteStringValue("Longitude");
            writer.WriteLiteralValue("EAST");
            writer.WriteEndObject();

            writer.WriteEndObject();
        }

        await Assert.That(System.Text.Encoding.UTF8.GetString(memoryStream.ToArray())).IsEqualTo(expected);
    }

    public IEnumerable<Func<(string, bool)>> GetWktValues()
    {
        yield return () => ("GEOGCS[\"WGS 84\",DATUM[\"World Geodetic System 1984\",SPHEROID[\"WGS 84\",6378137,298.257223563,AUTHORITY[\"EPSG\",7030]],AUTHORITY[\"EPSG\",6326]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",8901]],UNIT[\"degree\",0.017453292519943295,AUTHORITY[\"EPSG\",9122]],AXIS[\"Latitude\",NORTH],AXIS[\"Longitude\",EAST]]", false);
        yield return () => (
        """
        GEOGCS["WGS 84",
            DATUM["World Geodetic System 1984",
                SPHEROID["WGS 84",6378137,298.257223563,
                    AUTHORITY["EPSG",7030]],
                AUTHORITY["EPSG",6326]],
            PRIMEM["Greenwich",0,
                AUTHORITY["EPSG",8901]],
            UNIT["degree",0.017453292519943295,
                AUTHORITY["EPSG",9122]],
            AXIS["Latitude",NORTH],
            AXIS["Longitude",EAST]]
        """, true);
    }

    [Test]
    public async Task Write_StringWithEmbeddedQuotes_Minimized()
    {
        var memoryStream = new MemoryStream();
        var writer = new Utf8WktWriter(memoryStream);
        writer.WriteStringValue("Datum origin is 30°25'20\"N");
        writer.Flush();

        var text = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
        await Assert.That(text).IsEqualTo("\"Datum origin is 30°25'20\"\"N\"");
    }

    [Test]
    public async Task Write_StringWithEmbeddedQuotes_Indented()
    {
        var memoryStream = new MemoryStream();
        var options = new WktWriterOptions { Indented = true };
        var writer = new Utf8WktWriter(memoryStream, options);
        writer.WriteStringValue("Test\"Quote\"Test");
        writer.Flush();

        var text = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
        await Assert.That(text).IsEqualTo("\"Test\"\"Quote\"\"Test\"");
    }

    [Test]
    public async Task Write_ComplexWktWithEmbeddedQuotes()
    {
        var memoryStream = new MemoryStream();
        var writer = new Utf8WktWriter(memoryStream);
        writer.WriteStartObject("GEOGCS");
        writer.WriteStringValue("WGS\"84");
        writer.WriteStartObject("DATUM");
        writer.WriteStringValue("World Geodetic System 1984\" (G1762)");
        writer.WriteStartObject("SPHEROID");
        writer.WriteStringValue("WGS\"84");
        writer.WriteNumberValue(6378137.0);
        writer.WriteNumberValue(298.257223563);
        writer.WriteEndObject();
        writer.WriteEndObject();
        writer.WriteEndObject();
        writer.Flush();

        await Assert.That(System.Text.Encoding.UTF8.GetString(memoryStream.ToArray())).IsEqualTo("GEOGCS[\"WGS\"\"84\",DATUM[\"World Geodetic System 1984\"\" (G1762)\",SPHEROID[\"WGS\"\"84\",6378137,298.257223563]]]");
    }

    [Test]
    public async Task RoundTrip_StringWithEmbeddedQuotes()
    {
        const string originalValue = "Datum origin is 30°25'20\"N";

        using var memoryStream = new MemoryStream();
        using (var writer = new Utf8WktWriter(memoryStream))
        {
            writer.WriteStringValue(originalValue);
        }

        var reader = new Utf8WktReader(memoryStream.ToArray());
        reader.Read();
        await Assert.That(reader.GetString()).IsEqualTo(originalValue);
    }
}