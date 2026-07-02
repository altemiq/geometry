namespace Altemiq.Text.Geodesy;

public class Utf8WktReaderTests
{
    [Test]
    public async Task Read_FirstKeyword()
    {
        const string Text = "GEOGCS[\"WGS 84\",DATUM[\"World Geodetic System 1984\",SPHEROID[\"WGS 84\",6378137.0,298.257223563,AUTHORITY[\"EPSG\",7030]],AUTHORITY[\"EPSG\",6326]],PRIMEM[\"Greenwich\",0.0,AUTHORITY[\"EPSG\",8901]],UNIT[\"degree\",0.017453292519943295,AUTHORITY[\"EPSG\",9122]],AXIS[\"Latitude\",NORTH],AXIS[\"Longitude\",EAST]]";
        var bytes = System.Text.Encoding.UTF8.GetBytes(Text);

        var reader = new Utf8WktReader(bytes.AsSpan());
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

    [Test]
    public async Task ReadIncrementsBytesConsumed()
    {
        const string Text = "NORTH";
        var bytes = System.Text.Encoding.UTF8.GetBytes(Text);
        var reader = new Utf8WktReader(bytes.AsSpan());
        
        reader.Read();
        var bytesConsumedAfterRead = reader.BytesConsumed;
        
        await Assert.That(bytesConsumedAfterRead).IsGreaterThan(0);
    }

    [Test]
    public async Task TokenStartIndexGetsSetAfterRead()
    {
        const string Text = "NORTH";
        var bytes = System.Text.Encoding.UTF8.GetBytes(Text);
        var reader = new Utf8WktReader(bytes.AsSpan());
        
        reader.Read();
        var tokenStartIndex = reader.TokenStartIndex;
        
        await Assert.That(tokenStartIndex).IsEqualTo(0);
    }

    [Test]
    public async Task ValueSpanGetsSetAfterRead()
    {
        const string Text = "NORTH";
        var bytes = System.Text.Encoding.UTF8.GetBytes(Text);
        var reader = new Utf8WktReader(bytes.AsSpan());
        
        var read = reader.Read();
        var valueSpanLength = reader.ValueSpan.Length;

        await Assert.That(read).IsTrue();
        await Assert.That(valueSpanLength).IsGreaterThan(0);
    }

    [Test]
    public async Task Reset()
    {
        const string Text = "NORTH";
        var bytes = System.Text.Encoding.UTF8.GetBytes(Text);
        var reader = new Utf8WktReader(bytes.AsSpan());
        
        var read = reader.Read();
        reader.Reset();
        var bytesConsumed = reader.BytesConsumed;

        await Assert.That(read).IsTrue();
        await Assert.That(bytesConsumed).IsEqualTo(0);
    }

    [Test]
    public async Task Read_EOF_ReturnsFalse()
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes("");
        var reader = new Utf8WktReader(bytes.AsSpan());
                
        await Assert.That(reader.Read()).IsFalse();
    }

    [Test]
    public async Task Read_WhitespaceOnly()
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes("   \t\n  ");
        var reader = new Utf8WktReader(bytes.AsSpan());
                
        await Assert.That(reader.Read()).IsFalse();
    }

    [Test]
    public async Task Read_StringValue()
    {
        const string Text = "\"WGS 84\"";
        var bytes = System.Text.Encoding.UTF8.GetBytes(Text);
        var reader = new Utf8WktReader(bytes.AsSpan());
        
        var stringValue = reader.Read()
            ? reader.GetString()
            : default;

        await Assert.That(reader.TokenType).IsEqualTo(WktTokenType.String);
        await Assert.That(stringValue).IsEqualTo("WGS 84");
    }

    [Test]
    public async Task Read_NumberValue()
    {
        const string Text = "6378137.0";
        var bytes = System.Text.Encoding.UTF8.GetBytes(Text);
        var reader = new Utf8WktReader(bytes.AsSpan());
        
        var doubleValue = reader.Read()
            ? reader.GetDouble()
            : default;
        
        await Assert.That(reader.TokenType).IsEqualTo(WktTokenType.Number);
        await Assert.That(doubleValue).IsEqualTo(6378137.0);
    }

    [Test]
    public async Task Read_LiteralValue()
    {
        const string Text = "NORTH";
        var bytes = System.Text.Encoding.UTF8.GetBytes(Text);
        var reader = new Utf8WktReader(bytes.AsSpan());
        
        var literalString = reader.Read()
            ? reader.GetLiteral().ToString()
            : default;
        
        await Assert.That(reader.TokenType).IsEqualTo(WktTokenType.Literal);
        await Assert.That(literalString).IsEqualTo("NORTH");
    }

    [Test]
    public async Task Read_KeywordValue()
    {
        const string Text = "GEOGCS[]";
        var bytes = System.Text.Encoding.UTF8.GetBytes(Text);
        var reader = new Utf8WktReader(bytes.AsSpan());
        
        var keywordString = reader.Read()
            ? reader.GetLiteral().ToString()
            : default;
        
        await Assert.That(reader.TokenType).IsEqualTo(WktTokenType.Keyword);
        await Assert.That(keywordString).IsEqualTo("GEOGCS");
    }

    [Test]
    public async Task Read_StartObject()
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes("[");
        var reader = new Utf8WktReader(bytes.AsSpan());
        
        reader.Read();
        
        await Assert.That(reader.TokenType).IsEqualTo(WktTokenType.StartObject);
    }

    [Test]
    public async Task Read_EndObject()
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes("]");
        var reader = new Utf8WktReader(bytes.AsSpan());
        
        reader.Read();
        
        await Assert.That(reader.TokenType).IsEqualTo(WktTokenType.EndObject);
    }

    [Test]
    public async Task Read_SeparatorSkips()
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(",\"Test\"");
        var reader = new Utf8WktReader(bytes.AsSpan());
        
        var stringValue = reader.Read()
            ? reader.GetString()
            : default;
        
        await Assert.That(reader.TokenType).IsEqualTo(WktTokenType.String);
        await Assert.That(stringValue).IsEqualTo("Test");
    }

    [Test]
    public async Task Read_NegativeNumber()
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes("-6378137.0");
        var reader = new Utf8WktReader(bytes.AsSpan());
        
        var doubleValue = reader.Read()
            ? reader.GetDouble()
            : default;
        
        await Assert.That(reader.TokenType).IsEqualTo(WktTokenType.Number);
        await Assert.That(doubleValue).IsEqualTo(-6378137.0);
    }

    [Test]
    public async Task Read_PositiveSignNumber()
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes("+6378137.0");
        var reader = new Utf8WktReader(bytes.AsSpan());

        var doubleValue = reader.Read()
            ? reader.GetDouble()
            : default;

        await Assert.That(reader.TokenType).IsEqualTo(WktTokenType.Number);
        await Assert.That(doubleValue).IsEqualTo(6378137.0);
    }

    [Test]
    public async Task Read_IntegerNumber()
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes("6378137");
        var reader = new Utf8WktReader(bytes.AsSpan());

        var doubleValue = reader.Read()
            ? reader.GetDouble()
            : default;

        await Assert.That(reader.TokenType).IsEqualTo(WktTokenType.Number);
        await Assert.That(doubleValue).IsEqualTo(6378137.0);
    }

    [Test]
    public async Task TryGetString_Success()
    {
        const string Text = "\"WGS 84\"";
        var bytes = System.Text.Encoding.UTF8.GetBytes(Text);
        var reader = new Utf8WktReader(bytes.AsSpan());
        
        reader.Read();
        var result = reader.TryGetString(out var value);
        
        await Assert.That(result).IsTrue();
        await Assert.That(value).IsEqualTo("WGS 84");
    }

    [Test]
    public async Task TryGetString_Failure()
    {
        const string Text = "6378137.0";
        var bytes = System.Text.Encoding.UTF8.GetBytes(Text);
        var reader = new Utf8WktReader(bytes.AsSpan());
        
        reader.Read();
        var result = reader.TryGetString(out var value);
        
        await Assert.That(result).IsFalse();
        await Assert.That(value).IsDefault();
    }

    [Test]
    public async Task TryGetDouble_Success()
    {
        const string Text = "6378137.0";
        var bytes = System.Text.Encoding.UTF8.GetBytes(Text);
        var reader = new Utf8WktReader(bytes.AsSpan());
        
        reader.Read();
        var result = reader.TryGetDouble(out var value);
        
        await Assert.That(result).IsTrue();
        await Assert.That(value).IsEqualTo(6378137.0);
    }

    [Test]
    public async Task TryGetDouble_Failure()
    {
        const string Text = "\"WGS 84\"";
        var bytes = System.Text.Encoding.UTF8.GetBytes(Text);
        var reader = new Utf8WktReader(bytes.AsSpan());
        
        reader.Read();
        var result = reader.TryGetDouble(out var value);
        
        await Assert.That(result).IsFalse();
        await Assert.That(value).IsDefault();
    }

    [Test]
    public async Task TryGetLiteral_Success()
    {
        const string Text = "NORTH";
        var bytes = System.Text.Encoding.UTF8.GetBytes(Text);
        var reader = new Utf8WktReader(bytes.AsSpan());
        
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
        const string Text = "\"WGS 84\"";
        var bytes = System.Text.Encoding.UTF8.GetBytes(Text);
        var reader = new Utf8WktReader(bytes.AsSpan());
        
        reader.Read();
        var result = reader.TryGetLiteral(out _);
        
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task GetString_Throws_ForNonString()
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes("6378137.0");


        await Assert.That(() =>
        {
            var reader = new Utf8WktReader(bytes.AsSpan());
            reader.Read();
            return reader.GetString();
        }).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task GetDouble_Throws_ForNonNumber()
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes("\"WGS 84\"");
                
        await Assert.That(() =>
        {
            var reader = new Utf8WktReader(bytes.AsSpan());

            reader.Read();
            return reader.GetDouble();
        }).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task GetDouble_Throws_ForInvalidFormat()
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes("invalid");
                
        await Assert.That(() =>
        {
            var reader = new Utf8WktReader(bytes.AsSpan());

            reader.Read();
            return reader.GetDouble();
        }).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task GetLiteral_Throws_ForNonLiteral()
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes("\"WGS 84\"");
       
        await Assert.That(() =>
        {
            var reader = new Utf8WktReader(bytes.AsSpan());

            reader.Read();
            _ = reader.GetLiteral();
        }).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task MultipleReads_ConsumesAll()
    {
        const string Text = "NORTH,\"WGS 84\",6378137.0";
        var bytes = System.Text.Encoding.UTF8.GetBytes(Text);
        var reader = new Utf8WktReader(bytes.AsSpan());
        
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
        var bytes = System.Text.Encoding.UTF8.GetBytes("[]");
        var reader = new Utf8WktReader(bytes.AsSpan());
        
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
        var bytes = System.Text.Encoding.UTF8.GetBytes("[[1,2],[3,4]]");
        var reader = new Utf8WktReader(bytes.AsSpan());
        
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
        var bytes = System.Text.Encoding.UTF8.GetBytes("[ 1 , 2 , 3 ]");
        var reader = new Utf8WktReader(bytes.AsSpan());
        
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
