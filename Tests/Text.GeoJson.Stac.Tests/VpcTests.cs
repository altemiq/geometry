// -----------------------------------------------------------------------
// <copyright file="VpcTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson.Stac;

public class VpcTests
{
    private const string Json = """
        {
          "type": "FeatureCollection",
          "features": [
            {
              "type": "Feature",
              "stac_version": "1.0.0",
              "stac_extensions": [
                "https://stac-extensions.github.io/pointcloud/v1.0.0/schema.json",
                "https://stac-extensions.github.io/projection/v1.1.0/schema.json"
              ],
              "id": "LHD_C_LA93-IGN69_0932-6503_2021.copc",
              "geometry": {
                "coordinates": [
                  [
                    [
                      5.975588974438981,
                      45.57816046684541,
                      279.19
                    ],
                    [
                      5.976072141967906,
                      45.587158740849496,
                      279.19
                    ],
                    [
                      5.988887687744445,
                      45.58681870755276,
                      576.42
                    ],
                    [
                      5.988402443568857,
                      45.57782048905193,
                      576.42
                    ],
                    [
                      5.975588974438981,
                      45.57816046684541,
                      279.19
                    ]
                  ]
                ],
                "type": "Polygon"
              },
              "bbox": [
                5.975588974438981,
                45.57782048905193,
                279.19,
                5.988887687744445,
                45.587158740849496,
                576.42
              ],
              "properties": {
                "datetime": "2023-05-24T00:00:00Z",
                "pc:count": 33377564,
                "pc:encoding": "?",
                "pc:schemas": [
                  {
                    "name": "X",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Y",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Z",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Intensity",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "ReturnNumber",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "NumberOfReturns",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanDirectionFlag",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "EdgeOfFlightLine",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "Classification",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanAngleRank",
                    "size": 4,
                    "type": "floating"
                  },
                  {
                    "name": "UserData",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "PointSourceId",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "GpsTime",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "ScanChannel",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ClassFlags",
                    "size": 1,
                    "type": "unsigned"
                  }
                ],
                "pc:type": "lidar",
                "proj:bbox": [
                  932000.0,
                  6502000.0,
                  279.19,
                  932999.99,
                  6502999.99,
                  576.42
                ],
                "proj:geometry": {
                  "coordinates": [
                    [
                      [
                        932000.0,
                        6502000.0,
                        279.19
                      ],
                      [
                        932000.0,
                        6502999.99,
                        279.19
                      ],
                      [
                        932999.99,
                        6502999.99,
                        576.42
                      ],
                      [
                        932999.99,
                        6502000.0,
                        576.42
                      ],
                      [
                        932000.0,
                        6502000.0,
                        279.19
                      ]
                    ]
                  ],
                  "type": "Polygon"
                },
                "proj:wkt2": "PROJCS[\"RGF93 v1 / Lambert-93\",GEOGCS[\"RGF93 v1\",DATUM[\"Reseau_Geodesique_Francais_1993_v1\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],AUTHORITY[\"EPSG\",\"6171\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4171\"]],PROJECTION[\"Lambert_Conformal_Conic_2SP\"],PARAMETER[\"latitude_of_origin\",46.5],PARAMETER[\"central_meridian\",3],PARAMETER[\"standard_parallel_1\",49],PARAMETER[\"standard_parallel_2\",44],PARAMETER[\"false_easting\",700000],PARAMETER[\"false_northing\",6600000],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],AXIS[\"Easting\",EAST],AXIS[\"Northing\",NORTH],AUTHORITY[\"EPSG\",\"2154\"]]"
              },
              "links": [],
              "assets": {
                "data": {
                  "href": "https://storage.sbg.cloud.ovh.net/v1/AUTH_63234f509d6048bca3c9fd7928720ca1/ppk-lidar/PL/LHD_C_LA93-IGN69_0932-6503_2021.copc.laz",
                  "roles": [
                    "data"
                  ]
                }
              }
            },
            {
              "type": "Feature",
              "stac_version": "1.0.0",
              "stac_extensions": [
                "https://stac-extensions.github.io/pointcloud/v1.0.0/schema.json",
                "https://stac-extensions.github.io/projection/v1.1.0/schema.json"
              ],
              "id": "LHD_C_LA93-IGN69_0933-6503_2021.copc",
              "geometry": {
                "coordinates": [
                  [
                    [
                      5.988402571704044,
                      45.57782048564482,
                      328.75
                    ],
                    [
                      5.988887815900399,
                      45.58681870414509,
                      328.75
                    ],
                    [
                      6.001703204141277,
                      45.58647721045067,
                      639.48
                    ],
                    [
                      6.001215883374901,
                      45.577479047691654,
                      639.48
                    ],
                    [
                      5.988402571704044,
                      45.57782048564482,
                      328.75
                    ]
                  ]
                ],
                "type": "Polygon"
              },
              "bbox": [
                5.988402571704044,
                45.577479047691654,
                328.75,
                6.001703204141277,
                45.58681870414509,
                639.48
              ],
              "properties": {
                "datetime": "2023-05-24T00:00:00Z",
                "pc:count": 35018488,
                "pc:encoding": "?",
                "pc:schemas": [
                  {
                    "name": "X",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Y",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Z",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Intensity",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "ReturnNumber",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "NumberOfReturns",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanDirectionFlag",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "EdgeOfFlightLine",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "Classification",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanAngleRank",
                    "size": 4,
                    "type": "floating"
                  },
                  {
                    "name": "UserData",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "PointSourceId",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "GpsTime",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "ScanChannel",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ClassFlags",
                    "size": 1,
                    "type": "unsigned"
                  }
                ],
                "pc:type": "lidar",
                "proj:bbox": [
                  933000.0,
                  6502000.0,
                  328.75,
                  933999.99,
                  6502999.99,
                  639.48
                ],
                "proj:geometry": {
                  "coordinates": [
                    [
                      [
                        933000.0,
                        6502000.0,
                        328.75
                      ],
                      [
                        933000.0,
                        6502999.99,
                        328.75
                      ],
                      [
                        933999.99,
                        6502999.99,
                        639.48
                      ],
                      [
                        933999.99,
                        6502000.0,
                        639.48
                      ],
                      [
                        933000.0,
                        6502000.0,
                        328.75
                      ]
                    ]
                  ],
                  "type": "Polygon"
                },
                "proj:wkt2": "PROJCS[\"RGF93 v1 / Lambert-93\",GEOGCS[\"RGF93 v1\",DATUM[\"Reseau_Geodesique_Francais_1993_v1\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],AUTHORITY[\"EPSG\",\"6171\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4171\"]],PROJECTION[\"Lambert_Conformal_Conic_2SP\"],PARAMETER[\"latitude_of_origin\",46.5],PARAMETER[\"central_meridian\",3],PARAMETER[\"standard_parallel_1\",49],PARAMETER[\"standard_parallel_2\",44],PARAMETER[\"false_easting\",700000],PARAMETER[\"false_northing\",6600000],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],AXIS[\"Easting\",EAST],AXIS[\"Northing\",NORTH],AUTHORITY[\"EPSG\",\"2154\"]]"
              },
              "links": [],
              "assets": {
                "data": {
                  "href": "https://storage.sbg.cloud.ovh.net/v1/AUTH_63234f509d6048bca3c9fd7928720ca1/ppk-lidar/PL/LHD_C_LA93-IGN69_0933-6503_2021.copc.laz",
                  "roles": [
                    "data"
                  ]
                }
              }
            },
            {
              "type": "Feature",
              "stac_version": "1.0.0",
              "stac_extensions": [
                "https://stac-extensions.github.io/pointcloud/v1.0.0/schema.json",
                "https://stac-extensions.github.io/projection/v1.1.0/schema.json"
              ],
              "id": "LHD_C_LA93-IGN69_0934-6503_2021.copc",
              "geometry": {
                "coordinates": [
                  [
                    [
                      6.00121601150851,
                      45.577479044269936,
                      375.88
                    ],
                    [
                      6.001703332295652,
                      45.586477207028395,
                      375.88
                    ],
                    [
                      6.01451856232865,
                      45.58613425296344,
                      626.53
                    ],
                    [
                      6.014029165048453,
                      45.57713614618425,
                      626.53
                    ],
                    [
                      6.00121601150851,
                      45.577479044269936,
                      375.88
                    ]
                  ]
                ],
                "type": "Polygon"
              },
              "bbox": [
                6.00121601150851,
                45.57713614618425,
                375.88,
                6.01451856232865,
                45.586477207028395,
                626.53
              ],
              "properties": {
                "datetime": "2023-05-25T00:00:00Z",
                "pc:count": 32774995,
                "pc:encoding": "?",
                "pc:schemas": [
                  {
                    "name": "X",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Y",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Z",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Intensity",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "ReturnNumber",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "NumberOfReturns",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanDirectionFlag",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "EdgeOfFlightLine",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "Classification",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanAngleRank",
                    "size": 4,
                    "type": "floating"
                  },
                  {
                    "name": "UserData",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "PointSourceId",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "GpsTime",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "ScanChannel",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ClassFlags",
                    "size": 1,
                    "type": "unsigned"
                  }
                ],
                "pc:type": "lidar",
                "proj:bbox": [
                  934000.0,
                  6502000.0,
                  375.88,
                  934999.99,
                  6502999.99,
                  626.53
                ],
                "proj:geometry": {
                  "coordinates": [
                    [
                      [
                        934000.0,
                        6502000.0,
                        375.88
                      ],
                      [
                        934000.0,
                        6502999.99,
                        375.88
                      ],
                      [
                        934999.99,
                        6502999.99,
                        626.53
                      ],
                      [
                        934999.99,
                        6502000.0,
                        626.53
                      ],
                      [
                        934000.0,
                        6502000.0,
                        375.88
                      ]
                    ]
                  ],
                  "type": "Polygon"
                },
                "proj:wkt2": "PROJCS[\"RGF93 v1 / Lambert-93\",GEOGCS[\"RGF93 v1\",DATUM[\"Reseau_Geodesique_Francais_1993_v1\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],AUTHORITY[\"EPSG\",\"6171\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4171\"]],PROJECTION[\"Lambert_Conformal_Conic_2SP\"],PARAMETER[\"latitude_of_origin\",46.5],PARAMETER[\"central_meridian\",3],PARAMETER[\"standard_parallel_1\",49],PARAMETER[\"standard_parallel_2\",44],PARAMETER[\"false_easting\",700000],PARAMETER[\"false_northing\",6600000],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],AXIS[\"Easting\",EAST],AXIS[\"Northing\",NORTH],AUTHORITY[\"EPSG\",\"2154\"]]"
              },
              "links": [],
              "assets": {
                "data": {
                  "href": "https://storage.sbg.cloud.ovh.net/v1/AUTH_63234f509d6048bca3c9fd7928720ca1/ppk-lidar/PL/LHD_C_LA93-IGN69_0934-6503_2021.copc.laz",
                  "roles": [
                    "data"
                  ]
                }
              }
            },
            {
              "type": "Feature",
              "stac_version": "1.0.0",
              "stac_extensions": [
                "https://stac-extensions.github.io/pointcloud/v1.0.0/schema.json",
                "https://stac-extensions.github.io/projection/v1.1.0/schema.json"
              ],
              "id": "LHD_C_LA93-IGN69_0935-6503_2021.copc",
              "geometry": {
                "coordinates": [
                  [
                    [
                      6.014029293180478,
                      45.577136142747925,
                      431.17
                    ],
                    [
                      6.014518690481439,
                      45.58613424952657,
                      431.17
                    ],
                    [
                      6.027333761634385,
                      45.58578983511835,
                      702.23
                    ],
                    [
                      6.026842287917664,
                      45.57679178455695,
                      702.23
                    ],
                    [
                      6.014029293180478,
                      45.577136142747925,
                      431.17
                    ]
                  ]
                ],
                "type": "Polygon"
              },
              "bbox": [
                6.014029293180478,
                45.57679178455695,
                431.17,
                6.027333761634385,
                45.58613424952657,
                702.23
              ],
              "properties": {
                "datetime": "2023-05-24T00:00:00Z",
                "pc:count": 31156232,
                "pc:encoding": "?",
                "pc:schemas": [
                  {
                    "name": "X",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Y",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Z",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Intensity",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "ReturnNumber",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "NumberOfReturns",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanDirectionFlag",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "EdgeOfFlightLine",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "Classification",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanAngleRank",
                    "size": 4,
                    "type": "floating"
                  },
                  {
                    "name": "UserData",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "PointSourceId",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "GpsTime",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "ScanChannel",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ClassFlags",
                    "size": 1,
                    "type": "unsigned"
                  }
                ],
                "pc:type": "lidar",
                "proj:bbox": [
                  935000.0,
                  6502000.0,
                  431.17,
                  935999.99,
                  6502999.99,
                  702.23
                ],
                "proj:geometry": {
                  "coordinates": [
                    [
                      [
                        935000.0,
                        6502000.0,
                        431.17
                      ],
                      [
                        935000.0,
                        6502999.99,
                        431.17
                      ],
                      [
                        935999.99,
                        6502999.99,
                        702.23
                      ],
                      [
                        935999.99,
                        6502000.0,
                        702.23
                      ],
                      [
                        935000.0,
                        6502000.0,
                        431.17
                      ]
                    ]
                  ],
                  "type": "Polygon"
                },
                "proj:wkt2": "PROJCS[\"RGF93 v1 / Lambert-93\",GEOGCS[\"RGF93 v1\",DATUM[\"Reseau_Geodesique_Francais_1993_v1\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],AUTHORITY[\"EPSG\",\"6171\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4171\"]],PROJECTION[\"Lambert_Conformal_Conic_2SP\"],PARAMETER[\"latitude_of_origin\",46.5],PARAMETER[\"central_meridian\",3],PARAMETER[\"standard_parallel_1\",49],PARAMETER[\"standard_parallel_2\",44],PARAMETER[\"false_easting\",700000],PARAMETER[\"false_northing\",6600000],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],AXIS[\"Easting\",EAST],AXIS[\"Northing\",NORTH],AUTHORITY[\"EPSG\",\"2154\"]]"
              },
              "links": [],
              "assets": {
                "data": {
                  "href": "https://storage.sbg.cloud.ovh.net/v1/AUTH_63234f509d6048bca3c9fd7928720ca1/ppk-lidar/PL/LHD_C_LA93-IGN69_0935-6503_2021.copc.laz",
                  "roles": [
                    "data"
                  ]
                }
              }
            },
            {
              "type": "Feature",
              "stac_version": "1.0.0",
              "stac_extensions": [
                "https://stac-extensions.github.io/pointcloud/v1.0.0/schema.json",
                "https://stac-extensions.github.io/projection/v1.1.0/schema.json"
              ],
              "id": "LHD_C_LA93-IGN69_0932-6504_2021.copc",
              "geometry": {
                "coordinates": [
                  [
                    [
                      5.976072146800414,
                      45.58715883083317,
                      414.12
                    ],
                    [
                      5.97655547119274,
                      45.596157111251756,
                      414.12
                    ],
                    [
                      5.989373094308977,
                      45.59581702244143,
                      999.76
                    ],
                    [
                      5.988887692597723,
                      45.58681879753589,
                      999.76
                    ],
                    [
                      5.976072146800414,
                      45.58715883083317,
                      414.12
                    ]
                  ]
                ],
                "type": "Polygon"
              },
              "bbox": [
                5.976072146800414,
                45.58681879753589,
                414.12,
                5.989373094308977,
                45.596157111251756,
                999.76
              ],
              "properties": {
                "datetime": "2023-05-24T00:00:00Z",
                "pc:count": 52510367,
                "pc:encoding": "?",
                "pc:schemas": [
                  {
                    "name": "X",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Y",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Z",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Intensity",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "ReturnNumber",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "NumberOfReturns",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanDirectionFlag",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "EdgeOfFlightLine",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "Classification",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanAngleRank",
                    "size": 4,
                    "type": "floating"
                  },
                  {
                    "name": "UserData",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "PointSourceId",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "GpsTime",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "ScanChannel",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ClassFlags",
                    "size": 1,
                    "type": "unsigned"
                  }
                ],
                "pc:type": "lidar",
                "proj:bbox": [
                  932000.0,
                  6503000.0,
                  414.12,
                  932999.99,
                  6503999.99,
                  999.76
                ],
                "proj:geometry": {
                  "coordinates": [
                    [
                      [
                        932000.0,
                        6503000.0,
                        414.12
                      ],
                      [
                        932000.0,
                        6503999.99,
                        414.12
                      ],
                      [
                        932999.99,
                        6503999.99,
                        999.76
                      ],
                      [
                        932999.99,
                        6503000.0,
                        999.76
                      ],
                      [
                        932000.0,
                        6503000.0,
                        414.12
                      ]
                    ]
                  ],
                  "type": "Polygon"
                },
                "proj:wkt2": "PROJCS[\"RGF93 v1 / Lambert-93\",GEOGCS[\"RGF93 v1\",DATUM[\"Reseau_Geodesique_Francais_1993_v1\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],AUTHORITY[\"EPSG\",\"6171\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4171\"]],PROJECTION[\"Lambert_Conformal_Conic_2SP\"],PARAMETER[\"latitude_of_origin\",46.5],PARAMETER[\"central_meridian\",3],PARAMETER[\"standard_parallel_1\",49],PARAMETER[\"standard_parallel_2\",44],PARAMETER[\"false_easting\",700000],PARAMETER[\"false_northing\",6600000],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],AXIS[\"Easting\",EAST],AXIS[\"Northing\",NORTH],AUTHORITY[\"EPSG\",\"2154\"]]"
              },
              "links": [],
              "assets": {
                "data": {
                  "href": "https://storage.sbg.cloud.ovh.net/v1/AUTH_63234f509d6048bca3c9fd7928720ca1/ppk-lidar/PL/LHD_C_LA93-IGN69_0932-6504_2021.copc.laz",
                  "roles": [
                    "data"
                  ]
                }
              }
            },
            {
              "type": "Feature",
              "stac_version": "1.0.0",
              "stac_extensions": [
                "https://stac-extensions.github.io/pointcloud/v1.0.0/schema.json",
                "https://stac-extensions.github.io/projection/v1.1.0/schema.json"
              ],
              "id": "LHD_C_LA93-IGN69_0933-6504_2021.copc",
              "geometry": {
                "coordinates": [
                  [
                    [
                      5.988887820753677,
                      45.58681879412821,
                      562.71
                    ],
                    [
                      5.989373222485703,
                      45.5958170190332,
                      562.71
                    ],
                    [
                      6.002190687989588,
                      45.59547546958712,
                      1193.38
                    ],
                    [
                      6.001703209015325,
                      45.586477300433224,
                      1193.38
                    ],
                    [
                      5.988887820753677,
                      45.58681879412821,
                      562.71
                    ]
                  ]
                ],
                "type": "Polygon"
              },
              "bbox": [
                5.988887820753677,
                45.586477300433224,
                562.71,
                6.002190687989588,
                45.5958170190332,
                1193.38
              ],
              "properties": {
                "datetime": "2023-05-25T00:00:00Z",
                "pc:count": 50124269,
                "pc:encoding": "?",
                "pc:schemas": [
                  {
                    "name": "X",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Y",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Z",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Intensity",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "ReturnNumber",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "NumberOfReturns",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanDirectionFlag",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "EdgeOfFlightLine",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "Classification",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanAngleRank",
                    "size": 4,
                    "type": "floating"
                  },
                  {
                    "name": "UserData",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "PointSourceId",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "GpsTime",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "ScanChannel",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ClassFlags",
                    "size": 1,
                    "type": "unsigned"
                  }
                ],
                "pc:type": "lidar",
                "proj:bbox": [
                  933000.0,
                  6503000.0,
                  562.71,
                  933999.99,
                  6503999.99,
                  1193.38
                ],
                "proj:geometry": {
                  "coordinates": [
                    [
                      [
                        933000.0,
                        6503000.0,
                        562.71
                      ],
                      [
                        933000.0,
                        6503999.99,
                        562.71
                      ],
                      [
                        933999.99,
                        6503999.99,
                        1193.38
                      ],
                      [
                        933999.99,
                        6503000.0,
                        1193.38
                      ],
                      [
                        933000.0,
                        6503000.0,
                        562.71
                      ]
                    ]
                  ],
                  "type": "Polygon"
                },
                "proj:wkt2": "PROJCS[\"RGF93 v1 / Lambert-93\",GEOGCS[\"RGF93 v1\",DATUM[\"Reseau_Geodesique_Francais_1993_v1\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],AUTHORITY[\"EPSG\",\"6171\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4171\"]],PROJECTION[\"Lambert_Conformal_Conic_2SP\"],PARAMETER[\"latitude_of_origin\",46.5],PARAMETER[\"central_meridian\",3],PARAMETER[\"standard_parallel_1\",49],PARAMETER[\"standard_parallel_2\",44],PARAMETER[\"false_easting\",700000],PARAMETER[\"false_northing\",6600000],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],AXIS[\"Easting\",EAST],AXIS[\"Northing\",NORTH],AUTHORITY[\"EPSG\",\"2154\"]]"
              },
              "links": [],
              "assets": {
                "data": {
                  "href": "https://storage.sbg.cloud.ovh.net/v1/AUTH_63234f509d6048bca3c9fd7928720ca1/ppk-lidar/PL/LHD_C_LA93-IGN69_0933-6504_2021.copc.laz",
                  "roles": [
                    "data"
                  ]
                }
              }
            },
            {
              "type": "Feature",
              "stac_version": "1.0.0",
              "stac_extensions": [
                "https://stac-extensions.github.io/pointcloud/v1.0.0/schema.json",
                "https://stac-extensions.github.io/projection/v1.1.0/schema.json"
              ],
              "id": "LHD_C_LA93-IGN69_0934-6504_2021.copc",
              "geometry": {
                "coordinates": [
                  [
                    [
                      6.001703337169699,
                      45.58647729701096,
                      553.53
                    ],
                    [
                      6.002190816164735,
                      45.595475466164295,
                      553.53
                    ],
                    [
                      6.01500812338372,
                      45.59513245610964,
                      1103.16
                    ],
                    [
                      6.014518567223466,
                      45.58613434294544,
                      1103.16
                    ],
                    [
                      6.001703337169699,
                      45.58647729701096,
                      553.53
                    ]
                  ]
                ],
                "type": "Polygon"
              },
              "bbox": [
                6.001703337169699,
                45.58613434294544,
                553.53,
                6.01500812338372,
                45.595475466164295,
                1103.16
              ],
              "properties": {
                "datetime": "2023-05-25T00:00:00Z",
                "pc:count": 44937735,
                "pc:encoding": "?",
                "pc:schemas": [
                  {
                    "name": "X",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Y",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Z",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Intensity",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "ReturnNumber",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "NumberOfReturns",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanDirectionFlag",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "EdgeOfFlightLine",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "Classification",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanAngleRank",
                    "size": 4,
                    "type": "floating"
                  },
                  {
                    "name": "UserData",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "PointSourceId",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "GpsTime",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "ScanChannel",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ClassFlags",
                    "size": 1,
                    "type": "unsigned"
                  }
                ],
                "pc:type": "lidar",
                "proj:bbox": [
                  934000.0,
                  6503000.0,
                  553.53,
                  934999.99,
                  6503999.99,
                  1103.16
                ],
                "proj:geometry": {
                  "coordinates": [
                    [
                      [
                        934000.0,
                        6503000.0,
                        553.53
                      ],
                      [
                        934000.0,
                        6503999.99,
                        553.53
                      ],
                      [
                        934999.99,
                        6503999.99,
                        1103.16
                      ],
                      [
                        934999.99,
                        6503000.0,
                        1103.16
                      ],
                      [
                        934000.0,
                        6503000.0,
                        553.53
                      ]
                    ]
                  ],
                  "type": "Polygon"
                },
                "proj:wkt2": "PROJCS[\"RGF93 v1 / Lambert-93\",GEOGCS[\"RGF93 v1\",DATUM[\"Reseau_Geodesique_Francais_1993_v1\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],AUTHORITY[\"EPSG\",\"6171\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4171\"]],PROJECTION[\"Lambert_Conformal_Conic_2SP\"],PARAMETER[\"latitude_of_origin\",46.5],PARAMETER[\"central_meridian\",3],PARAMETER[\"standard_parallel_1\",49],PARAMETER[\"standard_parallel_2\",44],PARAMETER[\"false_easting\",700000],PARAMETER[\"false_northing\",6600000],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],AXIS[\"Easting\",EAST],AXIS[\"Northing\",NORTH],AUTHORITY[\"EPSG\",\"2154\"]]"
              },
              "links": [],
              "assets": {
                "data": {
                  "href": "https://storage.sbg.cloud.ovh.net/v1/AUTH_63234f509d6048bca3c9fd7928720ca1/ppk-lidar/PL/LHD_C_LA93-IGN69_0934-6504_2021.copc.laz",
                  "roles": [
                    "data"
                  ]
                }
              }
            },
            {
              "type": "Feature",
              "stac_version": "1.0.0",
              "stac_extensions": [
                "https://stac-extensions.github.io/pointcloud/v1.0.0/schema.json",
                "https://stac-extensions.github.io/projection/v1.1.0/schema.json"
              ],
              "id": "LHD_C_LA93-IGN69_0935-6504_2021.copc",
              "geometry": {
                "coordinates": [
                  [
                    [
                      6.014518695376256,
                      45.58613433950857,
                      464.02
                    ],
                    [
                      6.015008251557281,
                      45.5951324526722,
                      464.02
                    ],
                    [
                      6.027825399818869,
                      45.59478798203625,
                      749.04
                    ],
                    [
                      6.027333766549969,
                      45.585789925099775,
                      749.04
                    ],
                    [
                      6.014518695376256,
                      45.58613433950857,
                      464.02
                    ]
                  ]
                ],
                "type": "Polygon"
              },
              "bbox": [
                6.014518695376256,
                45.585789925099775,
                464.02,
                6.027825399818869,
                45.5951324526722,
                749.04
              ],
              "properties": {
                "datetime": "2023-05-25T00:00:00Z",
                "pc:count": 31435807,
                "pc:encoding": "?",
                "pc:schemas": [
                  {
                    "name": "X",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Y",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Z",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Intensity",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "ReturnNumber",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "NumberOfReturns",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanDirectionFlag",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "EdgeOfFlightLine",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "Classification",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanAngleRank",
                    "size": 4,
                    "type": "floating"
                  },
                  {
                    "name": "UserData",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "PointSourceId",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "GpsTime",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "ScanChannel",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ClassFlags",
                    "size": 1,
                    "type": "unsigned"
                  }
                ],
                "pc:type": "lidar",
                "proj:bbox": [
                  935000.0,
                  6503000.0,
                  464.02,
                  935999.99,
                  6503999.99,
                  749.04
                ],
                "proj:geometry": {
                  "coordinates": [
                    [
                      [
                        935000.0,
                        6503000.0,
                        464.02
                      ],
                      [
                        935000.0,
                        6503999.99,
                        464.02
                      ],
                      [
                        935999.99,
                        6503999.99,
                        749.04
                      ],
                      [
                        935999.99,
                        6503000.0,
                        749.04
                      ],
                      [
                        935000.0,
                        6503000.0,
                        464.02
                      ]
                    ]
                  ],
                  "type": "Polygon"
                },
                "proj:wkt2": "PROJCS[\"RGF93 v1 / Lambert-93\",GEOGCS[\"RGF93 v1\",DATUM[\"Reseau_Geodesique_Francais_1993_v1\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],AUTHORITY[\"EPSG\",\"6171\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4171\"]],PROJECTION[\"Lambert_Conformal_Conic_2SP\"],PARAMETER[\"latitude_of_origin\",46.5],PARAMETER[\"central_meridian\",3],PARAMETER[\"standard_parallel_1\",49],PARAMETER[\"standard_parallel_2\",44],PARAMETER[\"false_easting\",700000],PARAMETER[\"false_northing\",6600000],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],AXIS[\"Easting\",EAST],AXIS[\"Northing\",NORTH],AUTHORITY[\"EPSG\",\"2154\"]]"
              },
              "links": [],
              "assets": {
                "data": {
                  "href": "https://storage.sbg.cloud.ovh.net/v1/AUTH_63234f509d6048bca3c9fd7928720ca1/ppk-lidar/PL/LHD_C_LA93-IGN69_0935-6504_2021.copc.laz",
                  "roles": [
                    "data"
                  ]
                }
              }
            },
            {
              "type": "Feature",
              "stac_version": "1.0.0",
              "stac_extensions": [
                "https://stac-extensions.github.io/pointcloud/v1.0.0/schema.json",
                "https://stac-extensions.github.io/projection/v1.1.0/schema.json"
              ],
              "id": "LHD_C_LA93-IGN69_0932-6505_2021.copc",
              "geometry": {
                "coordinates": [
                  [
                    [
                      5.976555476026816,
                      45.596157201235506,
                      616.35
                    ],
                    [
                      5.977038957358912,
                      45.6051554878501,
                      616.35
                    ],
                    [
                      5.989858658487442,
                      45.60481534351637,
                      1382.64
                    ],
                    [
                      5.989373099163831,
                      45.595817112424605,
                      1382.64
                    ],
                    [
                      5.976555476026816,
                      45.596157201235506,
                      616.35
                    ]
                  ]
                ],
                "type": "Polygon"
              },
              "bbox": [
                5.9765554760268165,
                45.595817112424605,
                616.35,
                5.989858658487442,
                45.6051554878501,
                1382.64
              ],
              "properties": {
                "datetime": "2023-05-24T00:00:00Z",
                "pc:count": 58284672,
                "pc:encoding": "?",
                "pc:schemas": [
                  {
                    "name": "X",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Y",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Z",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Intensity",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "ReturnNumber",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "NumberOfReturns",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanDirectionFlag",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "EdgeOfFlightLine",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "Classification",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanAngleRank",
                    "size": 4,
                    "type": "floating"
                  },
                  {
                    "name": "UserData",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "PointSourceId",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "GpsTime",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "ScanChannel",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ClassFlags",
                    "size": 1,
                    "type": "unsigned"
                  }
                ],
                "pc:type": "lidar",
                "proj:bbox": [
                  932000.0,
                  6504000.0,
                  616.35,
                  932999.99,
                  6504999.99,
                  1382.64
                ],
                "proj:geometry": {
                  "coordinates": [
                    [
                      [
                        932000.0,
                        6504000.0,
                        616.35
                      ],
                      [
                        932000.0,
                        6504999.99,
                        616.35
                      ],
                      [
                        932999.99,
                        6504999.99,
                        1382.64
                      ],
                      [
                        932999.99,
                        6504000.0,
                        1382.64
                      ],
                      [
                        932000.0,
                        6504000.0,
                        616.35
                      ]
                    ]
                  ],
                  "type": "Polygon"
                },
                "proj:wkt2": "PROJCS[\"RGF93 v1 / Lambert-93\",GEOGCS[\"RGF93 v1\",DATUM[\"Reseau_Geodesique_Francais_1993_v1\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],AUTHORITY[\"EPSG\",\"6171\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4171\"]],PROJECTION[\"Lambert_Conformal_Conic_2SP\"],PARAMETER[\"latitude_of_origin\",46.5],PARAMETER[\"central_meridian\",3],PARAMETER[\"standard_parallel_1\",49],PARAMETER[\"standard_parallel_2\",44],PARAMETER[\"false_easting\",700000],PARAMETER[\"false_northing\",6600000],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],AXIS[\"Easting\",EAST],AXIS[\"Northing\",NORTH],AUTHORITY[\"EPSG\",\"2154\"]]"
              },
              "links": [],
              "assets": {
                "data": {
                  "href": "https://storage.sbg.cloud.ovh.net/v1/AUTH_63234f509d6048bca3c9fd7928720ca1/ppk-lidar/PL/LHD_C_LA93-IGN69_0932-6505_2021.copc.laz",
                  "roles": [
                    "data"
                  ]
                }
              }
            },
            {
              "type": "Feature",
              "stac_version": "1.0.0",
              "stac_extensions": [
                "https://stac-extensions.github.io/pointcloud/v1.0.0/schema.json",
                "https://stac-extensions.github.io/projection/v1.1.0/schema.json"
              ],
              "id": "LHD_C_LA93-IGN69_0933-6505_2021.copc",
              "geometry": {
                "coordinates": [
                  [
                    [
                      5.989373227340557,
                      45.59581710901638,
                      964.67
                    ],
                    [
                      5.989858786684949,
                      45.604815340107585,
                      964.67
                    ],
                    [
                      6.002678330124385,
                      45.60447373490003,
                      1381.07
                    ],
                    [
                      6.002190692865217,
                      45.595475559569756,
                      1381.07
                    ],
                    [
                      5.989373227340557,
                      45.59581710901638,
                      964.67
                    ]
                  ]
                ],
                "type": "Polygon"
              },
              "bbox": [
                5.989373227340557,
                45.595475559569756,
                964.67,
                6.002678330124385,
                45.604815340107585,
                1381.07
              ],
              "properties": {
                "datetime": "2023-05-25T00:00:00Z",
                "pc:count": 47922645,
                "pc:encoding": "?",
                "pc:schemas": [
                  {
                    "name": "X",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Y",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Z",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Intensity",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "ReturnNumber",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "NumberOfReturns",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanDirectionFlag",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "EdgeOfFlightLine",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "Classification",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanAngleRank",
                    "size": 4,
                    "type": "floating"
                  },
                  {
                    "name": "UserData",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "PointSourceId",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "GpsTime",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "ScanChannel",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ClassFlags",
                    "size": 1,
                    "type": "unsigned"
                  }
                ],
                "pc:type": "lidar",
                "proj:bbox": [
                  933000.0,
                  6504000.0,
                  964.67,
                  933999.99,
                  6504999.99,
                  1381.07
                ],
                "proj:geometry": {
                  "coordinates": [
                    [
                      [
                        933000.0,
                        6504000.0,
                        964.67
                      ],
                      [
                        933000.0,
                        6504999.99,
                        964.67
                      ],
                      [
                        933999.99,
                        6504999.99,
                        1381.07
                      ],
                      [
                        933999.99,
                        6504000.0,
                        1381.07
                      ],
                      [
                        933000.0,
                        6504000.0,
                        964.67
                      ]
                    ]
                  ],
                  "type": "Polygon"
                },
                "proj:wkt2": "PROJCS[\"RGF93 v1 / Lambert-93\",GEOGCS[\"RGF93 v1\",DATUM[\"Reseau_Geodesique_Francais_1993_v1\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],AUTHORITY[\"EPSG\",\"6171\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4171\"]],PROJECTION[\"Lambert_Conformal_Conic_2SP\"],PARAMETER[\"latitude_of_origin\",46.5],PARAMETER[\"central_meridian\",3],PARAMETER[\"standard_parallel_1\",49],PARAMETER[\"standard_parallel_2\",44],PARAMETER[\"false_easting\",700000],PARAMETER[\"false_northing\",6600000],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],AXIS[\"Easting\",EAST],AXIS[\"Northing\",NORTH],AUTHORITY[\"EPSG\",\"2154\"]]"
              },
              "links": [],
              "assets": {
                "data": {
                  "href": "https://storage.sbg.cloud.ovh.net/v1/AUTH_63234f509d6048bca3c9fd7928720ca1/ppk-lidar/PL/LHD_C_LA93-IGN69_0933-6505_2021.copc.laz",
                  "roles": [
                    "data"
                  ]
                }
              }
            },
            {
              "type": "Feature",
              "stac_version": "1.0.0",
              "stac_extensions": [
                "https://stac-extensions.github.io/pointcloud/v1.0.0/schema.json",
                "https://stac-extensions.github.io/projection/v1.1.0/schema.json"
              ],
              "id": "LHD_C_LA93-IGN69_0934-6505_2021.copc",
              "geometry": {
                "coordinates": [
                  [
                    [
                      6.002190821040364,
                      45.595475556146916,
                      725.2
                    ],
                    [
                      6.00267845832031,
                      45.60447373147663,
                      725.2
                    ],
                    [
                      6.015497843397779,
                      45.60413066542242,
                      1300.72
                    ],
                    [
                      6.015008128280125,
                      45.59513254609171,
                      1300.72
                    ],
                    [
                      6.002190821040364,
                      45.595475556146916,
                      725.2
                    ]
                  ]
                ],
                "type": "Polygon"
              },
              "bbox": [
                6.002190821040364,
                45.59513254609171,
                725.2,
                6.015497843397779,
                45.60447373147663,
                1300.72
              ],
              "properties": {
                "datetime": "2023-05-24T00:00:00Z",
                "pc:count": 57789071,
                "pc:encoding": "?",
                "pc:schemas": [
                  {
                    "name": "X",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Y",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Z",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Intensity",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "ReturnNumber",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "NumberOfReturns",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanDirectionFlag",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "EdgeOfFlightLine",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "Classification",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanAngleRank",
                    "size": 4,
                    "type": "floating"
                  },
                  {
                    "name": "UserData",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "PointSourceId",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "GpsTime",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "ScanChannel",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ClassFlags",
                    "size": 1,
                    "type": "unsigned"
                  }
                ],
                "pc:type": "lidar",
                "proj:bbox": [
                  934000.0,
                  6504000.0,
                  725.2,
                  934999.99,
                  6504999.99,
                  1300.72
                ],
                "proj:geometry": {
                  "coordinates": [
                    [
                      [
                        934000.0,
                        6504000.0,
                        725.2
                      ],
                      [
                        934000.0,
                        6504999.99,
                        725.2
                      ],
                      [
                        934999.99,
                        6504999.99,
                        1300.72
                      ],
                      [
                        934999.99,
                        6504000.0,
                        1300.72
                      ],
                      [
                        934000.0,
                        6504000.0,
                        725.2
                      ]
                    ]
                  ],
                  "type": "Polygon"
                },
                "proj:wkt2": "PROJCS[\"RGF93 v1 / Lambert-93\",GEOGCS[\"RGF93 v1\",DATUM[\"Reseau_Geodesique_Francais_1993_v1\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],AUTHORITY[\"EPSG\",\"6171\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4171\"]],PROJECTION[\"Lambert_Conformal_Conic_2SP\"],PARAMETER[\"latitude_of_origin\",46.5],PARAMETER[\"central_meridian\",3],PARAMETER[\"standard_parallel_1\",49],PARAMETER[\"standard_parallel_2\",44],PARAMETER[\"false_easting\",700000],PARAMETER[\"false_northing\",6600000],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],AXIS[\"Easting\",EAST],AXIS[\"Northing\",NORTH],AUTHORITY[\"EPSG\",\"2154\"]]"
              },
              "links": [],
              "assets": {
                "data": {
                  "href": "https://storage.sbg.cloud.ovh.net/v1/AUTH_63234f509d6048bca3c9fd7928720ca1/ppk-lidar/PL/LHD_C_LA93-IGN69_0934-6505_2021.copc.laz",
                  "roles": [
                    "data"
                  ]
                }
              }
            },
            {
              "type": "Feature",
              "stac_version": "1.0.0",
              "stac_extensions": [
                "https://stac-extensions.github.io/pointcloud/v1.0.0/schema.json",
                "https://stac-extensions.github.io/projection/v1.1.0/schema.json"
              ],
              "id": "LHD_C_LA93-IGN69_0935-6505_2021.copc",
              "geometry": {
                "coordinates": [
                  [
                    [
                      6.015008256453686,
                      45.59513254265427,
                      532.42
                    ],
                    [
                      6.015497971592118,
                      45.604130661984414,
                      532.42
                    ],
                    [
                      6.028317197634796,
                      45.60378613511082,
                      852.07
                    ],
                    [
                      6.027825404736049,
                      45.59478807201776,
                      852.07
                    ],
                    [
                      6.015008256453686,
                      45.59513254265427,
                      532.42
                    ]
                  ]
                ],
                "type": "Polygon"
              },
              "bbox": [
                6.015008256453686,
                45.59478807201776,
                532.42,
                6.028317197634796,
                45.604130661984414,
                852.07
              ],
              "properties": {
                "datetime": "2023-05-25T00:00:00Z",
                "pc:count": 39439732,
                "pc:encoding": "?",
                "pc:schemas": [
                  {
                    "name": "X",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Y",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Z",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Intensity",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "ReturnNumber",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "NumberOfReturns",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanDirectionFlag",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "EdgeOfFlightLine",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "Classification",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanAngleRank",
                    "size": 4,
                    "type": "floating"
                  },
                  {
                    "name": "UserData",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "PointSourceId",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "GpsTime",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "ScanChannel",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ClassFlags",
                    "size": 1,
                    "type": "unsigned"
                  }
                ],
                "pc:type": "lidar",
                "proj:bbox": [
                  935000.0,
                  6504000.0,
                  532.42,
                  935999.99,
                  6504999.99,
                  852.07
                ],
                "proj:geometry": {
                  "coordinates": [
                    [
                      [
                        935000.0,
                        6504000.0,
                        532.42
                      ],
                      [
                        935000.0,
                        6504999.99,
                        532.42
                      ],
                      [
                        935999.99,
                        6504999.99,
                        852.07
                      ],
                      [
                        935999.99,
                        6504000.0,
                        852.07
                      ],
                      [
                        935000.0,
                        6504000.0,
                        532.42
                      ]
                    ]
                  ],
                  "type": "Polygon"
                },
                "proj:wkt2": "PROJCS[\"RGF93 v1 / Lambert-93\",GEOGCS[\"RGF93 v1\",DATUM[\"Reseau_Geodesique_Francais_1993_v1\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],AUTHORITY[\"EPSG\",\"6171\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4171\"]],PROJECTION[\"Lambert_Conformal_Conic_2SP\"],PARAMETER[\"latitude_of_origin\",46.5],PARAMETER[\"central_meridian\",3],PARAMETER[\"standard_parallel_1\",49],PARAMETER[\"standard_parallel_2\",44],PARAMETER[\"false_easting\",700000],PARAMETER[\"false_northing\",6600000],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],AXIS[\"Easting\",EAST],AXIS[\"Northing\",NORTH],AUTHORITY[\"EPSG\",\"2154\"]]"
              },
              "links": [],
              "assets": {
                "data": {
                  "href": "https://storage.sbg.cloud.ovh.net/v1/AUTH_63234f509d6048bca3c9fd7928720ca1/ppk-lidar/PL/LHD_C_LA93-IGN69_0935-6505_2021.copc.laz",
                  "roles": [
                    "data"
                  ]
                }
              }
            },
            {
              "type": "Feature",
              "stac_version": "1.0.0",
              "stac_extensions": [
                "https://stac-extensions.github.io/pointcloud/v1.0.0/schema.json",
                "https://stac-extensions.github.io/projection/v1.1.0/schema.json"
              ],
              "id": "LHD_C_LA93-IGN69_0932-6506_2021.copc",
              "geometry": {
                "coordinates": [
                  [
                    [
                      5.977038962194559,
                      45.605155577833884,
                      766.83
                    ],
                    [
                      5.977522600542836,
                      45.61415387042599,
                      766.83
                    ],
                    [
                      5.990344380356584,
                      45.61381367055907,
                      1306.15
                    ],
                    [
                      5.989858663343872,
                      45.6048154334996,
                      1306.15
                    ],
                    [
                      5.977038962194559,
                      45.605155577833884,
                      766.83
                    ]
                  ]
                ],
                "type": "Polygon"
              },
              "bbox": [
                5.977038962194559,
                45.6048154334996,
                766.83,
                5.990344380356584,
                45.61415387042599,
                1306.15
              ],
              "properties": {
                "datetime": "2023-05-25T00:00:00Z",
                "pc:count": 40708575,
                "pc:encoding": "?",
                "pc:schemas": [
                  {
                    "name": "X",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Y",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Z",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Intensity",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "ReturnNumber",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "NumberOfReturns",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanDirectionFlag",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "EdgeOfFlightLine",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "Classification",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanAngleRank",
                    "size": 4,
                    "type": "floating"
                  },
                  {
                    "name": "UserData",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "PointSourceId",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "GpsTime",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "ScanChannel",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ClassFlags",
                    "size": 1,
                    "type": "unsigned"
                  }
                ],
                "pc:type": "lidar",
                "proj:bbox": [
                  932000.0,
                  6505000.0,
                  766.83,
                  932999.99,
                  6505999.99,
                  1306.15
                ],
                "proj:geometry": {
                  "coordinates": [
                    [
                      [
                        932000.0,
                        6505000.0,
                        766.83
                      ],
                      [
                        932000.0,
                        6505999.99,
                        766.83
                      ],
                      [
                        932999.99,
                        6505999.99,
                        1306.15
                      ],
                      [
                        932999.99,
                        6505000.0,
                        1306.15
                      ],
                      [
                        932000.0,
                        6505000.0,
                        766.83
                      ]
                    ]
                  ],
                  "type": "Polygon"
                },
                "proj:wkt2": "PROJCS[\"RGF93 v1 / Lambert-93\",GEOGCS[\"RGF93 v1\",DATUM[\"Reseau_Geodesique_Francais_1993_v1\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],AUTHORITY[\"EPSG\",\"6171\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4171\"]],PROJECTION[\"Lambert_Conformal_Conic_2SP\"],PARAMETER[\"latitude_of_origin\",46.5],PARAMETER[\"central_meridian\",3],PARAMETER[\"standard_parallel_1\",49],PARAMETER[\"standard_parallel_2\",44],PARAMETER[\"false_easting\",700000],PARAMETER[\"false_northing\",6600000],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],AXIS[\"Easting\",EAST],AXIS[\"Northing\",NORTH],AUTHORITY[\"EPSG\",\"2154\"]]"
              },
              "links": [],
              "assets": {
                "data": {
                  "href": "https://storage.sbg.cloud.ovh.net/v1/AUTH_63234f509d6048bca3c9fd7928720ca1/ppk-lidar/PL/LHD_C_LA93-IGN69_0932-6506_2021.copc.laz",
                  "roles": [
                    "data"
                  ]
                }
              }
            },
            {
              "type": "Feature",
              "stac_version": "1.0.0",
              "stac_extensions": [
                "https://stac-extensions.github.io/pointcloud/v1.0.0/schema.json",
                "https://stac-extensions.github.io/projection/v1.1.0/schema.json"
              ],
              "id": "LHD_C_LA93-IGN69_0933-6506_2021.copc",
              "geometry": {
                "coordinates": [
                  [
                    [
                      5.989858791541379,
                      45.604815430090824,
                      1024.48
                    ],
                    [
                      5.990344508574876,
                      45.613813667149735,
                      1024.48
                    ],
                    [
                      6.003166130622738,
                      45.61347200617084,
                      1249.12
                    ],
                    [
                      6.002678335001597,
                      45.6044738248827,
                      1249.12
                    ],
                    [
                      5.989858791541379,
                      45.604815430090824,
                      1024.48
                    ]
                  ]
                ],
                "type": "Polygon"
              },
              "bbox": [
                5.989858791541379,
                45.6044738248827,
                1024.48,
                6.003166130622738,
                45.613813667149735,
                1249.12
              ],
              "properties": {
                "datetime": "2023-05-24T00:00:00Z",
                "pc:count": 40452596,
                "pc:encoding": "?",
                "pc:schemas": [
                  {
                    "name": "X",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Y",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Z",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Intensity",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "ReturnNumber",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "NumberOfReturns",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanDirectionFlag",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "EdgeOfFlightLine",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "Classification",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanAngleRank",
                    "size": 4,
                    "type": "floating"
                  },
                  {
                    "name": "UserData",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "PointSourceId",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "GpsTime",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "ScanChannel",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ClassFlags",
                    "size": 1,
                    "type": "unsigned"
                  }
                ],
                "pc:type": "lidar",
                "proj:bbox": [
                  933000.0,
                  6505000.0,
                  1024.48,
                  933999.99,
                  6505999.99,
                  1249.12
                ],
                "proj:geometry": {
                  "coordinates": [
                    [
                      [
                        933000.0,
                        6505000.0,
                        1024.48
                      ],
                      [
                        933000.0,
                        6505999.99,
                        1024.48
                      ],
                      [
                        933999.99,
                        6505999.99,
                        1249.12
                      ],
                      [
                        933999.99,
                        6505000.0,
                        1249.12
                      ],
                      [
                        933000.0,
                        6505000.0,
                        1024.48
                      ]
                    ]
                  ],
                  "type": "Polygon"
                },
                "proj:wkt2": "PROJCS[\"RGF93 v1 / Lambert-93\",GEOGCS[\"RGF93 v1\",DATUM[\"Reseau_Geodesique_Francais_1993_v1\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],AUTHORITY[\"EPSG\",\"6171\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4171\"]],PROJECTION[\"Lambert_Conformal_Conic_2SP\"],PARAMETER[\"latitude_of_origin\",46.5],PARAMETER[\"central_meridian\",3],PARAMETER[\"standard_parallel_1\",49],PARAMETER[\"standard_parallel_2\",44],PARAMETER[\"false_easting\",700000],PARAMETER[\"false_northing\",6600000],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],AXIS[\"Easting\",EAST],AXIS[\"Northing\",NORTH],AUTHORITY[\"EPSG\",\"2154\"]]"
              },
              "links": [],
              "assets": {
                "data": {
                  "href": "https://storage.sbg.cloud.ovh.net/v1/AUTH_63234f509d6048bca3c9fd7928720ca1/ppk-lidar/PL/LHD_C_LA93-IGN69_0933-6506_2021.copc.laz",
                  "roles": [
                    "data"
                  ]
                }
              }
            },
            {
              "type": "Feature",
              "stac_version": "1.0.0",
              "stac_extensions": [
                "https://stac-extensions.github.io/pointcloud/v1.0.0/schema.json",
                "https://stac-extensions.github.io/projection/v1.1.0/schema.json"
              ],
              "id": "LHD_C_LA93-IGN69_0934-6506_2021.copc",
              "geometry": {
                "coordinates": [
                  [
                    [
                      6.002678463197523,
                      45.60447382145932,
                      757.61
                    ],
                    [
                      6.00316625883945,
                      45.613472002746896,
                      757.61
                    ],
                    [
                      6.015987722448225,
                      45.613128880683206,
                      1165.95
                    ],
                    [
                      6.015497848295775,
                      45.60413075540454,
                      1165.95
                    ],
                    [
                      6.002678463197523,
                      45.60447382145932,
                      757.61
                    ]
                  ]
                ],
                "type": "Polygon"
              },
              "bbox": [
                6.002678463197523,
                45.60413075540454,
                757.61,
                6.015987722448225,
                45.613472002746896,
                1165.95
              ],
              "properties": {
                "datetime": "2023-05-25T00:00:00Z",
                "pc:count": 57832829,
                "pc:encoding": "?",
                "pc:schemas": [
                  {
                    "name": "X",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Y",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Z",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Intensity",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "ReturnNumber",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "NumberOfReturns",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanDirectionFlag",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "EdgeOfFlightLine",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "Classification",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanAngleRank",
                    "size": 4,
                    "type": "floating"
                  },
                  {
                    "name": "UserData",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "PointSourceId",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "GpsTime",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "ScanChannel",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ClassFlags",
                    "size": 1,
                    "type": "unsigned"
                  }
                ],
                "pc:type": "lidar",
                "proj:bbox": [
                  934000.0,
                  6505000.0,
                  757.61,
                  934999.99,
                  6505999.99,
                  1165.95
                ],
                "proj:geometry": {
                  "coordinates": [
                    [
                      [
                        934000.0,
                        6505000.0,
                        757.61
                      ],
                      [
                        934000.0,
                        6505999.99,
                        757.61
                      ],
                      [
                        934999.99,
                        6505999.99,
                        1165.95
                      ],
                      [
                        934999.99,
                        6505000.0,
                        1165.95
                      ],
                      [
                        934000.0,
                        6505000.0,
                        757.61
                      ]
                    ]
                  ],
                  "type": "Polygon"
                },
                "proj:wkt2": "PROJCS[\"RGF93 v1 / Lambert-93\",GEOGCS[\"RGF93 v1\",DATUM[\"Reseau_Geodesique_Francais_1993_v1\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],AUTHORITY[\"EPSG\",\"6171\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4171\"]],PROJECTION[\"Lambert_Conformal_Conic_2SP\"],PARAMETER[\"latitude_of_origin\",46.5],PARAMETER[\"central_meridian\",3],PARAMETER[\"standard_parallel_1\",49],PARAMETER[\"standard_parallel_2\",44],PARAMETER[\"false_easting\",700000],PARAMETER[\"false_northing\",6600000],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],AXIS[\"Easting\",EAST],AXIS[\"Northing\",NORTH],AUTHORITY[\"EPSG\",\"2154\"]]"
              },
              "links": [],
              "assets": {
                "data": {
                  "href": "https://storage.sbg.cloud.ovh.net/v1/AUTH_63234f509d6048bca3c9fd7928720ca1/ppk-lidar/PL/LHD_C_LA93-IGN69_0934-6506_2021.copc.laz",
                  "roles": [
                    "data"
                  ]
                }
              }
            },
            {
              "type": "Feature",
              "stac_version": "1.0.0",
              "stac_extensions": [
                "https://stac-extensions.github.io/pointcloud/v1.0.0/schema.json",
                "https://stac-extensions.github.io/projection/v1.1.0/schema.json"
              ],
              "id": "LHD_C_LA93-IGN69_0935-6506_2021.copc",
              "geometry": {
                "coordinates": [
                  [
                    [
                      6.015497976490114,
                      45.604130751966544,
                      591.38
                    ],
                    [
                      6.015987850663349,
                      45.61312887724466,
                      591.38
                    ],
                    [
                      6.028809155159888,
                      45.612784294123486,
                      855.52
                    ],
                    [
                      6.028317202553573,
                      45.60378622509238,
                      855.52
                    ],
                    [
                      6.015497976490114,
                      45.604130751966544,
                      591.38
                    ]
                  ]
                ],
                "type": "Polygon"
              },
              "bbox": [
                6.0154979764901135,
                45.60378622509238,
                591.38,
                6.028809155159888,
                45.61312887724466,
                855.52
              ],
              "properties": {
                "datetime": "2023-05-24T00:00:00Z",
                "pc:count": 39291429,
                "pc:encoding": "?",
                "pc:schemas": [
                  {
                    "name": "X",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Y",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Z",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "Intensity",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "ReturnNumber",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "NumberOfReturns",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanDirectionFlag",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "EdgeOfFlightLine",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "Classification",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ScanAngleRank",
                    "size": 4,
                    "type": "floating"
                  },
                  {
                    "name": "UserData",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "PointSourceId",
                    "size": 2,
                    "type": "unsigned"
                  },
                  {
                    "name": "GpsTime",
                    "size": 8,
                    "type": "floating"
                  },
                  {
                    "name": "ScanChannel",
                    "size": 1,
                    "type": "unsigned"
                  },
                  {
                    "name": "ClassFlags",
                    "size": 1,
                    "type": "unsigned"
                  }
                ],
                "pc:type": "lidar",
                "proj:bbox": [
                  935000.0,
                  6505000.0,
                  591.38,
                  935999.99,
                  6505999.99,
                  855.52
                ],
                "proj:geometry": {
                  "coordinates": [
                    [
                      [
                        935000.0,
                        6505000.0,
                        591.38
                      ],
                      [
                        935000.0,
                        6505999.99,
                        591.38
                      ],
                      [
                        935999.99,
                        6505999.99,
                        855.52
                      ],
                      [
                        935999.99,
                        6505000.0,
                        855.52
                      ],
                      [
                        935000.0,
                        6505000.0,
                        591.38
                      ]
                    ]
                  ],
                  "type": "Polygon"
                },
                "proj:wkt2": "PROJCS[\"RGF93 v1 / Lambert-93\",GEOGCS[\"RGF93 v1\",DATUM[\"Reseau_Geodesique_Francais_1993_v1\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],AUTHORITY[\"EPSG\",\"6171\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4171\"]],PROJECTION[\"Lambert_Conformal_Conic_2SP\"],PARAMETER[\"latitude_of_origin\",46.5],PARAMETER[\"central_meridian\",3],PARAMETER[\"standard_parallel_1\",49],PARAMETER[\"standard_parallel_2\",44],PARAMETER[\"false_easting\",700000],PARAMETER[\"false_northing\",6600000],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],AXIS[\"Easting\",EAST],AXIS[\"Northing\",NORTH],AUTHORITY[\"EPSG\",\"2154\"]]"
              },
              "links": [],
              "assets": {
                "data": {
                  "href": "https://storage.sbg.cloud.ovh.net/v1/AUTH_63234f509d6048bca3c9fd7928720ca1/ppk-lidar/PL/LHD_C_LA93-IGN69_0935-6506_2021.copc.laz",
                  "roles": [
                    "data"
                  ]
                }
              }
            }
          ]
        }
        """;

    [Test]
    public async Task ReadVpc() => await Assert.That(Serializer.Deserialize<ItemCollection>(Json)).IsNotNull();
}