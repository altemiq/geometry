// -----------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson;

using System.Text.Json;

internal static class Extensions
{
    /// <summary>
    /// Replaces all characters that might conflict with formatting placeholders with their escaped counterparts.
    /// </summary>
    public static string EscapePlaceholders(this string value) => value.Replace("{", "{{").Replace("}", "}}");

    public static TUnit.Assertions.AssertionBuilders.InvokableValueAssertionBuilder<string> IsSameJsonAs(this TUnit.Assertions.AssertConditions.Interfaces.IValueSource<string> valueSource, string expected, [System.Runtime.CompilerServices.CallerArgumentExpression(nameof(expected))] string doNotPopulateThisValue1 = "") =>
        valueSource.RegisterAssertion(
            assertCondition: new JsonEqualsExpectedAssertCondition(expected),
            argumentExpressions: [doNotPopulateThisValue1]
            );

    public class JsonEqualsExpectedAssertCondition(string expected) : TUnit.Assertions.AssertConditions.ExpectedValueAssertCondition<string, string>(expected)
    {
        protected override string GetExpectation() => $"to be equal to \"{this.ExpectedValue}\"";

        protected override ValueTask<TUnit.Assertions.AssertConditions.AssertionResult> GetResult(string? actualValue, string? expectedValue)
        {
            if (actualValue is null)
            {
                return TUnit.Assertions.AssertConditions.AssertionResult
                    .FailIf(
                        expectedValue is not null,
                        "it was null");
            }

            if (expectedValue is null)
            {
                return TUnit.Assertions.AssertConditions.AssertionResult
                    .Fail("it was not null");
            }

            var subjectJsonNode = JsonDocument.Parse(actualValue);
            var expectedJsonNode = JsonDocument.Parse(expectedValue);

            var comparer = new JsonElementComparer();

            return TUnit.Assertions.AssertConditions.AssertionResult
                .FailIf(
                    !comparer.Equals(subjectJsonNode.RootElement, expectedJsonNode.RootElement),
                    $"found \"{actualValue}\"");
        }
    }

    private class JsonElementComparer(int maxHashDepth) : IEqualityComparer<JsonElement>
    {
        public JsonElementComparer()
            : this(-1)
        {
        }

        private int MaxHashDepth { get; } = maxHashDepth;

        public bool Equals(JsonElement x, JsonElement y)
        {
            return x.ValueKind == y.ValueKind
                && x.ValueKind switch
                {
                    JsonValueKind.Null or JsonValueKind.True or JsonValueKind.False or JsonValueKind.Undefined => true,
                    JsonValueKind.Number => CompareNumbers(x, y),
                    JsonValueKind.String => x.GetString() == y.GetString(), // Do not use GetRawText() here, it does not automatically resolve JSON escape sequences to their corresponding characters.
                    JsonValueKind.Array => x.EnumerateArray().SequenceEqual(y.EnumerateArray(), this),
                    JsonValueKind.Object => CompareObject(x, y, this.Equals),
                    _ => throw new JsonException(string.Format($"Unknown {nameof(JsonValueKind)} {{0}}", x.ValueKind)),
                };

            static bool CompareNumbers(JsonElement x, JsonElement y)
            {
                var first = x.GetDouble();
                var second = y.GetDouble();

                return System.Math.Round(first, 13).Equals(System.Math.Round(second, 13));
            }

            static bool CompareObject(JsonElement x, JsonElement y, Func<JsonElement, JsonElement, bool> equals)
            {
                // Surprisingly, JsonDocument fully supports duplicate property names.
                // I.e. it's perfectly happy to parse {"Value":"a", "Value" : "b"} and will store both
                // key/value pairs inside the document!
                // A close reading of https://www.rfc-editor.org/rfc/rfc8259#section-4 seems to indicate that
                // such objects are allowed but not recommended, and when they arise, interpretation of 
                // identically-named properties is order-dependent.
                // So stably sorting by name then comparing values seems the way to go.
                var xPropertiesUnsorted = x.EnumerateObject().ToList();
                var yPropertiesUnsorted = y.EnumerateObject().ToList();
                if (xPropertiesUnsorted.Count != yPropertiesUnsorted.Count)
                {
                    return false;
                }

                var xProperties = xPropertiesUnsorted.OrderBy(static p => p.Name, StringComparer.Ordinal);
                var yProperties = yPropertiesUnsorted.OrderBy(static p => p.Name, StringComparer.Ordinal);
                foreach (var (px, py) in xProperties.Zip(yProperties, static (x, y) => (x, y)))
                {
                    if (px.Name != py.Name || !equals(px.Value, py.Value))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public int GetHashCode(JsonElement obj)
        {
            var hash = new HashCode();
            this.ComputeHashCode(obj, ref hash, 0);
            return hash.ToHashCode();
        }

        private void ComputeHashCode(JsonElement obj, ref HashCode hash, int depth)
        {
            hash.Add(obj.ValueKind);

            switch (obj.ValueKind)
            {
                case JsonValueKind.Null:
                case JsonValueKind.True:
                case JsonValueKind.False:
                case JsonValueKind.Undefined:
                    break;

                case JsonValueKind.Number:
                    hash.Add(obj.GetRawText());
                    break;

                case JsonValueKind.String:
                    hash.Add(obj.GetString());
                    break;

                case JsonValueKind.Array:
                    if (depth != this.MaxHashDepth)
                    {
                        foreach (var item in obj.EnumerateArray())
                        {
                            this.ComputeHashCode(item, ref hash, depth + 1);
                        }
                    }
                    else
                    {
                        hash.Add(obj.GetArrayLength());
                    }

                    break;

                case JsonValueKind.Object:
                    foreach (var property in obj.EnumerateObject().OrderBy(static p => p.Name, StringComparer.Ordinal))
                    {
                        hash.Add(property.Name);
                        if (depth != this.MaxHashDepth)
                        {
                            this.ComputeHashCode(property.Value, ref hash, depth + 1);
                        }
                    }

                    break;

                default:
                    throw new JsonException($"Unknown JsonValueKind {obj.ValueKind}");
            }
        }
    }
}