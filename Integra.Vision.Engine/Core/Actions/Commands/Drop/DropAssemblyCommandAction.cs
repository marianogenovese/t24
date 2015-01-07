//-----------------------------------------------------------------------
// <copyright file="DropAssemblyCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;

    /// <summary>
    /// Implements all the process of delete a user.
    /// </summary>
    internal sealed class DropAssemblyCommandAction : ExecutionCommandAction
    {
        /// <summary>
        /// Create adapter command
        /// </summary>
        private DropAssemblyCommand command;

        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            this.command = command as DropAssemblyCommand;

            try
            {
                this.DeleteObject();
                return new QueryCommandResult();
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Contains drop assembly logic
        /// </summary>
        private void DeleteObject()
        {
            // initialize context
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            // create repository
            Database.Repositories.Repository<Database.Models.Assembly> repo = new Database.Repositories.Repository<Database.Models.Assembly>(vc);

            // delete the object
            repo.Delete(x => x.Name == this.command.Name);
            int objectCount = vc.SaveChanges();

            // throw an exception if not deleted an object
            if (objectCount != 1)
            {
                // close connection
                vc.Dispose();

                // throw the exception 
                throw new Integra.Vision.Engine.Exceptions.DropUserDefinedObjectException("The object '" + this.command.Name + "' was not removed");
            }

            // close connection
            vc.Dispose();
        }
    }
}
