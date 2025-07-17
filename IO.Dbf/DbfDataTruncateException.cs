// -----------------------------------------------------------------------
// <copyright file="DbfDataTruncateException.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Dbf;

/// <summary>
/// The exception for when the <see cref="Dbf"/> data is truncated.
/// </summary>
[Serializable]
public class DbfDataTruncateException : Exception
{
    /// <inheritdoc cref="Exception"/>
    public DbfDataTruncateException()
    {
    }

    /// <inheritdoc cref="Exception(string)"/>
    public DbfDataTruncateException(string message)
        : base(message)
    {
    }

    /// <inheritdoc cref="Exception(string,Exception)"/>
    public DbfDataTruncateException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    /// <inheritdoc cref="Exception(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)"/>
    protected DbfDataTruncateException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
      : base(info, context)
    {
    }
}