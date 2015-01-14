//-----------------------------------------------------------------------
// <copyright file="BootException.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine
{
    using System;

    /// <summary>
    /// Boot engine exception.
    /// </summary>
    internal sealed class BootException : EngineExceptionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BootException"/> class
        /// </summary>
        /// <param name="message">Boot exception message</param>
        public BootException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BootException"/> class
        /// </summary>
        /// <param name="message">Boot exception message</param>
        /// <param name="innerException">Inner exception</param>
        public BootException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
