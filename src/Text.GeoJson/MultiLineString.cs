// -----------------------------------------------------------------------
// <copyright file="MultiLineString.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

#pragma warning disable SA1402, MA0048
/// <summary>
/// Multiple <see cref="Geometry.Polyline"/>.
/// </summary>
[JsonConverter(typeof(MultiLineStringConverters.MultiLineStringConverter))]
public class MultiLineString(IList<Geometry.Polyline> lines) : Geometry.MultiGeometry<Geometry.Polyline>(lines);

/// <summary>
/// Multiple <see cref="Geometry.PolylineZ"/>.
/// </summary>
[JsonConverter(typeof(MultiLineStringConverters.MultiLineStringZConverter))]

public class MultiLineStringZ(IList<Geometry.PolylineZ> lines) : Geometry.MultiGeometry<Geometry.PolylineZ>(lines);

/// <summary>
/// Multiple <see cref="Geometry.PolylineM"/>.
/// </summary>
[JsonConverter(typeof(MultiLineStringConverters.MultiLineStringMConverter))]
public class MultiLineStringM(IList<Geometry.PolylineM> lines) : Geometry.MultiGeometry<Geometry.PolylineM>(lines);

/// <summary>
/// Multiple <see cref="Geometry.PolylineZM"/>.
/// </summary>
[JsonConverter(typeof(MultiLineStringConverters.MultiLineStringZMConverter))]
public class MultiLineStringZM(IList<Geometry.PolylineZM> lines) : Geometry.MultiGeometry<Geometry.PolylineZM>(lines);
#pragma warning restore MA0048, SA1402