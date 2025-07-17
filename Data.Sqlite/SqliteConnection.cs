// -----------------------------------------------------------------------
// <copyright file="SqliteConnection.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data.Sqlite;

using System.Data;
using System.Data.Common;

/// <summary>
/// Represents a connection to a <c>SQLite</c> database which extends <see cref="Microsoft.Data.Sqlite.SqliteConnection"/>.
/// </summary>
public class SqliteConnection : Microsoft.Data.Sqlite.SqliteConnection
{
    private const string DefaultMasterTableName = "sqlite_master";

    private const string TempMasterTableName = "sqlite_temp_master";

    private const string DefaultCatalogName = "main";

    private const string TempCatalogName = "temp";

    private const string TableCatalog = "TABLE_CATALOG";

    private const string TableSchema = "TABLE_SCHEMA";

    private const string TableName = "TABLE_NAME";

    private const string TableType = "TABLE_TYPE";

    private const string TableId = "TABLE_ID";

    private const string TableRootPage = "TABLE_ROOTPAGE";

    private const string TableDefinition = "TABLE_DEFINITION";

    private const string ColumnName = "COLUMN_NAME";

    private const string OrdinalPosition = "ORDINAL_POSITION";

    private const string ColumnHasDefault = "COLUMN_HASDEFAULT";

    private const string ColumnDefault = "COLUMN_DEFAULT";

    private const string IsNullable = "IS_NULLABLE";

    private const string DataType = "DATA_TYPE";

    private const string CharacterMaximumLength = "CHARACTER_MAXIMUM_LENGTH";

    private const string NumericPrecision = "NUMERIC_PRECISION";

    private const string NumericScale = "NUMERIC_SCALE";

    private const string PrimaryKey = "PRIMARY_KEY";

    private const string AutoIncrement = "AUTOINCREMENT";

    private const string Unique = "UNIQUE";

    private const string IndexName = "INDEX_NAME";

    private const string IndexCatalog = "INDEX_CATALOG";

    private const string IndexDefinition = "INDEX_DEFINITION";

    /// <summary>
    /// Initialises a new instance of the <see cref="SqliteConnection"/> class.
    /// </summary>
    public SqliteConnection()
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="SqliteConnection"/> class.
    /// </summary>
    /// <param name="connectionString">The string used to open the connection.</param>
    public SqliteConnection(string connectionString)
        : base(connectionString)
    {
    }

    /// <inheritdoc/>
    public override DataTable GetSchema() => this.GetSchema(DbMetaDataCollectionNames.MetaDataCollections);

    /// <inheritdoc/>
    public override DataTable GetSchema(string collectionName) => this.GetSchema(collectionName, []);

    /// <inheritdoc/>
    public override DataTable GetSchema(string collectionName, string?[] restrictionValues)
    {
        if (this.State is not ConnectionState.Open)
        {
            throw new InvalidOperationException();
        }

        var parms = new string?[5];
        restrictionValues?.CopyTo(parms, 0);

        return collectionName.ToUpper(System.Globalization.CultureInfo.InvariantCulture) switch
        {
            "METADATACOLLECTIONS" => this.GetMetadataCollectionsSchema(),
            "RESERVEDWORDS" => this.GetReservedWordsSchema(),
            "DATASOURCEINFORMATION" => this.GetDataSourceInformationSchema(),
            "COLUMNS" or "TABLECOLUMNS" => this.GetColumnsSchema(parms[0], parms[2], parms[3]),
            "DATATYPES" => this.GetDataTypesSchema(),
            "TABLES" => this.GetTablesSchema(parms[0], parms[2], parms[3]),
            "INDEXES" => this.GetIndexesSchema(parms[0], parms[2], parms[3]),
            _ => base.GetSchema(collectionName, restrictionValues),
        };
    }

    /// <summary>
    /// Sets the command transaction to the connection transaction.
    /// </summary>
    /// <param name="command">The command to update.</param>
    public void SetTransaction(IDbCommand command)
    {
        if (command.Transaction is null && this.Transaction is not null)
        {
            command.Transaction = this.Transaction;
        }
    }

    /// <summary>
    /// Gets the metadata collections schema.
    /// </summary>
    /// <returns>The metadata collections schema.</returns>
    protected virtual DataTable GetMetadataCollectionsSchema()
    {
        var dataTable = new DataTable(DbMetaDataCollectionNames.MetaDataCollections)
        {
            Locale = System.Globalization.CultureInfo.InvariantCulture,
            Columns =
            {
                { DbMetaDataColumnNames.CollectionName, typeof(string) },
                { DbMetaDataColumnNames.NumberOfRestrictions, typeof(int) },
                { DbMetaDataColumnNames.NumberOfIdentifierParts, typeof(int) },
            },
        };

        dataTable.BeginLoadData();

        using (var reader = System.Xml.XmlReader.Create(typeof(SqliteConnection).Assembly.GetManifestResourceStream(typeof(SqliteConnection), "MetaDataCollections.xml")))
        {
            _ = dataTable.ReadXml(reader);
        }

        dataTable.AcceptChanges();
        dataTable.EndLoadData();

        return dataTable;
    }

    /// <summary>
    /// Gets the data source information schema.
    /// </summary>
    /// <returns>The data source information schema.</returns>
    protected virtual DataTable GetDataSourceInformationSchema()
    {
        var dataTable = new DataTable(DbMetaDataCollectionNames.DataSourceInformation)
        {
            Locale = System.Globalization.CultureInfo.InvariantCulture,
            Columns =
            {
                { DbMetaDataColumnNames.CompositeIdentifierSeparatorPattern, typeof(string) },
                { DbMetaDataColumnNames.DataSourceProductName, typeof(string) },
                { DbMetaDataColumnNames.DataSourceProductVersion, typeof(string) },
                { DbMetaDataColumnNames.DataSourceProductVersionNormalized, typeof(string) },
                { DbMetaDataColumnNames.GroupByBehavior, typeof(int) },
                { DbMetaDataColumnNames.IdentifierPattern, typeof(string) },
                { DbMetaDataColumnNames.IdentifierCase, typeof(int) },
                { DbMetaDataColumnNames.OrderByColumnsInSelect, typeof(bool) },
                { DbMetaDataColumnNames.ParameterMarkerFormat, typeof(string) },
                { DbMetaDataColumnNames.ParameterMarkerPattern, typeof(string) },
                { DbMetaDataColumnNames.ParameterNameMaxLength, typeof(int) },
                { DbMetaDataColumnNames.ParameterNamePattern, typeof(string) },
                { DbMetaDataColumnNames.QuotedIdentifierPattern, typeof(string) },
                { DbMetaDataColumnNames.QuotedIdentifierCase, typeof(int) },
                { DbMetaDataColumnNames.StatementSeparatorPattern, typeof(string) },
                { DbMetaDataColumnNames.StringLiteralPattern, typeof(string) },
                { DbMetaDataColumnNames.SupportedJoinOperators, typeof(int) },
            },
        };

        dataTable.BeginLoadData();

        var row = dataTable.NewRow();
        row.ItemArray =
            [
                null,
                "SQLite",
                this.ServerVersion,
                this.ServerVersion,
                3,
                "(^\\[\\p{Lo}\\p{Lu}\\p{Ll}_@#][\\p{Lo}\\p{Lu}\\p{Ll}\\p{Nd}@$#_]*$)|(^\\[[^\\]\\0]|\\]\\]+\\]$)|(^\\\"[^\\\"\\0]|\\\"\\\"+\\\"$)",
                1,
                false,
                "{0}",
                "@[\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}_@#][\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}\\p{Nd}\\uff3f_@#\\$]*(?=\\s+|$)",
                255,
                "^[\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}_@#][\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}\\p{Nd}\\uff3f_@#\\$]*(?=\\s+|$)",
                "(([^\\[]|\\]\\])*)",
                1,
                ";",
                "'(([^']|'')*)'",
                15,
            ];

        dataTable.Rows.Add(row);

        dataTable.AcceptChanges();
        dataTable.EndLoadData();

        return dataTable;
    }

    /// <summary>
    /// Gets the data types schema.
    /// </summary>
    /// <returns>The data types schema.</returns>
    protected virtual DataTable GetDataTypesSchema()
    {
        var dataTable = new DataTable(DbMetaDataCollectionNames.DataTypes)
        {
            Locale = System.Globalization.CultureInfo.InvariantCulture,
            Columns =
            {
                { DbMetaDataColumnNames.TypeName, typeof(string) },
                { DbMetaDataColumnNames.ProviderDbType, typeof(int) },
                { DbMetaDataColumnNames.ColumnSize, typeof(long) },
                { DbMetaDataColumnNames.CreateFormat, typeof(string) },
                { DbMetaDataColumnNames.CreateParameters, typeof(string) },
                { DbMetaDataColumnNames.DataType, typeof(string) },
                { DbMetaDataColumnNames.IsAutoIncrementable, typeof(bool) },
                { DbMetaDataColumnNames.IsBestMatch, typeof(bool) },
                { DbMetaDataColumnNames.IsCaseSensitive, typeof(bool) },
                { DbMetaDataColumnNames.IsFixedLength, typeof(bool) },
                { DbMetaDataColumnNames.IsFixedPrecisionScale, typeof(bool) },
                { DbMetaDataColumnNames.IsLong, typeof(bool) },
                { DbMetaDataColumnNames.IsNullable, typeof(bool) },
                { DbMetaDataColumnNames.IsSearchable, typeof(bool) },
                { DbMetaDataColumnNames.IsSearchableWithLike, typeof(bool) },
                { DbMetaDataColumnNames.IsLiteralSupported, typeof(bool) },
                { DbMetaDataColumnNames.LiteralPrefix, typeof(string) },
                { DbMetaDataColumnNames.LiteralSuffix, typeof(string) },
                { DbMetaDataColumnNames.IsUnsigned, typeof(bool) },
                { DbMetaDataColumnNames.MaximumScale, typeof(short) },
                { DbMetaDataColumnNames.MinimumScale, typeof(short) },
                { DbMetaDataColumnNames.IsConcurrencyType, typeof(bool) },
            },
        };

        dataTable.BeginLoadData();

        using (var reader = System.Xml.XmlReader.Create(typeof(SqliteConnection).Assembly.GetManifestResourceStream(typeof(SqliteConnection), "DataTypes.xml")))
        {
            _ = dataTable.ReadXml(reader);
        }

        dataTable.AcceptChanges();
        dataTable.EndLoadData();

        return dataTable;
    }

    /// <summary>
    /// Gets the table schema.
    /// </summary>
    /// <param name="catalog">The catalog (attached database) to query.</param>
    /// <param name="table">The table to retrieve index information for.</param>
    /// <param name="tableType">The type of the table to retrieve information for.</param>
    /// <returns>The table schema.</returns>
    protected virtual DataTable GetTablesSchema(string? catalog, string? table, string? tableType)
    {
        if (string.IsNullOrEmpty(catalog))
        {
            catalog = GetDefaultCatalogName();
        }

        var master = string.Equals(catalog, TempCatalogName, StringComparison.OrdinalIgnoreCase) ? TempMasterTableName : DefaultMasterTableName;

        var dataTable = new DataTable(SqliteMetadataCollectionNames.Tables)
        {
            Locale = System.Globalization.CultureInfo.InvariantCulture,
            Columns =
            {
                { TableCatalog, typeof(string) },
                { TableSchema, typeof(string) },
                { TableName, typeof(string) },
                { TableType, typeof(string) },
                { TableId, typeof(long) },
                { TableRootPage, typeof(int) },
                { TableDefinition, typeof(string) },
            },
        };

        dataTable.BeginLoadData();

        using (var command = new Microsoft.Data.Sqlite.SqliteCommand($"SELECT [type], [name], [tbl_name], [rootpage], [sql], [rowid] FROM [{catalog}].[{master}] WHERE [type] LIKE 'table'", this))
        {
            using var dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                var tableName = dataReader.GetString(2);
                var type = tableName.StartsWith("SQLITE_", StringComparison.OrdinalIgnoreCase)
                    ? "SYSTEM_TABLE"
                    : dataReader.GetString(0);
                if (tableType?.Equals(type, StringComparison.OrdinalIgnoreCase) is false)
                {
                    continue;
                }

                if (table?.Equals(tableName, StringComparison.OrdinalIgnoreCase) is false)
                {
                    continue;
                }

                var row = dataTable.NewRow();

                row[TableCatalog] = catalog;
                row[TableName] = tableName;
                row[TableType] = type;
                row[TableId] = dataReader.GetInt64(5);
                row[TableRootPage] = dataReader.GetInt32(3);
                row[TableDefinition] = dataReader.GetString(4);

                dataTable.Rows.Add(row);
            }
        }

        dataTable.AcceptChanges();
        dataTable.EndLoadData();

        return dataTable;
    }

    /// <summary>
    /// Gets the indexed schema.
    /// </summary>
    /// <param name="catalog">The catalog (attached database) to query.</param>
    /// <param name="table">The table to retrieve index information for.</param>
    /// <param name="index">The name of the index to retrieve information for.</param>
    /// <returns>The indexed schema.</returns>
    protected virtual DataTable GetIndexesSchema(string? catalog, string? table, string? index)
    {
        var dataTable = new DataTable("Indexes")
        {
            Locale = System.Globalization.CultureInfo.InvariantCulture,
            Columns =
            {
                { TableCatalog, typeof(string) },
                { TableSchema, typeof(string) },
                { TableName, typeof(string) },
                { IndexCatalog, typeof(string) },
                { "INDEX_SCHEMA", typeof(string) },
                { IndexName, typeof(string) },
                { PrimaryKey, typeof(bool) },
                { Unique, typeof(bool) },
                { "CLUSTERED", typeof(bool) },
                { "TYPE", typeof(int) },
                { "FILL_FACTOR", typeof(int) },
                { "INITIAL_SIZE", typeof(int) },
                { "NULLS", typeof(int) },
                { "SORT_BOOKMARKS", typeof(bool) },
                { "AUTO_UPDATE", typeof(bool) },
                { "NULL_COLLATION", typeof(int) },
                { OrdinalPosition, typeof(int) },
                { ColumnName, typeof(string) },
                { "COLUMN_GUID", typeof(Guid) },
                { "COLUMN_PROPID", typeof(long) },
                { "COLLATION", typeof(short) },
                { "CARDINALITY", typeof(decimal) },
                { "PAGES", typeof(int) },
                { "FILTER_CONDITION", typeof(string) },
                { "INTEGRATED", typeof(bool) },
                { IndexDefinition, typeof(string) },
            },
        };

        dataTable.BeginLoadData();

        if (string.IsNullOrEmpty(catalog))
        {
            catalog = GetDefaultCatalogName();
        }

        var master = GetMasterTableName(IsTemporaryCatalogName(catalog));

        using (var tablesCommand = new Microsoft.Data.Sqlite.SqliteCommand($"SELECT * FROM [{catalog}].[{master}] WHERE [type] LIKE 'table'", this))
        {
            using var tablesDataReader = tablesCommand.ExecuteReader();
            while (tablesDataReader.Read())
            {
                var maybeRowId = false;
                var primaryKeys = new List<int>();
                if (string.IsNullOrEmpty(table) || string.Equals(tablesDataReader.GetString(2), table, StringComparison.OrdinalIgnoreCase))
                {
                    // First, look for any rowid indexes -- which sqlite defines are INTEGER PRIMARY KEY columns.
                    // Such indexes are not listed in the indexes list but count as indexes just the same.
                    try
                    {
                        using var tableCommand = new Microsoft.Data.Sqlite.SqliteCommand($"PRAGMA [{catalog}].table_info([{tablesDataReader.GetString(2)}])", this);
                        using var tableDataReader = tableCommand.ExecuteReader();
                        while (tableDataReader.Read())
                        {
                            if (tableDataReader.GetInt32(5) is not 0)
                            {
                                primaryKeys.Add(tableDataReader.GetInt32(0));

                                // If the primary key is of type INTEGER, then its a rowid and we need to make a fake index entry for it.
                                if (string.Equals(tableDataReader.GetString(2), "INTEGER", StringComparison.OrdinalIgnoreCase))
                                {
                                    maybeRowId = true;
                                }
                            }
                        }
                    }
                    catch (Microsoft.Data.Sqlite.SqliteException)
                    {
                        // catch any SQLite exception
                    }

                    if (primaryKeys.Count is 1 && maybeRowId)
                    {
                        var row = dataTable.NewRow();

                        row[TableCatalog] = catalog;
                        row[TableName] = tablesDataReader.GetString(2);
                        row[IndexCatalog] = catalog;
                        row[PrimaryKey] = true;
                        row[IndexName] = $"{tablesDataReader.GetString(2)}_PK_{master}";
                        row[Unique] = true;

                        if (string.Equals((string)row[IndexName], index, StringComparison.OrdinalIgnoreCase) || index is null)
                        {
                            dataTable.Rows.Add(row);
                        }

                        primaryKeys.Clear();
                    }

                    // Now fetch all the rest of the indexes.
                    try
                    {
                        using var command = new Microsoft.Data.Sqlite.SqliteCommand($"PRAGMA [{catalog}].index_list([{tablesDataReader.GetString(2)}])", this);
                        using var dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            if (string.Equals(dataReader.GetString(1), index, StringComparison.OrdinalIgnoreCase) || index is null)
                            {
                                var row = dataTable.NewRow();

                                row[TableCatalog] = catalog;
                                row[TableName] = tablesDataReader.GetString(2);
                                row[IndexCatalog] = catalog;
                                row[IndexName] = dataReader.GetString(1);
                                row[Unique] = ToBoolean(dataReader.GetValue(2), System.Globalization.CultureInfo.InvariantCulture, viaFramework: false);
                                row[PrimaryKey] = false;

                                // get the index definition
                                using (var indexesCommand = new Microsoft.Data.Sqlite.SqliteCommand($"SELECT * FROM [{catalog}].[{master}] WHERE [type] LIKE 'index' AND [name] LIKE '{Sanitize(dataReader.GetString(1))}'", this))
                                using (var indexesDataReader = indexesCommand.ExecuteReader())
                                {
                                    if (indexesDataReader.Read() && !indexesDataReader.IsDBNull(4))
                                    {
                                        row[IndexDefinition] = indexesDataReader.GetString(4);
                                    }
                                }

                                // Now for the really hard work.  Figure out which index is the primary key index.
                                // The only way to figure it out is to check if the index was an autoindex and if we have a non-rowid
                                // primary key, and all the columns in the given index match the primary key columns
                                if (primaryKeys.Count > 0 && dataReader.GetString(1).StartsWith("sqlite_autoindex_" + tablesDataReader.GetString(2), StringComparison.InvariantCultureIgnoreCase))
                                {
                                    using var detailsCommand = new Microsoft.Data.Sqlite.SqliteCommand($"PRAGMA [{catalog}].index_info([{dataReader.GetString(1)}])", this);
                                    using var detailsDataReader = detailsCommand.ExecuteReader();

                                    var matchCount = 0;
                                    while (detailsDataReader.Read())
                                    {
                                        if (!primaryKeys.Contains(detailsDataReader.GetInt32(1)))
                                        {
                                            matchCount = 0;
                                            break;
                                        }

                                        matchCount++;
                                    }

                                    if (matchCount == primaryKeys.Count)
                                    {
                                        row[PrimaryKey] = true;
                                        primaryKeys.Clear();
                                    }
                                }

                                dataTable.Rows.Add(row);
                            }
                        }
                    }
                    catch (Microsoft.Data.Sqlite.SqliteException)
                    {
                        // catch any SQLite exception
                    }

                    static bool ToBoolean(object? obj, IFormatProvider provider, bool viaFramework)
                    {
                        const char CharDefault = default;
                        const sbyte SByteDefault = default;
                        const byte ByteDefault = default;
                        const short Int16Default = default;
                        const ushort UInt16Default = default;

                        return obj switch
                        {
                            null => false,
                            bool @bool => @bool,
                            char @char => @char is not CharDefault,
                            sbyte @sbyte => @sbyte is not SByteDefault,
                            byte @byte => @byte is not ByteDefault,
                            short @short => @short is not Int16Default,
                            ushort @ushort => @ushort is not UInt16Default,
                            int @int => @int is not 0,
                            uint @uint => @uint is not 0U,
                            long @long => @long is not 0L,
                            ulong @ulong => @ulong is not 0UL,
                            float @float => @float is not 0F,
                            double @double => @double is not 0D,
                            decimal @decimal => @decimal is not decimal.Zero,
                            string @string when viaFramework => Convert.ToBoolean(@string, provider),
                            string @string => ToBoolean(@string),
                            _ => throw new Microsoft.Data.Sqlite.SqliteException($"Cannot convert type {obj.GetType()} to boolean", -1),
                        };

                        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2589:Boolean expressions should not be gratuitous", Justification = "Checked")]
                        static bool ToBoolean(string? source)
                        {
                            return source is null
                                ? throw new ArgumentNullException(nameof(source))
                                : source.ToLower(System.Globalization.CultureInfo.InvariantCulture) switch
                                {
                                    { } value when string.Compare(value, 0, bool.TrueString, 0, value.Length, StringComparison.OrdinalIgnoreCase) is 0 => true,
                                    { } value when string.Compare(value, 0, bool.FalseString, 0, value.Length, StringComparison.OrdinalIgnoreCase) is 0 => false,
                                    "y" or "yes" or "on" or "1" => true,
                                    "n" or "no" or "off" or "0" => false,
                                    _ => throw new ArgumentException("Invalid boolean value", nameof(source)),
                                };
                        }
                    }
                }
            }
        }

        dataTable.AcceptChanges();
        dataTable.EndLoadData();

        return dataTable;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static string Sanitize(string input)
        {
#if NETSTANDARD2_1_OR_GREATER
            return input.Replace("'", "''", StringComparison.Ordinal);
#else
            return input.Replace("'", "''");
#endif
        }
    }

    /// <summary>
    /// Gets the column schema.
    /// </summary>
    /// <param name="catalog">The catalog (attached database) to query.</param>
    /// <param name="table">The table to retrieve index information for.</param>
    /// <param name="column">The name of the column to retrieve information for.</param>
    /// <returns>The column schema.</returns>
    protected virtual DataTable GetColumnsSchema(string? catalog, string? table, string? column)
    {
        if (string.IsNullOrEmpty(catalog))
        {
            catalog = DefaultCatalogName;
        }

        var master = GetMasterTableName(IsTemporaryCatalogName(catalog));

        var dataTable = new DataTable(SqliteMetadataCollectionNames.Columns)
        {
            Locale = System.Globalization.CultureInfo.InvariantCulture,
            Columns =
            {
                { TableCatalog, typeof(string) },
                { TableSchema, typeof(string) },
                { TableName, typeof(string) },
                { ColumnName, typeof(string) },
                { "COLUMN_GUID", typeof(Guid) },
                { "COLUMN_PROPID", typeof(long) },
                { OrdinalPosition, typeof(int) },
                { ColumnHasDefault, typeof(bool) },
                { ColumnDefault, typeof(string) },
                { "COLUMN_FLAGS", typeof(long) },
                { IsNullable, typeof(bool) },
                { DataType, typeof(string) },
                { "TYPE_GUID", typeof(Guid) },
                { CharacterMaximumLength, typeof(int) },
                { "CHARACTER_OCTET_LENGTH", typeof(int) },
                { NumericPrecision, typeof(int) },
                { NumericScale, typeof(int) },
                { "DATETIME_PRECISION", typeof(long) },
                { "CHARACTER_SET_CATALOG", typeof(string) },
                { "CHARACTER_SET_SCHEMA", typeof(string) },
                { "CHARACTER_SET_NAME", typeof(string) },
                { "COLLATION_CATALOG", typeof(string) },
                { "COLLATION_SCHEMA", typeof(string) },
                { "COLLATION_NAME", typeof(string) },
                { "DOMAIN_CATALOG", typeof(string) },
                { "DOMAIN_NAME", typeof(string) },
                { "DESCRIPTION", typeof(string) },
                { PrimaryKey, typeof(bool) },
                { "EDM_TYPE", typeof(string) },
                { AutoIncrement, typeof(bool) },
                { Unique, typeof(bool) },
            },
        };

        dataTable.BeginLoadData();

        using (var tablesCommand = new Microsoft.Data.Sqlite.SqliteCommand($"SELECT * FROM [{catalog}].[{master}] WHERE [type] LIKE 'table' OR [type] LIKE 'view'", this))
        {
            using var tablesDataReader = tablesCommand.ExecuteReader();
            while (tablesDataReader.Read())
            {
                if (table?.Equals(tablesDataReader.GetString(2), StringComparison.OrdinalIgnoreCase) is false)
                {
                    continue;
                }

                try
                {
                    using var detailsCommand = new Microsoft.Data.Sqlite.SqliteCommand($"SELECT * FROM [{catalog}].[{tablesDataReader.GetString(2)}] LIMIT 1", this);
                    using var detailsDataReader = detailsCommand.ExecuteReader();
                    using var tableSchema = detailsDataReader.GetSchemaTable();
                    foreach (var schemaRow in tableSchema.Rows
                        .Cast<DataRow>()
                        .Where(schemaRow => column?.Equals(schemaRow[SchemaTableColumn.ColumnName].ToString(), StringComparison.OrdinalIgnoreCase) is not false))
                    {
                        var row = dataTable.NewRow();
                        row[NumericPrecision] = schemaRow[SchemaTableColumn.NumericPrecision];
                        row[NumericScale] = schemaRow[SchemaTableColumn.NumericScale];
                        row[TableName] = tablesDataReader.GetString(2);
                        row[ColumnName] = schemaRow[SchemaTableColumn.ColumnName];
                        row[TableCatalog] = catalog;
                        row[OrdinalPosition] = schemaRow[SchemaTableColumn.ColumnOrdinal];
                        row[ColumnHasDefault] = schemaRow.Table.Columns.Contains(SchemaTableOptionalColumn.DefaultValue) && schemaRow[SchemaTableOptionalColumn.DefaultValue] != DBNull.Value;
                        row[ColumnDefault] = schemaRow.Table.Columns.Contains(SchemaTableOptionalColumn.DefaultValue) ? schemaRow[SchemaTableOptionalColumn.DefaultValue] : DBNull.Value;
                        row[IsNullable] = schemaRow[SchemaTableColumn.AllowDBNull];
                        row[DataType] = schemaRow[SqliteSchemaTableColumn.DataTypeName].ToString().ToLowerInvariant();
                        row[CharacterMaximumLength] = schemaRow[SchemaTableColumn.ColumnSize];
                        row[TableSchema] = schemaRow[SchemaTableColumn.BaseSchemaName];
                        row[PrimaryKey] = schemaRow[SchemaTableColumn.IsKey];
                        row[AutoIncrement] = schemaRow[SchemaTableOptionalColumn.IsAutoIncrement];
                        row[Unique] = schemaRow[SchemaTableColumn.IsUnique];
                        dataTable.Rows.Add(row);
                    }
                }
                catch (Microsoft.Data.Sqlite.SqliteException)
                {
                    // prefer to not get this data than error
                }
            }
        }

        dataTable.AcceptChanges();
        dataTable.EndLoadData();

        return dataTable;
    }

    /// <summary>
    /// Gets the reserved words schema.
    /// </summary>
    /// <returns>The column schema.</returns>
    protected virtual DataTable GetReservedWordsSchema()
    {
        var dataTable = new DataTable(DbMetaDataCollectionNames.ReservedWords)
        {
            Locale = System.Globalization.CultureInfo.InvariantCulture,
            Columns =
            {
                { DbMetaDataColumnNames.ReservedWord, typeof(string) },
                { "MaximumVersion", typeof(string) },
                { "MinimumVersion", typeof(string) },
            },
        };

        dataTable.BeginLoadData();

        foreach (var word in Properties.Resources.Keywords.Split(','))
        {
            var row = dataTable.NewRow();
            row[0] = word;
            dataTable.Rows.Add(row);
        }

        dataTable.AcceptChanges();
        dataTable.EndLoadData();

        return dataTable;
    }

    private static string GetMasterTableName(bool temporary) => temporary ? TempMasterTableName : DefaultMasterTableName;

    private static bool IsTemporaryCatalogName(string? catalogName)
    {
        return string.Equals(catalogName, GetTemporaryCatalogName(), StringComparison.OrdinalIgnoreCase);

        static string GetTemporaryCatalogName()
        {
            return TempCatalogName;
        }
    }

    private static string GetDefaultCatalogName() => DefaultCatalogName;
}