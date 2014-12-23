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
        /// Gets the completion signal.
        /// </summary>
        public abstract TaskCompletionSource<bool> CompletionSignal
        {
            get;
        }
        
        /// <summary>
        /// Wait for the completion of the context.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task WaitForCompletion()
        {
            return this.CompletionSignal.Task;
        }

        /// <summary>
        /// Mark the context as completed.
        /// </summary>
        public void Success()
        {
            this.CompletionSignal.SetResult(true);
        }

        /// <summary>
        /// Mark the context as failure.
        /// </summary>
        /// <param name="e">The error occurred.</param>
        public void Failure(Exception e)
        {
            this.CompletionSignal.SetException(e);
        }
    }
}
