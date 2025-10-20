// -----------------------------------------------------------------------
// <copyright file="MapMultiPolygon.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.MapInfo;

/// <summary>
/// The <see cref="MapInfo"/> <see cref="IEnumerable{T}"/> of <see cref="Altemiq.Geometry.Point"/>.
/// </summary>
/// <param name="polygons">The polygons.</param>
/// <param name="brushId">The pen ID.</param>
/// <param name="label">The label.</param>
/// <param name="envelope">The envelope.</param>
public sealed class MapMultiPolygon(IList<Altemiq.Geometry.Polygon> polygons, byte brushId, Altemiq.Geometry.Point? label, Altemiq.Geometry.Envelope envelope) : Altemiq.Geometry.MultiGeometry<Altemiq.Geometry.Polygon>(polygons)
{
    /// <summary>
    /// Initialises a new instance of the <see cref="MapMultiPolygon"/> class.
    /// </summary>
    /// <param name="polygons">The polygons.</param>
    /// <param name="brushId">The pen ID.</param>
    /// <param name="label">The label.</param>
    public MapMultiPolygon(IList<Altemiq.Geometry.Polygon> polygons, byte brushId, Altemiq.Geometry.Point? label)
        : this(polygons, brushId, label, CreateEnvelop(polygons))
    {
    }

    /// <summary>
    /// Gets the brush ID.
    /// </summary>
    public byte BrushId { get; } = brushId;

    /// <summary>
    /// Gets the label location.
    /// </summary>
    public Altemiq.Geometry.Point? Label { get; } = label;

    /// <summary>
    /// Gets the envelope.
    /// </summary>
    public Altemiq.Geometry.Envelope Envelope { get; private set; } = envelope;

    /// <inheritdoc />
    public override Altemiq.Geometry.Polygon this[int index]
    {
        get => base[index];
        set
        {
            base[index] = value;
            this.UpdateEnvelope();
        }
    }

    /// <inheritdoc/>
    public override void Add(Altemiq.Geometry.Polygon item)
    {
        base.Add(item);
        this.UpdateEnvelope();
    }

    /// <inheritdoc/>
    public override void AddRange(params IEnumerable<Altemiq.Geometry.Polygon> collection)
    {
        base.AddRange(collection);
        this.UpdateEnvelope();
    }

    /// <inheritdoc/>
    public override void Clear()
    {
        base.Clear();
        this.UpdateEnvelope();
    }

    /// <inheritdoc/>
    public override void Insert(int index, Altemiq.Geometry.Polygon item)
    {
        base.Insert(index, item);
        this.UpdateEnvelope();
    }

    /// <inheritdoc/>
    public override bool Remove(Altemiq.Geometry.Polygon item)
    {
        if (!base.Remove(item))
        {
            return false;
        }

        this.UpdateEnvelope();
        return true;
    }

    /// <inheritdoc/>
    public override void RemoveAt(int index)
    {
        base.RemoveAt(index);
        this.UpdateEnvelope();
    }

    /// <summary>
    /// Converts this instance into a <see cref="MapPolygon"/> if <see cref="Altemiq.Geometry.MultiGeometry{T}.Count"/> is 1.
    /// </summary>
    /// <returns>The polygon.</returns>
    /// <exception cref="InvalidOperationException"><see cref="Altemiq.Geometry.MultiGeometry{T}.Count"/> is not 1.</exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:Identifiers should not contain type names", Justification = "This matches the LINQ Single naming")]
    public MapPolygon Single() => this.Count is 1 ? new(this[0], this.BrushId, this.Label, this.Envelope) : throw new InvalidOperationException();

    private static Altemiq.Geometry.Envelope CreateEnvelop(ICollection<Altemiq.Geometry.Polygon> polygons)
    {
        if (polygons.Count is 0)
        {
            return default;
        }

        var minX = double.MaxValue;
        var minY = double.MaxValue;
        var maxX = double.MinValue;
        var maxY = double.MinValue;

        foreach (var rings in polygons)
        {
            foreach (var points in rings)
            {
                foreach (var point in points)
                {
                    if (point.X < minX)
                    {
                        minX = point.X;
                    }

                    if (point.X > maxX)
                    {
                        maxX = point.X;
                    }

                    if (point.Y < minY)
                    {
                        minY = point.Y;
                    }

                    if (point.Y > maxY)
                    {
                        maxY = point.Y;
                    }
                }
            }
        }

        return new(minX, minY, maxX, maxY);
    }

    private void UpdateEnvelope() => this.Envelope = CreateEnvelop(this);
}