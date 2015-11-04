//-----------------------------------------------------------------------
// <copyright file="CommandExecutingContext.cs" company="Integra.Vision.Engine">
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
    
    /// <summary>
    /// Represents a action execution context.
    /// </summary>
    internal class CommandExecutingContext : OperationContext
    {
        /// <summary>
        /// Current command to execute.
        /// </summary>
        private readonly CommandBase command;

        /// <summary>
        /// The context information of the operation.
        /// </summary>
        private readonly OperationContext innerContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExecutingContext"/> class.
        /// </summary>
        /// <param name="innerContext">The operation context related to the execution of the command</param>
        /// <param name="command">Current command to execute</param>
        public CommandExecutingContext(OperationContext innerContext, CommandBase command)
        {
            this.innerContext = innerContext;
            this.command = command;
        }

        /// <summary>
        /// Gets or sets the result of a action execution.
        /// </summary>
        public CommandActionResult Result { get; set; }

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

        /// <summary>
        /// Gets the current command.
        /// </summary>
        public CommandBase Command
        {
            get
            {
                return this.command;
            }
        }

        /// <inheritdoc />
        public override System.ServiceModel.OperationContext Callback
        {
            get { return this.innerContext.Callback; }
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
