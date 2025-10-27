namespace Altemiq.Text.GeoJson.Stac;

public class CollectionTests
{
    private const string Json = """
                                {
                                  "id": "simple-collection",
                                  "type": "Collection",
                                  "stac_extensions": [
                                    "https://stac-extensions.github.io/eo/v2.0.0/schema.json",
                                    "https://stac-extensions.github.io/projection/v2.0.0/schema.json",
                                    "https://stac-extensions.github.io/view/v1.0.0/schema.json"
                                  ],
                                  "stac_version": "1.1.0",
                                  "description": "A simple collection demonstrating core catalog fields with links to a couple of items",
                                  "title": "Simple Example Collection",
                                  "keywords": [
                                    "simple",
                                    "example",
                                    "collection"
                                  ],
                                  "providers": [
                                    {
                                      "name": "Remote Data, Inc",
                                      "description": "Producers of awesome spatiotemporal assets",
                                      "roles": [
                                        "producer",
                                        "processor"
                                      ],
                                      "url": "http://remotedata.io"
                                    }
                                  ],
                                  "extent": {
                                    "spatial": {
                                      "bbox": [
                                        [
                                          172.91173669923782,
                                          1.3438851951615003,
                                          172.95469614953714,
                                          1.3690476620161975
                                        ]
                                      ]
                                    },
                                    "temporal": {
                                      "interval": [
                                        [
                                          "2020-12-11T22:38:32.125Z",
                                          "2020-12-14T18:02:31.437Z"
                                        ]
                                      ]
                                    }
                                  },
                                  "license": "CC-BY-4.0",
                                  "links": [
                                    {
                                      "rel": "root",
                                      "href": "./collection.json",
                                      "type": "application/json",
                                      "title": "Simple Example Collection"
                                    },
                                    {
                                      "rel": "item",
                                      "href": "./simple-item.json",
                                      "type": "application/geo+json",
                                      "title": "Simple Item"
                                    },
                                    {
                                      "rel": "item",
                                      "href": "./core-item.json",
                                      "type": "application/geo+json",
                                      "title": "Core Item"
                                    },
                                    {
                                      "rel": "item",
                                      "href": "./extended-item.json",
                                      "type": "application/geo+json",
                                      "title": "Extended Item"
                                    },
                                    {
                                      "rel": "self",
                                      "href": "https://raw.githubusercontent.com/radiantearth/stac-spec/v1.1.0/examples/collection.json",
                                      "type": "application/json"
                                    }
                                  ]
                                }
                                """;

    private static readonly Collection Collection = new()
    {
        Id = "simple-collection",
        Extensions =
        [
            "https://stac-extensions.github.io/eo/v2.0.0/schema.json",
            "https://stac-extensions.github.io/projection/v2.0.0/schema.json",
            "https://stac-extensions.github.io/view/v1.0.0/schema.json",
        ],
        Version = "1.1.0",
        Description = "A simple collection demonstrating core catalog fields with links to a couple of items",
        Title = "Simple Example Collection",
        Keywords =
        [
            "simple",
            "example",
            "collection",
        ],
        Providers =
        [
            new()
            {
                Name = "Remote Data, Inc",
                Description = "Producers of awesome spatiotemporal assets",
                Roles = ProviderRoles.Producer | ProviderRoles.Processor,
                Url = new("http://remotedata.io"),
            },
        ],
        Extent = new()
        {
            Spatial = new()
            {
                BoundingBox =
                [
                    [
                        172.91173669923782,
                        1.3438851951615003,
                        172.95469614953714,
                        1.3690476620161975,
                    ],
                ],
            },
            Temporal = new()
            {
                Interval =
                [
                    [
                        new(2020, 12, 11, 22, 38, 32, 125, DateTimeKind.Utc),
                        new(2020, 12, 14, 18, 02, 31, 437, DateTimeKind.Utc),
                    ],
                ],
            },
        },
        License = "CC-BY-4.0",
        Links =
        [
            new()
            {
                Relation = "root",
                Location = Create("./collection.json"),
                Type = "application/json",
                Title = "Simple Example Collection",
            },
            new()
            {
                Relation = "item",
                Location =  Create("./simple-item.json"),
                Type = "application/geo+json",
                Title = "Simple Item",
            },
            new()
            {
                Relation = "item",
                Location =  Create("./core-item.json"),
                Type = "application/geo+json",
                Title = "Core Item",
            },
            new()
            {
                Relation = "item",
                Location = Create( "./extended-item.json"),
                Type = "application/geo+json",
                Title = "Extended Item",
            },
            new()
            {
                Relation = "self",
                Location = Create( "https://raw.githubusercontent.com/radiantearth/stac-spec/v1.1.0/examples/collection.json"),
                Type = "application/json",
            },
        ],
    };

    [Test]
    public async Task Read() => await Assert.That(Serializer.Deserialize<Collection>(Json)).IsEquivalentTo(Collection).IgnoringType<Uri>();

    [Test]
    public async Task Write() => await Assert.That(Serializer.Serialize(Collection)).IsSameJsonAs(Json);

    private static Uri Create(string uriString)
    {
        return Uri.TryCreate(uriString, UriKind.RelativeOrAbsolute, out var uri) ? uri : throw new($"Uri '{uri}' is not a valid URI");
    }
}