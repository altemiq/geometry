// -----------------------------------------------------------------------
// <copyright file="StacTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson.Stac;

public class StacTests
{
    private const string Minimal = """
        {
          "type": "FeatureCollection",
          "features": []
        }
        """;

    private const string FullJson = """
        {
          "type": "FeatureCollection",
          "numberMatched": 10,
          "numberReturned": 1,
          "features": [
            {
              "stac_version": "1.0.0",
              "stac_extensions": [],
              "type": "Feature",
              "id": "cs3-20160503_132131_05",
              "bbox": [
                -122.59750209,
                37.48803556,
                -122.2880486,
                37.613537207
              ],
              "geometry": {
                "type": "Polygon",
                "coordinates": [
                  [
                    [
                      -122.308150179,
                      37.488035566
                    ],
                    [
                      -122.597502109,
                      37.538869539
                    ],
                    [
                      -122.576687533,
                       37.613537207
                    ],
                    [
                      -122.288048600,
                      37.562818007
                    ],
                    [
                      -122.308150179,
                      37.488035566
                    ]
                  ]
                ]
              },
              "properties": {
                "datetime": "2016-05-03T13:22:30.040Z",
                "title": "A CS3 item",
                "license": "PDDL-1.0",
                "providers": [
                  {
                    "name": "CoolSat",
                    "roles": [
                      "producer",
                      "licensor"
                    ],
                    "url": "https://stac-api.example.com"
                  }
                ]
              },
              "collection": "cs3",
              "links": [
                {
                  "rel": "self",
                  "type": "application/json",
                  "href": "https://stac-api.example.com/collections/cs3/items/CS3-20160503_132131_05"
                },
                {
                  "rel": "root",
                  "type": "application/json",
                  "href": "https://stac-api.example.com/"
                },
                {
                  "rel": "collection",
                  "type": "application/json",
                  "href": "https://stac-api.example.com/collections/cs3"
                }
              ],
              "assets": {
                "analytic": {
                  "href": "https://stac-api.example.com/catalog/cs3-20160503_132130_04/analytic.tif",
                  "type": "image/tiff; application=geotiff; profile=cloud-optimized",
                  "title": "4-Band Analytic"
                },
                "thumbnail": {
                  "href": "https://stac-api.example.com/catalog/cs3-20160503_132130_04/thumbnail.png",
                  "type": "image/png",
                  "title": "Thumbnail",
                  "roles": [
                    "thumbnail"
                  ]
                }
              }
            }
          ],
          "links": [
            {
              "rel": "root",
              "href": "http://stac.example.com/",
              "type": "application/json"
            }
          ]
        }
        """;

    private static readonly ItemCollection Full = new()
    {
        NumberMatched = 10,
        NumberReturned = 1,
        Features =
        [
            new Item
            {
                Version = "1.0.0",
                Extensions = [],
                Id = "cs3-20160503_132131_05",
                BoundingBox = new Envelope(-122.59750209, 37.48803556, -122.2880486, 37.613537207),
                Geometry = new Polygon(new Point(-122.308150179, 37.488035566), new Point(-122.597502109, 37.538869539), new Point(-122.576687533, 37.613537207), new Point(-122.288048600, 37.562818007), new Point(-122.308150179, 37.488035566)),
                Properties = new Dictionary<string, object?>
                {
                    { "datetime", "2016-05-03T13:22:30.040Z" },
                    { "title", "A CS3 item" },
                    { "license", "PDDL-1.0" },
                    {
                        "providers",
                        new object?[]
                        {
                            new Dictionary<string, object?>
                            {
                                { "name", "CoolSat" },
                                {
                                    "roles",
                                    new object?[]
                                    {
                                        "producer",
                                        "licensor",
                                    }
                                },
                                { "url", "https://stac-api.example.com" },
                            }
                        }
                    }
                },
                Collection = "cs3",
                Links =
                [
                    new()
                    {
                        Relation = "self",
                        Type = "application/json",
                        Location = new("https://stac-api.example.com/collections/cs3/items/CS3-20160503_132131_05"),
                    },
                    new()
                    {
                        Relation = "root",
                        Type = "application/json",
                        Location = new("https://stac-api.example.com/"),
                    },
                    new()
                    {
                        Relation = "collection",
                        Type = "application/json",
                        Location = new("https://stac-api.example.com/collections/cs3"),
                    },
                ],
                Assets = new Dictionary<string, Asset?>
                {
                    {
                        "analytic",
                        new()
                        {
                            Location = new("https://stac-api.example.com/catalog/cs3-20160503_132130_04/analytic.tif"),
                            Type = "image/tiff; application=geotiff; profile=cloud-optimized",
                            Title = "4-Band Analytic",
                        }
                    },
                    {
                        "thumbnail",
                        new()
                        {
                            Location = new("https://stac-api.example.com/catalog/cs3-20160503_132130_04/thumbnail.png"),
                            Type = "image/png",
                            Title = "Thumbnail",
                            Roles =
                            [
                                "thumbnail",
                            ],
                        }
                    },
                }
            },
        ],
        Links =
        [
            new()
            {
                Relation = "root",
                Location = new("http://stac.example.com/"),
                Type = "application/json",
            }
        ],
    };

    [Test]
    public async Task ReadMinimal() => await Assert.That(System.Text.Json.JsonSerializer.Deserialize<ItemCollection>(Minimal)).IsEquivalentTo(new ItemCollection());

    [Test]
    public async Task ReadFull() => await Assert.That(System.Text.Json.JsonSerializer.Deserialize<ItemCollection>(FullJson)).IsEquivalentTo(Full);

    [Test]
    public async Task WriteFull() => await Assert.That(System.Text.Json.JsonSerializer.Serialize(Full)).IsSameJsonAs(FullJson);
}