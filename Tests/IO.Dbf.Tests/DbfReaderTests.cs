namespace Altemiq.IO.Dbf;

public class DbfReaderTests
{
    [Test]
    public async Task MultiPoint()
    {
        _ = await Assert.That(GetReader("multipnt.dbf"))
            .Satisfies(reader => reader.Header.FieldCount, fieldCount => fieldCount.IsEqualTo(29))
            .Satisfies(reader => reader.Header.RecordCount, recordCount => recordCount.IsEqualTo(1U))
            .Satisfies(reader => reader.Read(), read => read.IsTrue())
            .Satisfies(reader => reader.GetDouble(0), d => d.IsEqualTo(7280457.000))
            .Satisfies(reader => reader.GetString(4).Trim(), s => s.IsEqualTo("35044124")!)
            .Satisfies(reader => reader.IsDBNull(5), d => d.IsTrue())
            .Satisfies(reader => reader.GetInt32(6), i => i.IsEqualTo(0));
    }

    [Test]
    public async Task DBase()
    {
        _ = await Assert.That(GetReader("dbase_03.dbf"))
            .Satisfies(GetValues, values => values.IsNotNull().All().Satisfy(v => v.IsNotNull().And.IsNotEqualTo(DBNull.Value))!);
    }

    [Test]
    public async Task XBase()
    {
        _ = await Assert.That(GetReader("test.dbf"))
            .Satisfies(GetValues, values => values.IsNotNull().All().Satisfy(v => v.IsNotNull().And.IsNotEqualTo(DBNull.Value))!);
    }

    private static DbfReader GetReader(string name) => new(typeof(DbfReaderTests).Assembly.GetManifestResourceStream(typeof(DbfReaderTests), "Data." + name) ?? throw new InvalidOperationException());

    private static IEnumerable<object> GetValues(DbfReader reader)
    {
        var values = new object[reader.FieldCount];
        for (var i = 0; i < reader.FieldCount; i++)
        {
            values[i] = reader.GetValue(i);
        }

        return values;
    }
}