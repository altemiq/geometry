// -----------------------------------------------------------------------
// <copyright file="FeatureCollectionTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

public class FeatureCollectionTests
{
    private const string FeatureCollection = """
        {
          "type": "FeatureCollection",
          "features": [
            {
              "type": "Feature",
              "geometry": {
                "type": "Point",
                "coordinates": [ 102.0, 0.5 ]
              },
              "properties": {
                "prop0": "value0"
              }
            },
            {
              "type": "Feature",
              "geometry": {
                "type": "LineString",
                "coordinates": [
                  [ 102.0, 0.0 ],
                  [ 103.0, 1.0 ],
                  [ 104.0, 0.0 ],
                  [ 105.0, 1.0 ]
                ]
              },
              "properties": {
                "prop0": "value0",
                "prop1": 0.0
              }
            },
            {
              "type": "Feature",
              "geometry": {
                "type": "Polygon",
                "coordinates": [
                  [
                    [ 100.0, 0.0 ],
                    [ 101.0, 0.0 ],
                    [ 101.0, 1.0 ],
                    [ 100.0, 1.0 ],
                    [ 100.0, 0.0 ]
                  ]
                ]
              },
              "properties": {
                "prop0": "value0",
                "prop1": {
                  "this": "that"
                }
              }
            }
          ]
        }
        """;

    [Test]
    public async Task ReadFeatureCollection() => await Assert.That(Serializer.Deserialize<FeatureCollection>(FeatureCollection))
        .IsEquivalentTo(new FeatureCollection
        {
            Features =
            [
                new() {
                    Geometry = new Point(102.0, 0.5),
                    Properties = new Dictionary<string, object?> { { "prop0", "value0" } },
                },
                new() {
                    Geometry = new Polyline(new(102, 0), new(103, 1), new(104, 0), new(105, 1)),
                    Properties = new Dictionary<string, object?> { { "prop0", "value0" }, { "prop1", 0M } },
                },
                new() {
                    Geometry = new Polygon([new Point(100.0, 0.0), new (101.0, 0.0), new (101.0, 1.0), new (100.0, 1.0), new (100.0, 0.0)]),
                    Properties = new Dictionary<string, object?>
                    {
                        { "prop0", "value0" },
                        {
                            "prop1",
                            new Dictionary<string, object?>
                            {
                                { "this", "that" },
                            }
                        },
                    },
                },
            ]
        });

    [Test]
    public async Task WriteFeatureCollection() => await Assert.That(Serializer.Serialize(new FeatureCollection
    {
        Features =
        [
            new() {
                Geometry = new Point(102.0, 0.5),
                Properties = new Dictionary<string, object?> { { "prop0", "value0" } },
            },
            new() {
                Geometry = new Polyline(new(102, 0), new(103, 1), new(104, 0), new(105, 1)),
                Properties = new Dictionary<string, object?> { { "prop0", "value0" }, { "prop1", 0D } },
            },
            new() {
                Geometry = new Polygon([new Point(100.0, 0.0), new Point(101.0, 0.0), new Point(101.0, 1.0), new Point(100.0, 1.0), new Point(100.0, 0.0)]),
                Properties = new Dictionary<string, object?>
                {
                    { "prop0", "value0" },
                    {
                        "prop1",
                        new Dictionary<string, object?>
                        {
                            { "this", "that" },
                        }
                    },
                },
            },
        ]
    })).IsSameJsonAs(FeatureCollection);
}