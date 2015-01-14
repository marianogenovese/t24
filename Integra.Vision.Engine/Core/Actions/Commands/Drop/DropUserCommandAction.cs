//-----------------------------------------------------------------------
// <copyright file="DropUserCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;

    /// <summary>
    /// Implements all the process of delete a user.
    /// </summary>
    internal sealed class DropUserCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                using (ViewsContext context = new ViewsContext("EngineDatabase"))
                {
                    this.DeleteObject(context, command as DropUserCommand);
                    return new OkCommandResult();
                }
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Contains drop user logic
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Drop user command</param>
        private void DeleteObject(ViewsContext vc, DropUserCommand command)
        {
            // create repository
            Database.Repositories.Repository<Database.Models.User> repo = new Database.Repositories.Repository<Database.Models.User>(vc);

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
