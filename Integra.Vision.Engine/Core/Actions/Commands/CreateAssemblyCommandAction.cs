//-----------------------------------------------------------------------
// <copyright file="CreateAssemblyCommandAction.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Implements all the process of create a new assembly.
    /// </summary>
    internal sealed class CreateAssemblyCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand()
        {
            return new QueryCommandResult();
        }
    }
}
