//-----------------------------------------------------------------------
// <copyright file="ICommandActionExecutionContext.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// This interface implements a context used for execute command action.
    /// </summary>
    internal interface ICommandActionExecutionContext
    {
        /// <summary>
        /// Gets or sets the factory for creates command action pipelines.
        /// </summary>
        CommandActionFactory ActionPipelineFactory { get; set; }
        
        /// <summary>
        /// Gets or sets the pipeline for command action.
        /// </summary>
        CommandAction ActionPipeline { get; set; }

        /// <summary>
        /// Gets or sets the result of command execution.
        /// </summary>
        CommandActionResult Result { get; set; }

        /// <summary>
        /// Creates a command action pipeline.
        /// </summary>
        /// <returns>A new instance of command action pipeline.</returns>
        CommandAction CreateActionPipeline();

        // Aqui poner una propiedad que permita obtener el comando actual a ejecutarse
    }
}
