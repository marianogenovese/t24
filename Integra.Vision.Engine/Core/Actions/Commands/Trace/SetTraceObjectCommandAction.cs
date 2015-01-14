﻿//-----------------------------------------------------------------------
// <copyright file="SetTraceObjectCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Repositories;

    /// <summary>
    /// Implements all the process of set the trace level of a object.
    /// </summary>
    internal sealed class SetTraceObjectCommandAction : ExecutionCommandAction
    {
        /// <summary>
        /// Set trace object command
        /// </summary>
        private SetTraceObjectCommand command;

        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            this.command = command as SetTraceObjectCommand;

            try
            {
                this.SetTrace();
                return new OkCommandResult();
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Contains trace stream logic
        /// </summary>
        private void SetTrace()
        {
            // initialize context
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc2 = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            // create repository
            Database.Repositories.Repository<Database.Models.SetTrace> repoSetTrace = new Database.Repositories.Repository<Database.Models.SetTrace>(vc);
            Database.Repositories.Repository<Database.Models.UserDefinedObject> repoObject = new Database.Repositories.Repository<Database.Models.UserDefinedObject>(vc2);

            int level = this.command.Level;
            Database.Models.SetTrace setTrace;

            // find the user defined object
            Database.Models.UserDefinedObject userDefinedObject = repoObject.Find(x => x.Name == this.command.ObjectName);

            setTrace = repoSetTrace.Find(x => x.UserDefinedObjectId == userDefinedObject.Id);

            if (setTrace != null)
            {
                // update the trace
                setTrace.Level = level;
                setTrace.CreationDate = System.DateTime.Now;
                repoSetTrace.Update(setTrace);
            }
            else
            {
                // create the trace
                setTrace = new Database.Models.SetTrace() { Level = level, UserDefinedObjectId = userDefinedObject.Id, CreationDate = System.DateTime.Now };
                repoSetTrace.Create(setTrace);
            }

            // save changes
            vc.SaveChanges();
            vc2.SaveChanges();

            // close connections
            vc.Dispose();
            vc2.Dispose();
        }
    }
}
