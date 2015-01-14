//-----------------------------------------------------------------------
// <copyright file="DropAssemblyCommandAction.cs" company="Ingetra.Vision.Engine">
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
    internal sealed class DropAssemblyCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                using (ViewsContext context = new ViewsContext("EngineDatabase"))
                {
                    this.DeleteObject(context, command as DropAssemblyCommand);
                    return new OkCommandResult();
                }
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Contains drop assembly logic
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Drop assembly command</param>
        private void DeleteObject(ViewsContext vc, DropAssemblyCommand command)
        {
            // create repository
            Database.Repositories.Repository<Database.Models.Assembly> repo = new Database.Repositories.Repository<Database.Models.Assembly>(vc);

            // delete the object
            repo.Delete(x => x.Name == command.Name);
            int objectCount = vc.SaveChanges();

            // throw an exception if not deleted an object
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
