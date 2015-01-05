//-----------------------------------------------------------------------
// <copyright file="DependencyEnumerationException.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine
{
    using System;

    /// <summary>
    /// Exception used for errors in dependency enumeration logic
    /// </summary>
    public sealed class DependencyEnumerationException : EngineExceptionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyEnumerationException"/> class
        /// </summary>
        /// <param name="message">Exception message</param>
        public DependencyEnumerationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyEnumerationException"/> class
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public DependencyEnumerationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
