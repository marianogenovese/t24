//-----------------------------------------------------------------------
// <copyright file="LoadAssemblyCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;

    /// <summary>
    /// Implements all the process of load an assembly.
    /// </summary>
    internal sealed class LoadAssemblyCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                using (ViewsContext context = new ViewsContext("EngineDatabase"))
                {
                    this.LoadAssembly(context, command as LoadAssemblyCommand);
                    return new OkCommandResult();
                }
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Contains load assembly logic
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Load assembly command</param>
        private void LoadAssembly(ViewsContext vc, LoadAssemblyCommand command)
        {
        }
    }
}
