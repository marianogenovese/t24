//-----------------------------------------------------------------------
// <copyright file="CommandActionPipelineFactory.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Implements a pipeline, which can be used to combine on or more filters together in sequence.
    /// </summary>
    internal class CommandActionPipelineFactory : CommandActionFactory
    {
        /// <summary>
        /// The source factory of action filters.
        /// </summary>
        private readonly CommandActionFactory source;
        
        /// <summary>
        /// The destination factory of action filters.
        /// </summary>
        private readonly CommandActionFactory destination;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandActionPipelineFactory"/> class.
        /// </summary>
        /// <param name="source">The source factory.</param>
        /// <param name="destination">The destination factory.</param>
        public CommandActionPipelineFactory(CommandActionFactory source, CommandActionFactory destination)
        {
            this.source = source;
            this.destination = destination;
        }

        /// <inheritdoc />
        public override sealed CommandAction Create()
        {
            // Aqui se crean los filtros de origen y destino, y se los relaciona, 
            // el resultado de la combinación de ambos se retorna para que pueda
            // ser combinado con otros filtros.            
            CommandAction sourceFilter = this.source.Create();
            CommandAction destinationFilter = this.destination.Create();
            return sourceFilter.Next(destinationFilter);
        }
    }
}
