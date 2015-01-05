//-----------------------------------------------------------------------
// <copyright file="ExecutionCommandAction.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using Integra.Vision.Engine.Commands;

    /// <summary>
    /// Provides a base class implementation for handling command specific execution logic.
    /// </summary>
    internal abstract class ExecutionCommandAction : CommandAction
    {
        /// <inheritdoc />
        protected override void OnExecute(CommandExecutingContext context)
        {
            context.Result = this.OnExecuteCommand(context.Command);
        }

        /// <summary>
        /// Allow to implement custom logic for command action execution.
        /// </summary>
        /// <param name="command">The information of the command.</param>
        /// <returns>A result of the action execution.</returns>
        protected virtual CommandActionResult OnExecuteCommand(CommandBase command)
        {
            return null;
        }
    }
}
