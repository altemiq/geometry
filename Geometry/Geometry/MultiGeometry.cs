// -----------------------------------------------------------------------
// <copyright file="MultiGeometry.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

/// <summary>
/// Helper classes for <see cref="MultiGeometry{T}"/>.
/// </summary>
public static class MultiGeometry
{
    /// <summary>
    /// Creates an instance of <see cref="MultiGeometry{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of geometries.</typeparam>
    /// <param name="values">The values.</param>
    /// <returns>The <see cref="MultiGeometry{T}"/>.</returns>
    public static MultiGeometry<T> Create<T>(ReadOnlySpan<T> values)
        where T : IGeometry =>
        new(new List<T>(values.ToArray()));

    /// <summary>
    /// Returns an empty geometry.
    /// </summary>
    /// <typeparam name="T">The type of geometries.</typeparam>
    /// <returns>An empty geometry.</returns>
    public static MultiGeometry<T> Empty<T>()
        where T : IGeometry => EmptyMultiGeometry<T>.Value;

    private static class EmptyMultiGeometry<T>
        where T : IGeometry
    {
#pragma warning disable CA1825, IDE0300 // this is the implementation of Array.Empty<T>()
        internal static readonly MultiGeometry<T> Value = new(Array.Empty<T>());
#pragma warning restore CA1825, IDE0300
    }
}

#pragma warning disable SA1402
/// <summary>
/// Represents a collection of geometry instances.
/// </summary>
/// <typeparam name="T">The type of geometries.</typeparam>
/// <param name="geometries">The geometries.</param>
[System.Runtime.CompilerServices.CollectionBuilder(typeof(MultiGeometry), nameof(MultiGeometry.Create))]
public class MultiGeometry<T>(IList<T> geometries) : IMultiGeometry<T>, IList<T>
    where T : IGeometry
{
    /// <inheritdoc />
    public int Count => geometries.Count;

    /// <inheritdoc />
    int IReadOnlyCollection<T>.Count => this.Count;

    /// <inheritdoc />
    public bool IsReadOnly => geometries.IsReadOnly;

    /// <inheritdoc cref="IList{T}.this" />
    public T this[int index]
    {
        get => geometries[index];
        set => geometries[index] = value;
    }

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator() => geometries.GetEnumerator();

    /// <inheritdoc />
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => geometries.GetEnumerator();

    /// <inheritdoc />
    public void Add(T item) => geometries.Add(item);

    /// <inheritdoc />
    public void Clear() => geometries.Clear();

    /// <inheritdoc />
    public bool Contains(T item) => geometries.Contains(item);

    /// <inheritdoc />
    public void CopyTo(T[] array, int arrayIndex) => geometries.CopyTo(array, arrayIndex);

    /// <inheritdoc />
    public bool Remove(T item) => geometries.Remove(item);

    /// <inheritdoc />
    public int IndexOf(T item) => geometries.IndexOf(item);

    /// <inheritdoc />
    public void Insert(int index, T item) => geometries.Insert(index, item);

    /// <inheritdoc />
    public void RemoveAt(int index) => geometries.RemoveAt(index);
}
#pragma warning restore SA1402