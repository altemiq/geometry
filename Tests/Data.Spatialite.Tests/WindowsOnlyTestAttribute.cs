// -----------------------------------------------------------------------
// <copyright file="WindowsOnlyTestAttribute.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data.Spatialite;

public class WindowsOnlyTestAttribute() : SkipAttribute("Windows only test")
{
    public override Task<bool> ShouldSkip(BeforeTestContext context) =>
#if NET5_0_OR_GREATER
        Task.FromResult(!OperatingSystem.IsWindows());
#else
        Task.FromResult(!System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows));
#endif
}
