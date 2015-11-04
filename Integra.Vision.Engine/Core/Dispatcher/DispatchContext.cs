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
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Language;

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
        public CommandBase[] Commands
        {
            get;
            set;
        }

        /// <inheritdoc />
        public IEnumerable<PlanNode> Nodes
        {
            get;
            set;
        }

        /// <inheritdoc />
        public CommandBase Command
        {
            get;
            set;
        }

        /// <inheritdoc />
        public override System.ServiceModel.OperationContext Callback
        {
            get { return this.innerContext.Callback; }
        }

        /// <inheritdoc />
        public CommandAction CreateActionPipeline()
        {
            return this.ActionPipelineFactory.Create();
        }

        /// <inheritdoc />
        public override Task WaitForCompletion()
        {
            return this.innerContext.WaitForCompletion();
        }

        /// <inheritdoc />
        public override void Success()
        {
            this.innerContext.Success();
        }
        
        /// <inheritdoc />
        public override void Failure(Exception e)
        {
            this.innerContext.Failure(e);
        }
    }
}
