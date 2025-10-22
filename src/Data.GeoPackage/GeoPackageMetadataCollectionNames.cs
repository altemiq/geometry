// -----------------------------------------------------------------------
// <copyright file="GeoPackageMetadataCollectionNames.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data.GeoPackage;

/// <summary>
/// The <c>GeoPackage</c> collection names.
/// </summary>
public static class GeoPackageMetadataCollectionNames
{
    /// <summary>
    /// A constant for use with the <see cref="Microsoft.Data.Sqlite.SqliteConnection.GetSchema(string)"/> or <see cref="GeoPackageConnection.GetSchema(string, string[])"/> methods method that represents the Contents collection.
    /// </summary>
    public static readonly string Contents = nameof(Contents);

    /// <summary>
    /// A constant for use with the <see cref="Microsoft.Data.Sqlite.SqliteConnection.GetSchema(string)"/> or <see cref="GeoPackageConnection.GetSchema(string, string[])"/> methods method that represents the GeometryColumns collection.
    /// </summary>
    public static readonly string GeometryColumns = nameof(GeometryColumns);

    /// <summary>
    /// A constant for use with the <see cref="Microsoft.Data.Sqlite.SqliteConnection.GetSchema(string)"/> or <see cref="GeoPackageConnection.GetSchema(string, string[])"/> methods method that represents the SpatialReferenceSystems collection.
    /// </summary>
    public static readonly string SpatialReferenceSystems = nameof(SpatialReferenceSystems);
}