//-----------------------------------------------------------------------
// <copyright file="StartAdapterCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Repositories;

    /// <summary>
    /// Implements all the process of start an adapter.
    /// </summary>
    internal sealed class StartAdapterCommandAction : ExecutionCommandAction
    {
        /// <summary>
        /// Create adapter command
        /// </summary>
        private StartAdapterCommand command;

        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            this.command = command as StartAdapterCommand;

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

            /*
            // get the object name
            string objectName = this.command.Name;

            if (this.command.UserDefinedObject.Equals(ObjectTypeEnum.Adapter))
            {
                // create repository
                Database.Repositories.Repository<Database.Models.Adapter> repoAdapter = new Database.Repositories.Repository<Database.Models.Adapter>(vc);

                // get the adapter
                Database.Models.Adapter adapter = repoAdapter.Find(x => x.Name == objectName);

                // update the adapter
                adapter.State = (int)UserDefinedObjectStateEnum.Started;
            }
            else if (this.command.UserDefinedObject.Equals(ObjectTypeEnum.Source))
            {
                // create repository
                Database.Repositories.Repository<Database.Models.Source> repoSource = new Database.Repositories.Repository<Database.Models.Source>(vc);

                // get the source
                Database.Models.Source source = repoSource.Find(x => x.Name == objectName);

                // update the state
                source.State = (int)UserDefinedObjectStateEnum.Started;
            }
            else if (this.command.UserDefinedObject.Equals(ObjectTypeEnum.Stream))
            {
                // create repository
                Database.Repositories.Repository<Database.Models.Stream> repoStream = new Database.Repositories.Repository<Database.Models.Stream>(vc);

                // get the source
                Database.Models.Stream stream = repoStream.Find(x => x.Name == objectName);

                // update the state
                stream.State = (int)UserDefinedObjectStateEnum.Started;
            }
            else if (this.command.UserDefinedObject.Equals(ObjectTypeEnum.Trigger))
            {
                // create repository
                Database.Repositories.Repository<Database.Models.Trigger> repoTrigger = new Database.Repositories.Repository<Database.Models.Trigger>(vc);

                // get the source
                Database.Models.Trigger trigger = repoTrigger.Find(x => x.Name == objectName);

                // update the state
                trigger.State = (int)UserDefinedObjectStateEnum.Started;
            }
            */

            // save changes
            vc.SaveChanges();

            // close connection
            vc.Dispose();
        }
    }
}
