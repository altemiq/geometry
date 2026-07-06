// -----------------------------------------------------------------------
// <copyright file="MapPolygon.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapInfo;

using Altemiq.Geometry;

/// <summary>
/// The <see cref="MapInfo"/> <see cref="IEnumerable{T}"/> of <see cref="Altemiq.Geometry.Point"/>.
/// </summary>
/// <param name="rings">The rings.</param>
/// <param name="brushId">The brush ID.</param>
/// <param name="label">The label.</param>
/// <param name="envelope">The envelope.</param>
public class MapPolygon(IList<LinearRing<Altemiq.Geometry.Point>> rings, byte brushId, Altemiq.Geometry.Point? label, Envelope envelope) : Polygon(rings)
{
    /// <summary>
    /// Initialises a new instance of the <see cref="MapPolygon"/> class.
    /// </summary>
    /// <param name="rings">The rings.</param>
    /// <param name="brushId">The brush ID.</param>
    /// <param name="label">The label.</param>
    public MapPolygon(IList<LinearRing<Altemiq.Geometry.Point>> rings, byte brushId, Altemiq.Geometry.Point? label)
        : this(rings, brushId, label, CreateEnvelop(rings))
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="MapPolygon"/> class.
    /// </summary>
    /// <param name="polygon">The polygon.</param>
    /// <param name="brushId">The brush ID.</param>
    /// <param name="label">The label.</param>
    public MapPolygon(Polygon polygon, byte brushId, Altemiq.Geometry.Point? label)
        : this((IList<LinearRing<Altemiq.Geometry.Point>>)polygon, brushId, label, CreateEnvelop(polygon))
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="MapPolygon"/> class.
    /// </summary>
    /// <param name="polygon">The polygon.</param>
    /// <param name="brushId">The brush ID.</param>
    /// <param name="label">The label.</param>
    /// <param name="envelope">The envelope.</param>
    public MapPolygon(Polygon polygon, byte brushId, Altemiq.Geometry.Point? label, Envelope envelope)
        : this((IList<LinearRing<Altemiq.Geometry.Point>>)polygon, brushId, label, envelope)
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
    public Envelope Envelope { get; private set; } = envelope;

    /// <inheritdoc/>
    public override LinearRing<Altemiq.Geometry.Point> this[int index]
    {
        get => base[index];
        set
        {
            base[index] = value;
            this.UpdateEnvelope();
        }
    }

    /// <inheritdoc/>
    public override void Add(LinearRing<Altemiq.Geometry.Point> item)
    {
        base.Add(item);
        this.UpdateEnvelope();
    }

    /// <inheritdoc/>
    public override void Clear()
    {
        base.Clear();
        this.UpdateEnvelope();
    }

    /// <inheritdoc/>
    public override void Insert(int index, LinearRing<Altemiq.Geometry.Point> item)
    {
        base.Insert(index, item);
        this.UpdateEnvelope();
    }

    /// <inheritdoc/>
    public override bool Remove(LinearRing<Altemiq.Geometry.Point> item)
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

    private static Envelope CreateEnvelop<T>(ICollection<T> rings)
        where T : LinearRing<Altemiq.Geometry.Point>
    {
        if (rings.Count is 0)
        {
            return default;
        }

        var minX = double.MaxValue;
        var minY = double.MaxValue;
        var maxX = double.MinValue;
        var maxY = double.MinValue;

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

        return new(minX, minY, maxX, maxY);
    }

    private void UpdateEnvelope() => this.Envelope = CreateEnvelop(rings);
}