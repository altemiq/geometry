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
#if NET6_0_OR_GREATER
    [System.Diagnostics.CodeAnalysis.RequiresDynamicCode("The native code for this instantiation might not be available at runtime.")]
#endif
    public static MultiGeometry<T> Create<T>(ReadOnlySpan<T> values)
        where T : IGeometry
    {
        IList<T> list = [.. values];
        if (typeof(IGeometryZ).IsAssignableFrom(typeof(T)))
        {
            if (typeof(IGeometryM).IsAssignableFrom(typeof(T)))
            {
                return (MultiGeometry<T>)Activator.CreateInstance(typeof(MultiGeometryZM<>).MakeGenericType(typeof(T)), list)!;
            }

            return (MultiGeometry<T>)Activator.CreateInstance(typeof(MultiGeometryZ<>).MakeGenericType(typeof(T)), list)!;
        }

        if (typeof(IGeometryM).IsAssignableFrom(typeof(T)))
        {
            return (MultiGeometry<T>)Activator.CreateInstance(typeof(MultiGeometryM<>).MakeGenericType(typeof(T)), list)!;
        }

        return new(list);
    }

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
        internal static readonly MultiGeometry<T> Value = [.. Array.Empty<T>()];
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
    public virtual T this[int index]
    {
        get => geometries[index];
        set => geometries[index] = value;
    }

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator() => geometries.GetEnumerator();

    /// <inheritdoc />
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => geometries.GetEnumerator();

    /// <inheritdoc />
    public virtual void Add(T item) => geometries.Add(item);

    /// <inheritdoc />
    public virtual void Clear() => geometries.Clear();

    /// <inheritdoc />
    public bool Contains(T item) => geometries.Contains(item);

    /// <inheritdoc />
    public void CopyTo(T[] array, int arrayIndex) => geometries.CopyTo(array, arrayIndex);

    /// <inheritdoc />
    public virtual bool Remove(T item) => geometries.Remove(item);

    /// <inheritdoc />
    public int IndexOf(T item) => geometries.IndexOf(item);

    /// <inheritdoc />
    public virtual void Insert(int index, T item) => geometries.Insert(index, item);

    /// <inheritdoc />
    public virtual void RemoveAt(int index) => geometries.RemoveAt(index);

    /// <inheritdoc cref="System.Collections.Generic.List{T}.AddRange" />
    public virtual void AddRange(params IEnumerable<T> collection)
    {
        if (geometries is List<T> list)
        {
            list.AddRange(collection);
        }
        else
        {
            foreach (var item in collection)
            {
                geometries.Add(item);
            }
        }
    }

    /// <inheritdoc />
    double IGeometry.MinX() => this.Min(x => x.MinX());

    /// <inheritdoc />
    double IGeometry.MaxX() => this.Max(x => x.MaxX());

    /// <inheritdoc />
    double IGeometry.MinY() => this.Min(x => x.MinY());

    /// <inheritdoc />
    double IGeometry.MaxY() => this.Max(x => x.MaxY());
}

/// <summary>
/// Represents a collection of geometry instances.
/// </summary>
/// <typeparam name="T">The type of geometries.</typeparam>
/// <param name="geometries">The geometries.</param>
[System.Runtime.CompilerServices.CollectionBuilder(typeof(MultiGeometry), nameof(MultiGeometry.Create))]
public class MultiGeometryM<T>(IList<T> geometries) : MultiGeometry<T>(geometries), IGeometryM
    where T : IGeometryM
{
    /// <inheritdoc />
    double IGeometryM.MinM() => this.Min(x => x.MinM());

    /// <inheritdoc />
    double IGeometryM.MaxM() => this.Max(x => x.MaxM());
}

/// <summary>
/// Represents a collection of geometry instances.
/// </summary>
/// <typeparam name="T">The type of geometries.</typeparam>
/// <param name="geometries">The geometries.</param>
[System.Runtime.CompilerServices.CollectionBuilder(typeof(MultiGeometry), nameof(MultiGeometry.Create))]
public class MultiGeometryZ<T>(IList<T> geometries) : MultiGeometry<T>(geometries), IGeometryZ
    where T : IGeometryZ
{
    /// <inheritdoc />
    double IGeometryZ.MinZ() => this.Min(x => x.MinZ());

    /// <inheritdoc />
    double IGeometryZ.MaxZ() => this.Max(x => x.MaxZ());
}

/// <summary>
/// Represents a collection of geometry instances.
/// </summary>
/// <typeparam name="T">The type of geometries.</typeparam>
/// <param name="geometries">The geometries.</param>
[System.Runtime.CompilerServices.CollectionBuilder(typeof(MultiGeometry), nameof(MultiGeometry.Create))]
public class MultiGeometryZM<T>(IList<T> geometries) : MultiGeometry<T>(geometries), IGeometryZ, IGeometryM
    where T : IGeometryZ, IGeometryM
{
    /// <inheritdoc />
    double IGeometryZ.MinZ() => this.Min(x => x.MinZ());

    /// <inheritdoc />
    double IGeometryZ.MaxZ() => this.Max(x => x.MaxZ());

    /// <inheritdoc />
    double IGeometryM.MinM() => this.Min(x => x.MinM());

    /// <inheritdoc />
    double IGeometryM.MaxM() => this.Max(x => x.MaxM());
}
#pragma warning restore SA1402