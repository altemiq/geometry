namespace Altemiq.Text.GeoJson.Stac;

public class CatalogTests
{
    private const string Json = """
                                {
                                  "id": "examples",
                                  "type": "Catalog",
                                  "title": "Example Catalog",
                                  "stac_version": "1.1.0",
                                  "description": "This catalog is a simple demonstration of an example catalog that is used to organize a hierarchy of collections and their items.",
                                  "links": [
                                    {
                                      "rel": "root",
                                      "href": "./catalog.json",
                                      "type": "application/json"
                                    },
                                    {
                                      "rel": "child",
                                      "href": "./extensions-collection/collection.json",
                                      "type": "application/json",
                                      "title": "Collection Demonstrating STAC Extensions"
                                    },
                                    {
                                      "rel": "child",
                                      "href": "./collection-only/collection.json",
                                      "type": "application/json",
                                      "title": "Collection with no items (standalone)"
                                    },
                                    {
                                      "rel": "child",
                                      "href": "./collection-only/collection-with-schemas.json",
                                      "type": "application/json",
                                      "title": "Collection with no items (standalone with JSON Schemas)"
                                    },
                                    {
                                      "rel": "item",
                                      "href": "./collectionless-item.json",
                                      "type": "application/json",
                                      "title": "Item that does not have a collection (not recommended, but allowed by the spec)"
                                    },
                                    {
                                      "rel": "self",
                                      "href": "https://raw.githubusercontent.com/radiantearth/stac-spec/v1.1.0/examples/catalog.json",
                                      "type": "application/json"
                                    }
                                  ]
                                }
                                """;

    private static readonly Catalog Catalog = new()
    {
        Id = "examples",
        Title = "Example Catalog",
        Version = "1.1.0",
        Description = "This catalog is a simple demonstration of an example catalog that is used to organize a hierarchy of collections and their items.",
        Links =
        [
            new() { Relation = "root", Location = Create("./catalog.json"), Type = "application/json" },
            new()
            {
                Relation = "child",
                Location = Create("./extensions-collection/collection.json"),
                Type = "application/json",
                Title = "Collection Demonstrating STAC Extensions",
            },
            new()
            {
                Relation = "child",
                Location = Create("./collection-only/collection.json"),
                Type = "application/json",
                Title = "Collection with no items (standalone)",
            },
            new()
            {
                Relation = "child",
                Location = Create("./collection-only/collection-with-schemas.json"),
                Type = "application/json",
                Title = "Collection with no items (standalone with JSON Schemas)",
            },
            new()
            {
                Relation = "item",
                Location = Create("./collectionless-item.json"),
                Type = "application/json",
                Title = "Item that does not have a collection (not recommended, but allowed by the spec)",
            },
            new()
            {
                Relation = "self",
                Location = Create("https://raw.githubusercontent.com/radiantearth/stac-spec/v1.1.0/examples/catalog.json"),
                Type = "application/json",
            },
        ],
    };

    [Test]
    public async Task Read() => await Assert.That(Serializer.Deserialize<Catalog>(Json)).IsEquivalentTo(Catalog).IgnoringType<Uri>();

    [Test]
    public async Task Write() => await Assert.That(Serializer.Serialize(Catalog)).IsSameJsonAs(Json);

    private static Uri Create(string uriString)
    {
        return Uri.TryCreate(uriString, UriKind.RelativeOrAbsolute, out var uri) ? uri : throw new($"Uri '{uri}' is not a valid URI");
    }
}