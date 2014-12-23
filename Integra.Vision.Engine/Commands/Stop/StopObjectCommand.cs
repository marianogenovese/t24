//-----------------------------------------------------------------------
// <copyright file="StopObjectCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Stop
{
    /// <summary>
    /// Base class for stop objects
    /// </summary>
    internal sealed class StopObjectCommand : StartStopObjectCommandBase
    {
        /// <summary>
        /// Argument enumerator implementation for this command
        /// </summary>
        private IArgumentEnumerator argumentEnumerator = new StopObjectArgumentEnumerator();

        /// <summary>
        /// Dependency enumerator implementation for this command
        /// </summary>
        private IDependencyEnumerator dependencyEnumerator = new StopObjectDependencyEnumerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="StopObjectCommand"/> class
        /// </summary>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public StopObjectCommand(string commandText, ISecurityContext securityContext) : base(CommandTypeEnum.StopAdapter, commandText, securityContext)
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
        /// Contains stop object logic
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
                adapter.State = (int)UserDefinedObjectStateEnum.Stopped;
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
                source.State = (int)UserDefinedObjectStateEnum.Stopped;
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
                stream.State = (int)UserDefinedObjectStateEnum.Stopped;
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
                trigger.State = (int)UserDefinedObjectStateEnum.Stopped;
                repoTrigger.Commit();

                // close connection
                repoTrigger.Dispose();
            }

            // close connection
            vc.Dispose();
        }
    }
}
