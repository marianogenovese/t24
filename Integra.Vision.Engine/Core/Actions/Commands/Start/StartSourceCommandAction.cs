//-----------------------------------------------------------------------
// <copyright file="StartSourceCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Repositories;

    /// <summary>
    /// Implements all the process of start a source.
    /// </summary>
    internal sealed class StartSourceCommandAction : ExecutionCommandAction
    {        
        /// <summary>
        /// Create adapter command
        /// </summary>
        private StartSourceCommand command;

        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            this.command = command as StartSourceCommand;

            try
            {
                this.StartObject();
                return new QueryCommandResult();
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Contains start object logic
        /// </summary>
        private void StartObject()
        {
            // initialize context
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            // create repository
            Database.Repositories.Repository<Database.Models.UserDefinedObject> repoUserDefinedObject = new Database.Repositories.Repository<Database.Models.UserDefinedObject>(vc);

            // get the adapter
            Database.Models.UserDefinedObject adapter = repoUserDefinedObject.Find(x => x.Name == this.command.Name);

            // update the adapter
            adapter.State = (int)UserDefinedObjectStateEnum.Started;

            // save changes
            vc.SaveChanges();

            // close connection
            vc.Dispose();
        }
    }
}
