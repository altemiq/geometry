namespace Altemiq.IO.Dbf;

public class DbfVersionTests
{
    [Test]
    public async Task DBaseWithoutMemo() =>
        await Assert.That(DbfVersion.DBase4WithoutMemo)
            .Satisfies(static x => x.Memo, static memo => memo.IsFalse()).And
            .Satisfies(static x => x.Version, static version => version.IsEqualTo((byte)4));
}