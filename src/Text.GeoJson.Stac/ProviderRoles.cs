// -----------------------------------------------------------------------
// <copyright file="ProviderRoles.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson.Stac;

/// <summary>
/// The provider roles.
/// </summary>
[Flags]
public enum ProviderRoles
{
    /// <summary>
    /// None.
    /// </summary>
    None = 0,

    /// <summary>
    /// The organization that is licensing the dataset under the license specified in the <see cref="Collection"/>'s <see cref="Collection.License"/> field.
    /// </summary>
    Licensor = 1 << 0,

    /// <summary>
    /// The producer of the data is the provider that initially captured and processed the source data, e.g. ESA for Sentinel-2 data.
    /// </summary>
    Producer = 1 << 1,

    /// <summary>
    /// A processor is any provider who processed data to a derived product.
    /// </summary>
    Processor = 1 << 2,

    /// <summary>
    /// The host is the actual provider offering the data on their storage. There should be no more than one host, specified as last element of the list.
    /// </summary>
    Host = 1 << 3,
}