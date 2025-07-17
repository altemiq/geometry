namespace Altemiq.IO.Dbf;

public class DBaseReaderTests
{
    [Test]
    public async Task DBase()
    {
        using var reader = GetReader("dbase_83.dbf");
        var record = await Read(reader);

        for (var i = 0; i < record.FieldCount; i++)
        {
            if (!record.IsDBNull(i))
            {
                _ = await Assert.That(record.GetValue(i)).IsNotNull().And.IsNotEqualTo(DBNull.Value);
            }
        }

        static DBaseReader GetReader(string name)
        {
            var dbfStream = typeof(DbfReaderTests).Assembly.GetManifestResourceStream(typeof(DbfReaderTests), "Data." + name) ?? throw new InvalidOperationException();
            var dbtStream = typeof(DbfReaderTests).Assembly.GetManifestResourceStream(typeof(DbfReaderTests), "Data." + Path.ChangeExtension(name, ".dbt")) ?? throw new InvalidOperationException();
            return new(dbfStream, dbtStream);
        }

        static async Task<DBaseReader> Read(DBaseReader reader)
        {
            _ = await Assert.That(reader.Read()).IsTrue();
            return reader;
        }
    }
}