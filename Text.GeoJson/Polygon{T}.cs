// -----------------------------------------------------------------------
// <copyright file="Polygon{T}.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

/// <summary>
/// The <see cref="IGeometry"/> <see cref="Altemiq.Geometry.Polygon{T}"/>.
/// </summary>
/// <typeparam name="T">The type of point.</typeparam>
public abstract class Polygon<T> : Altemiq.Geometry.Polygon<T>, IGeometry
{
    /// <summary>
    /// Initialises a new instance of the <see cref="Polygon{T}"/> class.
    /// </summary>
    /// <param name="points">The single ring that is wrapped by the new collection.</param>
    protected Polygon(params T[] points)
        : base([.. points])
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="Polygon{T}"/> class.
    /// </summary>
    /// <param name="rings">The list that is wrapped by the new collection.</param>
    protected Polygon(params Geometry.LinearRing<T>[] rings)
        : base(rings)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="Polygon{T}"/> class.
    /// </summary>
    /// <param name="points">The single ring that is wrapped by the new collection.</param>
    protected Polygon(IEnumerable<T> points)
        : base([.. points])
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="Polygon{T}"/> class.
    /// </summary>
    /// <param name="polygon">The polygon.</param>
    protected Polygon(Polygon<T> polygon)
        : base(Concat(polygon.Points, polygon.Holes))
    {
    }

    /// <inheritdoc/>
    public override string ToString() => JsonSerializer.Serialize(this);

    private static IEnumerable<Geometry.LinearRing<T>> Concat(IEnumerable<T> points, IEnumerable<Geometry.LinearRing<T>> holes)
    {
        yield return new(points);
        foreach (var hole in holes)
        {
            yield return hole;
        }
    }
}