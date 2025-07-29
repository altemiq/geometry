// -----------------------------------------------------------------------
// <copyright file="SpatialiteConnection.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data.Spatialite;

/// <summary>
/// The <c>SpatiaLite</c> connection.
/// </summary>
public sealed class SpatialiteConnection : Sqlite.SqliteConnection
{
    private const string InitSpatialMetadataParameterisedCommandText = "SELECT InitSpatialMetadataFull(1,$mode) WHERE CheckSpatialMetadata() = 0;";

    private const string InitSpatialMetadataCommandText = "SELECT InitSpatialMetadataFull(1) WHERE CheckSpatialMetadata() = 0;";

    private static readonly string[] Dimensions = [string.Empty, "z", "m", "zm"];

    private static readonly string[] SpatialDataTypes = ["point", "linestring", "polygon", "multipoint", "multilinestring", "multipolygon", "geometrycollection", "geometry"];

    private string? connectionString;

    /// <summary>
    /// Initialises a new instance of the <see cref="SpatialiteConnection"/> class.
    /// </summary>
    public SpatialiteConnection() => SpatialiteLoader.Load(this);

    /// <summary>
    /// Initialises a new instance of the <see cref="SpatialiteConnection"/> class.
    /// </summary>
    /// <param name="connectionString">The string used to open the connection.</param>
    public SpatialiteConnection(string connectionString)
        : this() => this.ConnectionString = connectionString;

    /// <summary>
    /// Gets the spatialite version.
    /// </summary>
    public string? SpatialiteVersion
    {
        get
        {
            if (this.State is System.Data.ConnectionState.Open)
            {
                using var command = this.CreateCommand();
                command.CommandText = "SELECT spatialite_version();";
                return command.ExecuteScalar() as string;
            }

            return null;
        }
    }

    /// <inheritdoc/>
    [System.Diagnostics.CodeAnalysis.AllowNull]
    public override string ConnectionString
    {
        get => this.connectionString!;
        set
        {
            this.connectionString = value;
            base.ConnectionString = CreateConnectionStringBuilder(this.connectionString)?.ConnectionString!;
        }
    }

    /// <summary>
    /// Returns whether the specified data type is a spatial data type.
    /// </summary>
    /// <param name="dataType">The data type.</param>
    /// <returns><see langword="true"/> if <paramref name="dataType"/> is spatial; otherwise <see langword="false"/>.</returns>
    public static bool IsSpatialDataType(string dataType) => Array.Exists(SpatialDataTypes, spatialDataType => dataType.StartsWith(spatialDataType, StringComparison.Ordinal));

    /// <inheritdoc/>
    public override void Open()
    {
        if (this.State is System.Data.ConnectionState.Open)
        {
            return;
        }

        var cachedConnectionString = this.connectionString;
        var builder = CreateConnectionStringBuilder(cachedConnectionString);
        this.ConnectionString = builder?.ConnectionString;
        try
        {
            base.Open();
        }
        finally
        {
            this.connectionString = cachedConnectionString;
        }

        using var command = this.Initialise(builder);
        _ = command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public override async Task OpenAsync(CancellationToken cancellationToken)
    {
        if (this.State is System.Data.ConnectionState.Open)
        {
            return;
        }

        var cachedConnectionString = this.connectionString;
        var builder = CreateConnectionStringBuilder(this.connectionString);
        this.ConnectionString = builder?.ConnectionString;
        try
        {
            await OpenBaseAsync(cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            this.connectionString = cachedConnectionString;
        }

        var command = this.Initialise(builder);
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
        await using (command.ConfigureAwait(false))
#else
        using (command)
#endif
        {
            _ = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
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
    public new SpatialiteCommand CreateCommand() => new()
    {
        Connection = this,
        CommandTimeout = this.DefaultTimeout,
        Transaction = this.Transaction,
    };

    /// <inheritdoc/>
    protected override System.Data.Common.DbCommand CreateDbCommand() => this.CreateCommand();

    /// <inheritdoc/>
    protected override System.Data.DataTable GetDataTypesSchema()
    {
        var dataTable = base.GetDataTypesSchema();

        // Add the spatial types
        foreach (var typeName in SpatialDataTypes.SelectMany(spatialDataType => Dimensions.Select(dimension => spatialDataType + dimension)))
        {
            var row = dataTable.NewRow();
            row[System.Data.Common.DbMetaDataColumnNames.TypeName] = typeName;
            row[System.Data.Common.DbMetaDataColumnNames.ProviderDbType] = System.Data.DbType.Binary;
            dataTable.Rows.Add(row);
        }

        return dataTable;
    }

    [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(connectionString))]
    private static SpatialiteConnectionStringBuilder? CreateConnectionStringBuilder(string? connectionString) => connectionString is null
        ? null
        : new(connectionString) { SerializeAsSqlite = true };

    private Microsoft.Data.Sqlite.SqliteCommand Initialise(SpatialiteConnectionStringBuilder? options)
    {
        this.EnableExtensions();
        return options?.InitSpatialMetadata switch
        {
            InitSpatialMetadataMode.None => InitialiseCommand(this.CreateCommand(), "NONE"),
            InitSpatialMetadataMode.Wgs84 => InitialiseCommand(this.CreateCommand(), "WGS84"),
            _ => InitialiseCommand(this.CreateCommand()),
        };

        static Microsoft.Data.Sqlite.SqliteCommand InitialiseCommand(Microsoft.Data.Sqlite.SqliteCommand command, string? mode = default)
        {
            if (mode is not null)
            {
                command.CommandText = InitSpatialMetadataParameterisedCommandText;
                _ = command.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter("$mode", mode));
            }
            else
            {
                command.CommandText = InitSpatialMetadataCommandText;
            }

            return command;
        }
    }
}