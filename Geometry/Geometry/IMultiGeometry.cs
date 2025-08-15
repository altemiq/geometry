// -----------------------------------------------------------------------
// <copyright file="IMultiGeometry.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

/// <summary>
/// Represents a collection of geometry instances.
/// </summary>
/// <typeparam name="T">The type of geometries.</typeparam>
[System.Runtime.CompilerServices.CollectionBuilder(typeof(MultiGeometry), nameof(MultiGeometry.Create))]
public interface IMultiGeometry<out T> : IGeometry, IReadOnlyList<T>
    where T : IGeometry;