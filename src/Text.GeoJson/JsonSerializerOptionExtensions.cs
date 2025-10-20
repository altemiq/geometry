// -----------------------------------------------------------------------
// <copyright file="JsonSerializerOptionExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130, CheckNamespace
namespace System.Text.Json;
#pragma warning restore IDE0130, CheckNamespace

using Altemiq.Text.GeoJson;

/// <summary>
/// The <see cref="JsonSerializerOptions"/> extensions.
/// </summary>
public static class JsonSerializerOptionExtensions
{
    /// <summary>
    /// Adds <see cref="Altemiq.Text.GeoJson"/> support to the <see cref="JsonSerializerOptions"/> instance.
    /// </summary>
    /// <param name="options">The input options.</param>
    /// <returns>The configured options.</returns>
    public static JsonSerializerOptions AddGeoJson(this JsonSerializerOptions options)
    {
        options.Converters.Add(new PointConverters.PointConverter());
        options.Converters.Add(new PointConverters.PointZConverter());
        options.Converters.Add(new PointConverters.PointMConverter());
        options.Converters.Add(new PointConverters.PointZMConverter());

        options.Converters.Add(new MultiPointConverters.MultiPointConverter());
        options.Converters.Add(new MultiPointConverters.MultiPointZConverter());
        options.Converters.Add(new MultiPointConverters.MultiPointMConverter());
        options.Converters.Add(new MultiPointConverters.MultiPointZMConverter());

        options.Converters.Add(new LineStringConverters.LineStringConverter());
        options.Converters.Add(new LineStringConverters.LineStringZConverter());
        options.Converters.Add(new LineStringConverters.LineStringMConverter());
        options.Converters.Add(new LineStringConverters.LineStringZMConverter());

        options.Converters.Add(new MultiLineStringConverters.MultiLineStringConverter());
        options.Converters.Add(new MultiLineStringConverters.MultiLineStringZConverter());
        options.Converters.Add(new MultiLineStringConverters.MultiLineStringMConverter());
        options.Converters.Add(new MultiLineStringConverters.MultiLineStringZMConverter());

        options.Converters.Add(new PolygonConverters.PolygonConverter());
        options.Converters.Add(new PolygonConverters.PolygonZConverter());
        options.Converters.Add(new PolygonConverters.PolygonMConverter());
        options.Converters.Add(new PolygonConverters.PolygonZMConverter());

        options.Converters.Add(new MultiPolygonConverters.MultiPolygonConverter());
        options.Converters.Add(new MultiPolygonConverters.MultiPolygonZConverter());
        options.Converters.Add(new MultiPolygonConverters.MultiPolygonMConverter());
        options.Converters.Add(new MultiPolygonConverters.MultiPolygonZMConverter());

        options.Converters.Add(new GeometryConverter());

        return options;
    }
}