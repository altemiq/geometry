namespace Altemiq.Data.GeoPackage;

public class GeoPackageConnectionTests
{
    [Test]
    public async Task TestApplicationId()
    {
        await using var connection = CreateConnection();
        connection.Open();
        
        var command = connection.CreateCommand();
        command.CommandText = "PRAGMA application_id;";
        var applicationId = command.ExecuteScalar();

        await Assert.That(applicationId).IsTypeOf<long>().And.IsEqualTo(0x47504B47);
        
        command.CommandText = "PRAGMA user_version;";
        var userVersion = command.ExecuteScalar();
        
        await Assert.That(userVersion).IsTypeOf<long>().And.IsGreaterThanOrEqualTo(10200);
    }
    
    [Test]
    public async Task GetContentsSchema()
    {
        await using var connection = CreateConnection();
        connection.Open();

        // get the schema
        var schema = connection.GetSchema(GeoPackageMetadataCollectionNames.Contents);

        await Assert.That(schema.Rows.Count).IsDefault();

        connection.Close();
    }
    
    [Test]
    public async Task GetSysRefSchema()
    {
        await using var connection = CreateConnection();
        connection.Open();

        // get the schema
        var schema = connection.GetSchema(GeoPackageMetadataCollectionNames.SpatialReferenceSystems);

        await Assert.That(schema.Rows.Count).IsEqualTo(3);

        connection.Close();
    }
    
    [Test]
    public async Task GetColumnsSchema()
    {
        await using var connection = CreateConnection();
        connection.Open();

        // get the schema
        var schema = connection.GetSchema(GeoPackageMetadataCollectionNames.GeometryColumns);

        await Assert.That(schema.Rows.Count).IsDefault();

        connection.Close();
    }
    
    private static GeoPackageConnection CreateConnection()
    {
        var connectionStringBuilder = new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder { DataSource = ":memory:" };

        return new(connectionStringBuilder.ConnectionString);
    }
}