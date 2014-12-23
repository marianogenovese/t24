//-----------------------------------------------------------------------
// <copyright file="NotAuthorizedCommandResult.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Runtime.Serialization;
    
    /// <summary>
    /// Represents a result when an operation is not authorized.
    /// </summary>
    [DataContract(Name = "NotAuthorizedCommandResult", Namespace = "http://Integra.Vision.Engine/")]
    internal class NotAuthorizedCommandResult : ErrorCommandResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotAuthorizedCommandResult"/> class.
        /// </summary>
        public NotAuthorizedCommandResult() : base(new UnauthorizedAccessException())
        {
        }
    }
}
