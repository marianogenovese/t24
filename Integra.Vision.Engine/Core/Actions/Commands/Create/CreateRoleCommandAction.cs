//-----------------------------------------------------------------------
// <copyright file="CreateRoleCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;

    /// <summary>
    /// Base class for create roles
    /// </summary>
    internal sealed class CreateRoleCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                using (ObjectsContext context = new ObjectsContext("EngineDatabase"))
                {
                    this.SaveArguments(context, command as CreateRoleCommand);
                }

                return new OkCommandResult();
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Save the command arguments.
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Create role command</param>
        private void SaveArguments(ObjectsContext vc, CreateRoleCommand command)
        {
            // create repository
            Database.Repositories.Repository<Database.Models.Role> repoRole = new Database.Repositories.Repository<Database.Models.Role>(vc);

            // create user
            Database.Models.Role role = new Database.Models.Role() { CreationDate = DateTime.Now, IsServerRole = false, IsSystemObject = false, Name = command.Name, State = (int)UserDefinedObjectStateEnum.Stopped, Type = ObjectTypeEnum.Role.ToString() };
            repoRole.Create(role);

            // save the object script
            ScriptActions scriptActions = new ScriptActions(vc);
            scriptActions.SaveScript(command.Script, role.Id);

            // save changes
            vc.SaveChanges();

            // close connections
            vc.Dispose();
        }
    }
}
