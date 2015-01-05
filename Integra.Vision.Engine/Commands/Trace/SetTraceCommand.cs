//-----------------------------------------------------------------------
// <copyright file="SetTraceCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Trace
{
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for trace objects
    /// </summary>
    internal sealed class SetTraceCommand : SetCommandBase
    {
        /// <summary>
        /// Argument enumerator implementation for this command
        /// </summary>
        private IArgumentEnumerator argumentEnumerator = new SetTraceArgumentEnumerator();

        /// <summary>
        /// Dependency enumerator implementation for this command
        /// </summary>
        private IDependencyEnumerator dependencyEnumerator = new SetTraceDependencyEnumerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="SetTraceCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public SetTraceCommand(PlanNode node) : base(node)
        {
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
        /// Contains trace object logic
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            // initialize context
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            // create repository
            Database.Repositories.Repository<Database.Models.SetTrace> repoSetTrace = new Database.Repositories.Repository<Database.Models.SetTrace>(vc);
            Database.Repositories.Repository<Database.Models.UserDefinedObject> repoObject = new Database.Repositories.Repository<Database.Models.UserDefinedObject>(vc);

            // get the object
            string objectName = this.Arguments["ObjectOrFamily"].Value.ToString();
            int level = int.Parse(this.Arguments["Level"].Value.ToString());

            if (objectName.ToLower().Equals(ObjectTypeEnum.Adapter.ToString().ToLower()))
            {
                string type = ObjectTypeEnum.Adapter.ToString();
                Database.Models.SetTrace setTrace;
                var objects = repoObject.Filter(x => x.Type == type);

                foreach (Database.Models.UserDefinedObject userDefinedObject in objects)
                {
                    // create the trace
                    setTrace = new Database.Models.SetTrace() { Level = level, UserDefinedObjectId = userDefinedObject.Id, CreationDate = System.DateTime.Now };
                    repoSetTrace.Create(setTrace);
                }

                repoSetTrace.Commit();
            }
            else if (objectName.ToLower().Equals(ObjectTypeEnum.Source.ToString().ToLower()))
            {
                string type = ObjectTypeEnum.Source.ToString();
                var objects = repoObject.Filter(x => x.Type == type);
                foreach (Database.Models.UserDefinedObject userDefinedObject in objects)
                {
                    // create the trace
                    Database.Models.SetTrace setTrace = new Database.Models.SetTrace() { Level = level, UserDefinedObjectId = userDefinedObject.Id, CreationDate = System.DateTime.Now };
                    repoSetTrace.Create(setTrace);
                }

                repoSetTrace.Commit();
            }
            else if (objectName.ToLower().Equals(ObjectTypeEnum.Stream.ToString().ToLower()))
            {
                string type = ObjectTypeEnum.Stream.ToString();
                var objects = repoObject.Filter(x => x.Type == type);
                foreach (Database.Models.UserDefinedObject userDefinedObject in objects)
                {
                    // create the trace
                    Database.Models.SetTrace setTrace = new Database.Models.SetTrace() { Level = level, UserDefinedObjectId = userDefinedObject.Id, CreationDate = System.DateTime.Now };
                    repoSetTrace.Create(setTrace);
                }

                repoSetTrace.Commit();
            }
            else if (objectName.ToLower().Equals(ObjectTypeEnum.Trigger.ToString().ToLower()))
            {
                string type = ObjectTypeEnum.Trigger.ToString();
                var objects = repoObject.Filter(x => x.Type == type);
                foreach (Database.Models.UserDefinedObject userDefinedObject in objects)
                {
                    // create the trace
                    Database.Models.SetTrace setTrace = new Database.Models.SetTrace() { Level = level, UserDefinedObjectId = userDefinedObject.Id, CreationDate = System.DateTime.Now };
                    repoSetTrace.Create(setTrace);
                }

                repoSetTrace.Commit();
            }
            else if (objectName.ToLower().Equals(ObjectTypeEnum.Engine.ToString().ToLower()))
            {
                string type = ObjectTypeEnum.Engine.ToString();
                var objects = repoObject.GetAll();
                foreach (Database.Models.UserDefinedObject userDefinedObject in objects)
                {
                    // create the trace
                    Database.Models.SetTrace setTrace = new Database.Models.SetTrace() { Level = level, UserDefinedObjectId = userDefinedObject.Id, CreationDate = System.DateTime.Now };
                    repoSetTrace.Create(setTrace);
                }

                repoSetTrace.Commit();
            }
            else
            {
                // find the user defined object
                Database.Models.UserDefinedObject userDefinedObject = repoObject.Find(x => x.Name == objectName);

                // create the trace
                Database.Models.SetTrace setTrace = new Database.Models.SetTrace() { Level = level, UserDefinedObjectId = userDefinedObject.Id, CreationDate = System.DateTime.Now };
                repoSetTrace.Create(setTrace);
                repoSetTrace.Commit();
            }

            // close connections
            repoSetTrace.Dispose();
            repoObject.Dispose();
            vc.Dispose();
        }
    }
}
