//-----------------------------------------------------------------------
// <copyright file="ComponentFaultedException.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The exception that is thrown when a call is made to a component that has faulted.
    /// </summary>
    internal class ComponentFaultedException : ComponentException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentFaultedException"/> class.
        /// </summary>
        public ComponentFaultedException()
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentFaultedException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the exception.</param>
        public ComponentFaultedException(string message) : base(message)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentFaultedException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ComponentFaultedException(string message, Exception innerException) : base(message, innerException)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentFaultedException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The serialization information used to create the exception object.</param>
        /// <param name="context">The context within which the exception object is created.</param>
        protected ComponentFaultedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
