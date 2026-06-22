namespace Altemiq.Data.GeoPackage;

public class GeoPackageConnectionTests
{
    [Test]
    public async Task TestApplicationId()
    {
        var connection = CreateConnection();
#if NETCOREAPP3_0_OR_GREATER
        await
#endif
        using (connection)
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "PRAGMA application_id;";
            var applicationId = command.ExecuteScalar();

            await Assert.That(applicationId).IsTypeOf<long>().And.IsEqualTo(0x47504B47);

            command.CommandText = "PRAGMA user_version;";
            var userVersion = command.ExecuteScalar();

            await Assert.That(userVersion).IsTypeOf<long>().And.IsGreaterThanOrEqualTo(10200);
        }
    }

    [Test]
    public async Task GetContentsSchema()
    {
        var connection = CreateConnection();
#if NETCOREAPP3_0_OR_GREATER
        await
#endif
        using (connection)
        {
            connection.Open();

            // get the schema
            var schema = connection.GetSchema(GeoPackageMetadataCollectionNames.Contents);

            await Assert.That(schema.Rows.Count).IsDefault();
        }
    }

    [Test]
    public async Task GetSysRefSchema()
    {
        var connection = CreateConnection();
#if NETCOREAPP3_0_OR_GREATER
        await
#endif
        using (connection)
        {
            connection.Open();

            // get the schema
            var schema = connection.GetSchema(GeoPackageMetadataCollectionNames.SpatialReferenceSystems);

            await Assert.That(schema.Rows.Count).IsEqualTo(3);
        }
    }

    [Test]
    public async Task GetColumnsSchema()
    {
        var connection = CreateConnection();
#if NETCOREAPP3_0_OR_GREATER
        await
#endif
        using (connection)
        {
            connection.Open();

            // get the schema
            var schema = connection.GetSchema(GeoPackageMetadataCollectionNames.GeometryColumns);

            await Assert.That(schema.Rows.Count).IsDefault();
        }
    }

    private static GeoPackageConnection CreateConnection()
    {
        var connectionStringBuilder = new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder { DataSource = ":memory:" };

        return new(connectionStringBuilder.ConnectionString);
    }

    private static void CreateFeatureTable(System.Data.IDbConnection connection)
    {
        // add a new table
        using var command = connection.CreateCommand();
        command.CommandText = """
                              CREATE TABLE sample_feature_table (
                                id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                geometry GEOMETRY,
                                text_attribute TEXT,
                                real_attribute REAL,
                                boolean_attribute BOOLEAN,
                                raster_or_photo BLOB
                              );
                              """;

        _ = command.ExecuteNonQuery();
    }

    private static void CreateTilePyramid(System.Data.IDbConnection connection)
    {
        // add a new table
        using var command = connection.CreateCommand();
        command.CommandText = """
                              CREATE TABLE sample_tile_pyramid (
                                id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                zoom_level INTEGER NOT NULL,
                                tile_column INTEGER NOT NULL,
                                tile_row INTEGER NOT NULL,
                                tile_data BLOB NOT NULL,
                                UNIQUE (zoom_level, tile_column, tile_row)
                              )
                              """;

        _ = command.ExecuteNonQuery();
    }
}