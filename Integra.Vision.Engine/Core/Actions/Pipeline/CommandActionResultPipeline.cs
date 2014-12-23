//-----------------------------------------------------------------------
// <copyright file="CommandActionResultPipeline.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Implements a pipeline, which can be used to combine on or more filters together in sequence.
    /// </summary>
    internal class CommandActionResultPipeline : CommandActionResult
    {
        /// <summary>
        /// The source of inputs
        /// </summary>
        private readonly CommandActionResult source;
        
        /// <summary>
        /// The destination of outputs.
        /// </summary>
        private readonly CommandActionResult destination;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandActionResultPipeline"/> class.
        /// </summary>
        /// <param name="source">The source filter.</param>
        /// <param name="destination">The destination filter.</param>
        public CommandActionResultPipeline(CommandActionResult source, CommandActionResult destination)
        {
            this.source = source;
            this.destination = destination;
        }

        /// <inheritdoc />
        protected override void OnExecute(OperationContext context)
        {
            this.source.Execute(context);
            this.destination.Execute(context);
        }
    }
}
