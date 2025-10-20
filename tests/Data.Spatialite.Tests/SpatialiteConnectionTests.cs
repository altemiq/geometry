// -----------------------------------------------------------------------
// <copyright file="SpatialiteConnectionTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data.Spatialite;

[WindowsOnlyTest]
public class SpatialiteConnectionTests
{
    [Test]
    [Arguments(InitSpatialMetadataMode.None, default, 0)]
    [Arguments(InitSpatialMetadataMode.Wgs84, 3, 150)]
    [Arguments(InitSpatialMetadataMode.All, 6000, default)]
    public async Task InitialiseSpatialMetadata(InitSpatialMetadataMode mode, int? minimum, int? maximum)
    {
        var builder = new SpatialiteConnectionStringBuilder
        {
            DataSource = ":memory:",
            InitSpatialMetadata = mode,
        };

        var connection = new SpatialiteConnection(builder.ConnectionString);

        connection.Open();

        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(srid) FROM spatial_ref_sys;";

        _ = await Assert.That(command.ExecuteScalar())
            .IsTypeOf<int>().And
            .IsGreaterThanOrEqualTo(minimum.GetValueOrDefault()).And
            .IsLessThanOrEqualTo(maximum.GetValueOrDefault(int.MaxValue));
    }

    [Test]
    [Arguments(InitSpatialMetadataMode.None, default, 0)]
    [Arguments(InitSpatialMetadataMode.Wgs84, 3, 150)]
    [Arguments(InitSpatialMetadataMode.All, 6000, default)]
    public async Task InitialiseSpatialMetadataAsync(InitSpatialMetadataMode mode, int? minimum, int? maximum)
    {
        var builder = new SpatialiteConnectionStringBuilder
        {
            DataSource = ":memory:",
            InitSpatialMetadata = mode,
        };

        var connection = new SpatialiteConnection(builder.ConnectionString);

        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(srid) FROM spatial_ref_sys;";
        _ = await Assert.That(command.ExecuteScalar())
            .IsTypeOf<int>().And
            .IsGreaterThanOrEqualTo(minimum.GetValueOrDefault()).And
            .IsLessThanOrEqualTo(maximum.GetValueOrDefault(int.MaxValue));
    }

    [Test]
    public async Task RecoverGeometryColumn()
    {
        const string Table = "Tbl";
        const string Column = "PT";
        const string Srid = "4326";

        var builder = new SpatialiteConnectionStringBuilder
        {
            DataSource = ":memory:",
            InitSpatialMetadata = InitSpatialMetadataMode.Wgs84,
        };

        var connection = new SpatialiteConnection(builder.ConnectionString);

        await connection.OpenAsync();

        await using var command = connection.CreateCommand();

        command.CommandText = $"CREATE TABLE {Table}(ID INT, {Column} POINT);";
        await command.ExecuteNonQueryAsync();

        command.CommandText = $"SELECT RecoverGeometryColumn('{Table}', '{Column}', {Srid}, 'POINT', 'XY');";

        await command.ExecuteNonQueryAsync();

        command.CommandText = $"SELECT COUNT(*) FROM geometry_columns WHERE f_table_name == '{Table}' COLLATE NOCASE AND f_geometry_column == '{Column}' COLLATE NOCASE AND SRID = {Srid};";

        _ = await Assert.That(await command.ExecuteScalarAsync()).IsTypeOf<int>().IsEqualTo(1);
    }
}