//-----------------------------------------------------------------------
// <copyright file="DropRoleCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;
    using Integra.Vision.Engine.Database.Repositories;

    /// <summary>
    /// Implements all the process of delete a role.
    /// </summary>
    internal sealed class DropRoleCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                using (ObjectsContext context = new ObjectsContext("EngineDatabase"))
                {
                    this.DeleteObject(context, command as DropRoleCommand);
                    return new OkCommandResult();
                }
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Contains drop role logic
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Drop role command</param>
        private void DeleteObject(ObjectsContext vc, DropRoleCommand command)
        {
            // create repository
            Database.Repositories.Repository<Database.Models.Role> repo = new Database.Repositories.Repository<Database.Models.Role>(vc);

            // delete the object
            repo.Delete(x => x.Name == command.Name);
            int objectCount = vc.SaveChanges();

            // throw an exception if not deleted a object
            if (objectCount != 1)
            {
                // close connection
                vc.Dispose();

                // throw the exception 
                throw new Integra.Vision.Engine.Exceptions.DropUserDefinedObjectException("The object '" + command.Name + "' was not removed");
            }

            // close connection
            vc.Dispose();
        }
    }
}
