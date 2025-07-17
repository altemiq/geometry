namespace Altemiq.IO.Dbf;

public class NdxReaderTests
{
    [Test]
    public async Task Read()
    {
        var reader = GetReader("test.ndx");

        _ = await Assert.That(reader.StartingPageNumber).IsEqualTo(1);
        _ = await Assert.That(reader.TotalNoOfPages).IsEqualTo(2);
        _ = await Assert.That(reader.KeyLength).IsEqualTo((short)8);


        var page = reader.Read();
        _ = await Assert.That(page).IsNotNull();
        _ = await Assert.That(page!.Count).IsEqualTo(3);

        _ = await Assert.That(page[0].ReadInt32()).IsEqualTo(1);
        _ = await Assert.That(page[1].ReadInt32()).IsEqualTo(2);
        _ = await Assert.That(page[2].ReadInt32()).IsEqualTo(3);
    }

    private static NdxReader GetReader(string name) => new(typeof(NdxReaderTests).Assembly.GetManifestResourceStream(typeof(DbfReaderTests), "Data." + name) ?? throw new InvalidOperationException());
}