//-----------------------------------------------------------------------
// <copyright file="InterpretationException.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine
{
    using System;

    /// <summary>
    /// Exception used for errors in interpretation logic
    /// </summary>
    public sealed class InterpretationException : EngineExceptionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InterpretationException"/> class
        /// </summary>
        /// <param name="message">Exception message</param>
        public InterpretationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InterpretationException"/> class
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public InterpretationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
