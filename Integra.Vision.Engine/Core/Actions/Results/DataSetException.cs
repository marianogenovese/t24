//-----------------------------------------------------------------------
// <copyright file="DataSetException.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;

    /// <summary>
    /// Exception used for DataSet errors.
    /// </summary>
    internal sealed class DataSetException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataSetException"/> class
        /// </summary>
        /// <param name="message">Exception message</param>
        public DataSetException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSetException"/> class
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public DataSetException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
