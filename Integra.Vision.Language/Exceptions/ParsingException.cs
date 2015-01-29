//-----------------------------------------------------------------------
// <copyright file="ParsingException.cs" company="Ingetra.Vision.Language">
//     Copyright (c) Ingetra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.Exceptions
{
    using System;

    /// <summary>
    /// Parsing exception.
    /// </summary>
    internal sealed class ParsingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingException"/> class
        /// </summary>
        /// <param name="message">Exception message</param>
        public ParsingException(string message) : base(message)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingException"/> class
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public ParsingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
