// -----------------------------------------------------------------------
// <copyright file="ExceptionHelper.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson.Stac;

/// <summary>
/// The <see cref="Exception"/> helper.
/// </summary>
internal static class ExceptionHelper
{
    /// <summary>
    /// Throw an exception if the specified value is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of value.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="propertyName">The property name.</param>
    /// <returns><paramref name="value"/> verified as not being null.</returns>
    /// <exception cref="InvalidOperationException"><paramref name="value"/> is <see langword="null"/>.</exception>
    public static T ThrowIfNull<T>(T? value, [System.Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? propertyName = null) => value ?? throw new InvalidOperationException($"{propertyName} cannot be null.");
}