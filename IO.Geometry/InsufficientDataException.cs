// -----------------------------------------------------------------------
// <copyright file="InsufficientDataException.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

/// <summary>
/// Represents an exception that may be thrown when input contains insufficient data.
/// </summary>
[Serializable]
public class InsufficientDataException : Exception
{
    private readonly bool hasMessage;

    /// <summary>
    /// Initialises a new instance of the <see cref="InsufficientDataException"/> class.
    /// </summary>
    /// <inheritdoc cref="Exception()"/>
    public InsufficientDataException()
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="InsufficientDataException"/> class.
    /// </summary>
    /// <inheritdoc cref="Exception(string)"/>
    public InsufficientDataException(string message)
        : base(message) => this.hasMessage = message is not null;

    /// <summary>
    /// Initialises a new instance of the <see cref="InsufficientDataException"/> class.
    /// </summary>
    /// <inheritdoc cref="Exception(string, Exception)"/>
    public InsufficientDataException(string message, Exception inner)
        : base(message, inner) => this.hasMessage = message is not null;

#if NETSTANDARD2_0_OR_GREATER || NET20_OR_GREATER || NETCOREAPP2_0_OR_GREATER
    /// <summary>
    /// Initialises a new instance of the <see cref="InsufficientDataException"/> class.
    /// </summary>
    /// <inheritdoc cref="Exception(System.Runtime.Serialization.SerializationInfo, System.Runtime.Serialization.StreamingContext)"/>
    protected InsufficientDataException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context)
        : base(info, context)
    {
    }
#endif

    /// <summary>
    /// Gets the required data length.
    /// </summary>
    public long RequiredDataLength { get; init; }

    /// <summary>
    /// Gets the actual data length.
    /// </summary>
    public long ActualDataLength { get; init; }

    /// <inheritdoc/>
    public override string Message => this.hasMessage ? base.Message : string.Format(Properties.Resources.Culture, Properties.Resources.InsufficientData, this.RequiredDataLength, this.ActualDataLength);
}