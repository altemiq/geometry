namespace Altemiq.IO.Geodesy;

public class WellKnownTextNodeTests
{
    [Test]
    [MethodDataSource(nameof(GetWKT))]
    public async Task ParseText(string text, WellKnownTextNode expected)
    {    
        await Assert.That(new WellKnownTextNode(text))
            .IsEquivalentTo(expected);
    }

    public static IEnumerable<Func<(string, WellKnownTextNode)>> GetWKT()
    {
        yield return () => (
            """
            GEOGCS["WGS 84",
                DATUM["WGS_1984",
                    SPHEROID["WGS 84",6378137,298.257223563,
                        AUTHORITY["EPSG","7030"]],
                    AUTHORITY["EPSG","6326"]],
                PRIMEM["Greenwich",0,
                    AUTHORITY["EPSG","8901"]],
                UNIT["degree",0.0174532925199433,
                    AUTHORITY["EPSG","9122"]],
                AUTHORITY["EPSG","4326"]]
            """,
            new WellKnownTextNode(
                "GEOGCS",
                "WGS 84",
                new WellKnownTextNode(
                    "DATUM",
                    "WGS_1984",
                    new WellKnownTextNode(
                        "SPHEROID",
                        "WGS 84",
                        6378137,
                        298.257223563,
                        new WellKnownTextNode("AUTHORITY", "EPSG", "7030")),
                    new WellKnownTextNode("AUTHORITY", "EPSG", "6326")),
                new WellKnownTextNode(
                    "PRIMEM",
                    "Greenwich",
                    0,
                    new WellKnownTextNode("AUTHORITY", "EPSG", "8901")),
                new WellKnownTextNode(
                    "UNIT",
                    "degree",
                    0.0174532925199433,
                    new WellKnownTextNode("AUTHORITY", "EPSG", "9122")),
                new WellKnownTextNode("AUTHORITY", "EPSG", "4326")));
        yield return () => (
            """
            GEOGCRS["WGS 84",
                ENSEMBLE["World Geodetic System 1984 ensemble",
                    MEMBER["World Geodetic System 1984 (Transit)"],
                    MEMBER["World Geodetic System 1984 (G730)"],
                    MEMBER["World Geodetic System 1984 (G873)"],
                    MEMBER["World Geodetic System 1984 (G1150)"],
                    MEMBER["World Geodetic System 1984 (G1674)"],
                    MEMBER["World Geodetic System 1984 (G1762)"],
                    MEMBER["World Geodetic System 1984 (G2139)"],
                    MEMBER["World Geodetic System 1984 (G2296)"],
                    ELLIPSOID["WGS 84",6378137,298.257223563,
                        LENGTHUNIT["metre",1]],
                    ENSEMBLEACCURACY[2.0]],
                PRIMEM["Greenwich",0,
                    ANGLEUNIT["degree",0.0174532925199433]],
                CS[ellipsoidal,2],
                    AXIS["geodetic latitude (Lat)",north,
                        ORDER[1],
                        ANGLEUNIT["degree",0.0174532925199433]],
                    AXIS["geodetic longitude (Lon)",east,
                        ORDER[2],
                        ANGLEUNIT["degree",0.0174532925199433]],
                USAGE[
                    SCOPE["Horizontal component of 3D system."],
                    AREA["World."],
                    BBOX[-90,-180,90,180]],
                ID["EPSG",4326]]
            """,
            new WellKnownTextNode(
                "GEOGCRS",
                "WGS 84",
                    new WellKnownTextNode(
                        "ENSEMBLE",
                        "World Geodetic System 1984 ensemble",
                        new WellKnownTextNode("MEMBER", "World Geodetic System 1984 (Transit)"),
                        new WellKnownTextNode("MEMBER", "World Geodetic System 1984 (G730)"),
                        new WellKnownTextNode("MEMBER", "World Geodetic System 1984 (G873)"),
                        new WellKnownTextNode("MEMBER", "World Geodetic System 1984 (G1150)"),
                        new WellKnownTextNode("MEMBER", "World Geodetic System 1984 (G1674)"),
                        new WellKnownTextNode("MEMBER", "World Geodetic System 1984 (G1762)"),
                        new WellKnownTextNode("MEMBER", "World Geodetic System 1984 (G2139)"),
                        new WellKnownTextNode("MEMBER", "World Geodetic System 1984 (G2296)"),
                        new WellKnownTextNode(
                            "ELLIPSOID",
                            "WGS 84",
                            6378137,
                            298.257223563,
                            new WellKnownTextNode("LENGTHUNIT", "metre", 1)),
                        new WellKnownTextNode("ENSEMBLEACCURACY", 2.0)),
                    new WellKnownTextNode(
                        "PRIMEM",
                        "Greenwich",
                        0,
                        new WellKnownTextNode("ANGLEUNIT","degree", 0.0174532925199433)),
                    new WellKnownTextNode(
                        "CS",
                        new Literal("ellipsoidal"),
                        2),
                    new WellKnownTextNode(
                        "AXIS",
                        "geodetic latitude (Lat)",
                        new Literal("north"),
                        new WellKnownTextNode("ORDER",1),
                        new WellKnownTextNode("ANGLEUNIT","degree", 0.0174532925199433)),
                    new WellKnownTextNode(
                        "AXIS",
                        "geodetic longitude (Lon)",
                        new Literal("east"),
                        new WellKnownTextNode("ORDER",2),
                        new WellKnownTextNode("ANGLEUNIT","degree", 0.0174532925199433)),
                new WellKnownTextNode(
                    "USAGE",
                    new WellKnownTextNode("SCOPE","Horizontal component of 3D system."),
                    new WellKnownTextNode("AREA","World."),
                    new WellKnownTextNode("BBOX",-90, -180, 90, 180)),
                new WellKnownTextNode("ID","EPSG", 4326)));
    }
}
