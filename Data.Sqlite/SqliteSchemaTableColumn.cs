// -----------------------------------------------------------------------
// <copyright file="SqliteSchemaTableColumn.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data.Sqlite;

/// <summary>
/// Describes the column metadata of the schema for a database table.
/// </summary>
public static class SqliteSchemaTableColumn
{
    /// <summary>
    /// Specifies the name of the type of data in the column.
    /// </summary>
    public static readonly string DataTypeName = nameof(DataTypeName);
}