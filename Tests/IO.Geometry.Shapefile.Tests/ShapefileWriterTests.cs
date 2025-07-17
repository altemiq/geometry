// -----------------------------------------------------------------------
// <copyright file="ShapefileWriterTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

public class ShapefileWriterTests
{
    [Test]
    public async Task WriteMultiPoint()
    {
        var shpStream = new MemoryStream();
        var shxStream = new MemoryStream();
        var dbfStream = new MemoryStream();

        using (var writer = new ShapefileWriter(
            shpStream,
            shxStream,
            dbfStream,
            ShpType.MultiPoint,
        true,
            Dbf.DbfColumn.Number("AREA", 12, 3),
            Dbf.DbfColumn.Number("PERIMETER", 12, 3),
            Dbf.DbfColumn.Number("EAS_", 11, 0),
            Dbf.DbfColumn.Number("EAS_ID", 11, 0),
            Dbf.DbfColumn.String("ATLAS_P", 16),
            Dbf.DbfColumn.String("ATLAS_S", 16),
            Dbf.DbfColumn.Number("EDLOW", 3, 0),
            Dbf.DbfColumn.Number("EDMED", 3, 0),
            Dbf.DbfColumn.Number("EDHIGH", 3, 0),
            Dbf.DbfColumn.Number("HHNUMBER", 3, 0),
            Dbf.DbfColumn.Number("AVGHHINC", 6, 0),
            Dbf.DbfColumn.Number("EDUC", 1, 0),
            Dbf.DbfColumn.Number("POTENT", 7, 0),
            Dbf.DbfColumn.Number("ELAT", 8, 5),
            Dbf.DbfColumn.Number("ELON", 8, 4),
            Dbf.DbfColumn.Number("DIS58", 19, 17),
            Dbf.DbfColumn.Number("DIS130", 19, 17),
            Dbf.DbfColumn.Number("DIS208", 19, 17),
            Dbf.DbfColumn.Number("DIS425", 19, 17),
            Dbf.DbfColumn.Number("MKTSHR58", 20, 18),
            Dbf.DbfColumn.Number("MKTSHR130", 20, 18),
            Dbf.DbfColumn.Number("MKTSHR208", 20, 18),
            Dbf.DbfColumn.Number("MKTSHR425", 20, 18),
            Dbf.DbfColumn.Number("LIFESTYLES", 1, 0),
            Dbf.DbfColumn.Number("CUMMKTSHR", 19, 17),
            Dbf.DbfColumn.Number("PENTRA", 7, 0),
            Dbf.DbfColumn.Number("OPPT", 7, 0),
            Dbf.DbfColumn.String("PRFEDEA", 16),
            Dbf.DbfColumn.Number("AA", 8, 0)))
        {
            object?[] values = [7280457.000, 15020.598, 433, 241, "35044124", null, 0, 0, 0, 0, 0, 0, 0, 42.93361, -81.2028, 1.7886172706304899M, 5.0270435331921695M, 6.646826494350309M, 9.0130812812881M, 0.3078568155644309M, 0.091231790219781284M, 0.062075348742658798M, 0.038319062176947298M, 0, 0.49948301670381806M, 0, 0, "35044124", 35044124];
            Point[] geometry = [new(483575.5, 4753046)];
            var envelope = new EnvelopeZM(483575.5, 4753046, 0.0, 0.0, 483575.5, 4753046, 0.0, 0.0);

            writer.Write(geometry, values);

            writer.Update(envelope);
        }

        shpStream.Position = 0;
        shxStream.Position = 0;
        dbfStream.Position = 0;

        using (var written = new ShapefileReader(shpStream, shxStream, dbfStream, leaveOpen: true))
        {
            using var expected = new ShapefileReader(Resources.GetStream("multipnt.shp"), Resources.GetStream("multipnt.shx"), Resources.GetStream("multipnt.dbf"), leaveOpen: true);
            var writtenHeader = written.ShpHeader;
            var expectedHeader = expected.ShpHeader;

            _ = await Assert.That(writtenHeader.ShpType).IsEqualTo(expectedHeader.ShpType);
            _ = await Assert.That(writtenHeader.Extents).IsEqualTo(expectedHeader.Extents);

            var writtenRecord = await Assert.That(written.Read()).IsTypeOf<ShapefileRecord>();
            var expectedRecord = await Assert.That(expected.Read()).IsTypeOf<ShapefileRecord>();

            await CheckRecord(writtenRecord!, expectedRecord!);

            static async Task CheckRecord(System.Data.IDataRecord first, System.Data.IDataRecord second)
            {
                for (var i = 0; i < first.FieldCount; i++)
                {
                    var firstValue = first[i];
                    var secondValue = second[i];

                    _ = await Assert.That(firstValue).IsEqualTo(secondValue);
                }
            }
        }

        _ = await Assert.That(shpStream.ToArray()).IsEquivalentTo(Resources.GetBytes("multipnt.shp"));
        _ = await Assert.That(shxStream.ToArray()).IsEquivalentTo(Resources.GetBytes("multipnt.shx"));
        _ = await Assert.That(dbfStream.ToArray().Skip(5)).IsEquivalentTo(Resources.GetBytes("multipnt.dbf").Skip(5));
    }
}