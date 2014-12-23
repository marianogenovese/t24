//-----------------------------------------------------------------------
// <copyright file="ExecutionCommandAction.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Provides a base class implementation for handling command specific execution logic.
    /// </summary>
    internal abstract class ExecutionCommandAction : CommandAction
    {
        /// <inheritdoc />
        protected override void OnExecute(CommandExecutingContext context)
        {
            context.Result = this.OnExecuteCommand();
        }

        /// <summary>
        /// Allow to implement custom logic for command action execution.
        /// </summary>
        /// <returns>A result of the action execution.</returns>
        protected virtual CommandActionResult OnExecuteCommand(/*Agregar parametro de comando*/)
        {
            return null;
        }
    }
}
