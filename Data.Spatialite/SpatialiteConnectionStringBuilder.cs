// -----------------------------------------------------------------------
// <copyright file="SpatialiteConnectionStringBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data.Spatialite;

/// <summary>
/// Provides a simple way to create and manage the contents of connection strings used by <see cref="SpatialiteConnection"/>.
/// </summary>
public class SpatialiteConnectionStringBuilder : Microsoft.Data.Sqlite.SqliteConnectionStringBuilder
{
    private const string InitSpatialMetadataKeyword = "Init Spatial Metdata";

    private static readonly string[] ValidKeywordsList = [InitSpatialMetadataKeyword];

    private static readonly Dictionary<string, Keywords> KeywordsDictionary = new(1, StringComparer.OrdinalIgnoreCase)
    {
        { InitSpatialMetadataKeyword, Keywords.InitSpatialMetadata },
    };

    private Action<string, object>? baseIndexer;

    private Func<string, bool>? baseShouldSerialize;

    private InitSpatialMetadataMode initSpatialMetadata = InitSpatialMetadataMode.All;

    /// <summary>
    /// Initialises a new instance of the <see cref="SpatialiteConnectionStringBuilder"/> class.
    /// </summary>
    public SpatialiteConnectionStringBuilder()
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="SpatialiteConnectionStringBuilder"/> class.
    /// </summary>
    /// <inheritdoc cref="Microsoft.Data.Sqlite.SqliteConnectionStringBuilder(string)" />
    public SpatialiteConnectionStringBuilder(string connectionString)
        : base(connectionString)
    {
    }

    private enum Keywords
    {
        InitSpatialMetadata,
    }

    /// <summary>
    /// Gets or sets the value of the init spatial metadata mode.
    /// </summary>
    public InitSpatialMetadataMode InitSpatialMetadata
    {
        get => this.initSpatialMetadata;
        set
        {
            if (this.initSpatialMetadata != value)
            {
                this.initSpatialMetadata = value;
                GetBaseIndexer().Invoke(InitSpatialMetadataKeyword, this.initSpatialMetadata);

                Action<string, object> GetBaseIndexer()
                {
                    return this.baseIndexer ??= GetBaseIndexerCore();

                    Action<string, object> GetBaseIndexerCore()
                    {
                        var handle = typeof(System.Data.Common.DbConnectionStringBuilder).GetProperty("Item")?.GetSetMethod()?.MethodHandle.GetFunctionPointer() ?? throw new InvalidOperationException();
                        return (Action<string, object>)Activator.CreateInstance(typeof(Action<string, object>), this, handle)!;
                    }
                }
            }
        }
    }

    /// <inheritdoc/>
    public override System.Collections.ICollection Keys
    {
        get
        {
            return new System.Collections.ObjectModel.ReadOnlyCollection<string>(GetKeys());

            string[] GetKeys()
            {
                var keys = (IList<string>)base.Keys!;
                return [.. keys, .. ValidKeywordsList];
            }
        }
    }

    /// <inheritdoc/>
    public override System.Collections.ICollection Values
    {
        get
        {
            return new System.Collections.ObjectModel.ReadOnlyCollection<object?>(GetValues());

            object?[] GetValues()
            {
                var values = (IList<object?>)base.Values;
                return [.. values, .. GetValuesCore()];

                IEnumerable<object?> GetValuesCore()
                {
                    for (var i = 0; i < ValidKeywordsList.Length; i++)
                    {
                        yield return this.GetAt((Keywords)i);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this should be serialized as a <see cref="Microsoft.Data.Sqlite.SqliteConnectionStringBuilder"/>.
    /// </summary>
    internal bool SerializeAsSqlite { get; set; }

    /// <inheritdoc/>
    public override object? this[string keyword]
    {
        get => TryGetIndex(keyword, out var keywordValue) ? this.GetAt(keywordValue) : base[keyword];
        set
        {
            if (value is null)
            {
                _ = this.Remove(keyword);
                return;
            }

            switch (GetIndex(keyword))
            {
                case Keywords.InitSpatialMetadata:
                    this.InitSpatialMetadata = ConvertToEnum<InitSpatialMetadataMode>(value);
                    return;

                default:
                    base[keyword] = value;
                    return;
            }

            static Keywords GetIndex(string keyword) => TryGetIndex(keyword, out var value) ? value : (Keywords)(-1);

            static TEnum ConvertToEnum<TEnum>(object value)
                where TEnum : struct
            {
                var enumValue = value switch
                {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
                    string stringValue => Enum.Parse<TEnum>(stringValue, ignoreCase: true),
#else
                    string stringValue => (TEnum)Enum.Parse(typeof(TEnum), stringValue, ignoreCase: true),
#endif
                    TEnum t => t,
                    { } v when v.GetType().IsEnum => throw new ArgumentException(string.Format(Properties.Resources.Culture, Properties.Resources.ConvertFailed, value.GetType(), typeof(TEnum)), nameof(value)),
                    { } v => (TEnum)Enum.ToObject(typeof(TEnum), v),
                };

                return Enum.IsDefined(typeof(TEnum), enumValue)
                    ? enumValue
                    : throw new ArgumentOutOfRangeException(nameof(value), value, string.Format(Properties.Resources.Culture, Properties.Resources.InvalidEnumValue, typeof(TEnum), enumValue));
            }
        }
    }

    /// <inheritdoc/>
    public override void Clear()
    {
        base.Clear();
        for (var i = 0; i < ValidKeywordsList.Length; i++)
        {
            this.Reset((Keywords)i);
        }
    }

    /// <inheritdoc/>
    public override bool ContainsKey(string keyword) => KeywordsDictionary.ContainsKey(keyword) || base.ContainsKey(keyword);

    /// <inheritdoc/>
    public override bool Remove(string keyword)
    {
        if (KeywordsDictionary.TryGetValue(keyword, out var value) && base.Remove(ValidKeywordsList[(int)value]))
        {
            this.Reset(value);
            return true;
        }

        return base.Remove(keyword);
    }

    /// <inheritdoc/>
    public override bool ShouldSerialize(string keyword)
    {
        return KeywordsDictionary.TryGetValue(keyword, out var value)
            ? !this.SerializeAsSqlite && GetBaseShouldSerialize().Invoke(ValidKeywordsList[(int)value])
            : base.ShouldSerialize(keyword);

        Func<string, bool> GetBaseShouldSerialize()
        {
            return this.baseShouldSerialize ??= GetBaseShouldSerializeCore();

            Func<string, bool> GetBaseShouldSerializeCore()
            {
                var handle = typeof(System.Data.Common.DbConnectionStringBuilder).GetMethod(nameof(this.ShouldSerialize))?.MethodHandle.GetFunctionPointer() ?? throw new InvalidOperationException();
                return (Func<string, bool>)Activator.CreateInstance(typeof(Func<string, bool>), this, handle)!;
            }
        }
    }

    /// <inheritdoc/>
    public override bool TryGetValue(string keyword, out object? value)
    {
        if (KeywordsDictionary.TryGetValue(keyword, out var keywordValue))
        {
            value = this.GetAt(keywordValue);
            return true;
        }

        return base.TryGetValue(keyword, out value);
    }

    private static bool TryGetIndex(string keyword, out Keywords value) => KeywordsDictionary.TryGetValue(keyword, out value);

    private object GetAt(Keywords index) => index switch
    {
        Keywords.InitSpatialMetadata => this.InitSpatialMetadata,
        _ => default,
    };

    private void Reset(Keywords index) =>
        this.initSpatialMetadata = index switch
        {
            Keywords.InitSpatialMetadata => InitSpatialMetadataMode.All,
            _ => this.initSpatialMetadata,
        };
}