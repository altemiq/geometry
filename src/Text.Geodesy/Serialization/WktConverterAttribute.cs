// -----------------------------------------------------------------------
// <copyright file="WktConverterAttribute.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.Geodesy.Serialization;

/// <summary>
/// When placed on a property or type, specifies the converter type to use.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property, AllowMultiple = false)]
public class WktConverterAttribute : WktAttribute
{
    /// <summary>
    /// Initialises a new instance of the <see cref="WktConverterAttribute"/> class with the specified converter type.
    /// </summary>
    /// <param name="converterType">The type of the converter.</param>
    public WktConverterAttribute(
#if NET6_0_OR_GREATER
        [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
#endif
        Type converterType) => this.ConverterType = converterType;

    /// <summary>
    /// Initialises a new instance of the <see cref="WktConverterAttribute"/> class.
    /// </summary>
    protected WktConverterAttribute()
    {
    }

    /// <summary>
    /// Gets the type of the <see cref="WktConverterAttribute"/>, or <see langword="null"/> if it was created without a type.
    /// </summary>
#if NET6_0_OR_GREATER
    [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
#endif
    public Type? ConverterType { get; }

    /// <summary>
    /// When overridden in a derived class and <see cref="ConverterType"/> is <see langword="null"/>, allows the derived class to create a <see cref="WktConverter"/> in order to pass additional state.
    /// </summary>
    /// <param name="typeToConvert">The type of the converter.</param>
    /// <returns>The custom converter.</returns>
    public virtual WktConverter? CreateConverter(Type typeToConvert) => null;
}