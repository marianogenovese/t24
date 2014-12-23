//-----------------------------------------------------------------------
// <copyright file="CreateSourceCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Create.CreateSource
{
    using System;
    using Integra.Vision.Engine.Database.Repositories;

    /// <summary>
    /// Base class for create sources
    /// </summary>
    internal class CreateSourceCommand : CreateObjectCommandBase
    {
        /// <summary>
        /// Argument enumerator implementation for this command
        /// </summary>
        private IArgumentEnumerator argumentEnumerator = new CreateSourceArgumentEnumerator();

        /// <summary>
        /// Dependency enumerator implementation for this command
        /// </summary>
        private IDependencyEnumerator dependencyEnumerator = new CreateSourceDependencyEnumerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSourceCommand"/> class
        /// </summary>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public CreateSourceCommand(string commandText, ISecurityContext securityContext) : base(CommandTypeEnum.CreateSource, commandText, securityContext)
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
        /// save the command arguments
        /// </summary>
        public virtual void SaveArguments()
        {
            // Initialize the context
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            // get the adapter
            string adapterName = this.Arguments["From"].Value.ToString();
            Database.Repositories.Repository<Database.Models.Adapter> repoAdapter = new Database.Repositories.Repository<Database.Models.Adapter>(vc);
            Database.Models.Adapter adapter = repoAdapter.Find(x => x.Name == adapterName);

            if (adapter == null)
            {
                throw new Integra.Vision.Engine.Exceptions.NonExistentObjectException("The adapter '" + adapterName + "' does not exist");
            }

            // create the source
            Database.Repositories.Repository<Database.Models.Source> repoSource = new Database.Repositories.Repository<Database.Models.Source>(vc);
            Database.Models.Source source = new Database.Models.Source() { AdapterId = adapter.Id, CreationDate = DateTime.Now, IsSystemObject = false, Name = this.Arguments["Name"].Value.ToString(), State = (int)UserDefinedObjectStateEnum.Stopped, Type = ObjectTypeEnum.Source.ToString() };
            repoSource.Create(source);
            repoSource.Commit();

            // create the conditions
            Database.Repositories.Repository<Database.Models.SourceCondition> repoSourceCondition = new Database.Repositories.Repository<Database.Models.SourceCondition>(vc);
            Database.Models.SourceCondition sourceCondition = new Database.Models.SourceCondition() { Expression = this.Arguments["Where"].Value.ToString(), SourceId = source.Id, Type = (int)ConditionTypeEnum.FilterCondition };
            repoSourceCondition.Create(sourceCondition);
            repoSourceCondition.Commit();

            // close connections
            repoAdapter.Dispose();
            repoSource.Dispose();
            repoSourceCondition.Dispose();
            vc.Dispose();

            // save dependencies of the source
            this.SaveDependencies(source);
        }

        /// <summary>
        /// Contains create source logic
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            // save arguments
            this.SaveArguments();
        }

        /// <summary>
        /// save the dependencies of the actual object
        /// </summary>
        /// <param name="source">the actual source</param>
        private void SaveDependencies(Database.Models.Source source)
        {
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            Repository<Database.Models.Dependency> repoDependency = new Repository<Database.Models.Dependency>(vc);
            Repository<Database.Models.Adapter> repoAdapter = new Repository<Database.Models.Adapter>(vc);

            foreach (var sourceDependency in this.Dependencies)
            {
                Database.Models.Adapter adapter = repoAdapter.Find(x => x.Name == sourceDependency.Name);

                if (adapter == null)
                {
                    throw new Integra.Vision.Engine.Exceptions.NonExistentObjectException("The adapter '" + sourceDependency.Name + "' does not exist");
                }

                Database.Models.Dependency dependency = new Database.Models.Dependency() { DependencyObjectId = adapter.Id, PrincipalObjectId = source.Id };
                repoDependency.Create(dependency);
                repoDependency.Commit();
            }

            repoDependency.Dispose();
            repoAdapter.Dispose();
            vc.Dispose();
        }
    }
}
