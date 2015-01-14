//-----------------------------------------------------------------------
// <copyright file="AlterTriggerCommandAction.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;
    using Integra.Vision.Engine.Database.Repositories;

    /// <summary>
    /// Implements all the process of alter an adapter.
    /// </summary>
    internal sealed class AlterTriggerCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                AlterTriggerCommand alterTriggerCommand = command as AlterTriggerCommand;

                using (ViewsContext context = new ViewsContext("EngineDatabase"))
                {
                    if (alterTriggerCommand.IsSimpleTrigger)
                    {
                        this.SaveSimpleTriggerArguments(context, alterTriggerCommand);
                    }
                    else
                    {
                        this.SaveTriggerWithWindowArguments(context, alterTriggerCommand);
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
        /// Contains alter trigger logic.
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Alter trigger command</param>
        private void SaveSimpleTriggerArguments(ViewsContext vc, AlterTriggerCommand command)
        {
            // initialize the repositories
            Database.Repositories.Repository<Database.Models.Trigger> repoTrigger = new Database.Repositories.Repository<Database.Models.Trigger>(vc);
            Database.Repositories.Repository<Database.Models.Stream> repoStream = new Database.Repositories.Repository<Database.Models.Stream>(vc);
            Database.Repositories.Repository<Database.Models.Stmt> repoStmt = new Database.Repositories.Repository<Database.Models.Stmt>(vc);
            Database.Repositories.Repository<Database.Models.Adapter> repoAdapter = new Database.Repositories.Repository<Database.Models.Adapter>(vc);
            Database.Repositories.Repository<Database.Models.Dependency> repoDependency = new Database.Repositories.Repository<Database.Models.Dependency>(vc);

            string streamName = command.StreamName;
            Database.Models.Stream stream = repoStream.Find(x => x.Name == streamName);

            // get the trigger
            Database.Models.Trigger trigger = repoTrigger.Find(x => x.Name == command.Name);

            // delete the statements
            repoStmt.Delete(x => x.TriggerId == trigger.Id);

            // delete the dependencies
            repoDependency.Delete(x => x.PrincipalObjectId == trigger.Id);

            // update the trigger arguments
            trigger.CreationDate = DateTime.Now;
            trigger.State = (int)UserDefinedObjectStateEnum.Stopped;
            trigger.Type = command.Type.ToString();
            trigger.IsSystemObject = false;
            trigger.StreamId = stream.Id;

            // update the trigger
            repoTrigger.Update(trigger);

            foreach (string adapterName in command.SendList)
            {
                Database.Models.Adapter adapter = repoAdapter.Find(x => x.Name == adapterName);
                Database.Models.Stmt stmt = new Database.Models.Stmt() { TriggerId = trigger.Id, AdapterId = adapter.Id, Type = (int)StmtTypeEnum.SendAlways };
                repoStmt.Create(stmt);
            }

            // update the object script
            ScriptActions scriptActions = new ScriptActions(vc);
            scriptActions.UpdateScript(command.Script, trigger.Id);

            // save trigger dependencies
            DependencyActions dependencyAction = new DependencyActions(vc, command.Dependencies);
            dependencyAction.SaveDependencies(trigger);

            // save changes
            vc.SaveChanges();

            // close connections
            vc.Dispose();
        }

        /// <summary>
        /// Contains alter trigger logic.
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Alter trigger command</param>
        private void SaveTriggerWithWindowArguments(ViewsContext vc, AlterTriggerCommand command)
        {
            // initialize the repositories
            Database.Repositories.Repository<Database.Models.Trigger> repoTrigger = new Database.Repositories.Repository<Database.Models.Trigger>(vc);
            Database.Repositories.Repository<Database.Models.Stream> repoStream = new Database.Repositories.Repository<Database.Models.Stream>(vc);
            Database.Repositories.Repository<Database.Models.Stmt> repoStmt = new Database.Repositories.Repository<Database.Models.Stmt>(vc);
            Database.Repositories.Repository<Database.Models.Adapter> repoAdapter = new Database.Repositories.Repository<Database.Models.Adapter>(vc);
            Database.Repositories.Repository<Database.Models.Dependency> repoDependency = new Database.Repositories.Repository<Database.Models.Dependency>(vc);

            string streamName = command.StreamName;
            Database.Models.Stream stream = repoStream.Find(x => x.Name == streamName);
            
            // get the trigger
            Database.Models.Trigger trigger = repoTrigger.Find(x => x.Name == command.Name);

            // delete the statements
            repoStmt.Delete(x => x.TriggerId == trigger.Id);

            // delete the dependencies
            repoDependency.Delete(x => x.PrincipalObjectId == trigger.Id);

            TimeSpan d;
            TimeSpan.TryParse(command.ApplyWindow, out d);

            // update the trigger arguments
            trigger.CreationDate = DateTime.Now;
            trigger.State = (int)UserDefinedObjectStateEnum.Stopped;
            trigger.Type = command.Type.ToString();
            trigger.IsSystemObject = false;
            trigger.StreamId = stream.Id;
            trigger.DurationTime = d.TotalMilliseconds;

            // update the trigger
            repoTrigger.Update(trigger);

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

            // update the object script
            ScriptActions scriptActions = new ScriptActions(vc);
            scriptActions.UpdateScript(command.Script, trigger.Id);

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
