// -----------------------------------------------------------------------
// <copyright file="Usings.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable SA1200
global using AuthorityCode = Altemiq.OneOf<string, int>;
global using NodeValue = Altemiq.OneOf<Altemiq.IO.Geodesy.WellKnownTextNode, string, double, Altemiq.IO.Geodesy.Literal>;
#pragma warning restore SA1200