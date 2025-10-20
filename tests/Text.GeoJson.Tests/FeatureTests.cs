// -----------------------------------------------------------------------
// <copyright file="FeatureTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

public class FeatureTests
{
    private const string Feature = """
        {
          "type": "Feature",
          "geometry": {
            "type": "Point",
            "coordinates": [ 102.0, 0.5 ]
          },
          "properties": {
            "prop0": "value0"
          }
        }
        """;

    private const string FeatureDictionary = """
        {
          "type": "Feature",
          "geometry": {
            "type": "Point",
            "coordinates": [
              10.0,
              10.0
            ]
          },
          "properties": {
            "BooleanProperty": true,
            "DoubleProperty": 1.2345,
            "EnumProperty": 1,
            "IntProperty": -1,
            "StringProperty": "Hello, GeoJSON !"
          }
        }
        """;

    private const string FeatureWithNullGeometry = """
        {
          "type": "Feature",
          "geometry": null,
          "properties": {
            "prop0": "value0"
          }
        }
        """;

    private const string FeatureWithStringId = """
        {
          "type": "Feature",
          "id": "my_id",
          "geometry": {
            "type": "Point",
            "coordinates": [ 102.0, 0.5 ]
          },
          "properties": {
            "prop0": "value0"
          }
        }
        """;

    private const string FeatureWithIntId = """
        {
          "type": "Feature",
          "id": 123,
          "geometry": {
            "type": "Point",
            "coordinates": [ 102.0, 0.5 ]
          },
          "properties": {
            "prop0": "value0"
          }
        }
        """;

    private const string FeatureWithBoundingBox = """
        {
          "type": "Feature",
          "bbox": [ -10.0, -10.0, 10.0, 10.0 ],
          "geometry": {
            "type": "Point",
            "coordinates": [ 102.0, 0.5 ]
          },
          "properties": {
            "prop0": "value0"
          }
        }
        """;

    private const string FeatureWithExtraValue = """
        {
          "type": "Feature",
          "geometry": null,
          "properties": {
            "prop0": "value0"
          },
          "extra": "value0"
        }
        """;

    private const string FeatureWithExtraObject = """
        {
          "type": "Feature",
          "geometry": null,
          "properties": {
            "prop0": "value0"
          },
          "extra": {
            "extra0": "value0"
          }
        }
        """;

    private const string FeatureWithExtraArray = """
        {
          "type": "Feature",
          "geometry": null,
          "properties": {
            "prop0": "value0"
          },
          "extra": [
            "value0",
            "value1"
          ]
        }
        """;

    [Test]
    public async Task ReadFeature()
    {
        var feature = Serializer.Deserialize<Feature>(Feature);
        _ = await Assert.That(feature).IsNotNull().And
            .Satisfies(f => f.Geometry, geometry => geometry.IsEquivalentTo(new Point(102D, 0.5))).And
            .Satisfies(f => f.Properties, properties => properties.IsEquivalentTo(new Dictionary<string, object> { { "prop0", "value0" } }));
    }

    [Test]
    public async Task WriteFeature() => await Assert.That(Serializer.Serialize(new Feature
    {
        Geometry = new Geometry.Point(102D, 0.5),
        Properties = new Dictionary<string, object?> { { "prop0", "value0" } },
    })).IsSameJsonAs(Feature);

    [Test]
    public async Task WriteFeatureDictionary() => await Assert.That(Serializer.Serialize(new Feature
    {
        Geometry = new Geometry.Point(10, 10),
        Properties = new Dictionary<string, object?>
        {
            { "BooleanProperty", true },
            { "DoubleProperty", 1.2345 },
            { "EnumProperty", TestFeatureEnum.Value1 },
            { "IntProperty", -1 },
            { "StringProperty", "Hello, GeoJSON !" },
        },
    })).IsSameJsonAs(FeatureDictionary);

    [Test]
    public async Task ReadFeatureWithNullGeometry() => await Assert.That(Serializer.Deserialize<Feature>(FeatureWithNullGeometry))
        .IsNotNull()
        .And.IsTypeOf<Feature>().And.Satisfies(f => f.Geometry, geometry => geometry.IsNull());

    [Test]
    public async Task WriteFeatureWithNullGeometry() => await Assert.That(Serializer.Serialize(new Feature
    {
        Geometry = default,
        Properties = new Dictionary<string, object?> { { "prop0", "value0" } }
    })).IsSameJsonAs(FeatureWithNullGeometry);

    [Test]
    public async Task ReadFeatureWithStringId() => await Assert.That(Serializer.Deserialize<Feature>(FeatureWithStringId))
        .IsEquivalentTo(new Feature
        {
            Id = "my_id",
            Geometry = new Point(102.0, 0.5),
            Properties = new Dictionary<string, object?> { { "prop0", "value0" } }
        });

    [Test]
    public async Task WriteFeatureWithStringId() => await Assert.That(Serializer.Serialize(new Feature
    {
        Id = "my_id",
        Geometry = new Point(102.0, 0.5),
        Properties = new Dictionary<string, object?> { { "prop0", "value0" } }
    })).IsSameJsonAs(FeatureWithStringId);

    [Test]
    public async Task ReadFeatureWithIntId() => await Assert.That(Serializer.Deserialize<Feature>(FeatureWithIntId))
        .IsEquivalentTo(new Feature
        {
            Id = 123,
            Geometry = new Point(102.0, 0.5),
            Properties = new Dictionary<string, object?> { { "prop0", "value0" } }
        });

    [Test]
    public async Task WriteFeatureWithIntId() => await Assert.That(Serializer.Serialize(new Feature
    {
        Id = 123,
        Geometry = new Point(102.0, 0.5),
        Properties = new Dictionary<string, object?> { { "prop0", "value0" } }
    })).IsSameJsonAs(FeatureWithIntId);

    [Test]
    public async Task ReadFeatureWithBoundingBox() => await Assert.That(Serializer.Deserialize<Feature>(FeatureWithBoundingBox))
        .IsEquivalentTo(new Feature
        {
            BoundingBox = new Envelope(-10.0, -10.0, 10.0, 10.0),
            Geometry = new Point(102.0, 0.5),
            Properties = new Dictionary<string, object?> { { "prop0", "value0" } }
        });

    [Test]
    public async Task WriteFeatureWithBoundingBox() => await Assert.That(Serializer.Serialize(new Feature
    {
        BoundingBox = new Envelope(-10.0, -10.0, 10.0, 10.0),
        Geometry = new Point(102.0, 0.5),
        Properties = new Dictionary<string, object?> { { "prop0", "value0" } }
    })).IsSameJsonAs(FeatureWithBoundingBox);

    [Test]
    [Arguments(FeatureWithExtraValue)]
    [Arguments(FeatureWithExtraObject)]
    [Arguments(FeatureWithExtraArray)]
    public async Task ReadExtra(string json) => await Assert.That(Serializer.Deserialize<Feature>(json)).IsNotNull();
}