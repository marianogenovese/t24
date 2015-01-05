//-----------------------------------------------------------------------
// <copyright file="AuthorizationContext.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.Generic;
    using System.Security.Principal;
    using System.Threading.Tasks;
    
    /// <summary>
    /// Represents a authorization context.
    /// </summary>
    internal class AuthorizationContext : OperationContext
    {
        /// <summary>
        /// The context information of the operation.
        /// </summary>
        private readonly OperationContext innerContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationContext"/> class.
        /// </summary>
        /// <param name="innerContext">The context related to the operation.</param>
        public AuthorizationContext(OperationContext innerContext)
        {
            this.innerContext = innerContext;
        }

        /// <inheritdoc />
        public override IPrincipal User
        {
            get
            {
                return this.innerContext.User;
            }
        }

        /// <inheritdoc />
        public override OperationRequest Request
        {
            get
            {
                return this.innerContext.Request;
            }
        }

        /// <inheritdoc />
        public override OperationResponse Response
        {
            get
            {
                return this.innerContext.Response;
            }
        }

        /// <inheritdoc />
        public override Dictionary<string, object> Data
        {
            get
            {
                return this.innerContext.Data;
            }
        }

        /// <inheritdoc />
        public override TaskCompletionSource<bool> CompletionSignal
        {
            get
            {
                return this.innerContext.CompletionSignal;
            }
        }
    }
}
