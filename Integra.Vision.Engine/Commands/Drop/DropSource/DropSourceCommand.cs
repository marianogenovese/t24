//-----------------------------------------------------------------------
// <copyright file="DropSourceCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Drop.DropSource
{
    using System;
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for drop sources
    /// </summary>
    internal class DropSourceCommand : DropObjectCommandBase
    {
        /// <summary>
        /// Argument enumerator implementation for this command
        /// </summary>
        private IArgumentEnumerator argumentEnumerator = new DropSourceArgumentEnumerator();

        /// <summary>
        /// Dependency enumerator implementation for this command
        /// </summary>
        private IDependencyEnumerator dependencyEnumerator = new DropSourceDependencyEnumerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="DropSourceCommand"/> class
        /// </summary>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public DropSourceCommand(PlanNode node) : base(node)
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
        /// Delete object arguments
        /// </summary>
        public virtual void DeleteArguments()
        {
            // initialize context
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            // create repository
            Database.Repositories.Repository<Database.Models.Source> repoSource = new Database.Repositories.Repository<Database.Models.Source>(vc);
            Database.Repositories.Repository<Database.Models.SourceCondition> repoSourceCondition = new Database.Repositories.Repository<Database.Models.SourceCondition>(vc);
            Database.Repositories.Repository<Database.Models.Dependency> repoDependency = new Database.Repositories.Repository<Database.Models.Dependency>(vc);

            // get the object name
            string objectName = this.Arguments["Name"].Value.ToString();

            // get the stream
            Database.Models.Source source = repoSource.Find(x => x.Name == objectName);

            // detele the conditions
            repoSourceCondition.Delete(x => x.SourceId == source.Id);
            int sourceConditionCount = repoSourceCondition.Commit();

            // throw an exception if not deleted a statement
            if (sourceConditionCount < 1)
            {
                // close connection
                repoSource.Dispose();
                repoSourceCondition.Dispose();
                repoDependency.Dispose();
                vc.Dispose();

                // throw the exception 
                throw new Integra.Vision.Engine.Exceptions.DropUserDefinedObjectException("The source conditions were not removed");
            }

            // delete the dependencies
            repoDependency.Delete(x => x.PrincipalObjectId == source.Id);
            int dependencyCount = repoDependency.Commit();

            // throw an exception if not deleted a dependency
            if (dependencyCount < 1)
            {
                // close connection
                repoSource.Dispose();
                repoSourceCondition.Dispose();
                repoDependency.Dispose();
                vc.Dispose();

                // throw the exception 
                throw new Integra.Vision.Engine.Exceptions.DropUserDefinedObjectException("The dependencies were not removed");
            }

            // delete the object
            repoSource.Delete(x => x.Name == objectName);
            int objectCount = repoSource.Commit();

            // throw an exception if not deleted a object
            if (objectCount != 1)
            {
                // close connection
                repoSource.Dispose();
                repoSourceCondition.Dispose();
                repoDependency.Dispose();
                vc.Dispose();

                // throw the exception 
                throw new Integra.Vision.Engine.Exceptions.DropUserDefinedObjectException("The object '" + objectName + "' was not removed");
            }

            // close connection
            repoSource.Dispose();
            repoSourceCondition.Dispose();
            repoDependency.Dispose();
            vc.Dispose();
        }

        /// <summary>
        /// Contains drop source logic
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            // delete arguments
            this.DeleteArguments();
        }
    }
}
