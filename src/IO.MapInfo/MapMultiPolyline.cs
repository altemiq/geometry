// -----------------------------------------------------------------------
// <copyright file="MapMultiPolyline.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapInfo;

/// <summary>
/// The <see cref="MapInfo"/> <see cref="IEnumerable{T}"/> of <see cref="Altemiq.Geometry.Polyline"/>.
/// </summary>
/// <param name="lines">The lines.</param>
/// <param name="penId">The pen ID.</param>
/// <param name="label">The label.</param>
/// <param name="envelope">The envelope.</param>
public sealed class MapMultiPolyline(IList<Altemiq.Geometry.Polyline> lines, byte penId, Altemiq.Geometry.Point label, Altemiq.Geometry.Envelope envelope) : Altemiq.Geometry.MultiGeometry<Altemiq.Geometry.Polyline>(lines)
{
    /// <summary>
    /// Gets the pen ID.
    /// </summary>
    public byte PenId { get; } = penId;

    /// <summary>
    /// Gets the label location.
    /// </summary>
    public Altemiq.Geometry.Point Label { get; } = label;

    /// <summary>
    /// Gets the envelope.
    /// </summary>
    public Altemiq.Geometry.Envelope Envelope { get; private set; } = envelope;

    /// <inheritdoc />
    public override Altemiq.Geometry.Polyline this[int index]
    {
        get => base[index];
        set
        {
            base[index] = value;
            this.UpdateEnvelope();
        }
    }

    /// <summary>
    /// Converts this instance into a <see cref="MapPolygon"/> if <see cref="Altemiq.Geometry.MultiGeometry{T}.Count"/> is 1.
    /// </summary>
    /// <returns>The polyline.</returns>
    /// <exception cref="InvalidOperationException"><see cref="Altemiq.Geometry.MultiGeometry{T}.Count"/> is not 1.</exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:Identifiers should not contain type names", Justification = "This matches the LINQ Single naming")]
    public MapPolyline Single() => this.Count is 1 ? new(this[0], this.PenId, this.Label, this.Envelope) : throw new InvalidOperationException();

    /// <inheritdoc/>
    public override void Add(Altemiq.Geometry.Polyline item)
    {
        base.Add(item);
        this.UpdateEnvelope();
    }

    /// <inheritdoc/>
    public override void AddRange(params IEnumerable<Altemiq.Geometry.Polyline> collection)
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
    public override void Insert(int index, Altemiq.Geometry.Polyline item)
    {
        base.Insert(index, item);
        this.UpdateEnvelope();
    }

    /// <inheritdoc/>
    public override bool Remove(Altemiq.Geometry.Polyline item)
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

    private static Altemiq.Geometry.Envelope CreateEnvelop(MapMultiPolyline lines)
    {
        if (lines.Count is 0)
        {
            return default;
        }

        var minX = double.MaxValue;
        var minY = double.MaxValue;
        var maxX = double.MinValue;
        var maxY = double.MinValue;

        foreach (var line in lines)
        {
            foreach (var point in line)
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

    private void UpdateEnvelope() => this.Envelope = CreateEnvelop(this);
}