//-----------------------------------------------------------------------
// <copyright file="DispatchPipelineFactory.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Implements a pipeline, which can be used to combine on or more filters together in sequence.
    /// </summary>
    /// <typeparam name="TIn">Input type of the source filter.</typeparam>
    /// <typeparam name="T">Output type of the source filter used as input of the destination filter.</typeparam>
    /// <typeparam name="TOut">Output type of the destination filter.</typeparam>
    internal class DispatchPipelineFactory<TIn, T, TOut> : DispatchFilterFactory<TIn, TOut>
    {
        /// <summary>
        /// The source factory of dispatch filters.
        /// </summary>
        private readonly DispatchFilterFactory<TIn, T> source;
        
        /// <summary>
        /// The destination factory of dispatch filters.
        /// </summary>
        private readonly DispatchFilterFactory<T, TOut> destination;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchPipelineFactory{TIn,T,TOut}"/> class.
        /// </summary>
        /// <param name="source">The source factory.</param>
        /// <param name="destination">The destination factory.</param>
        public DispatchPipelineFactory(DispatchFilterFactory<TIn, T> source, DispatchFilterFactory<T, TOut> destination)
        {
            this.source = source;
            this.destination = destination;
        }

        /// <inheritdoc />
        public override sealed DispatchFilter<TIn, TOut> Create()
        {
            // Aqui se crean los filtros de origen y destino, y se los relaciona, 
            // el resultado de la combinación de ambos se retorna para que pueda
            // ser combinado con otros filtros.            
            DispatchFilter<TIn, T> sourceFilter = this.source.Create();
            DispatchFilter<T, TOut> destinationFilter = this.destination.Create();
            return sourceFilter.Next(destinationFilter);
        }
    }
}
