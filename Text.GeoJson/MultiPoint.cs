// -----------------------------------------------------------------------
// <copyright file="MultiPoint.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

#pragma warning disable SA1402
#pragma warning disable MA0048
/// <summary>
/// The multiple <see cref="Geometry.Point"/>.
/// </summary>
/// <param name="points">The points.</param>
[JsonConverter(typeof(MultiPointConverters.MultiPointConverter))]
public class MultiPoint(IList<Geometry.Point> points) : Geometry.MultiGeometry<Geometry.Point>(points);

/// <summary>
/// The multiple <see cref="Geometry.PointZ"/>.
/// </summary>
/// <param name="points">The points.</param>
[JsonConverter(typeof(MultiPointConverters.MultiPointZConverter))]
public class MultiPointZ(IList<Geometry.PointZ> points) : Geometry.MultiGeometry<Geometry.PointZ>(points);

/// <summary>
/// The multiple <see cref="Geometry.PointM"/>.
/// </summary>
/// <param name="points">The points.</param>
[JsonConverter(typeof(MultiPointConverters.MultiPointMConverter))]
public class MultiPointM(IList<Geometry.PointM> points) : Geometry.MultiGeometry<Geometry.PointM>(points);

/// <summary>
/// The multiple <see cref="Geometry.PointZM"/>.
/// </summary>
/// <param name="points">The points.</param>
[JsonConverter(typeof(MultiPointConverters.MultiPointZMConverter))]
public class MultiPointZM(IList<Geometry.PointZM> points) : Geometry.MultiGeometry<Geometry.PointZM>(points);
#pragma warning restore MA0048, SA1402