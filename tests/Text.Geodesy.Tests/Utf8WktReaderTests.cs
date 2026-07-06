namespace Altemiq.Text.Geodesy;

public class Utf8WktReaderTests
{
    [Test]
    public async Task Read_FirstKeyword()
    {
        var reader = new Utf8WktReader("GEOGCS[\"WGS 84\",DATUM[\"World Geodetic System 1984\",SPHEROID[\"WGS 84\",6378137.0,298.257223563,AUTHORITY[\"EPSG\",7030]],AUTHORITY[\"EPSG\",6326]],PRIMEM[\"Greenwich\",0.0,AUTHORITY[\"EPSG\",8901]],UNIT[\"degree\",0.017453292519943295,AUTHORITY[\"EPSG\",9122]],AXIS[\"Latitude\",NORTH],AXIS[\"Longitude\",EAST]]"u8);
        var read = reader.Read();
        var tokenType = reader.TokenType;
        var readLiteral = reader.TryGetLiteral(out var literal);
        var literalString = literal.ToString();
        await Assert.That(read).IsTrue();
        await Assert.That(tokenType).IsEqualTo(WktTokenType.Keyword);
        await Assert.That(readLiteral).IsTrue();
        await Assert.That(literalString).IsEqualTo("GEOGCS");
    }

    [Test]
    public async Task Read_All()
    {
        await Assert.That(() =>
        {
            var reader = new Utf8WktReader("GEOGCS[\"WGS 84\",DATUM[\"World Geodetic System 1984\",SPHEROID[\"WGS 84\",6378137.0,298.257223563,AUTHORITY[\"EPSG\",7030]],AUTHORITY[\"EPSG\",6326]],PRIMEM[\"Greenwich\",0.0,AUTHORITY[\"EPSG\",8901]],UNIT[\"degree\",0.017453292519943295,AUTHORITY[\"EPSG\",9122]],AXIS[\"Latitude\",NORTH],AXIS[\"Longitude\",EAST]]"u8);
            while (reader.Read())
            {
            }
        }).ThrowsNothing();
    }

    [Test]
    public async Task ReadIncrementsBytesConsumed()
    {
        var reader = new Utf8WktReader("NORTH"u8);

        reader.Read();

        await Assert.That(reader.BytesConsumed).IsGreaterThan(0);
    }

    [Test]
    public async Task TokenStartIndexGetsSetAfterRead()
    {
        var reader = new Utf8WktReader("NORTH"u8);

        reader.Read();

        await Assert.That(reader.TokenStartIndex).IsEqualTo(0);
    }

    [Test]
    public async Task ValueSpanGetsSetAfterRead()
    {
        var reader = new Utf8WktReader("NORTH"u8);

        var read = reader.Read();
        var valueSpanLength = reader.ValueSpan.Length;

        await Assert.That(read).IsTrue();
        await Assert.That(valueSpanLength).IsGreaterThan(0);
    }

    [Test]
    public async Task Reset()
    {
        var reader = new Utf8WktReader("NORTH"u8);

        var read = reader.Read();
        reader.Reset();
        var bytesConsumed = reader.BytesConsumed;

        await Assert.That(read).IsTrue();
        await Assert.That(bytesConsumed).IsEqualTo(0);
    }

    [Test]
    public async Task Read_EOF_ReturnsFalse()
    {
        var reader = new Utf8WktReader([]);

        await Assert.That(reader.Read()).IsFalse();
    }

    [Test]
    public async Task Read_WhitespaceOnly()
    {
        var reader = new Utf8WktReader("   \t\n  "u8);

        await Assert.That(reader.Read()).IsFalse();
    }

    [Test]
    [MethodDataSource(nameof(GetStringValues))]
    public async Task Read_StringValue(byte[] input, string expected)
    {
        var reader = new Utf8WktReader(input);

        var stringValue = reader.Read()
            ? reader.GetString()
            : default;

        await Assert.That(reader.TokenType).IsEqualTo(WktTokenType.String);
        await Assert.That(stringValue).IsEqualTo(expected);
    }

    public static IEnumerable<Func<(byte[], string)>> GetStringValues()
    {
        yield return () => (System.Text.Encoding.UTF8.GetBytes("\"WGS 84\""), "WGS 84");
        yield return () => (System.Text.Encoding.UTF8.GetBytes("\"Datum origin is 30°25'20\"\"N\""), "Datum origin is 30°25'20\"N");
        yield return () => (System.Text.Encoding.UTF8.GetBytes("\"Test\"\"Quote\"\"Test\""), "Test\"Quote\"Test");
        yield return () => (System.Text.Encoding.UTF8.GetBytes("\"A\"\"\"\"B\""), "A\"\"B");
        yield return () => (System.Text.Encoding.UTF8.GetBytes("\"WGS\"\"84\""), "WGS\"84");
    }

    [Test]
    public async Task Read_NumberValue()
    {
        var reader = new Utf8WktReader("6378137.0"u8);

        var doubleValue = reader.Read()
            ? reader.GetDouble()
            : default;

        await Assert.That(reader.TokenType).IsEqualTo(WktTokenType.Number);
        await Assert.That(doubleValue).IsEqualTo(6378137.0);
    }

    [Test]
    public async Task Read_LiteralValue()
    {
        var reader = new Utf8WktReader("NORTH"u8);

        var literalString = reader.Read()
            ? reader.GetLiteral().ToString()
            : default;

        await Assert.That(reader.TokenType).IsEqualTo(WktTokenType.Literal);
        await Assert.That(literalString).IsEqualTo("NORTH");
    }

    [Test]
    public async Task Read_KeywordValue()
    {
        var reader = new Utf8WktReader("GEOGCS[]"u8);

        var keywordString = reader.Read()
            ? reader.GetLiteral().ToString()
            : default;

        await Assert.That(reader.TokenType).IsEqualTo(WktTokenType.Keyword);
        await Assert.That(keywordString).IsEqualTo("GEOGCS");
    }

    [Test]
    public async Task Read_StartObject()
    {
        var reader = new Utf8WktReader("["u8);

        reader.Read();

        await Assert.That(reader.TokenType).IsEqualTo(WktTokenType.StartObject);
    }

    [Test]
    public async Task Read_EndObject()
    {
        var reader = new Utf8WktReader("]"u8);

        reader.Read();

        await Assert.That(reader.TokenType).IsEqualTo(WktTokenType.EndObject);
    }

    [Test]
    public async Task Read_SeparatorSkips()
    {
        var reader = new Utf8WktReader(",\"Test\""u8);

        var stringValue = reader.Read()
            ? reader.GetString()
            : default;

        await Assert.That(reader.TokenType).IsEqualTo(WktTokenType.String);
        await Assert.That(stringValue).IsEqualTo("Test");
    }

    [Test]
    public async Task Read_NegativeNumber()
    {
        var reader = new Utf8WktReader("-6378137.0"u8);

        var doubleValue = reader.Read()
            ? reader.GetDouble()
            : default;

        await Assert.That(reader.TokenType).IsEqualTo(WktTokenType.Number);
        await Assert.That(doubleValue).IsEqualTo(-6378137.0);
    }

    [Test]
    public async Task Read_PositiveSignNumber()
    {
        var reader = new Utf8WktReader("+6378137.0"u8);

        var doubleValue = reader.Read()
            ? reader.GetDouble()
            : default;

        await Assert.That(reader.TokenType).IsEqualTo(WktTokenType.Number);
        await Assert.That(doubleValue).IsEqualTo(6378137.0);
    }

    [Test]
    public async Task Read_IntegerNumber()
    {
        var reader = new Utf8WktReader("6378137"u8);

        var doubleValue = reader.Read()
            ? reader.GetDouble()
            : default;

        await Assert.That(reader.TokenType).IsEqualTo(WktTokenType.Number);
        await Assert.That(doubleValue).IsEqualTo(6378137.0);
    }

    [Test]
    public async Task TryGetString_Success()
    {
        var reader = new Utf8WktReader("\"WGS 84\""u8);

        reader.Read();
        var result = reader.TryGetString(out var value);

        await Assert.That(result).IsTrue();
        await Assert.That(value).IsEqualTo("WGS 84");
    }

    [Test]
    public async Task TryGetString_Failure()
    {
        var reader = new Utf8WktReader("6378137.0"u8);

        reader.Read();
        var result = reader.TryGetString(out var value);

        await Assert.That(result).IsFalse();
        await Assert.That(value).IsDefault();
    }

    [Test]
    public async Task TryGetDouble_Success()
    {
        var reader = new Utf8WktReader("6378137.0"u8);

        reader.Read();
        var result = reader.TryGetDouble(out var value);

        await Assert.That(result).IsTrue();
        await Assert.That(value).IsEqualTo(6378137.0);
    }

    [Test]
    public async Task TryGetDouble_Failure()
    {
        var reader = new Utf8WktReader("\"WGS 84\""u8);

        reader.Read();
        var result = reader.TryGetDouble(out var value);

        await Assert.That(result).IsFalse();
        await Assert.That(value).IsDefault();
    }

    [Test]
    public async Task TryGetLiteral_Success()
    {
        var reader = new Utf8WktReader("NORTH"u8);

        reader.Read();
        var result = reader.TryGetLiteral(out var value);
        var valueString = result
            ? value.ToString()
            : default;

        await Assert.That(result).IsTrue();
        await Assert.That(valueString).IsEqualTo("NORTH");
    }

    [Test]
    public async Task TryGetLiteral_Failure()
    {
        var reader = new Utf8WktReader("\"WGS 84\""u8);

        reader.Read();
        var result = reader.TryGetLiteral(out _);

        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task GetString_Throws_ForNonString()
    {
        await Assert.That(() =>
        {
            var reader = new Utf8WktReader("6378137.0"u8);
            reader.Read();
            return reader.GetString();
        }).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task GetDouble_Throws_ForNonNumber()
    {
        await Assert.That(() =>
        {
            var reader = new Utf8WktReader("\"WGS 84\""u8);

            reader.Read();
            return reader.GetDouble();
        }).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task GetDouble_Throws_ForInvalidFormat()
    {
        await Assert.That(() =>
        {
            var reader = new Utf8WktReader("123.blah.123"u8);

            reader.Read();
            return reader.GetDouble();
        }).Throws<FormatException>();
    }

    [Test]
    public async Task GetLiteral_Throws_ForNonLiteral()
    {
        await Assert.That(() =>
        {
            var reader = new Utf8WktReader("\"WGS 84\""u8);

            reader.Read();
            _ = reader.GetLiteral();
        }).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task MultipleReads_ConsumesAll()
    {
        var reader = new Utf8WktReader("NORTH,\"WGS 84\",6378137.0"u8);

        var count = 0;
        while (reader.Read())
        {
            count++;
        }

        await Assert.That(count).IsEqualTo(3);
    }

    [Test]
    public async Task EmptyObject()
    {
        var reader = new Utf8WktReader("[]"u8);

        reader.Read();
        var startObject = reader.TokenType;
        reader.Read();
        var endObject = reader.TokenType;

        await Assert.That(startObject).IsEqualTo(WktTokenType.StartObject);
        await Assert.That(endObject).IsEqualTo(WktTokenType.EndObject);
    }

    [Test]
    public async Task NestedObject()
    {
        var reader = new Utf8WktReader("[[1,2],[3,4]]"u8);

        reader.Read();
        reader.Read();
        reader.Read();
        reader.Read();
        reader.Read();
        reader.Read();
        reader.Read();
        reader.Read();
        reader.Read();

        await Assert.That(reader.TokenType).IsEqualTo(WktTokenType.EndObject);
    }

    [Test]
    public async Task ObjectWithWhitespace()
    {
        var reader = new Utf8WktReader("[ 1 , 2 , 3 ]"u8);

        reader.Read();
        reader.Read();
        var value1 = reader.GetDouble();
        reader.Read();
        var value2 = reader.GetDouble();
        reader.Read();
        var value3 = reader.GetDouble();

        await Assert.That(value1).IsEqualTo(1.0);
        await Assert.That(value2).IsEqualTo(2.0);
        await Assert.That(value3).IsEqualTo(3.0);
    }
}