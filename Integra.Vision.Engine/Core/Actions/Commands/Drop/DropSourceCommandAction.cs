//-----------------------------------------------------------------------
// <copyright file="DropSourceCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;

    /// <summary>
    /// Implements all the process of delete a source.
    /// </summary>
    internal sealed class DropSourceCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                using (ObjectsContext context = new ObjectsContext("EngineDatabase"))
                {
                    this.DeleteObject(context, command as DropSourceCommand);
                    return new OkCommandResult();
                }
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Contains drop source logic
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Drop source command</param>
        private void DeleteObject(ObjectsContext vc, DropSourceCommand command)
        {
            // create repository
            Database.Repositories.Repository<Database.Models.Source> repoSource = new Database.Repositories.Repository<Database.Models.Source>(vc);
            Database.Repositories.Repository<Database.Models.SourceCondition> repoSourceCondition = new Database.Repositories.Repository<Database.Models.SourceCondition>(vc);
            Database.Repositories.Repository<Database.Models.Dependency> repoDependency = new Database.Repositories.Repository<Database.Models.Dependency>(vc);

            // get the stream
            Database.Models.Source source = repoSource.Find(x => x.Name == command.Name);

            // detele the conditions
            repoSourceCondition.Delete(x => x.SourceId == source.Id);

            // delete the dependencies
            repoDependency.Delete(x => x.PrincipalObjectId == source.Id);

            // delete the object
            repoSource.Delete(x => x.Name == command.Name);
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
