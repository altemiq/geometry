namespace Altemiq.Data.Dbf;

public class DbfColumnTests
{
    [Test]
    public async Task StringColumn()
    {
        await Assert.That(DbfColumn.String("string-col", 10))
            .Satisfies(column => column.DbfType, dbfType => dbfType.IsEqualTo(DbfColumn.DbfColumnType.Character)).And
            .Satisfies(column => column.ColumnSize, columnSize => columnSize.IsEqualTo(10)).And
            .Satisfies(column => column.NumericPrecision, numericPrecision => numericPrecision.IsNull());
    }

    [Test]
    public async Task NumberColumn()
    {
        await Assert.That(DbfColumn.Number("number-col", 16, 10))
            .Satisfies(column => column.DbfType, dbfType => dbfType.IsEqualTo(DbfColumn.DbfColumnType.Number)).And
            .Satisfies(column => column.ColumnSize, columnSize => columnSize.IsEqualTo(16)).And
            .Satisfies(column => column.NumericPrecision, numericPrecision => numericPrecision.IsEqualTo(10));
    }

    [Test]
    public async Task BooleanColumn()
    {
        await Assert.That(DbfColumn.Boolean("boolean-col"))
            .Satisfies(column => column.DbfType, dbfType => dbfType.IsEqualTo(DbfColumn.DbfColumnType.Boolean)).And
            .Satisfies(column => column.ColumnSize, columnSize => columnSize.IsEqualTo(1)).And
            .Satisfies(column => column.NumericPrecision, numericPrecision => numericPrecision.IsNull());
    }

    [Test]
    public async Task FloatColumn()
    {
        await Assert.That(DbfColumn.Float("float-col", 16, 10))
            .Satisfies(column => column.DbfType, dbfType => dbfType.IsEqualTo(DbfColumn.DbfColumnType.Float)).And
            .Satisfies(column => column.ColumnSize, columnSize => columnSize.IsEqualTo(16)).And
            .Satisfies(column => column.NumericPrecision, numericPrecision => numericPrecision.IsEqualTo(10));
    }
}