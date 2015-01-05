//-----------------------------------------------------------------------
// <copyright file="DropStreamCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Drop.DropStream
{
    using System;
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for drop streams
    /// </summary>
    internal class DropStreamCommand : DropObjectCommandBase
    {
        /// <summary>
        /// Argument enumerator implementation for this command
        /// </summary>
        private IArgumentEnumerator argumentEnumerator = new DropStreamArgumentEnumerator();

        /// <summary>
        /// Dependency enumerator implementation for this command
        /// </summary>
        private IDependencyEnumerator dependencyEnumerator = new DropStreamDependencyEnumerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="DropStreamCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public DropStreamCommand(PlanNode node) : base(node)
        {
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get
            {
                return CommandTypeEnum.DropStream;
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
        /// Delete the stream arguments
        /// </summary>
        public virtual void DeleteArguments()
        {
            // initialize context
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            // create repository
            Database.Repositories.Repository<Database.Models.Stream> repoStream = new Database.Repositories.Repository<Database.Models.Stream>(vc);
            Database.Repositories.Repository<Database.Models.StreamCondition> repoStreamCondition = new Database.Repositories.Repository<Database.Models.StreamCondition>(vc);
            Database.Repositories.Repository<Database.Models.PList> repoProjectionList = new Database.Repositories.Repository<Database.Models.PList>(vc);
            Database.Repositories.Repository<Database.Models.Dependency> repoDependency = new Database.Repositories.Repository<Database.Models.Dependency>(vc);
            Database.Repositories.Repository<Database.Models.SourceAssignedToStream> repoSats = new Database.Repositories.Repository<Database.Models.SourceAssignedToStream>(vc);

            // get the object name
            string objectName = this.Arguments["Name"].Value.ToString();

            // get the stream
            Database.Models.Stream stream = repoStream.Find(x => x.Name == objectName);

            // detele the conditions
            repoStreamCondition.Delete(x => x.StreamId == stream.Id);
            int stmtCount = repoStreamCondition.Commit();

            // throw an exception if not deleted a statement
            if (stmtCount < 1)
            {
                // close connection
                repoStream.Dispose();
                repoStreamCondition.Dispose();
                repoProjectionList.Dispose();
                repoDependency.Dispose();
                vc.Dispose();

                // throw the exception 
                throw new Integra.Vision.Engine.Exceptions.DropUserDefinedObjectException("The stream conditions were not removed");
            }

            // delete the projection
            repoProjectionList.Delete(x => x.StreamId == stream.Id);
            int projectionCount = repoProjectionList.Commit();

            if (projectionCount < 1)
            {
                // close connection
                repoStream.Dispose();
                repoStreamCondition.Dispose();
                repoProjectionList.Dispose();
                repoDependency.Dispose();
                vc.Dispose();

                // throw the exception 
                throw new Integra.Vision.Engine.Exceptions.DropUserDefinedObjectException("The projection was not removed");
            }

            // delete asignations sources-stream
            repoSats.Delete(x => x.StreamId == stream.Id);
            int satsCount = repoSats.Commit();

            if (satsCount < 1)
            {
                // close connection
                repoStream.Dispose();
                repoStreamCondition.Dispose();
                repoProjectionList.Dispose();
                repoDependency.Dispose();
                vc.Dispose();

                // throw the exception 
                throw new Integra.Vision.Engine.Exceptions.DropUserDefinedObjectException("The asigned sources were not removed");
            }

            // delete the dependencies
            repoDependency.Delete(x => x.PrincipalObjectId == stream.Id);
            int dependencyCount = repoDependency.Commit();

            // throw an exception if not deleted a dependency
            if (dependencyCount < 1)
            {
                // close connection
                repoStream.Dispose();
                repoStreamCondition.Dispose();
                repoProjectionList.Dispose();
                repoDependency.Dispose();
                vc.Dispose();

                // throw the exception 
                throw new Integra.Vision.Engine.Exceptions.DropUserDefinedObjectException("The dependencies were not removed");
            }

            // delete the object
            repoStream.Delete(x => x.Name == objectName);
            int objectCount = repoStream.Commit();

            // throw an exception if not deleted a object
            if (objectCount != 1)
            {
                // close connection
                repoStream.Dispose();
                repoStreamCondition.Dispose();
                repoProjectionList.Dispose();
                repoDependency.Dispose();
                vc.Dispose();

                // throw the exception 
                throw new Integra.Vision.Engine.Exceptions.DropUserDefinedObjectException("The object '" + objectName + "' was not removed");
            }

            // close connection
            repoStream.Dispose();
            repoStreamCondition.Dispose();
            repoProjectionList.Dispose();
            repoDependency.Dispose();
            vc.Dispose();
        }

        /// <summary>
        /// Contains drop stream logic
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            // delete arguments
            this.DeleteArguments();
        }
    }
}
