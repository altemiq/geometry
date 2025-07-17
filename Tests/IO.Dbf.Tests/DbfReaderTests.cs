namespace Altemiq.IO.Dbf;

public class DbfReaderTests
{
    [Test]
    public async Task MultiPoint()
    {
        var reader = GetReader("multipnt.dbf");
        _ = await Assert.That(reader.Header.FieldCount).IsEqualTo(29);
        _ = await Assert.That(reader.Header.RecordCount).IsEqualTo(1U);

        _ = await Assert.That(reader.Read()).IsTrue();

        _ = await Assert.That(reader.GetDouble(0)).IsEqualTo(7280457.000);
        _ = await Assert.That(reader.GetString(4).Trim()).IsEqualTo("35044124");
        _ = await Assert.That(reader.IsDBNull(5)).IsTrue();
        _ = await Assert.That(reader.GetInt32(6)).IsEqualTo(0);
    }

    [Test]
    public async Task DBase()
    {
        using var reader = GetReader("dbase_03.dbf");
        var record = await Read(reader);

        for (var i = 0; i < record.FieldCount; i++)
        {
            if (!record.IsDBNull(i))
            {
                _ = await Assert.That(record.GetValue(i)).IsNotNull().And.IsNotEqualTo(DBNull.Value);
            }
        }
    }

    [Test]
    public async Task XBase()
    {
        using var reader = GetReader("test.dbf");
        while (reader.Read())
        {
            var record = reader;

            for (var i = 0; i < record.FieldCount; i++)
            {
                if (!record.IsDBNull(i))
                {
                    await Assert.That(record.GetValue(i)).IsNotNull().And.IsNotEqualTo(DBNull.Value);
                }
            }
        }
    }

    private static DbfReader GetReader(string name) => new(typeof(DbfReaderTests).Assembly.GetManifestResourceStream(typeof(DbfReaderTests), "Data." + name) ?? throw new InvalidOperationException());

    private static async Task<DbfReader> Read(DbfReader reader)
    {
        _ = await Assert.That(reader.Read()).IsTrue();
        return reader;
    }
}