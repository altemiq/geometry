// -----------------------------------------------------------------------
// <copyright file="WktFormat.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geodesy;

/// <summary>
/// The WKT format.
/// </summary>
public enum WktFormat
{
    /// <summary>
    /// No format.
    /// </summary>
    None = default,

    /// <summary>
    /// Well-known text, version 1.
    /// </summary>
    Wkt1 = 1,

    /// <summary>
    /// Well-known text, version 2.
    /// </summary>
    Wkt2 = 2,
}