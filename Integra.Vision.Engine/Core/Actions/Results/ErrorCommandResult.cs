//-----------------------------------------------------------------------
// <copyright file="ErrorCommandResult.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Runtime.Serialization;
    
    /// <summary>
    /// Represents a result when an command execution fails.
    /// </summary>
    [DataContract(Name = "ErrorCommandResult", Namespace = "http://Integra.Vision.Engine/")]
    internal class ErrorCommandResult : CommandActionResult
    {
        /// <summary>
        /// The exception occurred.
        /// </summary>
        private readonly Exception exception;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorCommandResult"/> class.
        /// </summary>
        /// <param name="exception">The exception occurred.</param>
        public ErrorCommandResult(Exception exception)
        {
            this.exception = exception;
            this.ExceptionType = this.exception.GetType().AssemblyQualifiedName;
            this.Message = this.exception.Message;
        }
        
        /// <summary>
        /// Gets or sets the type of the exception.
        /// </summary>
        [DataMember(Order = 0, Name = "ExceptionType", IsRequired = true)]
        public string ExceptionType
        {
            get;
            protected set;
        }
        
        /// <summary>
        /// Gets or sets the message of the exception.
        /// </summary>
        [DataMember(Order = 1, Name = "Message", IsRequired = true)]
        public string Message
        {
            get;
            protected set;
        }
    }
}
