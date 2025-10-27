// -----------------------------------------------------------------------
// <copyright file="IStacBase.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson.Stac;

/// <summary>
/// The STAC base.
/// </summary>
public interface IStacBase
{
    /// <summary>
    /// Gets the unique ID for this.
    /// </summary>
    string Id { get; init; }

    /// <summary>
    /// Gets the STAC version the <see cref="IStacBase"/> implements.
    /// </summary>
    string Version { get; init; }

    /// <summary>
    /// Gets the list of extension identifiers the <see cref="IStacBase"/> implements.
    /// </summary>
    IReadOnlyList<string?>? Extensions { get; init; }

    /// <summary>
    /// Gets the list of references to other documents.
    /// </summary>
    IReadOnlyList<Link> Links { get; init; }
}