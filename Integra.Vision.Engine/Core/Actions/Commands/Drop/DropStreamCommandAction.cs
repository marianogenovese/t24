//-----------------------------------------------------------------------
// <copyright file="DropStreamCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;

    /// <summary>
    /// Implements all the process of delete a stream.
    /// </summary>
    internal sealed class DropStreamCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                using (ObjectsContext context = new ObjectsContext("EngineDatabase"))
                {
                    this.DeleteObject(context, command as DropStreamCommand);
                }

                return new OkCommandResult();
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Contains drop stream logic
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Drop stream command</param>
        private void DeleteObject(ObjectsContext vc, DropStreamCommand command)
        {
            // create repository
            Database.Repositories.Repository<Database.Models.Stream> repoStream = new Database.Repositories.Repository<Database.Models.Stream>(vc);
            Database.Repositories.Repository<Database.Models.StreamCondition> repoStreamCondition = new Database.Repositories.Repository<Database.Models.StreamCondition>(vc);
            Database.Repositories.Repository<Database.Models.PList> repoProjectionList = new Database.Repositories.Repository<Database.Models.PList>(vc);
            Database.Repositories.Repository<Database.Models.Dependency> repoDependency = new Database.Repositories.Repository<Database.Models.Dependency>(vc);
            Database.Repositories.Repository<Database.Models.SourceAssignedToStream> repoSats = new Database.Repositories.Repository<Database.Models.SourceAssignedToStream>(vc);

            // get the stream
            Database.Models.Stream stream = repoStream.Find(x => x.Name == command.Name);

            // detele the conditions
            repoStreamCondition.Delete(x => x.StreamId == stream.Id);
            /*
            int stmtCount = repoStreamCondition.Commit();

            // throw an exception if not deleted a statement
            if (stmtCount < 1)
            {
                // close connection
                vc.Dispose();

                // throw the exception 
                throw new Integra.Vision.Engine.Exceptions.DropUserDefinedObjectException("The stream conditions were not removed");
            }
            */

            // delete the projection
            repoProjectionList.Delete(x => x.StreamId == stream.Id);
            int projectionCount = repoProjectionList.Commit();

            if (projectionCount < 1)
            {
                // close connection
                vc.Dispose();

                // throw the exception 
                throw new Integra.Vision.Engine.Exceptions.DropUserDefinedObjectException("The projection was not removed");
            }

            // delete asignations sources-stream
            repoSats.Delete(x => x.StreamId == stream.Id);
            /*
            int satsCount = repoSats.Commit();

            if (satsCount < 1)
            {
                // close connection
                vc.Dispose();

                // throw the exception 
                throw new Integra.Vision.Engine.Exceptions.DropUserDefinedObjectException("The asigned sources were not removed");
            }
            */

            // delete the dependencies
            repoDependency.Delete(x => x.PrincipalObjectId == stream.Id);
            /*
            int dependencyCount = repoDependency.Commit();

            // throw an exception if not deleted a dependency
            if (dependencyCount < 1)
            {
                // close connection
                vc.Dispose();

                // throw the exception 
                throw new Integra.Vision.Engine.Exceptions.DropUserDefinedObjectException("The dependencies were not removed");
            }
            */

            // delete the object
            repoStream.Delete(x => x.Name == command.Name);
            int objectCount = vc.SaveChanges();

            // throw an exception if not deleted a object
            if (objectCount < 1)
            {
                // close connection
                vc.Dispose();

                // throw the exception 
                throw new Integra.Vision.Engine.Exceptions.DropUserDefinedObjectException("The object '" + command.Name + "' was not removed");
            }

            // close connection
            vc.Dispose();
        }
    }
}
