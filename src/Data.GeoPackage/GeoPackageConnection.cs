// -----------------------------------------------------------------------
// <copyright file="GeoPackageConnection.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data.GeoPackage;

/// <summary>
/// The <c>GeoPackage</c> connection.
/// </summary>
public class GeoPackageConnection : Sqlite.SqliteConnection
{
    private const string SrsId = "SRS_ID";

    private const string GeometryDataType = $"GEOMETRY_{DataType}";
    private const long ApplicationId = 0x47504B47;

    private static readonly string[] Dimensions = [string.Empty, "z", "m", "zm"];

    private static readonly string[] SpatialDataTypes = ["point", "linestring", "polygon", "multipoint", "multilinestring", "multipolygon", "geometrycollection", "geometry"];

    /// <summary>
    /// Initialises a new instance of the <see cref="GeoPackageConnection"/> class.
    /// </summary>
    /// <param name="connectionString">The string used to open the connection.</param>
    public GeoPackageConnection(string connectionString)
        : base(connectionString)
    {
    }

    /// <summary>
    /// Gets the GeoPackage version.
    /// </summary>
    public Version? GeoPackageVersion
    {
        get
        {
            if (this.State is not System.Data.ConnectionState.Open)
            {
                return default;
            }

            using var command = this.CreateCommand();
            command.CommandText = "PRAGMA user_version;";
            return command.ExecuteScalar() is long userVersion ? LongToVersion(userVersion) : default;
        }
    }

    /// <inheritdoc/>
    public override void Open()
    {
        if (this.State is System.Data.ConnectionState.Open)
        {
            return;
        }

        base.Open();

        var command = this.CreateCommand();
        command.CommandText = "PRAGMA application_id;";
        switch (command.ExecuteScalar())
        {
            case null or 0L:
                // set this up as a GPKG
                InitialiseGeoPackage(connection: this, 10400);
                CreateFunctions(connection: this);
                return;
            case ApplicationId:
                CreateFunctions(connection: this);
                return;
            default:
                throw new InvalidOperationException();
        }
    }

    /// <inheritdoc/>
    public override async Task OpenAsync(CancellationToken cancellationToken)
    {
        if (this.State is System.Data.ConnectionState.Open)
        {
            return;
        }

        await OpenBaseAsync(cancellationToken).ConfigureAwait(false);

        var command = this.CreateCommand();
        command.CommandText = "PRAGMA application_id;";
        switch (await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false))
        {
            case null or 0L:
                // set this up as a GPKG
                InitialiseGeoPackage(connection: this, 10400);
                CreateFunctions(connection: this);
                return;
            case ApplicationId:
                CreateFunctions(connection: this);
                return;
            default:
                throw new InvalidOperationException();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0042:Do not use blocking calls in an async method", Justification = "`base.OpenAsync` proxies through to `this.Open`")]
        Task OpenBaseAsync(CancellationToken ct)
        {
            if (ct.IsCancellationRequested)
            {
                return Task.FromCanceled(ct);
            }

            try
            {
                // call Open on the base, as we do not want to call our own open
                base.Open();
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                return Task.FromException(e);
            }
        }
    }

    /// <summary>
    /// Creates a new command associated with the connection.
    /// </summary>
    /// <returns>The new command.</returns>
    /// <remarks>The command's <see cref="Microsoft.Data.Sqlite.SqliteCommand.Transaction"/> property will also be set to the current transaction.</remarks>
    public new GeoPackageCommand CreateCommand() => new()
    {
        Connection = this,
        CommandTimeout = this.DefaultTimeout,
        Transaction = this.Transaction,
    };

    /// <inheritdoc/>
    public override System.Data.DataTable GetSchema(string collectionName) => this.GetSchema(collectionName, []);

    /// <inheritdoc />
    public override System.Data.DataTable GetSchema(string collectionName, string?[] restrictionValues)
    {
        if (this.State is not System.Data.ConnectionState.Open)
        {
            throw new InvalidOperationException();
        }

        var parms = new string?[5];
        restrictionValues.CopyTo(parms, 0);

        return collectionName.ToUpper(System.Globalization.CultureInfo.InvariantCulture) switch
        {
            "CONTENTS" => this.GetContentsSchema(parms[0], parms[2]),
            "GEOMETRYCOLUMNS" => this.GetGeometryColumnsSchema(parms[0], parms[2], parms[3]),
            "SPATIALREFERENCESYSTEMS" => this.GetSpatialReferenceSystemsSchema(parms[0]),
            _ => base.GetSchema(collectionName, restrictionValues),
        };
    }

    /// <summary>
    /// Gets the content schema.
    /// </summary>
    /// <param name="catalog">The catalog (attached database) to query.</param>
    /// <param name="table">The table to retrieve index information for.</param>
    /// <returns>The table schema.</returns>
    protected virtual System.Data.DataTable GetContentsSchema(string? catalog, string? table)
    {
        if (string.IsNullOrEmpty(catalog))
        {
            catalog = GetDefaultCatalogName();
        }

        var dataTable = new System.Data.DataTable(GeoPackageMetadataCollectionNames.Contents)
        {
            Locale = System.Globalization.CultureInfo.InvariantCulture,
            Columns =
            {
                { TableCatalog, typeof(string) },
                { TableSchema, typeof(string) },
                { TableName, typeof(string) },
                { DataType, typeof(string) },
                { "IDENTIFIER", typeof(string) },
                { "DESCRIPTION", typeof(string) },
                { "LAST_CHANGE", typeof(DateTime) },
                { "MIN_X", typeof(double) },
                { "MIN_Y", typeof(double) },
                { "MAX_X", typeof(double) },
                { "MAX_Y", typeof(double) },
                { SrsId, typeof(int) },
            },
        };

        dataTable.BeginLoadData();

        using (var command = new Microsoft.Data.Sqlite.SqliteCommand($"SELECT [table_name], [data_type], [identifier], [description], [last_change], [min_x], [min_y], [max_x], [max_y], [srs_id] FROM [{catalog}].[gpkg_contents]", this))
        {
            using var dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                var tableName = dataReader.GetString(0);

                if (table?.Equals(tableName, StringComparison.OrdinalIgnoreCase) is false)
                {
                    continue;
                }

                var row = dataTable.NewRow();

                row[TableCatalog] = catalog;
                row[TableName] = tableName;
                row[DataType] = dataReader.GetString(1);
                row["IDENTIFIER"] = dataReader.GetString(2);
                row["DESCRIPTION"] = dataReader.GetString(3);
                row["LAST_CHANGE"] = dataReader.GetDateTime(4);
                row["MIN_X"] = dataReader.GetDouble(5);
                row["MIN_Y"] = dataReader.GetDouble(6);
                row["MAX_X"] = dataReader.GetDouble(7);
                row["MAX_Y"] = dataReader.GetDouble(8);
                row[SrsId] = dataReader.GetInt32(9);

                dataTable.Rows.Add(row);
            }
        }

        dataTable.AcceptChanges();
        dataTable.EndLoadData();

        return dataTable;
    }

    /// <summary>
    /// Gets the geometry columns schema.
    /// </summary>
    /// <param name="catalog">The catalog (attached database) to query.</param>
    /// <param name="table">The table to retrieve index information for.</param>
    /// <param name="column">The name of the column to retrieve information for.</param>
    /// <returns>The geometry columns schema.</returns>
    protected virtual System.Data.DataTable GetGeometryColumnsSchema(string? catalog, string? table, string? column)
    {
        if (string.IsNullOrEmpty(catalog))
        {
            catalog = GetDefaultCatalogName();
        }

        var dataTable = new System.Data.DataTable(GeoPackageMetadataCollectionNames.GeometryColumns)
        {
            Locale = System.Globalization.CultureInfo.InvariantCulture,
            Columns =
            {
                { TableCatalog, typeof(string) },
                { TableSchema, typeof(string) },
                { TableName, typeof(string) },
                { ColumnName, typeof(string) },
                { GeometryDataType, typeof(string) },
                { SrsId, typeof(int) },
                { "Z", typeof(byte) },
                { "M", typeof(byte) },
            },
        };

        dataTable.BeginLoadData();

        using (var command = new Microsoft.Data.Sqlite.SqliteCommand($"SELECT [table_name], [column_name], [geometry_type_name], [srs_id], [z], [m] FROM [{catalog}].[gpkg_geometry_columns]", this))
        {
            using var dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                var tableName = dataReader.GetString(0);
                var columnName = dataReader.GetString(1);

                if (table?.Equals(tableName, StringComparison.OrdinalIgnoreCase) is false
                    || column?.Equals(columnName, StringComparison.OrdinalIgnoreCase) is false)
                {
                    continue;
                }

                var row = dataTable.NewRow();

                row[TableCatalog] = catalog;
                row[TableName] = tableName;
                row[ColumnName] = columnName;
                row[GeometryDataType] = dataReader.GetString(2);
                row[SrsId] = dataReader.GetInt32(3);
                row["Z"] = dataReader.GetByte(4);
                row["M"] = dataReader.GetByte(5);

                dataTable.Rows.Add(row);
            }
        }

        dataTable.AcceptChanges();
        dataTable.EndLoadData();

        return dataTable;
    }

    /// <summary>
    /// Gets the spatial reference systems schema.
    /// </summary>
    /// <param name="catalog">The catalog (attached database) to query.</param>
    /// <returns>The spatial reference systems schema.</returns>
    protected virtual System.Data.DataTable GetSpatialReferenceSystemsSchema(string? catalog)
    {
        if (string.IsNullOrEmpty(catalog))
        {
            catalog = GetDefaultCatalogName();
        }

        var dataTable = new System.Data.DataTable(GeoPackageMetadataCollectionNames.SpatialReferenceSystems)
        {
            Locale = System.Globalization.CultureInfo.InvariantCulture,
            Columns =
            {
                { "SRS_NAME", typeof(string) },
                { SrsId, typeof(int) },
                { "OGANIZATION", typeof(string) },
                { "ORGANIZATION_COORDSYS_ID", typeof(int) },
                { "DEFINITION", typeof(string) },
                { "DESCRIPTION", typeof(string) },
            },
        };

        dataTable.BeginLoadData();

        using (var command = new Microsoft.Data.Sqlite.SqliteCommand($"SELECT [srs_name], [srs_id], [organization], [organization_coordsys_id], [definition], [description] FROM [{catalog}].[gpkg_spatial_ref_sys]", this))
        {
            using var dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                var row = dataTable.NewRow();

                row["SRS_NAME"] = dataReader.GetString(0);
                row[SrsId] = dataReader.GetInt32(1);
                row["OGANIZATION"] = dataReader.GetString(2);
                row["ORGANIZATION_COORDSYS_ID"] = dataReader.GetInt32(3);
                row["DEFINITION"] = dataReader.GetString(4);
                if (!dataReader.IsDBNull(5))
                {
                    row["DESCRIPTION"] = dataReader.GetString(5);
                }

                dataTable.Rows.Add(row);
            }
        }

        dataTable.AcceptChanges();
        dataTable.EndLoadData();

        return dataTable;
    }

    /// <inheritdoc/>
    protected override System.Data.DataTable GetDataTypesSchema()
    {
        var dataTable = base.GetDataTypesSchema();

        dataTable.BeginLoadData();

        // Add the spatial types
        foreach (var typeName in SpatialDataTypes.SelectMany(spatialDataType => Dimensions.Select(dimension => spatialDataType + dimension)))
        {
            var row = dataTable.NewRow();
            row[System.Data.Common.DbMetaDataColumnNames.TypeName] = typeName;
            row[System.Data.Common.DbMetaDataColumnNames.ProviderDbType] = System.Data.DbType.Binary;
            row[System.Data.Common.DbMetaDataColumnNames.DataType] = typeof(byte[]).ToString();
            row[System.Data.Common.DbMetaDataColumnNames.ColumnSize] = int.MaxValue;
            row[System.Data.Common.DbMetaDataColumnNames.IsNullable] = true;
            dataTable.Rows.Add(row);
        }

        dataTable.AcceptChanges();
        dataTable.EndLoadData();

        return dataTable;
    }

    /// <inheritdoc />
    protected override System.Data.DataTable GetMetadataCollectionsSchema()
    {
        var dataTable = base.GetMetadataCollectionsSchema();

        dataTable.BeginLoadData();

        foreach (var (collectionName, numberOfRestrictions, numberOfIdentifierParts) in GetMetadataCollections())
        {
            var row = dataTable.NewRow();
            row[System.Data.Common.DbMetaDataColumnNames.CollectionName] = collectionName;
            row[System.Data.Common.DbMetaDataColumnNames.NumberOfRestrictions] = numberOfRestrictions;
            row[System.Data.Common.DbMetaDataColumnNames.NumberOfIdentifierParts] = numberOfIdentifierParts;
            dataTable.Rows.Add(row);
        }

        dataTable.AcceptChanges();
        dataTable.EndLoadData();

        return dataTable;

        static IEnumerable<(string CollectionName, int NumberOfRestrictions, int NumberOfIdentifierParts)> GetMetadataCollections()
        {
            yield return (GeoPackageMetadataCollectionNames.Contents, 4, 3);
            yield return (GeoPackageMetadataCollectionNames.GeometryColumns, 4, 4);
            yield return (GeoPackageMetadataCollectionNames.SpatialReferenceSystems, 4, 2);
        }
    }

    private static Version LongToVersion(long version)
    {
        // get the encoded version
        var major = version / 10000;
        var minor = (version - (major * 10000)) / 100;
        var build = version - (major * 10000) - (minor * 100);

        return new((int)major, (int)minor, (int)build);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:Parameter should not span multiple lines", Justification = "Checked")]
    private static void InitialiseGeoPackage(Microsoft.Data.Sqlite.SqliteConnection connection, long version)
    {
        using var createCommand = connection.CreateCommand();
        ExecuteNonQuery(createCommand, $"PRAGMA application_id = {ApplicationId};");
#if NET6_0_OR_GREATER
        ExecuteNonQuery(createCommand, string.Create(System.Globalization.CultureInfo.InvariantCulture, $"PRAGMA user_version = {version};"));
#else
        ExecuteNonQuery(createCommand, FormattableString.Invariant($"PRAGMA user_version = {version};"));
#endif

        // ensure the tables exist
        if (version >= 10500)
        {
            throw new InvalidOperationException();
        }

        using var existsCommand = CreateExistsCommand(connection);

        CreateIfNotExists(
            createCommand,
            existsCommand,
            "gpkg_spatial_ref_sys",
            """
            CREATE TABLE gpkg_spatial_ref_sys(
                srs_name TEXT NOT NULL,
                srs_id INTEGER PRIMARY KEY,
                organization TEXT NOT NULL,
                organization_coordsys_id INTEGER NOT NULL,
                definition TEXT NOT NULL,
                description TEXT
            );
            """);

        _ = ExecuteNonQuery(createCommand, "INSERT OR IGNORE INTO gpkg_spatial_ref_sys VALUES ('Undefined cartesian SRS', -1, 'NONE', -1, 'undefined','undefined cartesian coordinate reference system');");
        _ = ExecuteNonQuery(createCommand, "INSERT OR IGNORE INTO gpkg_spatial_ref_sys VALUES ('Undefined geographic SRS', 0, 'NONE', 0, 'undefined', 'undefined geographic coordinate reference system');");
        _ = ExecuteNonQuery(
            createCommand,
            """INSERT OR IGNORE INTO gpkg_spatial_ref_sys VALUES ('WGS 84 geodetic', 4326, 'EPSG', 4326, 'GEOGCS["WGS 84",DATUM["WGS_1984",SPHEROID["WGS 84",6378137,298.257223563,AUTHORITY["EPSG","7030"]],AUTHORITY["EPSG","6326"]],PRIMEM["Greenwich",0,AUTHORITY["EPSG","8901"]],UNIT["degree",0.0174532925199433,AUTHORITY["EPSG","9122"]],AXIS["Latitude",NORTH],AXIS["Longitude",EAST],AUTHORITY["EPSG","4326"]]', 'longitude/latitude coordinates in decimal degrees on the WGS 84 spheroid');""");

        CreateIfNotExists(
            createCommand,
            existsCommand,
            "gpkg_contents",
            """
            CREATE TABLE gpkg_contents (
              table_name TEXT NOT NULL PRIMARY KEY,
              data_type TEXT NOT NULL,
              identifier TEXT UNIQUE,
              description TEXT DEFAULT '',
              last_change DATETIME NOT NULL DEFAULT (strftime('%Y-%m-%dT%H:%M:%fZ','now')),
              min_x DOUBLE,
              min_y DOUBLE,
              max_x DOUBLE,
              max_y DOUBLE,
              srs_id INTEGER,
              CONSTRAINT fk_gc_r_srs_id FOREIGN KEY (srs_id) REFERENCES gpkg_spatial_ref_sys(srs_id)
            );
            """);

        CreateIfNotExists(
            createCommand,
            existsCommand,
            "gpkg_geometry_columns",
            """
            CREATE TABLE gpkg_geometry_columns (
              table_name TEXT NOT NULL,
              column_name TEXT NOT NULL,
              geometry_type_name TEXT NOT NULL,
              srs_id INTEGER NOT NULL,
              z TINYINT NOT NULL,
              m TINYINT NOT NULL,
              CONSTRAINT pk_geom_cols PRIMARY KEY (table_name, column_name),
              CONSTRAINT uk_gc_table_name UNIQUE (table_name),
              CONSTRAINT fk_gc_tn FOREIGN KEY (table_name) REFERENCES gpkg_contents(table_name),
              CONSTRAINT fk_gc_srs FOREIGN KEY (srs_id) REFERENCES gpkg_spatial_ref_sys (srs_id)
            );
            """);

        static int ExecuteNonQuery(Microsoft.Data.Sqlite.SqliteCommand command, string sql)
        {
            command.CommandText = sql;
            return command.ExecuteNonQuery();
        }

        static void CreateIfNotExists(Microsoft.Data.Sqlite.SqliteCommand createCommand, Microsoft.Data.Sqlite.SqliteCommand existsCommand, string name, string sql)
        {
            if (CheckExists(existsCommand, name))
            {
                return;
            }

            createCommand.CommandText = sql;
            createCommand.ExecuteNonQuery();
        }

        static bool CheckExists(Microsoft.Data.Sqlite.SqliteCommand command, string name)
        {
            command.Parameters["@name"].Value = name;
            return command.ExecuteScalar() is not 0L;
        }

        static Microsoft.Data.Sqlite.SqliteCommand CreateExistsCommand(Microsoft.Data.Sqlite.SqliteConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT([name]) FROM [sqlite_master] WHERE [name] = @name";
            command.Parameters.Add("@name", Microsoft.Data.Sqlite.SqliteType.Text);
            return command;
        }
    }

    private static void CreateFunctions(Microsoft.Data.Sqlite.SqliteConnection connection)
    {
        /* Used by RTree Spatial Index Extension */
        connection.CreateFunction("ST_MinX", (byte[] blob) => GetGeometry<Geometry.IGeometry>(blob)?.MinX(), isDeterministic: true);
        connection.CreateFunction("ST_MinY", (byte[] blob) => GetGeometry<Geometry.IGeometry>(blob)?.MinY(), isDeterministic: true);
        connection.CreateFunction("ST_MaxX", (byte[] blob) => GetGeometry<Geometry.IGeometry>(blob)?.MaxX(), isDeterministic: true);
        connection.CreateFunction("ST_MaxY", (byte[] blob) => GetGeometry<Geometry.IGeometry>(blob)?.MaxY(), isDeterministic: true);
        connection.CreateFunction("ST_IsEmpty", (byte[] blob) => GeoPackageDataReader.ReadHeader(blob) is { Successful: true, Empty: true } or { Successful: false }, isDeterministic: true);

        static T? GetGeometry<T>(byte[] blob)
            where T : Geometry.IGeometry
        {
            return GeoPackageDataReader.ReadHeader(blob) switch
            {
                { Successful: true, Empty: false, Size: var size } when Buffers.Binary.WkbPrimitives.ReadGeometry(blob.AsSpan(size)) is T geom => geom,
                _ => default,
            };
        }
    }
}