//-----------------------------------------------------------------------
// <copyright file="StartObjectCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Start
{
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for start objects
    /// </summary>
    internal sealed class StartObjectCommand : StartStopObjectCommandBase
    {
        /// <summary>
        /// Argument enumerator implementation for this command
        /// </summary>
        private IArgumentEnumerator argumentEnumerator = new StartObjectArgumentEnumerator();

        /// <summary>
        /// Dependency enumerator implementation for this command
        /// </summary>
        private IDependencyEnumerator dependencyEnumerator = new StartObjectDependencyEnumerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="StartObjectCommand"/> class
        /// </summary>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public StartObjectCommand(PlanNode node)
            : base(node)
        {
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
        /// Contains start object logic
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            // initialize context
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            // get the object name
            string objectName = this.Arguments["Name"].Value.ToString();

            if (this.Arguments["UserDefinedObject"].Value.ToString().ToLower().Equals(ObjectTypeEnum.Adapter.ToString().ToLower()))
            {
                // create repository
                Database.Repositories.Repository<Database.Models.Adapter> repoAdapter = new Database.Repositories.Repository<Database.Models.Adapter>(vc);

                // get the adapter
                Database.Models.Adapter adapter = repoAdapter.Find(x => x.Name == objectName);

                // update the adapter
                adapter.State = (int)UserDefinedObjectStateEnum.Started;
                repoAdapter.Commit();
                
                // close connection
                repoAdapter.Dispose();
            }
            else if (this.Arguments["UserDefinedObject"].Value.ToString().ToLower().Equals(ObjectTypeEnum.Source.ToString().ToLower()))
            {
                // create repository
                Database.Repositories.Repository<Database.Models.Source> repoSource = new Database.Repositories.Repository<Database.Models.Source>(vc);

                // get the source
                Database.Models.Source source = repoSource.Find(x => x.Name == objectName);

                // update the state
                source.State = (int)UserDefinedObjectStateEnum.Started;
                repoSource.Commit();

                // close connection
                repoSource.Dispose();
            }
            else if (this.Arguments["UserDefinedObject"].Value.ToString().ToLower().Equals(ObjectTypeEnum.Stream.ToString().ToLower()))
            {
                // create repository
                Database.Repositories.Repository<Database.Models.Stream> repoStream = new Database.Repositories.Repository<Database.Models.Stream>(vc);
                
                // get the source
                Database.Models.Stream stream = repoStream.Find(x => x.Name == objectName);

                // update the state
                stream.State = (int)UserDefinedObjectStateEnum.Started;
                repoStream.Commit();

                // close connection
                repoStream.Dispose();
            }
            else if (this.Arguments["UserDefinedObject"].Value.ToString().ToLower().Equals(ObjectTypeEnum.Trigger.ToString().ToLower()))
            {
                // create repository
                Database.Repositories.Repository<Database.Models.Trigger> repoTrigger = new Database.Repositories.Repository<Database.Models.Trigger>(vc);

                // get the source
                Database.Models.Trigger trigger = repoTrigger.Find(x => x.Name == objectName);

                // update the state
                trigger.State = (int)UserDefinedObjectStateEnum.Started;
                repoTrigger.Commit();

                // close connection
                repoTrigger.Dispose();
            }

            // close connection
            vc.Dispose();
        }
    }
}
