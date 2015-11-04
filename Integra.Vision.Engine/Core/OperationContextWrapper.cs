//-----------------------------------------------------------------------
// <copyright file="OperationContextWrapper.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Security.Principal;
    using System.ServiceModel;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides access to the operation execution context of a request.
    /// </summary>
    internal sealed class OperationContextWrapper : OperationContext
    {       
        /// <summary>
        /// The security information for identify the user in the operation.
        /// </summary>
        private readonly IPrincipal user;

        /// <summary>
        /// Contains the request information of the operation.
        /// </summary>
        private readonly OperationRequest request;

        /// <summary>
        /// Contains the response of the operation.
        /// </summary>
        private readonly OperationResponse response;

        /// <summary>
        /// Used for store common data.
        /// </summary>
        private readonly Dictionary<string, object> data = new Dictionary<string, object>();

        /// <summary>
        /// Used for wait the completion of the operation.
        /// </summary>
        private TaskCompletionSource<bool> completionSignal = new TaskCompletionSource<bool>();

        /// <summary>
        /// Used for store the current channel for callback operations.
        /// </summary>
        private System.ServiceModel.OperationContext callback;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationContextWrapper"/> class.
        /// </summary>
        /// <param name="user">The security information for identify the user in the operation.</param>
        /// <param name="request">Contains the request information of the operation.</param>
        public OperationContextWrapper(IPrincipal user, OperationRequest request)
        {
            this.user = user;
            this.request = request;
            this.response = new OperationResponse();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationContextWrapper"/> class.
        /// </summary>
        /// <param name="user">The security information for identify the user in the operation.</param>
        /// <param name="request">Contains the request information of the operation.</param>
        /// <param name="callback">The channel for callback operations.</param>
        public OperationContextWrapper(IPrincipal user, OperationRequest request, System.ServiceModel.OperationContext callback)
        {
            this.user = user;
            this.request = request;
            this.callback = callback;
            this.response = new OperationResponse();
        }

        /// <summary>
        /// Gets the security information for identify the user in the operation.
        /// </summary>
        public override IPrincipal User
        {
            get
            {
                return this.user;
            }
        }

        /// <summary>
        /// Gets the request object for the current operation.
        /// </summary>
        public override OperationRequest Request
        {
            get
            {
                return this.request;
            }
        }

        /// <summary>
        /// Gets the response object for the current operation.
        /// </summary>
        public override OperationResponse Response
        {
            get
            {
                return this.response;
            }
        }

        /// <summary>
        /// Gets context data.
        /// </summary>
        public override Dictionary<string, object> Data
        {
            get
            {
                return this.data;
            }
        }

        /// <summary>
        /// Gets the current channel
        /// </summary>
        public override System.ServiceModel.OperationContext Callback
        {
            get
            {
                return this.callback;
            }
        }

        /// <inheritdoc />
        public override Task WaitForCompletion()
        {
            return this.completionSignal.Task;
        }

        /// <inheritdoc />
        public override void Success()
        {
            this.completionSignal.SetResult(true);
        }
        
        /// <inheritdoc />
        public override void Failure(Exception e)
        {
            this.completionSignal.SetException(e);
        }
    }
}
