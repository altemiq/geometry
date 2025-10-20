// -----------------------------------------------------------------------
// <copyright file="InvalidGeometryTypeException.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Geometry;

/// <summary>
/// Represents an <see cref="Exception"/> for when a geometry type is invalid.
/// </summary>
[Serializable]
public class InvalidGeometryTypeException : Exception
{
    /// <summary>
    /// Initialises a new instance of the <see cref="InvalidGeometryTypeException"/> class.
    /// </summary>
    public InvalidGeometryTypeException()
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="InvalidGeometryTypeException"/> class.
    /// </summary>
    /// <param name="message">A <see cref="string"/> that describes the error. The content of message is intended to be understood by humans. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
    public InvalidGeometryTypeException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="InvalidGeometryTypeException"/> class.
    /// </summary>
    /// <param name="message">A <see cref="string"/> that describes the error. The content of message is intended to be understood by humans. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
    /// <param name="inner">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
    public InvalidGeometryTypeException(string message, Exception inner)
        : base(message, inner)
    {
    }

#if NETSTANDARD2_0_OR_GREATER || NET20_OR_GREATER || NETCOREAPP2_0_OR_GREATER
    /// <inheritdoc cref="Exception(System.Runtime.Serialization.SerializationInfo, System.Runtime.Serialization.StreamingContext)" />
    protected InvalidGeometryTypeException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        : base(info, context)
    {
    }
#endif
}