// -----------------------------------------------------------------------
// <copyright file="ShpLinearRing.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

/// <summary>
/// The SHP linear ring.
/// </summary>
/// <typeparam name="T">The type of point contained in this linear ring.</typeparam>
public class ShpLinearRing<T> : LinearRing<T>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="ShpLinearRing{T}"/> class.
    /// </summary>
    /// <param name="partType">The part type.</param>
    public ShpLinearRing(ShpPartType partType) => this.PartType = partType;

    /// <summary>
    /// Initialises a new instance of the <see cref="ShpLinearRing{T}"/> class.
    /// </summary>
    /// <param name="partType">The part type.</param>
    /// <param name="points">The points.</param>
    public ShpLinearRing(ShpPartType partType, IEnumerable<T> points)
        : base(points) => this.PartType = partType;

    /// <summary>
    /// Gets the part type.
    /// </summary>
    public ShpPartType PartType { get; }
}