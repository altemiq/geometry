// -----------------------------------------------------------------------
// <copyright file="WindowsOnlyTestAttribute.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data.Spatialite;

public class WindowsOnlyTestAttribute() : SkipAttribute("Windows only test")
{
    public override Task<bool> ShouldSkip(TestRegisteredContext context) => Task.FromResult(!OperatingSystem.IsWindows());
}