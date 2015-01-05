//-----------------------------------------------------------------------
// <copyright file="CreateTriggerCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Create.CreateTrigger
{
    using System;
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for create triggers
    /// </summary>
    internal class CreateTriggerCommand : CreateObjectCommandBase
    {
        /// <summary>
        /// Argument enumerator implementation for this command
        /// </summary>
        private IArgumentEnumerator argumentEnumerator = new CreateTriggerArgumentEnumerator();

        /// <summary>
        /// Dependency enumerator implementation for this command
        /// </summary>
        private IDependencyEnumerator dependencyEnumerator = new CreateTriggerDependencyEnumerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTriggerCommand"/> class
        /// </summary>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public CreateTriggerCommand(PlanNode node) : base(node)
        {
        }

        /// <summary>
        /// Gets command dependency enumerator
        /// </summary>
        protected override IDependencyEnumerator DependencyEnumerator
        {
            get
            {
                return this.dependencyEnumerator;
            }
        }

        /// <summary>
        /// Gets command argument enumerator
        /// </summary>
        protected override IArgumentEnumerator ArgumentEnumerator
        {
            get
            {
                return this.argumentEnumerator;
            }
        }

        /// <summary>
        /// Save trigger arguments
        /// </summary>
        public virtual void SaveArguments()
        {
            // Initialize the context
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            // initialize the repositories
            Database.Repositories.Repository<Database.Models.Trigger> repoTrigger = new Database.Repositories.Repository<Database.Models.Trigger>(vc);
            Database.Repositories.Repository<Database.Models.Stream> repoStream = new Database.Repositories.Repository<Database.Models.Stream>(vc);
            Database.Repositories.Repository<Database.Models.Stmt> repoStmt = new Database.Repositories.Repository<Database.Models.Stmt>(vc);
            Database.Repositories.Repository<Database.Models.Adapter> repoAdapter = new Database.Repositories.Repository<Database.Models.Adapter>(vc);

            string streamName = this.Arguments["StreamName"].Value.ToString();
            Database.Models.Stream stream = repoStream.Find(x => x.Name == streamName);

            Database.Models.Trigger trigger = null;

            int argumentCount = this.Arguments.Count;
            if (argumentCount == 3)
            {
                trigger = new Database.Models.Trigger() { CreationDate = DateTime.Now, Name = this.Arguments["Name"].Value.ToString(), State = (int)UserDefinedObjectStateEnum.Stopped, Type = ObjectTypeEnum.Trigger.ToString(), IsSystemObject = false, StreamId = stream.Id };
                repoTrigger.Create(trigger);
                repoTrigger.Commit();

                foreach (string adapterName in (System.Collections.Generic.List<string>)this.Arguments["SendList"].Value)
                {
                    Database.Models.Adapter adapter = repoAdapter.Find(x => x.Name == adapterName);
                    Database.Models.Stmt stmt = new Database.Models.Stmt() { TriggerId = trigger.Id, AdapterId = adapter.Id, Type = (int)StmtTypeEnum.SendAlways };
                    repoStmt.Create(stmt);
                    repoStmt.Commit();
                }
            }
            else if (argumentCount == 4)
            {
                TimeSpan d;
                TimeSpan.TryParse(this.Arguments["ApplyWindow"].Value.ToString(), out d);

                trigger = new Database.Models.Trigger() { CreationDate = DateTime.Now, Name = this.Arguments["Name"].Value.ToString(), State = (int)UserDefinedObjectStateEnum.Stopped, Type = ObjectTypeEnum.Trigger.ToString(), IsSystemObject = false, StreamId = stream.Id, DurationTime = d.TotalMilliseconds };
                repoTrigger.Create(trigger);
                repoTrigger.Commit();

                int adapterOrder = 0;
                foreach (System.Tuple<string, System.Collections.Generic.List<string>> ifTuple in (System.Collections.Generic.List<System.Tuple<string, System.Collections.Generic.List<string>>>)this.Arguments["IfList"].Value)
                {
                    foreach (string adapterName in (System.Collections.Generic.List<string>)ifTuple.Item2)
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
                        repoStmt.Commit();

                        adapterOrder++;
                    }
                }
            }

            // close connections
            repoAdapter.Dispose();
            repoStmt.Dispose();
            repoStream.Dispose();
            repoTrigger.Dispose();
            vc.Dispose();

            // save trigger dependencies
            this.SaveDependencies(trigger);
        }

        /// <summary>
        /// Contains create trigger logic
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            // save arguments
            this.SaveArguments();
        }

        /// <summary>
        /// save trigger dependencies
        /// </summary>
        /// <param name="trigger">actual trigger</param>
        private void SaveDependencies(Database.Models.Trigger trigger)
        {
            // Initialize the context
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            // initialize repositories
            Database.Repositories.Repository<Database.Models.Dependency> repoDependency = new Database.Repositories.Repository<Database.Models.Dependency>(vc);
            Database.Repositories.Repository<Database.Models.Adapter> repoAdapter = new Database.Repositories.Repository<Database.Models.Adapter>(vc);
            Database.Repositories.Repository<Database.Models.Stream> repoStream = new Database.Repositories.Repository<Database.Models.Stream>(vc);

            foreach (var streamDependency in this.Dependencies)
            {
                if (streamDependency.ObjectType == ObjectTypeEnum.Adapter)
                {
                    Database.Models.Adapter adapter = repoAdapter.Find(x => x.Name == streamDependency.Name);
                    Database.Models.Dependency dependency = new Database.Models.Dependency() { DependencyObjectId = adapter.Id, PrincipalObjectId = trigger.Id };
                    repoDependency.Create(dependency);
                    repoDependency.Commit();
                }
                else if (streamDependency.ObjectType == ObjectTypeEnum.Stream)
                {
                    Database.Models.Stream stream = repoStream.Find(x => x.Name == streamDependency.Name);
                    Database.Models.Dependency dependency = new Database.Models.Dependency() { DependencyObjectId = stream.Id, PrincipalObjectId = trigger.Id };
                    repoDependency.Create(dependency);
                    repoDependency.Commit();
                }
            }

            // close connections
            repoStream.Dispose();
            repoAdapter.Dispose();
            repoDependency.Dispose();
            vc.Dispose();
        }
    }
}
