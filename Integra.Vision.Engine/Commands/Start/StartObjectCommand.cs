//-----------------------------------------------------------------------
// <copyright file="StartObjectCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for start objects
    /// </summary>
    internal sealed class StartObjectCommand : StartStopObjectCommandBase
    {
        /// <summary>
        /// Execution plan node
        /// </summary>
        private readonly PlanNode node;

        /// <summary>
        /// Argument enumerator implementation for this command
        /// </summary>
        private IArgumentEnumerator argumentEnumerator;

        /// <summary>
        /// Dependency enumerator implementation for this command
        /// </summary>
        private IDependencyEnumerator dependencyEnumerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartObjectCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public StartObjectCommand(PlanNode node) : base(node)
        {
            this.node = node;
        }
        
        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Gets command argument enumerator
        /// </summary>
        protected override IArgumentEnumerator ArgumentEnumerator
        {
            get
            {
                if (this.argumentEnumerator == null)
                {
                    this.argumentEnumerator = new StartObjectArgumentEnumerator(this.node);
                }

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
                if (this.dependencyEnumerator == null)
                {
                    this.dependencyEnumerator = new StartObjectDependencyEnumerator(this.node);
                }

                return this.dependencyEnumerator;
            }
        }

        /// <summary>
        /// Contains start object logic
        /// </summary>
        protected override void OnExecute()
        {
            /*
             * Cambiar
             */
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
            }
            else if (this.Arguments["UserDefinedObject"].Value.ToString().ToLower().Equals(ObjectTypeEnum.Source.ToString().ToLower()))
            {
                // create repository
                Database.Repositories.Repository<Database.Models.Source> repoSource = new Database.Repositories.Repository<Database.Models.Source>(vc);

                // get the source
                Database.Models.Source source = repoSource.Find(x => x.Name == objectName);

                // update the state
                source.State = (int)UserDefinedObjectStateEnum.Started;
            }
            else if (this.Arguments["UserDefinedObject"].Value.ToString().ToLower().Equals(ObjectTypeEnum.Stream.ToString().ToLower()))
            {
                // create repository
                Database.Repositories.Repository<Database.Models.Stream> repoStream = new Database.Repositories.Repository<Database.Models.Stream>(vc);
                
                // get the source
                Database.Models.Stream stream = repoStream.Find(x => x.Name == objectName);

                // update the state
                stream.State = (int)UserDefinedObjectStateEnum.Started;
            }
            else if (this.Arguments["UserDefinedObject"].Value.ToString().ToLower().Equals(ObjectTypeEnum.Trigger.ToString().ToLower()))
            {
                // create repository
                Database.Repositories.Repository<Database.Models.Trigger> repoTrigger = new Database.Repositories.Repository<Database.Models.Trigger>(vc);

                // get the source
                Database.Models.Trigger trigger = repoTrigger.Find(x => x.Name == objectName);

                // update the state
                trigger.State = (int)UserDefinedObjectStateEnum.Started;
            }

            // save changes
            vc.SaveChanges();

            // close connection
            vc.Dispose();
        }
    }
}
