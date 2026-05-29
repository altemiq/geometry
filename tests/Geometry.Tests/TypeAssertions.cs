namespace Altemiq.Geometry;

#pragma warning disable ConvertToExtensionBlock

using System.ComponentModel;
using TUnit.Assertions.Attributes;

public static class TypeAssertions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = "to be assignable to")]
    public static bool IsAssignableTo<TValue>(this TValue obj, Type other)
    {
        return obj is not null && obj.GetType().IsAssignableTo(other);
    }
}