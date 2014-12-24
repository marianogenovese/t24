//-----------------------------------------------------------------------
// <copyright file="DropTriggerCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Drop.DropTrigger
{
    using Integra.Vision.Language;
    using System;

    /// <summary>
    /// Base class for drop trigger
    /// </summary>
    internal class DropTriggerCommand : DropObjectCommandBase
    {
        /// <summary>
        /// Argument enumerator implementation for this command
        /// </summary>
        private IArgumentEnumerator argumentEnumerator = new DropTriggerArgumentEnumerator();

        /// <summary>
        /// Dependency enumerator implementation for this command
        /// </summary>
        private IDependencyEnumerator dependencyEnumerator = new DropTriggerDependencyEnumerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="DropTriggerCommand"/> class
        /// </summary>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public DropTriggerCommand(PlanNode node) : base(node)
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
        /// Delete the trigger arguments
        /// </summary>
        public virtual void DeleteArguments()
        {
            // initialize context
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            // create repository
            Database.Repositories.Repository<Database.Models.Trigger> repoTrigger = new Database.Repositories.Repository<Database.Models.Trigger>(vc);
            Database.Repositories.Repository<Database.Models.Stmt> repoStmt = new Database.Repositories.Repository<Database.Models.Stmt>(vc);
            Database.Repositories.Repository<Database.Models.Dependency> repoDependency = new Database.Repositories.Repository<Database.Models.Dependency>(vc);

            // get the object name
            string objectName = this.Arguments["Name"].Value.ToString();

            // get the trigger
            Database.Models.Trigger trigger = repoTrigger.Find(x => x.Name == objectName);

            // detele the statements
            repoStmt.Delete(x => x.TriggerId == trigger.Id);
            int stmtCount = repoStmt.Commit();

            // throw an exception if not deleted a statement
            if (stmtCount < 1)
            {
                // close connection
                repoTrigger.Dispose();
                repoStmt.Dispose();
                repoDependency.Dispose();
                vc.Dispose();

                // throw the exception 
                throw new Integra.Vision.Engine.Exceptions.DropUserDefinedObjectException("The trigger statements were not removed");
            }

            // delete the dependencies
            repoDependency.Delete(x => x.PrincipalObjectId == trigger.Id);
            int dependencyCount = repoDependency.Commit();

            // throw an exception if not deleted a dependency
            if (dependencyCount < 1)
            {
                // close connection
                repoTrigger.Dispose();
                repoStmt.Dispose();
                repoDependency.Dispose();
                vc.Dispose();

                // throw the exception 
                throw new Integra.Vision.Engine.Exceptions.DropUserDefinedObjectException("The dependencies were not removed");
            }

            // delete the object
            repoTrigger.Delete(x => x.Name == objectName);
            int objectCount = repoTrigger.Commit();

            // throw an exception if not deleted a object
            if (objectCount != 1)
            {
                // close connection
                repoTrigger.Dispose();
                repoStmt.Dispose();
                repoDependency.Dispose();
                vc.Dispose();

                // throw the exception 
                throw new Integra.Vision.Engine.Exceptions.DropUserDefinedObjectException("The object '" + objectName + "' was not removed");
            }

            // close connection
            repoTrigger.Dispose();
            repoStmt.Dispose();
            repoDependency.Dispose();
            vc.Dispose();
        }

        /// <summary>
        /// Contains drop trigger logic
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            // delete arguments
            this.DeleteArguments();
        }
    }
}
