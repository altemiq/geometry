namespace Altemiq.IO.Dbf;

public class DbfColumnTests
{
    [Test]
    public async Task StringColumn()
    {
        var column = DbfColumn.String("string-col", 10);
        _ = await Assert.That(column.DbfType).IsEqualTo(DbfColumn.DbfColumnType.Character);
        _ = await Assert.That(column.ColumnSize).IsEqualTo(10);
        _ = await Assert.That(column.NumericPrecision).IsNull();
    }

    [Test]
    public async Task NumberColumn()
    {
        var column = DbfColumn.Number("number-col", 16, 10);
        _ = await Assert.That(column.DbfType).IsEqualTo(DbfColumn.DbfColumnType.Number);
        _ = await Assert.That(column.ColumnSize).IsEqualTo(16);
        _ = await Assert.That(column.NumericPrecision).IsEqualTo(10);
    }

    [Test]
    public async Task BooleanColumn()
    {
        var column = DbfColumn.Boolean("boolean-col");
        _ = await Assert.That(column.DbfType).IsEqualTo(DbfColumn.DbfColumnType.Boolean);
        _ = await Assert.That(column.ColumnSize).IsEqualTo(1);
        _ = await Assert.That(column.NumericPrecision).IsNull();
    }

    [Test]
    public async Task FloatColumn()
    {
        var column = DbfColumn.Float("float-col", 16, 10);
        _ = await Assert.That(column.DbfType).IsEqualTo(DbfColumn.DbfColumnType.Float);
        _ = await Assert.That(column.ColumnSize).IsEqualTo(16);
        _ = await Assert.That(column.NumericPrecision).IsEqualTo(10);
    }
}