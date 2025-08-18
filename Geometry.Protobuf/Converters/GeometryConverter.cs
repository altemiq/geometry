// -----------------------------------------------------------------------
// <copyright file="GeometryConverter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry.Protobuf.Converters;

using System.Text.Json;
using Altemiq.Protobuf.WellKnownTypes;

/// <summary>
/// The <see cref="Altemiq.Geometry.IGeometry"/> <see cref="Google.Protobuf"/> converter.
/// </summary>
public static class GeometryConverter
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions().AddGeoJson();

    /// <summary>
    /// Parses the <see cref="GeometryData"/>.
    /// </summary>
    /// <typeparam name="TGeometry">The type of geometry to parse.</typeparam>
    /// <param name="geometryData">The geometry data.</param>
    /// <returns>The parsed geometry.</returns>
    /// <exception cref="InvalidDataException">The data value invalid.</exception>
    /// <exception cref="InvalidGeometryTypeException">The geometry type was incorrect.</exception>
    public static TGeometry Parse<TGeometry>(GeometryData geometryData)
    {
        return geometryData.DataCase switch
        {
            GeometryData.DataOneofCase.Wkt => ParseWkt(geometryData.Wkt),
            GeometryData.DataOneofCase.Wkb => ParseWkb(geometryData.Wkb),
            GeometryData.DataOneofCase.Ewkb => ParseEwkb(geometryData.Ewkb),
            GeometryData.DataOneofCase.Geojson => ParseGeoJson(geometryData.Geojson),
            _ => throw new InvalidDataException(),
        };

        static TGeometry ParseWkt(string wkt)
        {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
            Span<byte> bytes = stackalloc byte[System.Text.Encoding.UTF8.GetByteCount(wkt)];
            var count = System.Text.Encoding.UTF8.GetBytes(wkt.AsSpan(), bytes);
            ReadOnlySpan<byte> span = bytes[..count];
#else
            ReadOnlySpan<byte> span = System.Text.Encoding.UTF8.GetBytes(wkt);
#endif
            return Buffers.Text.WktParser.TryParse(span, out IGeometry? geometry, out _) && geometry is TGeometry geometryObject
                ? geometryObject
                : throw new InvalidGeometryTypeException();
        }

        static TGeometry ParseWkb(Google.Protobuf.ByteString wkb)
        {
            return Buffers.Binary.WkbPrimitives.ReadGeometry(wkb.Span) is TGeometry geometryObject
                ? geometryObject
                : throw new InvalidGeometryTypeException();
        }

        static TGeometry ParseEwkb(Google.Protobuf.ByteString wkb)
        {
            return Buffers.Binary.EwkbPrimitives.ReadGeometry(wkb.Span) is TGeometry geometryObject
                ? geometryObject
                : throw new InvalidGeometryTypeException();
        }

        static TGeometry ParseGeoJson(string geoJson)
        {
            return JsonSerializer.Deserialize<TGeometry>(geoJson, Options) ?? throw new InvalidDataException();
        }
    }
}