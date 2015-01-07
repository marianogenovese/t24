//-----------------------------------------------------------------------
// <copyright file="CreateAssemblyCommandAction.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;

    /// <summary>
    /// Implements all the process of create a new assembly.
    /// </summary>
    internal sealed class CreateAssemblyCommandAction : ExecutionCommandAction
    {
        /// <summary>
        /// Create adapter command
        /// </summary>
        private CreateAssemblyCommand command;

        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            this.command = command as CreateAssemblyCommand;

            try
            {
                this.SaveArguments();
                return new QueryCommandResult();
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Save the command arguments.
        /// </summary>
        private void SaveArguments()
        {
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");
            Database.Repositories.Repository<Database.Models.Assembly> repo = new Database.Repositories.Repository<Database.Models.Assembly>(vc);
            Database.Models.Assembly assembly = new Database.Models.Assembly() { CreationDate = System.DateTime.Now, IsSystemObject = false, Type = this.command.Type.ToString(), State = (int)UserDefinedObjectStateEnum.Stopped, Name = this.command.Name, LocalPath = this.command.LocalPath };
            repo.Create(assembly);

            // Guarda los cambios
            vc.SaveChanges();

            // Cierra la conexion
            vc.Dispose();
        }
    }
}
