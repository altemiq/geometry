// -----------------------------------------------------------------------
// <copyright file="TemporalExtent.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson.Stac;

/// <summary>
/// The temporal exent.
/// </summary>
public class TemporalExtent
{
    /// <summary>
    /// Gets the intervals.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Each outer array element can be a separate temporal extent. The first time interval always describes the overall temporal extent of the data. All subsequent time intervals can be used to provide a more precise description of the extent and identify clusters of data. Clients only interested in the overall extent will only need to access the first item in each array. It is recommended to only use multiple temporal extents if a union of them would then include a large uncovered time span (e.g. only having data for the years 2000, 2010 and 2020).
    /// </para>
    /// </remarks>
    [JsonPropertyName("interval")]
    public required IReadOnlyList<IReadOnlyList<DateTime>> Interval { get; init; }
}