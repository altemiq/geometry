// -----------------------------------------------------------------------
// <copyright file="InitSpatialMetadataMode.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data.Spatialite;

/// <summary>
/// The init spatial metadata mode.
/// </summary>
public enum InitSpatialMetadataMode
{
    /// <summary>
    /// No EPSG SRID will be inserted.
    /// </summary>
    None = 0,

    /// <summary>
    /// Only WGS84-related EPSG SRIDs will be inserted.
    /// </summary>
    Wgs84 = 1,

    /// <summary>
    /// All possible ESPG SRID definitions will be inserted.
    /// </summary>
    All = 2,
}