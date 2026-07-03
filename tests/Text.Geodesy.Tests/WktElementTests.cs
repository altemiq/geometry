namespace Altemiq.Text.Geodesy;

public class WktElementTests
{
    [Test]
    public async Task ParseText()
    {
        const string Text = "GEOGCS[\"WGS 84\",DATUM[\"World Geodetic System 1984\",SPHEROID[\"WGS 84\",6378137.0,298.257223563,AUTHORITY[\"EPSG\",7030]],AUTHORITY[\"EPSG\",6326]],PRIMEM[\"Greenwich\",0.0,AUTHORITY[\"EPSG\",8901]],UNIT[\"degree\",0.017453292519943295,AUTHORITY[\"EPSG\",9122]],AXIS[\"Latitude\",NORTH],AXIS[\"Longitude\",EAST]]";
        await Assert.That(() => WktElement.Parse(Text)).ThrowsNothing().And
            .Member(t => t.ValueKind, v => v.IsEqualTo(WktValueKind.Object));
    }


    [Test]
    public async Task ParseBytes()
    {
        await Assert.That(() => WktElement.Parse("GEOGCS[\"WGS 84\",DATUM[\"World Geodetic System 1984\",SPHEROID[\"WGS 84\",6378137.0,298.257223563,AUTHORITY[\"EPSG\",7030]],AUTHORITY[\"EPSG\",6326]],PRIMEM[\"Greenwich\",0.0,AUTHORITY[\"EPSG\",8901]],UNIT[\"degree\",0.017453292519943295,AUTHORITY[\"EPSG\",9122]],AXIS[\"Latitude\",NORTH],AXIS[\"Longitude\",EAST]]"u8))
            .ThrowsNothing().And
            .Member(t => t.ValueKind, v => v.IsEqualTo(WktValueKind.Object));
    }
}
