//-----------------------------------------------------------------------
// <copyright file="DispatchFilterFactory.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Allow to implement filters or steps in a pipeline.
    /// </summary>
    /// <typeparam name="TIn">Input type of the source filter</typeparam>
    /// <typeparam name="TOut">Output type of the destination filter</typeparam>
    internal abstract class DispatchFilterFactory<TIn, TOut>
    {
        /// <summary>
        /// Allow to implement the logic related with the creation of new filters.
        /// </summary>
        /// <returns>A new instance of dispatch filter.</returns>
        public abstract DispatchFilter<TIn, TOut> Create();

        /// <summary>
        /// Connect to a new filter or step to the pipeline.
        /// </summary>
        /// <typeparam name="TNext">The next type used in the dispatch filter factory.</typeparam>
        /// <param name="next">The next factory.</param>
        /// <returns>A new pipeline.</returns>
        public DispatchFilterFactory<TIn, TNext> Next<TNext>(DispatchFilterFactory<TOut, TNext> next)
        {
            return new DispatchPipelineFactory<TIn, TOut, TNext>(this, next);
        }
    }
}
