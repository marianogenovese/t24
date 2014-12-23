//-----------------------------------------------------------------------
// <copyright file="DropAdapterCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Drop.DropAdapter
{
    using System;

    /// <summary>
    /// Base class for drop adapters
    /// </summary>
    internal class DropAdapterCommand : DropObjectCommandBase
    {
        /// <summary>
        /// Argument enumerator implementation for this command
        /// </summary>
        private IArgumentEnumerator argumentEnumerator = new DropAdapterArgumentEnumerator();

        /// <summary>
        /// Dependency enumerator implementation for this command
        /// </summary>
        private IDependencyEnumerator dependencyEnumerator = new DropAdapterDependencyEnumerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="DropAdapterCommand"/> class
        /// </summary>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public DropAdapterCommand(string commandText, ISecurityContext securityContext) : base(CommandTypeEnum.DropAdapter, commandText, securityContext)
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
        /// Contains drop adapter logic
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            // save the arguments
            this.DeleteArguments();
        }

        /// <summary>
        /// Delete the command arguments
        /// </summary>
        protected virtual void DeleteArguments()
        {
            // initialize context
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            // create repository
            Database.Repositories.Repository<Database.Models.Adapter> repoAdapter = new Database.Repositories.Repository<Database.Models.Adapter>(vc);
            Database.Repositories.Repository<Database.Models.Arg> repoArg = new Database.Repositories.Repository<Database.Models.Arg>(vc);
            Database.Repositories.Repository<Database.Models.Dependency> repoDependency = new Database.Repositories.Repository<Database.Models.Dependency>(vc);

            // get the object name
            string objectName = this.Arguments["Name"].Value.ToString();

            // get the stream
            Database.Models.Adapter adapter = repoAdapter.Find(x => x.Name == objectName);

            // detele the conditions
            repoArg.Delete(x => x.AdapterId == adapter.Id);
            int sourceConditionCount = repoArg.Commit();

            // throw an exception if not deleted a statement
            if (sourceConditionCount < 1)
            {
                // close connection
                repoAdapter.Dispose();
                repoArg.Dispose();
                repoDependency.Dispose();
                vc.Dispose();

                // throw the exception 
                throw new Integra.Vision.Engine.Exceptions.DropUserDefinedObjectException("The adapter arguments were not removed");
            }

            // delete the dependencies
            repoDependency.Delete(x => x.PrincipalObjectId == adapter.Id);
            int dependencyCount = repoDependency.Commit();

            // throw an exception if not deleted a dependency
            if (dependencyCount < 1)
            {
                // close connection
                repoAdapter.Dispose();
                repoArg.Dispose();
                repoDependency.Dispose();
                vc.Dispose();

                // throw the exception 
                throw new Integra.Vision.Engine.Exceptions.DropUserDefinedObjectException("The dependencies were not removed");
            }

            // delete the object
            repoAdapter.Delete(x => x.Name == objectName);
            int objectCount = repoAdapter.Commit();

            // throw an exception if not deleted a object
            if (objectCount != 1)
            {
                // close connection
                repoAdapter.Dispose();
                repoArg.Dispose();
                repoDependency.Dispose();
                vc.Dispose();

                // throw the exception 
                throw new Integra.Vision.Engine.Exceptions.DropUserDefinedObjectException("The object '" + objectName + "' was not removed");
            }

            // close connection
            repoAdapter.Dispose();
            repoArg.Dispose();
            repoDependency.Dispose();
            vc.Dispose();
        }
    }
}
