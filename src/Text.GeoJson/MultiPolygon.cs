// -----------------------------------------------------------------------
// <copyright file="MultiPolygon.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

#pragma warning disable SA1402, MA0048
/// <summary>
/// Multiple <see cref="Geometry.Polygon"/>.
/// </summary>
[JsonConverter(typeof(MultiPolygonConverters.MultiPolygonConverter))]
public class MultiPolygon(IList<Geometry.Polygon> lines) : Geometry.MultiGeometry<Geometry.Polygon>(lines);

/// <summary>
/// Multiple <see cref="Geometry.PolygonZ"/>.
/// </summary>
[JsonConverter(typeof(MultiPolygonConverters.MultiPolygonZConverter))]

public class MultiPolygonZ(IList<Geometry.PolygonZ> lines) : Geometry.MultiGeometry<Geometry.PolygonZ>(lines);

/// <summary>
/// Multiple <see cref="Geometry.PolygonM"/>.
/// </summary>
[JsonConverter(typeof(MultiPolygonConverters.MultiPolygonMConverter))]
public class MultiPolygonM(IList<Geometry.PolygonM> lines) : Geometry.MultiGeometry<Geometry.PolygonM>(lines);

/// <summary>
/// Multiple <see cref="Geometry.PolygonZM"/>.
/// </summary>
[JsonConverter(typeof(MultiPolygonConverters.MultiPolygonZMConverter))]
public class MultiPolygonZM(IList<Geometry.PolygonZM> lines) : Geometry.MultiGeometry<Geometry.PolygonZM>(lines);
#pragma warning restore MA0048, SA1402