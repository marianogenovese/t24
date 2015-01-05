//-----------------------------------------------------------------------
// <copyright file="EngineExceptionBase.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine
{
    using System;

    /// <summary>
    /// Base engine exception
    /// </summary>
    public abstract class EngineExceptionBase : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EngineExceptionBase"/> class
        /// </summary>
        /// <param name="message">Exception message</param>
        public EngineExceptionBase(string message) : base(message)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EngineExceptionBase"/> class
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public EngineExceptionBase(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}