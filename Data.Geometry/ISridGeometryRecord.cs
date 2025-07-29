// -----------------------------------------------------------------------
// <copyright file="ISridGeometryRecord.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data;

/// <summary>
/// Represents an <see cref="IGeometryRecord"/> that has a spatial reference ID.
/// </summary>
public interface ISridGeometryRecord : IGeometryRecord
{
    /// <summary>
    /// Gets the spatial reference ID.
    /// </summary>
    /// <returns>The spatial reference ID.</returns>
    public int GetSrid();
}