// -----------------------------------------------------------------------
// <copyright file="DummyTest.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data.Spatialite;

public class DummyTest
{
    [Test]
    public async Task Dummy() => await Assert.That(bool.Parse(bool.TrueString)).IsTrue();
}
