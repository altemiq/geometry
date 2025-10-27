// -----------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson.Stac;

using System.ComponentModel;
using System.Text.Json;
using TUnit.Assertions.Attributes;
using TUnit.Assertions.Core;

internal static class Extensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = "{expected} to be equal")]
    public static AssertionResult IsSameJsonAs(this string source, string expected) => (source, expected) switch
    {
        (null, _) => AssertionResult.FailIf(expected is not null, "it was null"),
        (_, null) => AssertionResult.Failed("it was null"),
        _ => AssertionResult.FailIf(!CompareJson(source, expected), $"found {source}"),
    };

    private static bool CompareJson(string actualValue, string expectedValue)
    {
        var comparer = new JsonElementComparer();

        return comparer.Equals(JsonDocument.Parse(actualValue).RootElement, JsonDocument.Parse(expectedValue).RootElement);
    }

    private class JsonElementComparer(int maxHashDepth) : IEqualityComparer<JsonElement>, IComparer<JsonElement>
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
                    JsonValueKind.String => CompareStrings(x, y),
                    JsonValueKind.Array => CompareArrays(x, y, this),
                    JsonValueKind.Object => CompareObject(x, y, Equals),
                    _ => throw new JsonException(string.Format(System.Globalization.CultureInfo.CurrentCulture, "Unknown JsonValueKind {0}", x.ValueKind)),
                };

            static bool CompareNumbers(JsonElement x, JsonElement y)
            {
                return Math.Round(x.GetDouble(), 13).Equals(Math.Round(y.GetDouble(), 13));
            }

            static bool CompareArrays(JsonElement x, JsonElement y, JsonElementComparer comparer)
            {
                var firstArray = x.EnumerateArray().OrderBy(static x => x, comparer);
                var secondArray = y.EnumerateArray().OrderBy(static y => y, comparer);

                return firstArray.SequenceEqual(secondArray, comparer);
            }

            static bool CompareStrings(JsonElement x, JsonElement y)
            {
                // Do not use GetRawText() here, it does not automatically resolve JSON escape sequences to their corresponding characters.
                return x.GetString() == y.GetString();
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
            ComputeHashCode(obj, ref hash, 0);
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
                    if (depth != MaxHashDepth)
                    {
                        foreach (var item in obj.EnumerateArray())
                        {
                            ComputeHashCode(item, ref hash, depth + 1);
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
                        if (depth != MaxHashDepth)
                        {
                            ComputeHashCode(property.Value, ref hash, depth + 1);
                        }
                    }

                    break;

                default:
                    throw new JsonException(string.Format(System.Globalization.CultureInfo.CurrentCulture, "Unknown JsonValueKind {0}", obj.ValueKind));
            }
        }

        public int Compare(JsonElement x, JsonElement y)
        {
            return StringComparer.Ordinal.Compare(GetString(x), GetString(y));

            static string GetString(JsonElement element)
            {
                return element.GetRawText();
            }
        }
    }
}