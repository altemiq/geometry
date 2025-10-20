// -----------------------------------------------------------------------
// <copyright file="Asset.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson.Stac;

/// <summary>
/// The asset.
/// </summary>
public class Asset
{
    /// <summary>
    /// Gets the link to the asset object.
    /// </summary>
    [JsonPropertyName("href")]
    public required Uri Location { get; init; }

    /// <summary>
    /// Gets the media type of the asset.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Gets the displayed title.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; init; }

    /// <summary>
    /// Gets the multi-line description to explain the asset.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// Gets the purposes of the asset.
    /// </summary>
    [JsonPropertyName("roles")]
    public IReadOnlyList<string>? Roles { get; init; }
}