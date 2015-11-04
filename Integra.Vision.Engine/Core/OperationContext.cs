//-----------------------------------------------------------------------
// <copyright file="OperationContext.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Security.Principal;
    using System.Threading.Tasks;
    
    /// <summary>
    /// Provides access to the operation execution context of a request.
    /// </summary>
    internal abstract class OperationContext
    {
        /// <summary>
        /// Gets the security information for identify the user in the operation.
        /// </summary>
        public abstract IPrincipal User { get; }

        /// <summary>
        /// Gets the request object for the current operation.
        /// </summary>
        public abstract OperationRequest Request { get; }
        
        /// <summary>
        /// Gets the response object for the current operation.
        /// </summary>
        public abstract OperationResponse Response { get; }

        /// <summary>
        /// Gets context data.
        /// </summary>
        public abstract Dictionary<string, object> Data { get; }

        /// <summary>
        /// Gets the current channel for callback operations
        /// </summary>
        public abstract System.ServiceModel.OperationContext Callback { get; }
        
        /// <summary>
        /// Wait for the completion of the operation.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public abstract Task WaitForCompletion();

        /// <summary>
        /// Mark the operation as completed.
        /// </summary>
        public abstract void Success();

        /// <summary>
        /// Mark the operation as failure.
        /// </summary>
        /// <param name="e">The error occurred.</param>
        public abstract void Failure(Exception e);
    }
}
