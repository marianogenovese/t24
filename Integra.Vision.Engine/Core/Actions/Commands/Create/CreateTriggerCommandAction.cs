//-----------------------------------------------------------------------
// <copyright file="CreateTriggerCommandAction.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;

    /// <summary>
    /// Implements all the process of create a new trigger.
    /// </summary>
    internal sealed class CreateTriggerCommandAction : ExecutionCommandAction
    {
        /// <summary>
        /// Create adapter command
        /// </summary>
        private CreateTriggerCommand command;

        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            this.command = command as CreateTriggerCommand;

            try
            {
                if (this.command.IsSimpleTrigger)
                {
                    this.SaveSimpleTriggerArguments();
                }
                else
                {
                    this.SaveTriggerWithWindowArguments();
                }

                return new QueryCommandResult();
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Save the command arguments
        /// </summary>
        private void SaveSimpleTriggerArguments()
        {
            // Initialize the context
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            // initialize the repositories
            Database.Repositories.Repository<Database.Models.Trigger> repoTrigger = new Database.Repositories.Repository<Database.Models.Trigger>(vc);
            Database.Repositories.Repository<Database.Models.Stream> repoStream = new Database.Repositories.Repository<Database.Models.Stream>(vc);
            Database.Repositories.Repository<Database.Models.Stmt> repoStmt = new Database.Repositories.Repository<Database.Models.Stmt>(vc);
            Database.Repositories.Repository<Database.Models.Adapter> repoAdapter = new Database.Repositories.Repository<Database.Models.Adapter>(vc);

            string streamName = this.command.StreamName;
            Database.Models.Stream stream = repoStream.Find(x => x.Name == streamName);

            Database.Models.Trigger trigger = new Database.Models.Trigger() { CreationDate = DateTime.Now, Name = this.command.Name, State = (int)UserDefinedObjectStateEnum.Stopped, Type = this.command.Type.ToString(), IsSystemObject = false, StreamId = stream.Id };
            repoTrigger.Create(trigger);

            foreach (string adapterName in this.command.SendList)
            {
                Database.Models.Adapter adapter = repoAdapter.Find(x => x.Name == adapterName);
                Database.Models.Stmt stmt = new Database.Models.Stmt() { TriggerId = trigger.Id, AdapterId = adapter.Id, Type = (int)StmtTypeEnum.SendAlways };
                repoStmt.Create(stmt);
            }

            // save trigger dependencies
            DependencyActions dependencyAction = new DependencyActions(vc, this.command.Dependencies);
            dependencyAction.SaveDependencies(trigger);

            // save changes
            vc.SaveChanges();

            // close connections
            vc.Dispose();
        }

        /// <summary>
        /// Save the command arguments
        /// </summary>
        private void SaveTriggerWithWindowArguments()
        {
            // Initialize the context
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            // initialize the repositories
            Database.Repositories.Repository<Database.Models.Trigger> repoTrigger = new Database.Repositories.Repository<Database.Models.Trigger>(vc);
            Database.Repositories.Repository<Database.Models.Stream> repoStream = new Database.Repositories.Repository<Database.Models.Stream>(vc);
            Database.Repositories.Repository<Database.Models.Stmt> repoStmt = new Database.Repositories.Repository<Database.Models.Stmt>(vc);
            Database.Repositories.Repository<Database.Models.Adapter> repoAdapter = new Database.Repositories.Repository<Database.Models.Adapter>(vc);

            string streamName = this.command.StreamName;
            Database.Models.Stream stream = repoStream.Find(x => x.Name == streamName);

            Database.Models.Trigger trigger = null;

            TimeSpan d;
            TimeSpan.TryParse(this.command.ApplyWindow, out d);

            trigger = new Database.Models.Trigger() { CreationDate = DateTime.Now, Name = this.command.Name, State = (int)UserDefinedObjectStateEnum.Stopped, Type = this.command.Type.ToString(), IsSystemObject = false, StreamId = stream.Id, DurationTime = d.TotalMilliseconds };
            repoTrigger.Create(trigger);

            int adapterOrder = 0;
            foreach (Tuple<string, string[]> ifTuple in this.command.IfList)
            {
                foreach (string adapterName in (string[])ifTuple.Item2)
                {
                    Database.Models.Adapter adapter = repoAdapter.Find(x => x.Name == adapterName);
                    Database.Models.Stmt stmt = new Database.Models.Stmt() { TriggerId = trigger.Id, AdapterId = adapter.Id, Order = adapterOrder };

                    if (ifTuple.Item1.ToString().Equals("130"))
                    {
                        stmt.Type = (int)StmtTypeEnum.SendIfHasEvents;
                    }
                    else if (ifTuple.Item1.ToString().Equals("132"))
                    {
                        stmt.Type = (int)StmtTypeEnum.SendIfNotHasEvents;
                    }

                    repoStmt.Create(stmt);

                    adapterOrder++;
                }
            }

            // save trigger dependencies
            DependencyActions dependencyAction = new DependencyActions(vc, this.command.Dependencies);
            dependencyAction.SaveDependencies(trigger);

            // save changes
            vc.SaveChanges();

            // close connections
            vc.Dispose();
        }
    }
}
