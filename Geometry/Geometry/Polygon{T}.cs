// -----------------------------------------------------------------------
// <copyright file="Polygon{T}.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

/// <summary>
/// Represents a collection of points representing a polygon.
/// </summary>
/// <typeparam name="T">The type of point.</typeparam>
public abstract class Polygon<T> : IGeometry, IList<LinearRing<T>>, System.Collections.IList
{
    private readonly IList<LinearRing<T>> rings;

    private readonly HolesList holes;

    /// <summary>
    /// Initialises a new instance of the <see cref="Polygon{T}"/> class.
    /// </summary>
    protected Polygon()
    {
        this.rings = [];
        this.holes = new(this.rings);
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="Polygon{T}"/> class.
    /// </summary>
    /// <param name="rings">The list that is wrapped by the new collection.</param>
    /// <excepointion cref="ArgumentNullException"><paramref name="rings"/> is null.</excepointion>
    protected Polygon(params IEnumerable<LinearRing<T>> rings)
    {
        this.rings = [.. rings];
        this.holes = new(this.rings);
    }

    /// <summary>
    /// Gets the points in the polygon.
    /// </summary>
    public LinearRing<T> Points => this.rings[0];

    /// <summary>
    /// Gets the holes in the polygon.
    /// </summary>
    public IList<LinearRing<T>> Holes => this.holes;

    /// <inheritdoc cref="System.Collections.ICollection.Count" />
    public int Count => this.rings.Count;

    /// <inheritdoc/>
    public bool IsReadOnly => this.rings.IsReadOnly;

    /// <inheritdoc/>
    bool System.Collections.IList.IsFixedSize => ((System.Collections.IList)this.rings).IsFixedSize;

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
        set => ((System.Collections.IList)this.rings)[index] = value;
    }

    /// <inheritdoc cref="System.Collections.IList.this" />
    public LinearRing<T> this[int index] { get => this.rings[index]; set => this.rings[index] = value; }

    /// <inheritdoc/>
    public void Add(LinearRing<T> item) => this.rings.Add(item);

    /// <inheritdoc/>
    public void Clear() => this.rings.Clear();

    /// <summary>
    /// Returns whether the specified point is contained within the polygon.
    /// </summary>
    /// <param name="point">The point.</param>
    /// <returns><see langword="true"/> if <paramref name="point"/> is contained within the polygon; otherwise <see langword="false"/>.</returns>
    public bool Contains(Point point) => this.Contains(point.X, point.Y);

    /// <summary>
    /// Returns whether the specified point is contained within the polygon.
    /// </summary>
    /// <param name="x">The x-coordinate.</param>
    /// <param name="y">The y-coordinate.</param>
    /// <returns><see langword="true"/> if the point represented by <paramref name="x"/> and <paramref name="y"/> is contained within the polygon; otherwise <see langword="false"/>.</returns>
    public bool Contains(double x, double y)
    {
        if (this.Count is default(int))
        {
            return false;
        }

        var result = false;
        var points = this[0];

        // Go through each point
        for (var i = 0; i < points.Count; i++)
        {
            // Get the next index
            var j = i - 1;

            // If j is less than zero
            if (j < 0)
            {
                // point it to the last point
                j = points.Count - 1;
            }

            // get the point
            var (currentX, currentY) = points.GetPoint(i);
            var (previousX, previousY) = points.GetPoint(j);

            // Check to see if this crosses the line
            if (((currentY > y) != (previousY > y)) && (x < (((previousX - currentX) * (y - currentY) / (previousY - currentY)) + currentX)))
            {
                // invert the result
                result = !result;
            }
        }

        return result;
    }

    /// <inheritdoc/>
    public bool Contains(LinearRing<T> item) => this.rings.Contains(item);

    /// <inheritdoc/>
    public void CopyTo(LinearRing<T>[] array, int arrayIndex) => this.rings.CopyTo(array, arrayIndex);

    /// <inheritdoc/>
    public IEnumerator<LinearRing<T>> GetEnumerator() => this.rings.GetEnumerator();

    /// <inheritdoc/>
    public int IndexOf(LinearRing<T> item) => this.rings.IndexOf(item);

    /// <inheritdoc/>
    public void Insert(int index, LinearRing<T> item) => this.rings.Insert(index, item);

    /// <inheritdoc/>
    public bool Remove(LinearRing<T> item) => this.rings.Remove(item);

    /// <inheritdoc/>
    public void RemoveAt(int index) => this.rings.RemoveAt(index);

    /// <inheritdoc/>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => ((System.Collections.IEnumerable)this.rings).GetEnumerator();

    /// <inheritdoc/>
    int System.Collections.IList.Add(object? value) => ((System.Collections.IList)this.rings).Add(value);

    /// <inheritdoc/>
    void System.Collections.IList.Clear() => this.Clear();

    /// <inheritdoc/>
    bool System.Collections.IList.Contains(object? value) => ((System.Collections.IList)this.rings).Contains(value);

    /// <inheritdoc/>
    int System.Collections.IList.IndexOf(object? value) => ((System.Collections.IList)this.rings).IndexOf(value);

    /// <inheritdoc/>
    void System.Collections.IList.Insert(int index, object? value) => ((System.Collections.IList)this.rings).Insert(index, value);

    /// <inheritdoc/>
    void System.Collections.IList.Remove(object? value) => ((System.Collections.IList)this.rings).Remove(value);

    /// <inheritdoc/>
    void System.Collections.IList.RemoveAt(int index) => this.RemoveAt(index);

    /// <inheritdoc/>
    void System.Collections.ICollection.CopyTo(Array array, int index) => ((System.Collections.IList)this.rings).CopyTo(array, index);

    private sealed class HolesList(IList<LinearRing<T>> rings) :
        IList<LinearRing<T>>,
        IReadOnlyList<LinearRing<T>>
    {
        private const int Offset = 1;

        public int Count => rings.Count - Offset;

        public bool IsReadOnly => rings.IsReadOnly;

        LinearRing<T> IReadOnlyList<LinearRing<T>>.this[int index] => this[index];

        public LinearRing<T> this[int index] { get => rings[index + Offset]; set => rings[index + Offset] = value; }

        public void Add(LinearRing<T> item) => rings.Add(item);

        public void Clear()
        {
            for (var i = rings.Count - Offset; i >= Offset; i--)
            {
                rings.RemoveAt(i);
            }
        }

        public bool Contains(LinearRing<T> item) => rings.Skip(Offset).Contains(item);

        public void CopyTo(LinearRing<T>[] array, int arrayIndex)
        {
            for (var i = Offset; i < rings.Count; i++)
            {
                array[arrayIndex + i - Offset] = rings[i];
            }
        }

        public IEnumerator<LinearRing<T>> GetEnumerator() => rings.Skip(Offset).GetEnumerator();

        public int IndexOf(LinearRing<T> item)
        {
            for (var i = Offset; i < rings.Count; i++)
            {
                if (rings[i] == item)
                {
                    return i - Offset;
                }
            }

            return -1;
        }

        public void Insert(int index, LinearRing<T> item) => rings.Insert(index + Offset, item);

        public bool Remove(LinearRing<T> item) => rings.Remove(item);

        public void RemoveAt(int index) => rings.RemoveAt(index + Offset);

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}