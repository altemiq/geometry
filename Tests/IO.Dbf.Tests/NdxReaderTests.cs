namespace Altemiq.IO.Dbf;

public class NdxReaderTests
{
    [Test]
    public async Task Read()
    {
        await Assert.That(GetReader("test.ndx"))
            .Satisfies(reader => reader.StartingPageNumber, startingPageNumber => startingPageNumber.IsEqualTo(1)).And
            .Satisfies(reader => reader.TotalNoOfPages, totalNoOfPages => totalNoOfPages.IsEqualTo(2))
            .Satisfies(reader => reader.KeyLength, keyLength => keyLength.IsEqualTo((short)8))
            .Satisfies(
                reader => reader.Read(),
                page =>
                    page
                        .IsNotNull()
                        .Satisfies(p => p.Count, count => count.IsEqualTo(3))
                        .Satisfies(p => p[0].ReadInt32(), i => i.IsEqualTo(1))
                        .Satisfies(p => p[1].ReadInt32(), i => i.IsEqualTo(2))
                        .Satisfies(p => p[2].ReadInt32(), i => i.IsEqualTo(3))!);
    }

    private static NdxReader GetReader(string name) => new(typeof(NdxReaderTests).Assembly.GetManifestResourceStream(typeof(DbfReaderTests), "Data." + name) ?? throw new InvalidOperationException());
}