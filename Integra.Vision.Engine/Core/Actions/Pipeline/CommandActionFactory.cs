//-----------------------------------------------------------------------
// <copyright file="CommandActionFactory.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Allow to implement filters or steps in a pipeline.
    /// </summary>
    internal abstract class CommandActionFactory
    {
        /// <summary>
        /// Allow to implement the logic related with the creation of new filters.
        /// </summary>
        /// <returns>A new instance of action filter.</returns>
        public abstract CommandAction Create();

        /// <summary>
        /// Connect to a new filter or step to the pipeline.
        /// </summary>
        /// <param name="next">The next factory.</param>
        /// <returns>A new pipeline.</returns>
        public CommandActionFactory Next(CommandActionFactory next)
        {
            return new CommandActionPipelineFactory(this, next);
        }
    }
}
