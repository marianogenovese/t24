//-----------------------------------------------------------------------
// <copyright file="CommandAction.cs" company="Integra.Vision.Engine">
//     Copyright (c) CompanyName. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Allow to implement filters or steps in a pipeline.
    /// </summary>
    internal abstract class CommandAction
    {
        /// <summary>
        /// Execute the logic related with the filter or step.
        /// </summary>
        /// <param name="context">The action execution context.</param>
        public void Execute(CommandExecutingContext context)
        {
            this.OnExecute(context);
        }

        /// <summary>
        /// Connect to a new filter or step to the pipeline.
        /// </summary>
        /// <param name="next">The next filter in the pipeline.</param>
        /// <returns>A new pipeline.</returns>
        public CommandAction Next(CommandAction next)
        {
            return new CommandActionPipeline(this, next);
        }
        
        /// <summary>
        /// Allow to implement the logic related with the filter or step.
        /// </summary>
        /// <param name="context">The action execution context.</param>
        protected abstract void OnExecute(CommandExecutingContext context);
    }
}
