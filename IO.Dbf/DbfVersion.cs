// -----------------------------------------------------------------------
// <copyright file="DbfVersion.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Dbf;

/// <summary>
/// The <see cref="Dbf"/> version.
/// </summary>
public readonly struct DbfVersion : IEquatable<DbfVersion>, IEquatable<byte>
{
    /// <summary>
    /// dBASE III w/o memo file.
    /// </summary>
    public static readonly DbfVersion DBase3WithoutMemo = new(0x03);

    /// <summary>
    /// dBASE IV w/o memo file.
    /// </summary>
    public static readonly DbfVersion DBase4WithoutMemo = new(0x04);

    /// <summary>
    /// dBASE V w/o memo file.
    /// </summary>
    public static readonly DbfVersion DBase5WithoutMemo = new(0x05);

    /// <summary>
    /// dBASE IV with memo file.
    /// </summary>
    public static readonly DbfVersion DBase4WithMemoAndFloat = new(0x7b);

    /// <summary>
    /// dBASE III with memo file.
    /// </summary>
    public static readonly DbfVersion DBase3WithMemo = new(0x83);

    /// <summary>
    /// dBASE IV with memo file.
    /// </summary>
    public static readonly DbfVersion DBase4WithMemo = new(0x8b);

    private const byte MemoFlag = 1 << 3;

    private const byte DbtFlag = 1 << 7;

    private readonly byte value;

    private DbfVersion(byte value) => this.value = value;

    /// <summary>
    /// Gets a value indicating whether a MEMO file is required.
    /// </summary>
    public bool Memo => (this.value & MemoFlag) is not 0;

    /// <summary>
    /// Gets a value indicating whether a DBT file is required.
    /// </summary>
    public bool Dbt => (this.value & DbtFlag) is not 0;

    /// <summary>
    /// Gets the version.
    /// </summary>
    public byte Version => (byte)(this.value & ~248);

    /// <summary>
    /// Converts the byte value into a <see cref="DbfVersion"/>.
    /// </summary>
    /// <param name="value">The byte value.</param>
    public static implicit operator DbfVersion(byte value) => new(value);

    /// <summary>
    /// Converts the <see cref="DbfVersion"/> into a byte value.
    /// </summary>
    /// <param name="value">The version.</param>
    public static implicit operator byte(DbfVersion value) => value.value;

    /// <summary>
    /// Determines if the two specified <see cref="DbfVersion"/> instance are equal.
    /// </summary>
    /// <param name="lhs">The left-hand side.</param>
    /// <param name="rhs">The right-hand side.</param>
    /// <returns><paramref name="lhs"/> and <paramref name="rhs"/> are equal.</returns>
    public static bool operator ==(DbfVersion lhs, DbfVersion rhs) => lhs.value == rhs.value;

    /// <summary>
    /// Determines if the two specified <see cref="DbfVersion"/> instance are not-equal.
    /// </summary>
    /// <param name="lhs">The left-hand side.</param>
    /// <param name="rhs">The right-hand side.</param>
    /// <returns><paramref name="lhs"/> and <paramref name="rhs"/> are not-equal.</returns>
    public static bool operator !=(DbfVersion lhs, DbfVersion rhs) => lhs.value != rhs.value;

    /// <summary>
    /// Determines if the <see cref="DbfVersion"/> and <see cref="byte"/> instance are equal.
    /// </summary>
    /// <param name="lhs">The left-hand side.</param>
    /// <param name="rhs">The right-hand side.</param>
    /// <returns><paramref name="lhs"/> and <paramref name="rhs"/> are equal.</returns>
    public static bool operator ==(DbfVersion lhs, byte rhs) => lhs.value == rhs;

    /// <summary>
    /// Determines if the <see cref="DbfVersion"/> and <see cref="byte"/> instance are not-equal.
    /// </summary>
    /// <param name="lhs">The left-hand side.</param>
    /// <param name="rhs">The right-hand side.</param>
    /// <returns><paramref name="lhs"/> and <paramref name="rhs"/> are not-equal.</returns>
    public static bool operator !=(DbfVersion lhs, byte rhs) => lhs.value != rhs;

    /// <summary>
    /// Determines if the <see cref="byte"/> and <see cref="DbfVersion"/> instance are equal.
    /// </summary>
    /// <param name="lhs">The left-hand side.</param>
    /// <param name="rhs">The right-hand side.</param>
    /// <returns><paramref name="lhs"/> and <paramref name="rhs"/> are equal.</returns>
    public static bool operator ==(byte lhs, DbfVersion rhs) => lhs == rhs.value;

    /// <summary>
    /// Determines if the <see cref="byte"/> and <see cref="DbfVersion"/> instance are not-equal.
    /// </summary>
    /// <param name="lhs">The left-hand side.</param>
    /// <param name="rhs">The right-hand side.</param>
    /// <returns><paramref name="lhs"/> and <paramref name="rhs"/> are not-equal.</returns>
    public static bool operator !=(byte lhs, DbfVersion rhs) => lhs != rhs.value;

    /// <summary>
    /// Determines whether the version is defined.
    /// </summary>
    /// <param name="version">The version to check.</param>
    /// <returns><see langword="true"/> if <paramref name="version"/> is defined; otherwise <see langword="false"/>.</returns>
    public static bool IsDefined(DbfVersion version) => version.Equals(DBase3WithoutMemo)
        || version.Equals(DBase4WithoutMemo)
        || version.Equals(DBase5WithoutMemo)
        || version.Equals(DBase4WithMemoAndFloat)
        || version.Equals(DBase3WithMemo)
        || version.Equals(DBase4WithMemo);

    /// <inheritdoc/>
    public bool Equals(DbfVersion other) => this.value == other.value;

    /// <inheritdoc/>
    public bool Equals(byte other) => this.value == other;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj switch
    {
        DbfVersion version => this.Equals(version),
        byte @byte => this.Equals(@byte),
        _ => false,
    };

    /// <inheritdoc/>
    public override int GetHashCode() => this.value.GetHashCode();

    /// <inheritdoc/>
    public override string ToString()
    {
#pragma warning disable IDE0046
        if (this.Equals(DBase3WithoutMemo))
        {
            return "dBASE III w/o memo file";
        }

        if (this.Equals(DBase4WithoutMemo))
        {
            return "dBASE IV w/o memo file";
        }

        if (this.Equals(DBase5WithoutMemo))
        {
            return "dBASE V w/o memo file";
        }

        if (this.Equals(DBase3WithMemo))
        {
            return "dBASE III with memo file";
        }

        if (this.Equals(DBase4WithMemoAndFloat))
        {
            return "dBASE IV with memo file and float";
        }

        if (this.Equals(DBase4WithMemo))
        {
            return "dBASE IV with memo file";
        }

        return this.value.ToString(System.Globalization.CultureInfo.InvariantCulture);
#pragma warning restore IDE0046
    }
}