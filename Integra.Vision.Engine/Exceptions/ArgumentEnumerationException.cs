//-----------------------------------------------------------------------
// <copyright file="ArgumentEnumerationException.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine
{
    using System;

    /// <summary>
    /// Exception used for errors in argument enumeration logic
    /// </summary>
    public sealed class ArgumentEnumerationException : EngineExceptionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentEnumerationException"/> class
        /// </summary>
        /// <param name="message">Exception message</param>
        public ArgumentEnumerationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentEnumerationException"/> class
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public ArgumentEnumerationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
