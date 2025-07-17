// -----------------------------------------------------------------------
// <copyright file="LinearRing{T}.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

/// <summary>
/// Represents a linear ring.
/// </summary>
/// <typeparam name="T">The type of point contained in this linear ring.</typeparam>
public class LinearRing<T> : IList<T>, System.Collections.IList
    where T : struct
{
    private readonly IList<T> points;

    /// <summary>
    /// Initialises a new instance of the <see cref="LinearRing{T}"/> class.
    /// </summary>
    public LinearRing() => this.points = [];

    /// <summary>
    /// Initialises a new instance of the <see cref="LinearRing{T}"/> class.
    /// </summary>
    /// <param name="points">The points.</param>
    public LinearRing(IEnumerable<T> points) => this.points = [.. points];

    /// <inheritdoc cref="System.Collections.ICollection.Count"/>
    public int Count => this.points.Count;

    /// <inheritdoc/>
    public bool IsReadOnly => this.points.IsReadOnly;

    /// <inheritdoc/>
    bool System.Collections.IList.IsFixedSize => ((System.Collections.IList)this.points).IsFixedSize;

    /// <inheritdoc/>
    bool System.Collections.IList.IsReadOnly => this.IsReadOnly;

    /// <inheritdoc/>
    int System.Collections.ICollection.Count => this.Count;

    /// <inheritdoc/>
    bool System.Collections.ICollection.IsSynchronized => false;

    /// <inheritdoc/>
    object System.Collections.ICollection.SyncRoot => this;

    /// <inheritdoc/>
    object? System.Collections.IList.this[int index]
    {
        get => this[index];
        set => ((System.Collections.IList)this.points)[index] = value;
    }

    /// <inheritdoc cref="System.Collections.IList.this"/>
    public T this[int index] { get => this.points[index]; set => this.points[index] = value; }

    /// <inheritdoc/>
    public void Add(T item) => this.points.Add(item);

    /// <inheritdoc/>
    public void Clear() => this.points.Clear();

    /// <inheritdoc/>
    public bool Contains(T item) => this.points.Contains(item);

    /// <inheritdoc/>
    public void CopyTo(T[] array, int arrayIndex) => this.points.CopyTo(array, arrayIndex);

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => this.points.GetEnumerator();

    /// <inheritdoc/>
    public int IndexOf(T item) => this.points.IndexOf(item);

    /// <inheritdoc/>
    public void Insert(int index, T item) => this.points.Insert(index, item);

    /// <inheritdoc/>
    public bool Remove(T item) => this.points.Remove(item);

    /// <inheritdoc/>
    public void RemoveAt(int index) => this.points.RemoveAt(index);

    /// <inheritdoc/>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => ((System.Collections.IEnumerable)this.points).GetEnumerator();

    /// <inheritdoc/>
    int System.Collections.IList.Add(object? value) => ((System.Collections.IList)this.points).Add(value);

    /// <inheritdoc/>
    void System.Collections.IList.Clear() => this.Clear();

    /// <inheritdoc/>
    bool System.Collections.IList.Contains(object? value) => ((System.Collections.IList)this.points).Contains(value);

    /// <inheritdoc/>
    int System.Collections.IList.IndexOf(object? value) => ((System.Collections.IList)this.points).IndexOf(value);

    /// <inheritdoc/>
    void System.Collections.IList.Insert(int index, object? value) => ((System.Collections.IList)this.points).Insert(index, value);

    /// <inheritdoc/>
    void System.Collections.IList.Remove(object? value) => ((System.Collections.IList)this.points).Remove(value);

    /// <inheritdoc/>
    void System.Collections.IList.RemoveAt(int index) => this.RemoveAt(index);

    /// <inheritdoc/>
    void System.Collections.ICollection.CopyTo(Array array, int index) => ((System.Collections.IList)this.points).CopyTo(array, index);

    /// <summary>
    /// Gets the point at the specified index.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>The point.</returns>
    internal (double X, double Y) GetPoint(int index) =>
        this[index] switch
        {
            Point p => (p.X, p.Y),
            PointZ p => (p.X, p.Y),
            PointZM p => (p.X, p.Y),
            _ => throw new InvalidOperationException(),
        };
}