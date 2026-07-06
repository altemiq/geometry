// -----------------------------------------------------------------------
// <copyright file="TabField.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapInfo;

/// <summary>
/// The <c>TAB</c> field.
/// </summary>
/// <param name="Name">The name.</param>
/// <param name="Type">The type.</param>
/// <param name="Length">The length.</param>
/// <param name="Precision">The precision.</param>
public record TabField(string Name, TabFieldType Type, int Length = 0, int Precision = 0);