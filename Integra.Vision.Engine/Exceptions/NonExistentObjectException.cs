//-----------------------------------------------------------------------
// <copyright file="NonExistentObjectException.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Exceptions
{
    using System;

    /// <summary>
    /// Exception used when the user refers to a nonexistent object
    /// </summary>
    public sealed class NonExistentObjectException : EngineExceptionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NonExistentObjectException"/> class
        /// </summary>
        /// <param name="message">Exception message</param>
        public NonExistentObjectException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NonExistentObjectException"/> class
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public NonExistentObjectException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
