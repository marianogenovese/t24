﻿//-----------------------------------------------------------------------
// <copyright file="DropAdapterCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;
    using Integra.Vision.Engine.Database.Repositories;

    /// <summary>
    /// Implements all the process of delete an adapter.
    /// </summary>
    internal sealed class DropAdapterCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                using (ViewsContext context = new ViewsContext("EngineDatabase"))
                {
                    this.DeleteObject(context, command as DropAdapterCommand);
                    return new OkCommandResult();
                }
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Contains drop adapter logic.
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Drop adapter command</param>
        private void DeleteObject(ViewsContext vc, DropAdapterCommand command)
        {
            // create repository
            Database.Repositories.Repository<Database.Models.Adapter> repoAdapter = new Database.Repositories.Repository<Database.Models.Adapter>(vc);
            Database.Repositories.Repository<Database.Models.Arg> repoArg = new Database.Repositories.Repository<Database.Models.Arg>(vc);
            Database.Repositories.Repository<Database.Models.Dependency> repoDependency = new Database.Repositories.Repository<Database.Models.Dependency>(vc);

            // get the stream
            Database.Models.Adapter adapter = repoAdapter.Find(x => x.Name == command.Name);

            // detele the conditions
            repoArg.Delete(x => x.AdapterId == adapter.Id);

            /*
            int sourceConditionCount = vc.SaveChanges();

            // throw an exception if not deleted a statement
            if (sourceConditionCount < 1)
            {
                // close connection
                vc.Dispose();

                // throw the exception 
                throw new Integra.Vision.Engine.Exceptions.DropUserDefinedObjectException("The adapter arguments were not removed");
            }
            */

            // delete the dependencies
            repoDependency.Delete(x => x.PrincipalObjectId == adapter.Id);
            
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
            repoAdapter.Delete(x => x.Name == command.Name);            
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