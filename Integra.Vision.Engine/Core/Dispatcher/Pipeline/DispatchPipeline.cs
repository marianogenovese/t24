//-----------------------------------------------------------------------
// <copyright file="DispatchPipeline.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Collections.Generic;
    
    /// <summary>
    /// Implements a pipeline, which can be used to combine on or more filters together in sequence.
    /// </summary>
    /// <typeparam name="TIn">Input type of the source filter.</typeparam>
    /// <typeparam name="T">Output type of the source filter used as input of the destination filter.</typeparam>
    /// <typeparam name="TOut">Output type of the destination filter.</typeparam>
    internal class DispatchPipeline<TIn, T, TOut> : DispatchFilter<TIn, TOut>
    {
        /// <summary>
        /// The source of inputs
        /// </summary>
        private readonly DispatchFilter<TIn, T> source;
        
        /// <summary>
        /// The destination of outputs.
        /// </summary>
        private readonly DispatchFilter<T, TOut> destination;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchPipeline{TIn,T,TOut}"/> class.
        /// </summary>
        /// <param name="source">The source of inputs.</param>
        /// <param name="destination">The destination of outputs.</param>
        public DispatchPipeline(DispatchFilter<TIn, T> source, DispatchFilter<T, TOut> destination)
        {
            this.source = source;
            this.destination = destination;
        }

        /// <inheritdoc />
        public override TOut Execute(TIn input)
        {
            T source = this.source.Execute(input);
            TOut result = this.destination.Execute(source);
            return result;
        }
    }
}
