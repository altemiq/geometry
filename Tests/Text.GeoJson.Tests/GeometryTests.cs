// -----------------------------------------------------------------------
// <copyright file="GeometryTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

public class GeometryTests
{
    private const string Point = """
                                 {
                                   "type": "Point",
                                   "coordinates": [ 102.0, 0.5 ]
                                 }
                                 """;

    private const string MultiPoint = """
                                      {
                                        "type": "MultiPoint",
                                        "coordinates": [
                                          [ 102.0, 1.5 ],
                                          [ 103.0, 2.5 ],
                                          [ 101.0, 1.0 ],
                                          [ 100.0, 2.5 ],
                                          [ 101.0, 1.5 ]
                                        ]
                                      }
                                      """;

    private const string LineString = """
                                      {
                                        "type": "LineString",
                                        "coordinates": [
                                          [ 102.0, 0.0 ],
                                          [ 103.0, 1.0 ],
                                          [ 104.0, 0.0 ],
                                          [ 105.0, 1.0 ]
                                        ]
                                      }
                                      """;

    private const string MultiLineString = """
                                           {
                                             "type": "MultiLineString",
                                             "coordinates": [
                                               [
                                                 [ 170.0, 45.0 ],
                                                 [ 180.0, 45.0 ]
                                               ],
                                               [
                                                 [ -180.0, 45.0 ],
                                                 [ -170.0, 45.0 ]
                                               ]
                                             ]
                                           }
                                           """;

    private const string Polygon = """
                                   {
                                     "type": "Polygon",
                                     "coordinates": [
                                       [
                                         [ -10.0, -10.0 ],
                                         [ 10.0, -10.0 ],
                                         [ 10.0, 10.0 ],
                                         [ -10.0, -10.0 ]
                                       ]
                                     ]
                                   }
                                   """;

    private const string PolygonWithHole = """
                                           {
                                             "type": "Polygon",
                                             "coordinates": [
                                               [
                                                 [ 100.0, 0.0 ],
                                                 [ 101.0, 0.0 ],
                                                 [ 101.0, 1.0 ],
                                                 [ 100.0, 1.0 ],
                                                 [ 100.0, 0.0 ]
                                               ],
                                               [
                                                 [ 100.8, 0.8 ],
                                                 [ 100.8, 0.2 ],
                                                 [ 100.2, 0.2 ],
                                                 [ 100.2, 0.8 ],
                                                 [ 100.8, 0.8 ]
                                               ]
                                             ]
                                           }
                                           """;

    private const string MultiPolygon = """
                                        {
                                          "type": "MultiPolygon",
                                          "coordinates": [
                                            [
                                              [
                                                [ 180.0, 40.0 ],
                                                [ 180.0, 50.0 ],
                                                [ 170.0, 50.0 ],
                                                [ 170.0, 40.0 ],
                                                [ 180.0, 40.0 ]
                                              ]
                                            ],
                                            [
                                              [
                                                [ -170.0, 40.0 ],
                                                [ -170.0, 50.0 ],
                                                [ -180.0, 50.0 ],
                                                [ -180.0, 40.0 ],
                                                [ -170.0, 40.0 ]
                                              ]
                                            ]
                                          ]
                                        }
                                        """;

    [Test]
    public async Task ReadPoint() => await Assert.That(System.Text.Json.JsonSerializer.Deserialize<Point>(Point)).IsEquivalentTo(new Point(102, 0.5));

    [Test]
    public async Task WritePoint() => await Assert.That(System.Text.Json.JsonSerializer.Serialize(new Point(102, 0.5))).IsSameJsonAs(Point);

    [Test]
    public async Task ReadMultiPoint() => await Assert.That(System.Text.Json.JsonSerializer.Deserialize<MultiPoint>(MultiPoint)).IsEquivalentTo(new MultiPoint(
    [
        new(102.0, 1.5),
        new(103.0, 2.5),
        new(101.0, 1.0),
        new(100.0, 2.5),
        new(101.0, 1.5)
    ]));

    [Test]
    public async Task WriteMultiPoint() => await Assert.That(System.Text.Json.JsonSerializer.Serialize(new MultiPoint(
    [
        new(102.0, 1.5),
        new(103.0, 2.5),
        new(101.0, 1.0),
        new(100.0, 2.5),
        new(101.0, 1.5)
    ]))).IsSameJsonAs(MultiPoint);

    [Test]
    public async Task ReadLineString() => await Assert.That(System.Text.Json.JsonSerializer.Deserialize<LineString>(LineString)).IsEquivalentTo(
        new LineString(
            [
                new(102, 0),
                new(103, 1),
                new(104, 0),
                new(105, 1)
            ]));

    [Test]
    public async Task WriteLineString() => await Assert.That(System.Text.Json.JsonSerializer.Serialize(
        new LineString(
            [
                new(102, 0),
                new(103, 1),
                new(104, 0),
                new(105, 1)
            ]))).IsSameJsonAs(LineString);

    [Test]
    public async Task ReadMultiLineString() => await Assert.That(System.Text.Json.JsonSerializer.Deserialize<MultiLineString>(MultiLineString)).IsEquivalentTo(new MultiLineString(
        [
            new(
                [
                    new(170, 45),
                    new(180, 45),
                ]),
            new(
                [
                    new(-180, 45),
                    new(-170, 45),
                ])
        ]));

    [Test]
    public async Task WriteMultiLineString() => await Assert.That(System.Text.Json.JsonSerializer.Serialize(new MultiLineString(
        [
            new(
                [
                    new(170, 45),
                    new(180, 45),
                ]),
            new(
                [
                    new(-180, 45),
                    new(-170, 45),
                ])
        ]))).IsSameJsonAs(MultiLineString);

    [Test]
    public async Task ReadPolygon()
    {
        var polygon = System.Text.Json.JsonSerializer.Deserialize<Polygon>(Polygon);
        _ = await Assert.That(polygon).IsNotNull();
        _ = await Assert.That(polygon!.Points).IsEquivalentTo(
            new List<Point>
            {
                new(-10, -10),
                new(10, -10),
                new(10, 10),
                new(-10, -10),
            });
    }

    [Test]
    public async Task WritePolygon() => await Assert.That(System.Text.Json.JsonSerializer.Serialize(
        new Polygon(
            new List<Point>
            {
                new(-10, -10),
                new(10, -10),
                new(10, 10),
                new(-10, -10),
            }))).IsSameJsonAs(Polygon);

    [Test]
    public async Task ReadPolygonWithHoles()
    {
        var polygon = System.Text.Json.JsonSerializer.Deserialize<Polygon>(PolygonWithHole);
        _ = await Assert.That(polygon).IsNotNull();
        _ = await Assert.That(polygon!.Points).IsEquivalentTo(
            new LinearRing<Point>(
            [
                new(100.0, 0.0),
                new(101.0, 0.0),
                new(101.0, 1.0),
                new(100.0, 1.0),
                new(100.0, 0.0)
            ]));
        _ = await Assert.That(polygon.Holes).IsEquivalentTo(
            new List<LinearRing<Point>>
            {
                new(
                [
                    new(100.8, 0.8),
                    new(100.8, 0.2),
                    new(100.2, 0.2),
                    new(100.2, 0.8),
                    new(100.8, 0.8)
                ])
            });
    }

    [Test]
    public async Task WritePolygonWithHoles() => await Assert.That(System.Text.Json.JsonSerializer.Serialize(
        new Polygon(
            [new(100.0, 0.0), new(101.0, 0.0), new(101.0, 1.0), new(100.0, 1.0), new(100.0, 0.0)],
            [new(100.8, 0.8), new(100.8, 0.2), new(100.2, 0.2), new(100.2, 0.8), new(100.8, 0.8)]))).IsSameJsonAs(PolygonWithHole);

    [Test]
    public async Task ReadMultiPolygon() => await Assert.That(System.Text.Json.JsonSerializer.Deserialize<MultiPolygon>(MultiPolygon)).IsEquivalentTo(new MultiPolygon(
        [
            new(
                new List<Point>
                {
                    new(180.0, 40.0),
                    new(180.0, 50.0),
                    new(170.0, 50.0),
                    new(170.0, 40.0),
                    new(180.0, 40.0)
                }),
            new(
                new List<Point>
                {
                    new(-170.0, 40.0),
                    new(-170.0, 50.0),
                    new(-180.0, 50.0),
                    new(-180.0, 40.0),
                    new(-170.0, 40.0)
                })
        ]));

    [Test]
    public async Task WriteMultiPolygon() => await Assert.That(System.Text.Json.JsonSerializer.Serialize<MultiPolygon>(new(
        [
            new(
                new List<Point>
                {
                    new(180.0, 40.0),
                    new(180.0, 50.0),
                    new(170.0, 50.0),
                    new(170.0, 40.0),
                    new(180.0, 40.0)
                }),
            new(
                new List<Point>
                {
                    new(-170.0, 40.0),
                    new(-170.0, 50.0),
                    new(-180.0, 50.0),
                    new(-180.0, 40.0),
                    new(-170.0, 40.0)
                })
        ]))).IsSameJsonAs(MultiPolygon);
}