// -----------------------------------------------------------------------
// <copyright file="SqliteConnectionTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data.Sqlite;

public class SqliteConnectionTests
{
    [Test]
    public async Task GetTableSchema()
    {
        await using var connection = CreateConnection();
        connection.Open();

        // get the table schema
        var schemaBefore = connection.GetSchema(SqliteMetadataCollectionNames.Tables);

        CreateTable(connection);

        var schemaAfter = connection.GetSchema(SqliteMetadataCollectionNames.Tables);

        _ = await Assert.That(schemaAfter.Rows.Count).IsEqualTo(schemaBefore.Rows.Count + 1);
        _ = await Assert
            .That(Cast<System.Data.DataColumn>(schemaBefore.Columns).Select(x => x.ColumnName))
            .IsEquivalentTo(Cast<System.Data.DataColumn>(schemaAfter.Columns).Select(x => x.ColumnName));

        connection.Close();

        static IEnumerable<T> Cast<T>(System.Collections.IEnumerable enumerable)
        {
            return enumerable.Cast<T>();
        }
    }

    [Test]
    [Arguments(1, new[] { null, null, "alias_name" })]
    [Arguments(1, new[] { null, null, "alias_name", "table" })]
    [Arguments(1, new[] { null, null, null, "table" })]
    [Arguments(0, new[] { null, null, "alias" })]
    [Arguments(0, new[] { null, null, "alias", "table" })]
    [Arguments(0, new[] { null, null, "alias_name", "system" })]
    [Arguments(0, new[] { null, null, "alias", "system" })]
    public async Task GetFilteredTableSchema(int count, string[] restrictionValues)
    {
        await using var connection = CreateConnection();
        connection.Open();

        CreateTable(connection);

        var schema = connection.GetSchema(SqliteMetadataCollectionNames.Tables, restrictionValues);

        _ = await Assert.That(schema.Rows.Count).IsEqualTo(count);

        connection.Close();
    }

    private static SqliteConnection CreateConnection()
    {
        var connectionStringBuilder = new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder
        {
            DataSource = ":memory:"
        };

        return new(connectionStringBuilder.ConnectionString);
    }

    private static void CreateTable(System.Data.IDbConnection connection)
    {
        // add a new table
        using var command = connection.CreateCommand();
        command.CommandText = """
            CREATE TABLE alias_name (
                table_name TEXT            NOT NULL
                                            CHECK (table_name IN ('unit_of_measure', 'celestial_body', 'ellipsoid', 'extent', 'prime_meridian', 'geodetic_datum', 'vertical_datum', 'geodetic_crs', 'projected_crs', 'vertical_crs', 'compound_crs', 'conversion', 'grid_transformation', 'helmert_transformation', 'other_transformation', 'concatenated_operation') ),
                auth_name  TEXT            NOT NULL
                                            CHECK (length(auth_name) >= 1),
                code       INTEGER_OR_TEXT NOT NULL
                                            CHECK (length(code) >= 1),
                alt_name   TEXT            NOT NULL
                                            CHECK (length(alt_name) >= 2),
                source     TEXT
            );
            """;

        _ = command.ExecuteNonQuery();
    }
}