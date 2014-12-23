//-----------------------------------------------------------------------
// <copyright file="DispatchPipelineExtensions.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Collections.Generic;
    
    /// <summary>
    /// Helper functions used in pipelines
    /// </summary>
    internal static class DispatchPipelineExtensions
    {
        /// <summary>
        /// Start the execution of the dispatch pipeline.
        /// </summary>
        /// <typeparam name="TIn">Input type of the source filter</typeparam>
        /// <typeparam name="TOut">Output type of the destination filter</typeparam>
        /// <param name="filter">The first filter.</param>
        /// <param name="input">The input of the pipeline.</param>
        /// <returns>A enumerable of outputs.</returns>
        public static TOut Start<TIn, TOut>(this DispatchFilter<TIn, TOut> filter, TIn input)
        {
            return filter.Execute(input);
        }
    }
}
