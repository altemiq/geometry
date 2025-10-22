// -----------------------------------------------------------------------
// <copyright file="IGeometryDataReader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data;

/// <summary>
/// Provides access to the geometry value within each row for a DataReader.
/// </summary>
public interface IGeometryDataReader : System.Data.IDataReader, IGeometryDataRecord;