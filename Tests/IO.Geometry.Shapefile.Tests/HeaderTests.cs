// -----------------------------------------------------------------------
// <copyright file="HeaderTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

using TUnit.Assertions.AssertConditions.Throws;

public class HeaderTests
{
    private static readonly byte[] HeaderBytes = [0x00, 0x00, 0x27, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0xBE, 0xE8, 0x03, 0x00, 0x00, 0x0B, 0x00, 0x00, 0x00, 0xBA, 0xED, 0x53, 0xF9, 0xCA, 0x4E, 0x09, 0xC0, 0x15, 0x35, 0xFC, 0x6D, 0x53, 0x51, 0xE3, 0xBF, 0x3F, 0x5E, 0x73, 0x0F, 0xF4, 0xD9, 0x27, 0x40, 0x3E, 0xF1, 0x39, 0x53, 0xCB, 0x15, 0x25, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];

    [Test]
    public async Task ReadHeader()
    {
        using var memoryStream = new MemoryStream(HeaderBytes);
        var header = Header.ReadFrom(memoryStream);
        _ = await Assert.That(header).IsNotNull();
        _ = await Assert.That(header.ShpType).IsEqualTo(ShpType.PointZ);
        _ = await Assert.That(header.Extents).IsEquivalentTo(new EnvelopeZM(-3.163473079561041, -0.6036774776600021, 0, 0, 11.925690157731081, 10.54256687242798, 0, 0));
    }

    [Test]
    public async Task WriteHeader()
    {
        var header = new Header(ShpType.PointZ, -3.163473079561041, 11.925690157731081, -0.6036774776600021, 10.54256687242798);
        SetFileLength(header, 446U);

        using var memoryStream = new MemoryStream();
        header.CopyTo(memoryStream);

        _ = await Assert.That(memoryStream.ToArray()).IsEquivalentTo(HeaderBytes);

        static void SetFileLength(Header header, uint fileLength)
        {
            var field = typeof(Header).GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Single(static f => f.FieldType == typeof(uint));
            field.SetValue(header, fileLength);
        }
    }

    [Test]
    public async Task ReadHeaderAndIgnoreSignature()
    {
        using var memoryStream = new MemoryStream(HeaderBytes);
        memoryStream.Position += 4;
        var header = Header.ReadFrom(memoryStream, false);
        _ = await Assert.That(header).IsNotNull();
        _ = await Assert.That(header.ShpType).IsEqualTo(ShpType.PointZ);
        _ = await Assert.That(header.Extents).IsEquivalentTo(new EnvelopeZM(-3.163473079561041, -0.6036774776600021, 0, 0, 11.925690157731081, 10.54256687242798, 0, 0));
    }

    [Test]
    public async Task ReadHeaderTooSmall()
    {
        using var memoryStream = new MemoryStream(new byte[Header.Length]);
        await Assert.That(() => Header.ReadFrom(memoryStream, false)).Throws<InsufficientDataException>();
    }

    [Test]
    public async Task ReadIncorrectFileCode()
    {
        using var memoryStream = new MemoryStream(new byte[Header.Size]);
        await Assert.That(() => Header.ReadFrom(memoryStream)).Throws<InvalidDataException>().WithMessageMatching(TUnit.Assertions.AssertConditions.StringMatcher.AsWildcard("Invalid FileCode*"));
    }

    [Test]
    public async Task ReadIncorrectVersion()
    {
        using var memoryStream = new MemoryStream(new byte[Header.Size]);
        memoryStream.Position += 4;
        await Assert.That(() => Header.ReadFrom(memoryStream, false)).Throws<InvalidDataException>().WithMessageMatching(TUnit.Assertions.AssertConditions.StringMatcher.AsWildcard("Invalid Version*"));
    }
}