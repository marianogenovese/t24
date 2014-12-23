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
    
    /// <summary>
    /// Represents a action execution context.
    /// </summary>
    internal class CommandExecutingContext : OperationContext
    {
        /// <summary>
        /// The context information of the operation.
        /// </summary>
        private readonly OperationContext innerContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExecutingContext"/> class.
        /// </summary>
        /// <param name="innerContext">The operation context related to the execution of the command</param>
        public CommandExecutingContext(OperationContext innerContext)
        {
            this.innerContext = innerContext;
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

        // Aqui debe haber una propiedad que tenga el comando actual a ejecutarse.
        
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
