// -----------------------------------------------------------------------
// <copyright file="Link.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson.Stac;

/// <summary>
/// The link.
/// </summary>
public class Link
{
    /// <summary>
    /// Gets the relation type of the link.
    /// </summary>
    [JsonPropertyName("rel")]
    public required string Relation { get; init; }

    /// <summary>
    /// Gets the media type of the resource.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// Gets the location of the resource.
    /// </summary>
    [JsonPropertyName("href")]
    public required Uri Location { get; init; }

    /// <summary>
    /// Gets the title of the resource.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; init; }

    /// <summary>
    /// Gets the HTTP method that the resource expects.
    /// </summary>
    [JsonPropertyName("method")]
    public string? Method { get; init; }
}