//-----------------------------------------------------------------------
// <copyright file="CreateRoleCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;

    /// <summary>
    /// Base class for create roles
    /// </summary>
    internal sealed class CreateRoleCommandAction : ExecutionCommandAction
    {
        /// <summary>
        /// Create adapter command
        /// </summary>
        private CreateRoleCommand command;

        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            this.command = command as CreateRoleCommand;

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
            // initialize context
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            // create repository
            Database.Repositories.Repository<Database.Models.Role> repoRole = new Database.Repositories.Repository<Database.Models.Role>(vc);

            // create role
            Database.Models.Role role = new Database.Models.Role() { CreationDate = DateTime.Now, IsServerRole = false, IsSystemObject = false, Name = this.command.Name, State = (int)UserDefinedObjectStateEnum.Stopped, Type = this.command.Type.ToString() };
            repoRole.Create(role);

            // save changes
            vc.SaveChanges();

            // close connections
            vc.Dispose();
        }
    }
}
