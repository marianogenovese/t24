//-----------------------------------------------------------------------
// <copyright file="DispatchFilter.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Collections.Generic;
    
    /// <summary>
    /// Allow to implement filters or steps in a pipeline.
    /// </summary>
    /// <typeparam name="TIn">The type of input</typeparam>
    /// <typeparam name="TOut">The type of output</typeparam>
    internal abstract class DispatchFilter<TIn, TOut>
    {
        /// <summary>
        /// Allow to implement the logic related with the filter or step.
        /// </summary>
        /// <param name="input">A enumerable of inputs.</param>
        /// <returns>A enumerable of outputs.</returns>
        public abstract TOut Execute(TIn input);
        
        /// <summary>
        /// Connect to a new filter or step to the pipeline.
        /// </summary>
        /// <typeparam name="TNext">The next type used in the pipeline filter.</typeparam>
        /// <param name="next">The next filter in the pipeline.</param>
        /// <returns>A new pipeline.</returns>
        public DispatchFilter<TIn, TNext> Next<TNext>(DispatchFilter<TOut, TNext> next)
        {
            return new DispatchPipeline<TIn, TOut, TNext>(this, next);
        }
    }
}
