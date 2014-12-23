//-----------------------------------------------------------------------
// <copyright file="DropUserDefinedObjectException.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Exceptions
{
    using System;

    /// <summary>
    /// Exception used when the object is not removed
    /// </summary>
    public class DropUserDefinedObjectException : EngineExceptionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DropUserDefinedObjectException"/> class
        /// </summary>
        /// <param name="message">Exception message</param>
        public DropUserDefinedObjectException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DropUserDefinedObjectException"/> class
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public DropUserDefinedObjectException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
