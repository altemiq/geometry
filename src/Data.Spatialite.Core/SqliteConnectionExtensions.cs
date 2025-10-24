// -----------------------------------------------------------------------
// <copyright file="SqliteConnectionExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data.Spatialite;

/// <summary>
/// <see cref="Spatialite"/> extensions for <see cref="Microsoft.Data.Sqlite.SqliteConnection"/>.
/// </summary>
public static class SqliteConnectionExtensions
{
    /// <summary>
    /// Loads the <see cref="Spatialite"/> extension.
    /// </summary>
    /// <param name="connection">The connection to load the extension in.</param>
    public static void LoadSpatialiteExtension(this Microsoft.Data.Sqlite.SqliteConnection connection)
    {
        connection.EnableExtensions();
        SpatialiteLoader.Load(connection);
    }
}