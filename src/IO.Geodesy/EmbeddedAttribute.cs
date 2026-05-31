// -----------------------------------------------------------------------
// <copyright file="EmbeddedAttribute.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130, CheckNamespace
namespace Microsoft.CodeAnalysis;
#pragma warning restore IDE0130, CheckNamespace

/// <content>
/// The embedded attribute.
/// </content>
[AttributeUsage(AttributeTargets.All)]
internal sealed partial class EmbeddedAttribute : Attribute;