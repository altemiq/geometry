// -----------------------------------------------------------------------
// <copyright file="SpatialiteCommand.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data.Spatialite;

/// <summary>
/// A <c>SpatiaLite</c> <see cref="Microsoft.Data.Sqlite.SqliteCommand"/>.
/// </summary>
public class SpatialiteCommand : Microsoft.Data.Sqlite.SqliteCommand
{
    /// <summary>
    /// Executes the <see cref="Microsoft.Data.Sqlite.SqliteCommand.CommandText" /> against the database and returns a data reader.
    /// </summary>
    /// <returns>The data reader.</returns>
    public new SpatialiteDataReader ExecuteReader() => new(base.ExecuteReader());

    /// <summary>
    /// Executes the <see cref="Microsoft.Data.Sqlite.SqliteCommand.CommandText" /> against the database and returns a data reader.
    /// </summary>
    /// <param name="behavior">A description of the results of the query and its effect on the database.</param>
    /// <returns>The data reader.</returns>
    public new SpatialiteDataReader ExecuteReader(System.Data.CommandBehavior behavior) => new(base.ExecuteReader(behavior));

    /// <summary>
    /// Executes the <see cref="Microsoft.Data.Sqlite.SqliteCommand.CommandText" /> against the database and returns a data reader.
    /// </summary>
    /// <returns>The data reader.</returns>
    public new async Task<SpatialiteDataReader> ExecuteReaderAsync() => new(await base.ExecuteReaderAsync().ConfigureAwait(false));

    /// <summary>
    /// Executes the <see cref="Microsoft.Data.Sqlite.SqliteCommand.CommandText" /> against the database and returns a data reader.
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The data reader.</returns>
    public new async Task<SpatialiteDataReader> ExecuteReaderAsync(CancellationToken cancellationToken) => new(await base.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false));

    /// <summary>
    /// Executes the <see cref="Microsoft.Data.Sqlite.SqliteCommand.CommandText" /> against the database and returns a data reader.
    /// </summary>
    /// <param name="behavior">A description of the results of the query and its effect on the database.</param>
    /// <returns>The data reader.</returns>
    public new async Task<SpatialiteDataReader> ExecuteReaderAsync(System.Data.CommandBehavior behavior) => new(await base.ExecuteReaderAsync(behavior).ConfigureAwait(false));

    /// <summary>
    /// Executes the <see cref="Microsoft.Data.Sqlite.SqliteCommand.CommandText" /> against the database and returns a data reader.
    /// </summary>
    /// <param name="behavior">A description of the results of the query and its effect on the database.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The data reader.</returns>
    public new async Task<SpatialiteDataReader> ExecuteReaderAsync(System.Data.CommandBehavior behavior, CancellationToken cancellationToken) => new(await base.ExecuteReaderAsync(behavior, cancellationToken).ConfigureAwait(false));
}