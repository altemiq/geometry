// -----------------------------------------------------------------------
// <copyright file="GaiaConstants.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Spatialite;

/// <summary>
/// GAIA constants.
/// </summary>
internal static class GaiaConstants
{
    /// <summary>
    /// The GAIA bloc markers.
    /// </summary>
    public static class BlobMark
    {
        /// <summary>
        /// Start mark.
        /// </summary>
        public const byte Start = 0x00;

        /// <summary>
        /// Entity mark.
        /// </summary>
        public const byte Entity = 0x69;

        /// <summary>
        /// MBR mark.
        /// </summary>
        public const byte Mbr = 0x7C;

        /// <summary>
        /// End mark.
        /// </summary>
        public const byte End = 0xFE;
    }
}