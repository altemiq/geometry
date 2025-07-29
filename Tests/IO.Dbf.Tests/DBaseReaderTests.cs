namespace Altemiq.IO.Dbf;

public class DBaseReaderTests
{
    [Test]
    public async Task DBase()
    {
        await using var reader = GetReader("dbase_83.dbf");
        _ = await Assert.That(reader.Read()).IsTrue();

        for (var i = 0; i < reader.FieldCount; i++)
        {
            if (!reader.IsDBNull(i))
            {
                _ = await Assert.That(reader.GetValue(i)).IsNotNull().And.IsNotEqualTo(DBNull.Value);
            }
        }

        static DBaseReader GetReader(string name)
        {
            var dbfStream = typeof(DbfReaderTests).Assembly.GetManifestResourceStream(typeof(DbfReaderTests), "Data." + name) ?? throw new InvalidOperationException();
            var dbtStream = typeof(DbfReaderTests).Assembly.GetManifestResourceStream(typeof(DbfReaderTests), "Data." + Path.ChangeExtension(name, ".dbt")) ?? throw new InvalidOperationException();
            return new(dbfStream, dbtStream);
        }
    }
}