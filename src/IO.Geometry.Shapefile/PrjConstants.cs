// -----------------------------------------------------------------------
// <copyright file="PrjConstants.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

/// <summary>
/// PRJ constants.
/// </summary>
internal static class PrjConstants
{
    /// <summary>
    /// Name of the <see href="https://en.wikipedia.org/wiki/Geographic_coordinate_system">Geographic coordinate system</see> values.
    /// </summary>
    public const string GeographicCoordinateSystems = nameof(GeographicCoordinateSystems);

    /// <summary>
    /// The name of the <see href="https://en.wikipedia.org/wiki/Geographic_coordinate_system">Geographic coordinate system</see> JSON file.
    /// </summary>
    public const string GeogCSJson = "pe_list_geogcs.json";

    /// <summary>
    /// The <see href="https://en.wikipedia.org/wiki/Geographic_coordinate_system">Geographic coordinate system</see> keyword.
    /// </summary>
    public const string GeogCSKeyword = "GEOGCS";

    /// <summary>
    /// Name of the <see href="https://en.wikipedia.org/wiki/Grid_reference_system">Projected coordinate system</see> values.
    /// </summary>
    public const string ProjectedCoordinateSystems = nameof(ProjectedCoordinateSystems);

    /// <summary>
    /// The name of the <see href="https://en.wikipedia.org/wiki/Grid_reference_system">Projected coordinate system</see> JSON file.
    /// </summary>
    public const string ProjCSJson = "pe_list_projcs.json";

    /// <summary>
    /// The <see href="https://en.wikipedia.org/wiki/Grid_reference_system">Projected coordinate system</see> keyword.
    /// </summary>
    public const string ProjCSKeyword = "PROJCS";

    /// <summary>
    /// The well-known ID keyword.
    /// </summary>
    public const string WkIdKeyword = "wkid";

    /// <summary>
    /// The latest well-known ID keyword.
    /// </summary>
    public const string LatestWkIdKeyword = "latestWkid";

    /// <summary>
    /// The name keyword.
    /// </summary>
    public const string NameKeyword = "name";
}