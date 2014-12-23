//-----------------------------------------------------------------------
// <copyright file="DispatchContext.cs" company="Integra.Vision.Engine">
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
    /// Provides access to the dispatch context of a operation execution.
    /// </summary>
    internal class DispatchContext : OperationContext, IParseFilterContext, ICompileFilterContext, ICommandActionExecutionContext
    {
        /// <summary>
        /// The context information of the operation.
        /// </summary>
        private readonly OperationContext innerContext;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchContext"/> class.
        /// </summary>
        /// <param name="context">The context related to the operation.</param>
        public DispatchContext(OperationContext context)
        {
            this.innerContext = context;
        }
        
        /// <inheritdoc />
        public CommandActionFactory ActionPipelineFactory
        {
            get;
            set;
        }

        /// <inheritdoc />
        public CommandAction ActionPipeline
        {
            get;
            set;
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
        public CommandActionResult Result
        {
            get;
            set;
        }

        /// <inheritdoc />
        public override TaskCompletionSource<bool> CompletionSignal
        {
            get
            {
                return this.innerContext.CompletionSignal;
            }
        }

        /// <inheritdoc />
        public CommandAction CreateActionPipeline()
        {
            return this.ActionPipelineFactory.Create();
        }
    }
}
