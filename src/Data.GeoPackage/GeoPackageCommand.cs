// -----------------------------------------------------------------------
// <copyright file="GeoPackageCommand.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data.GeoPackage;

/// <summary>
/// A <c>GeoPackage</c> <see cref="Microsoft.Data.Sqlite.SqliteCommand"/>.
/// </summary>
public class GeoPackageCommand : Microsoft.Data.Sqlite.SqliteCommand
{
    /// <summary>
    /// Gets or sets the data reader currently being used by the command, or <see langword="null"/> if none.
    /// </summary>
    protected internal new virtual GeoPackageDataReader? DataReader { get; set; }

    /// <inheritdoc cref="Microsoft.Data.Sqlite.SqliteCommand.ExecuteReader()" />
    public new virtual GeoPackageDataReader ExecuteReader() => this.ExecuteReader(System.Data.CommandBehavior.Default);

    /// <inheritdoc cref="Microsoft.Data.Sqlite.SqliteCommand.ExecuteReader(System.Data.CommandBehavior)" />
    public new virtual GeoPackageDataReader ExecuteReader(System.Data.CommandBehavior behavior)
    {
        var (_, type) = GetNameAndType(this);

        return this.DataReader = new(base.ExecuteReader(behavior), type);
    }

    /// <inheritdoc cref="Microsoft.Data.Sqlite.SqliteCommand.ExecuteReaderAsync()" />
    public new virtual Task<GeoPackageDataReader> ExecuteReaderAsync() => this.ExecuteReaderAsync(System.Data.CommandBehavior.Default);

    /// <inheritdoc cref="Microsoft.Data.Sqlite.SqliteCommand.ExecuteReaderAsync(CancellationToken)" />
    public new virtual Task<GeoPackageDataReader> ExecuteReaderAsync(CancellationToken cancellationToken) => this.ExecuteReaderAsync(System.Data.CommandBehavior.Default, cancellationToken);

    /// <inheritdoc cref="Microsoft.Data.Sqlite.SqliteCommand.ExecuteReaderAsync(System.Data.CommandBehavior)" />
    public new virtual Task<GeoPackageDataReader> ExecuteReaderAsync(System.Data.CommandBehavior behavior) => this.ExecuteReaderAsync(behavior, CancellationToken.None);

    /// <inheritdoc cref="Microsoft.Data.Sqlite.SqliteCommand.ExecuteReaderAsync(System.Data.CommandBehavior,CancellationToken)" />
    public new virtual async Task<GeoPackageDataReader> ExecuteReaderAsync(System.Data.CommandBehavior behavior, CancellationToken cancellationToken)
    {
        var (_, type) = GetNameAndType(this);
        var dataReader = await base.ExecuteReaderAsync(behavior, cancellationToken).ConfigureAwait(false);
        return this.DataReader = new(dataReader, type);
    }

    /// <inheritdoc />
    protected override System.Data.Common.DbDataReader ExecuteDbDataReader(System.Data.CommandBehavior behavior) => this.ExecuteReader(behavior);

    private static (string? Name, Buffers.Binary.WkbPrimitives.WkbGeometryType Type) GetNameAndType(Microsoft.Data.Sqlite.SqliteCommand command)
    {
        // see if we can sniff out the table from the command text
        var connection = command.Connection;
        if (connection is null)
        {
            return default;
        }

        using var schemaCommand = connection.CreateCommand();

        schemaCommand.CommandText = "SELECT [column_name], [geometry_type_name], [z], [m] FROM [gpkg_geometry_columns] WHERE [table_name] = @tableName;";
        schemaCommand.Parameters.AddWithValue("@tableName", ParseTableName(command.CommandText));

        using var schemaReader = schemaCommand.ExecuteReader();
        if (schemaReader.Read())
        {
            var columnName = schemaReader.GetString(0);
            var geometryTypeName = schemaReader.GetString(1);
            var z = schemaReader.GetByte(2);
            var m = schemaReader.GetByte(3);

            return (columnName, GetGeometryType(geometryTypeName, z is not 0, m is not 0));
        }

        return (null, default);
    }

    private static string ParseTableName(string text)
    {
        if (FindFrom(text) is not (>= 0 and var fromStart))
        {
            return string.Empty;
        }

        var tableStart = FindNextNonWhiteSpace(text, fromStart + 5);
        var tableEnd = FindNextWhiteSpace(text, tableStart);

        // get the table name
        var tableName = text.Substring(tableStart, tableEnd - tableStart);

        return tableName.TrimStart('[', '"').TrimEnd(']', '"');

        static int FindFrom(string text)
        {
            for (var i = 0; i < text.Length; i++)
            {
                if (text[i] is 'F' or 'f'
                    && i is not 0
                    && char.IsWhiteSpace(text[i - 1])
                    && i + 5 < text.Length
                    && text[i + 1] is 'R' or 'r'
                    && text[i + 2] is 'O' or 'o'
                    && text[i + 3] is 'M' or 'm'
                    && char.IsWhiteSpace(text[i + 4]))
                {
                    return i;
                }
            }

            return -1;
        }

        static int FindNextNonWhiteSpace(string text, int start)
        {
            for (var i = start; i < text.Length; i++)
            {
                if (!char.IsWhiteSpace(text[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        static int FindNextWhiteSpace(string text, int start)
        {
            for (var i = start; i < text.Length; i++)
            {
                var chr = text[i];
                if (char.IsWhiteSpace(chr) || chr is ';')
                {
                    return i;
                }
            }

            return text.Length;
        }
    }

    private static Buffers.Binary.WkbPrimitives.WkbGeometryType GetGeometryType(string name, bool z, bool m)
    {
        var geometryType = (Buffers.Binary.WkbPrimitives.WkbGeometryType)Enum.Parse(typeof(Buffers.Binary.WkbPrimitives.WkbGeometryType), name, ignoreCase: true);
        if (z)
        {
            geometryType += 1000;
        }

        if (m)
        {
            geometryType += 1000;
        }

        return geometryType;
    }
}