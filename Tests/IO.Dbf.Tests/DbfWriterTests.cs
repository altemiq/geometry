namespace Altemiq.IO.Dbf;

public class DbfWriterTests
{
    private static readonly object?[][] Data =
    [
        ["0507121", "CMP", "circular", "12", null, "no", "Good", null, new DateTime(2005, 7, 12), "10:56:30am", 5.2, 2.0, "Postprocessed Code", "GeoXT", new DateTime(2005, 7, 12), "10:56:52am", "New", "Driveway", "050712TR2819.cor", 2, 2, "MS4", 1331, 226625.0, 1131.323, 3.1, 1.3, 0.897088, 557904.898, 2212577.192, 401],
        ["0507122", "CMP", "circular", "12", null, "no", "Good", null, new DateTime(2005, 7, 12), "10:57:34am", 4.9, 2.0, "Postprocessed Code", "GeoXT", new DateTime(2005, 7, 12), "10:57:37am", "New", "Driveway", "050712TR2819.cor", 1, 1, "MS4", 1331, 226670.0, 1125.142, 2.8, 1.3, null, 557997.831, 2212576.868, 402],
        ["0507123", "CMP", "circular", "12", null, "no", "Good", null, new DateTime(2005, 7, 12), "10:59:03am", 5.4, 4.4, "Postprocessed Code", "GeoXT", new DateTime(2005, 7, 12), "10:59:12am", "New", "Driveway", "050712TR2819.cor", 1, 1, "MS4", 1331, 226765.0, 1127.57, 2.2, 3.5, null, 558184.757, 2212571.349, 403],
        ["0507125", "CMP", "circular", "12", null, "no", "Good", null, new DateTime(2005, 7, 12), "11:02:43am", 3.4, 1.5, "Postprocessed Code", "GeoXT", new DateTime(2005, 7, 12), "11:03:12am", "New", "Driveway", "050712TR2819.cor", 1, 1, "MS4", 1331, 227005.0, 1125.364, 3.2, 1.6, null, 558703.723, 2212562.547, 405],
        ["05071210", "CMP", "circular", "15", null, "no", "Good", null, new DateTime(2005, 7, 12), "11:15:20am", 3.7, 2.2, "Postprocessed Code", "GeoXT", new DateTime(2005, 7, 12), "11:14:52am", "New", "Driveway", "050712TR2819.cor", 1, 1, "MS4", 1331, 227705.0, 1118.605, 1.8, 2.1, null, 558945.763, 2212739.979, 410],
        ["05071216", "CMP", "circular", "12", null, "no", "Good", null, new DateTime(2005, 7, 12), "12:13:23pm", 4.4, 1.8, "Postprocessed Code", "GeoXT", new DateTime(2005, 7, 12), "12:13:57pm", "New", "Driveway", "050712TR2819.cor", 1, 1, "MS4", 1331, 231250.0, 1117.39, 3.1, 1.2, null, 559024.234, 2212856.927, 416],
        ["05071217", "CMP", "circular", "12", null, "no", "Good", null, new DateTime(2005, 7, 12), "12:16:46pm", 4.4, 1.8, "Postprocessed Code", "GeoXT", new DateTime(2005, 7, 12), "12:17:12pm", "New", "Driveway", "050712TR2819.cor", 1, 1, "MS4", 1331, 231445.0, 1125.714, 3.2, 1.3, null, 559342.534, 2213340.161, 417],
        ["05071219", "CMP", "circular", "12", null, "no", "Plugged", null, new DateTime(2005, 7, 12), "12:22:55pm", 4.4, 1.8, "Postprocessed Code", "GeoXT", new DateTime(2005, 7, 12), "12:22:22pm", "New", "Driveway", "050712TR2819.cor", 1, 1, "MS4", 1331, 231755.0, 1110.786, 2.5, 1.1, null, 559578.776, 2213560.247, 419],
        ["05071224", "CMP", "circular", "12", null, "no", "Good", null, new DateTime(2005, 7, 12), "12:37:17pm", 4.1, 1.7, "Postprocessed Code", "GeoXT", new DateTime(2005, 7, 12), "12:38:32pm", "New", "Driveway", "050712TR2819.cor", 1, 1, "MS4", 1331, 232725.0, 1077.924, 2.8, 1.4, null, 560582.575, 2213759.022, 424],
        ["05071225", "CMP", "circular", "12", null, "no", "Good", null, new DateTime(2005, 7, 12), "12:39:48pm", 4.0, 1.7, "Postprocessed Code", "GeoXT", new DateTime(2005, 7, 12), "12:39:52pm", "New", "Driveway", "050712TR2819.cor", 1, 1, "MS4", 1331, 232805.0, 1082.99, 2.0, 1.0, null, 560678.501, 2213716.657, 425],
        ["05071229", "CMP", "circular", "12", null, "no", "Good", null, new DateTime(2005, 7, 12), "12:49:05pm", 3.7, 1.7, "Postprocessed Code", "GeoXT", new DateTime(2005, 7, 12), "12:49:07pm", "New", "Driveway", "050712TR2819.cor", 1, 1, "MS4", 1331, 233360.0, 1096.86, 2.4, 1.2, null, 560126.094, 2213720.301, 429],
        ["05071231", "CMP", "circular", "12", null, "no", "Plugged", null, new DateTime(2005, 7, 12), "12:53:58pm", 3.0, 1.6, "Postprocessed Code", "GeoXT", new DateTime(2005, 7, 12), "12:54:02pm", "New", "Driveway", "050712TR2819.cor", 1, 1, "MS4", 1331, 233655.0, 1105.113, 1.8, 1.1, null, 559952.331, 2213689.001, 431],
        ["05071232", "CMP", "circular", "12", null, "no", "Plugged", null, new DateTime(2005, 7, 12), "12:55:47pm", 3.5, 1.7, "Postprocessed Code", "GeoXT", new DateTime(2005, 7, 12), "12:55:47pm", "New", "Driveway", "050712TR2819.cor", 2, 2, "MS4", 1331, 233760.0, 1101.939, 2.1, 1.1, 1.223112, 559870.352, 2213661.918, 432],
        ["05071236", "CMP", "circular", "12", null, "no", "Plugged", null, new DateTime(2005, 7, 12), "01:08:40pm", 3.3, 1.6, "Postprocessed Code", "GeoXT", new DateTime(2005, 7, 12), "01:08:42pm", "New", "Driveway", "050712TR2819.cor", 1, 1, "MS4", 1331, 234535.0, 1125.517, 1.8, 1.2, null, 559195.031, 2213046.199, 436],
    ];

    [Test]
    public async Task WriteData()
    {
        var stream = new MemoryStream();
        using (var writer = new DbfWriter(stream, new() { WriteTrailingDecimals = true, WriteNullNumberAsSpace = true }, true))
        {
            var header = new DbfHeader(DbfVersion.DBase3WithoutMemo, default(System.Text.Encoding)!);
            header.AddColumns(
                new DbfColumn("Point_ID", DbfColumn.DbfColumnType.Character, 12),
                new DbfColumn("Type", DbfColumn.DbfColumnType.Character, 20),
                new DbfColumn("Shape", DbfColumn.DbfColumnType.Character, 20),
                new DbfColumn("Circular_D", DbfColumn.DbfColumnType.Character, 20),
                new DbfColumn("Non_circul", DbfColumn.DbfColumnType.Character, 60),
                new DbfColumn("Flow_prese", DbfColumn.DbfColumnType.Character, 20),
                new DbfColumn("Condition", DbfColumn.DbfColumnType.Character, 20),
                new DbfColumn("Comments", DbfColumn.DbfColumnType.Character, 60),
                new DbfColumn("Date_Visit", DbfColumn.DbfColumnType.Date, 8),
                new DbfColumn("Time", DbfColumn.DbfColumnType.Character, 10),
                new DbfColumn("Max_PDOP", DbfColumn.DbfColumnType.Number, 5, 1),
                new DbfColumn("Max_HDOP", DbfColumn.DbfColumnType.Number, 5, 1),
                new DbfColumn("Corr_Type", DbfColumn.DbfColumnType.Character, 36),
                new DbfColumn("Rcvr_Type", DbfColumn.DbfColumnType.Character, 36),
                new DbfColumn("GPS_Date", DbfColumn.DbfColumnType.Date, 8),
                new DbfColumn("GPS_Time", DbfColumn.DbfColumnType.Character, 10),
                new DbfColumn("Update_Sta", DbfColumn.DbfColumnType.Character, 36),
                new DbfColumn("Feat_Name", DbfColumn.DbfColumnType.Character, 20),
                new DbfColumn("Datafile", DbfColumn.DbfColumnType.Character, 20),
                new DbfColumn("Unfilt_Pos", DbfColumn.DbfColumnType.Number, 10),
                new DbfColumn("Filt_Pos", DbfColumn.DbfColumnType.Number, 10),
                new DbfColumn("Data_Dicti", DbfColumn.DbfColumnType.Character, 20),
                new DbfColumn("GPS_Week", DbfColumn.DbfColumnType.Number, 6),
                new DbfColumn("GPS_Second", DbfColumn.DbfColumnType.Number, 12, 3),
                new DbfColumn("GPS_Height", DbfColumn.DbfColumnType.Number, 16, 3),
                new DbfColumn("Vert_Prec", DbfColumn.DbfColumnType.Number, 16, 1),
                new DbfColumn("Horz_Prec", DbfColumn.DbfColumnType.Number, 16, 1),
                new DbfColumn("Std_Dev", DbfColumn.DbfColumnType.Number, 16, 6),
                new DbfColumn("Northing", DbfColumn.DbfColumnType.Number, 16, 3),
                new DbfColumn("Easting", DbfColumn.DbfColumnType.Number, 16, 3),
                new DbfColumn("Point_ID", DbfColumn.DbfColumnType.Number, 9));

            writer.Write(header, false);

            var count = 0;
            foreach (var data in Data)
            {
                writer.Write(data);
                count++;
            }

            writer.Update(count, false);
        }

        stream.Position = 0;
        int headerLength;
        uint recordCount;
        int recordLength;

        using (var written = new DbfReader(stream, true))
        {
            using var expected = new DbfReader(typeof(DbfReaderTests).Assembly.GetManifestResourceStream(typeof(DbfReaderTests), "Data." + "dbase_03.dbf") ?? throw new InvalidOperationException());

            await CheckHeader(written.Header, expected.Header);
            await CheckRecords(written, expected);

            headerLength = written.Header.HeaderLength;
            recordLength = written.Header.RecordLength;
            recordCount = written.Header.RecordCount;
        }

        var writtenBytes = stream.ToArray();
        var expectedBytes = GetBytes("dbase_03.dbf");

        var writtenHeaderBytes = writtenBytes.Skip(5).Take(headerLength - 5).ToArray();
        var expectedHeaderBytes = expectedBytes.Skip(5).Take(headerLength - 5).ToArray();

        _ = await Assert.That(writtenHeaderBytes).IsEquivalentTo(expectedHeaderBytes);

        for (var i = 0; i < recordCount; i++)
        {
            var offset = headerLength + (recordLength * i);
            var writtenRecordBytes = writtenBytes.Skip(offset).Take(recordLength).ToArray();
            var expectedRecordBytes = expectedBytes.Skip(offset).Take(recordLength).ToArray();

            _ = await Assert.That(writtenRecordBytes).IsEquivalentTo(expectedRecordBytes);
        }


        static async Task CheckHeader(DbfHeader first, DbfHeader second)
        {
            _ = await Assert.That(first.Version).IsEqualTo(second.Version);
            _ = await Assert.That(first.HeaderLength).IsEqualTo(second.HeaderLength);
            _ = await Assert.That(first.RecordCount).IsEqualTo(second.RecordCount);
            _ = await Assert.That(first.RecordLength).IsEqualTo(second.RecordLength);

            for (var i = 0; i < first.FieldCount; i++)
            {
                var firstColumn = first[i];
                var secondColumn = second[i];

                _ = await Assert.That(firstColumn.ColumnName).IsEqualTo(secondColumn.ColumnName);
                _ = await Assert.That(firstColumn.ColumnSize).IsEqualTo(secondColumn.ColumnSize);
                _ = await Assert.That(firstColumn.IsLong).IsEqualTo(secondColumn.IsLong);
                _ = await Assert.That(firstColumn.DataType).IsEqualTo(secondColumn.DataType);
                _ = await Assert.That(firstColumn.DbfType).IsEqualTo(secondColumn.DbfType);
                _ = await Assert.That(firstColumn.NumericPrecision).IsEqualTo(secondColumn.NumericPrecision);
                _ = await Assert.That(firstColumn.DataTypeName).IsEqualTo(secondColumn.DataTypeName);
            }
        }

        static async Task CheckRecords(DbfReader first, DbfReader second)
        {
            while (first.Read())
            {
                _ = await Assert.That(second.Read()).IsTrue();

                await CheckRecord(first, second);
            }

            _ = await Assert.That(second.Read()).IsFalse();

            static async Task CheckRecord(DbfReader first, DbfReader second)
            {
                for (var i = 0; i < first.FieldCount; i++)
                {
                    var firstValue = first[i];
                    var secondValue = second[i];

                    _ = await Assert.That(firstValue).IsEqualTo(secondValue);
                }
            }
        }
    }

    private static byte[] GetBytes(string name)
    {
        var stream = typeof(DbfReaderTests).Assembly.GetManifestResourceStream(typeof(DbfReaderTests), "Data." + name) ?? throw new InvalidOperationException();
        var bytes = new byte[stream.Length];
        _ = stream.Read(bytes, 0, bytes.Length);
        return bytes;
    }
}