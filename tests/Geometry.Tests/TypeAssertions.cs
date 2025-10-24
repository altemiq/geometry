namespace Altemiq.Geometry;

using System.ComponentModel;
using TUnit.Assertions.Attributes;

public static class TypeAssertions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = "to be assignable from")]
    public static bool IsAssignableFrom(this object obj, Type other)
    {
        return obj is not null && obj.GetType().IsAssignableFrom(other);
    }
    
    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = "to be assignable to")]
    public static bool IsAssignableTo(this object obj, Type other)
    {
        return obj is not null && obj.GetType().IsAssignableTo(other);
    }
}