//-----------------------------------------------------------------------
// <copyright file="CreateTriggerCommandAction.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;

    /// <summary>
    /// Implements all the process of create a new trigger.
    /// </summary>
    internal sealed class CreateTriggerCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            CreateTriggerCommand createTriggerCommand = command as CreateTriggerCommand;

            try
            {
                using (ViewsContext context = new ViewsContext("EngineDatabase"))
                {
                    if (createTriggerCommand.IsSimpleTrigger)
                    {
                        this.SaveSimpleTriggerArguments(context, createTriggerCommand);
                    }
                    else
                    {
                        this.SaveTriggerWithWindowArguments(context, createTriggerCommand);
                    }

                    return new OkCommandResult();
                }
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Save the simple trigger command arguments
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Create trigger command</param>
        private void SaveSimpleTriggerArguments(ViewsContext vc, CreateTriggerCommand command)
        {
            // initialize the repositories
            Database.Repositories.Repository<Database.Models.Trigger> repoTrigger = new Database.Repositories.Repository<Database.Models.Trigger>(vc);
            Database.Repositories.Repository<Database.Models.Stream> repoStream = new Database.Repositories.Repository<Database.Models.Stream>(vc);
            Database.Repositories.Repository<Database.Models.Stmt> repoStmt = new Database.Repositories.Repository<Database.Models.Stmt>(vc);
            Database.Repositories.Repository<Database.Models.Adapter> repoAdapter = new Database.Repositories.Repository<Database.Models.Adapter>(vc);

            string streamName = command.StreamName;
            Database.Models.Stream stream = repoStream.Find(x => x.Name == streamName);

            Database.Models.Trigger trigger = new Database.Models.Trigger() { CreationDate = DateTime.Now, Name = command.Name, State = (int)UserDefinedObjectStateEnum.Stopped, Type = ObjectTypeEnum.Trigger.ToString(), IsSystemObject = false, StreamId = stream.Id };
            repoTrigger.Create(trigger);

            foreach (string adapterName in command.SendList)
            {
                Database.Models.Adapter adapter = repoAdapter.Find(x => x.Name == adapterName);
                Database.Models.Stmt stmt = new Database.Models.Stmt() { TriggerId = trigger.Id, AdapterId = adapter.Id, Type = (int)StmtTypeEnum.SendAlways };
                repoStmt.Create(stmt);
            }

            // save the object script
            ScriptActions scriptActions = new ScriptActions(vc);
            scriptActions.SaveScript(command.Script, trigger.Id);

            // save trigger dependencies
            DependencyActions dependencyAction = new DependencyActions(vc, command.Dependencies);
            dependencyAction.SaveDependencies(trigger);

            // save changes
            vc.SaveChanges();

            // close connections
            vc.Dispose();
        }

        /// <summary>
        /// Save the trigger with window command arguments
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Create trigger command</param>
        private void SaveTriggerWithWindowArguments(ViewsContext vc, CreateTriggerCommand command)
        {
            // initialize the repositories
            Database.Repositories.Repository<Database.Models.Trigger> repoTrigger = new Database.Repositories.Repository<Database.Models.Trigger>(vc);
            Database.Repositories.Repository<Database.Models.Stream> repoStream = new Database.Repositories.Repository<Database.Models.Stream>(vc);
            Database.Repositories.Repository<Database.Models.Stmt> repoStmt = new Database.Repositories.Repository<Database.Models.Stmt>(vc);
            Database.Repositories.Repository<Database.Models.Adapter> repoAdapter = new Database.Repositories.Repository<Database.Models.Adapter>(vc);

            string streamName = command.StreamName;
            Database.Models.Stream stream = repoStream.Find(x => x.Name == streamName);

            Database.Models.Trigger trigger = null;

            TimeSpan d;
            TimeSpan.TryParse(command.ApplyWindow, out d);

            trigger = new Database.Models.Trigger() { CreationDate = DateTime.Now, Name = command.Name, State = (int)UserDefinedObjectStateEnum.Stopped, Type = command.Type.ToString(), IsSystemObject = false, StreamId = stream.Id, DurationTime = d.TotalMilliseconds };
            repoTrigger.Create(trigger);

            int adapterOrder = 0;
            foreach (Tuple<string, string[]> ifTuple in command.IfList)
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

            // save the object script
            ScriptActions scriptActions = new ScriptActions(vc);
            scriptActions.SaveScript(command.Script, trigger.Id);

            // save trigger dependencies
            DependencyActions dependencyAction = new DependencyActions(vc, command.Dependencies);
            dependencyAction.SaveDependencies(trigger);

            // save changes
            vc.SaveChanges();

            // close connections
            vc.Dispose();
        }
    }
}
