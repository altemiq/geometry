// -----------------------------------------------------------------------
// <copyright file="SqliteMetadataCollectionNames.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data.Sqlite;

/// <summary>
/// The <c>SQLite</c> collection names.
/// </summary>
public static class SqliteMetadataCollectionNames
{
    /// <summary>
    /// A constant for use with the <see cref="SqliteConnection.GetSchema(string)"/> or <see cref="SqliteConnection.GetSchema(string, string[])"/> methods method that represents the Columns collection.
    /// </summary>
    public static readonly string Columns = nameof(Columns);

    /// <summary>
    /// A constant for use with the <see cref="SqliteConnection.GetSchema(string)"/> or <see cref="SqliteConnection.GetSchema(string, string[])"/> methods method that represents the Tables collection.
    /// </summary>
    public static readonly string Tables = nameof(Tables);
}