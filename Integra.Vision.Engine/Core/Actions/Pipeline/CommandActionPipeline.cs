//-----------------------------------------------------------------------
// <copyright file="CommandActionPipeline.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Implements a pipeline, which can be used to combine on or more filters together in sequence.
    /// </summary>
    internal class CommandActionPipeline : CommandAction
    {
        /// <summary>
        /// The source of inputs
        /// </summary>
        private readonly CommandAction source;
        
        /// <summary>
        /// The destination of outputs.
        /// </summary>
        private readonly CommandAction destination;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandActionPipeline"/> class.
        /// </summary>
        /// <param name="source">The source filter.</param>
        /// <param name="destination">The destination filter.</param>
        public CommandActionPipeline(CommandAction source, CommandAction destination)
        {
            this.source = source;
            this.destination = destination;
        }

        /// <inheritdoc />
        protected override void OnExecute(CommandExecutingContext context)
        {
            this.source.Execute(context);

            // Si no hubo resultado de la ejecución de la acción anterior, se permite ejecutar la siguiente
            // esto significa que el proceso se puede dar por terminado sino ya la acción anterior produjo
            // algún resultado.
            if (context.Result == null)
            {
                this.destination.Execute(context);
            }
        }
    }
}
